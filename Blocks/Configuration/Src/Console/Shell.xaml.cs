//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Shell : Window, IWindowsFormsEditorService, IUIService, IServiceProvider, IUIServiceWpf
    {
        private UnityContainer container = new UnityContainer();

        private List<EnvironmentalOverridesViewModel> environments = new List<EnvironmentalOverridesViewModel>();
        private ObservableCollection<CommandModel> environmentCommands = new ObservableCollection<CommandModel>();
        private ApplicationViewModel applicationModel;

        public Shell()
        {
            InitializeComponent();

            container.RegisterType<AssemblyLocator, BinPathProbingAssemblyLocator>(new ContainerControlledLifetimeManager());
            container.RegisterType<ConfigurationSectionLocator, AssemblyAttributeSectionLocator>(new ContainerControlledLifetimeManager());
            container.RegisterType<AnnotationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ElementLookup>(new ContainerControlledLifetimeManager());
            container.RegisterType<ConfigurationSourceModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<MenuCommandService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ConfigurationSourceDependency>(new ContainerControlledLifetimeManager());
            container.RegisterType<ApplicationViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<ValidationModel>(new ContainerControlledLifetimeManager());

            container.RegisterInstance<IUIServiceWpf>(this);
            container.RegisterInstance<IWindowsFormsEditorService>(this);
            container.RegisterInstance<IUIService>(this);
            container.RegisterInstance<IServiceProvider>(this);

            AnnotationService annotationService = container.Resolve<AnnotationService>();
            AppSettingsDecorator.DecorateAppSettingsSection(annotationService);
            ConnectionStringsDecorator.DecorateConnectionStringsSection(annotationService);
            
            try
            {
                applicationModel = container.Resolve<ApplicationViewModel>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            container.RegisterInstance<IApplicationModel>(applicationModel);

            var commandService = container.Resolve<MenuCommandService>();

            AddApplicationBlockMenu.ItemsSource = commandService.GetCommands(CommandPlacement.BlocksMenu);

            DataContext = container.Resolve<ConfigurationSourceModel>();

            applicationModel.New();

            var validationModel = container.Resolve<ValidationModel>();
            errorListView.ItemsSource = validationModel.ValidationErrors;

            Binding titleBinding = new Binding("ApplicationTitle");
            titleBinding.Source = applicationModel;
            SetBinding(Window.TitleProperty, titleBinding);

            newConfigurationCommand = new DelegateCommand(x => NewConfiguration());
            saveConfigurationCommand = new DelegateCommand(x => SaveConfiguration());
            saveConfigurationAsCommand = new DelegateCommand(x => SaveAsConfiguration());
            openConfigurationCommand = new DelegateCommand(x => OpenConfiguration());
            exitCommand = new DelegateCommand(x => Close());

            InputBindings.Add(new InputBinding(newConfigurationCommand, new KeyGesture(Key.N, ModifierKeys.Control)));
            InputBindings.Add(new InputBinding(saveConfigurationCommand, new KeyGesture(Key.S, ModifierKeys.Control)));
            InputBindings.Add(new InputBinding(saveConfigurationAsCommand, new KeyGesture(Key.A, ModifierKeys.Control)));
            InputBindings.Add(new InputBinding(openConfigurationCommand, new KeyGesture(Key.O, ModifierKeys.Control)));
        }


        private ICommand newConfigurationCommand;
        private ICommand saveConfigurationCommand;
        private ICommand saveConfigurationAsCommand;
        private ICommand openConfigurationCommand;
        private ICommand exitCommand;



        private void NewConfiguration()
        {
            var service = container.Resolve<ApplicationViewModel>();
            service.New();
        }

        private void SaveConfiguration()
        {
            var shellService = container.Resolve<ApplicationViewModel>();
            shellService.Save();
        }

        private void SaveAsConfiguration()
        {
            var shellService = container.Resolve<ApplicationViewModel>();
            shellService.SaveAs();
        }

        private void OpenConfiguration()
        {
            var service = container.Resolve<ApplicationViewModel>();
            service.OpenConfigurationSource();
        }

        private void Exit()
        {
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel)
            {
                e.Cancel = !applicationModel.PromptSaveChangesIfDirtyAndContinue();
            }
        }


        private void FileMenu_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenConfiguration();
        }

        private void EnvironemntMenu_Open_Click(object sender, RoutedEventArgs e)
        {
            var openEnvironmentModel = container.Resolve<OpenEnvironmentConfigurationDeltaCommand>();
            openEnvironmentModel.Execute(null);
        }

        private void EnvironemntMenu_New_Click(object sender, RoutedEventArgs e)
        {
            var configurationModel = container.Resolve<ConfigurationSourceModel>();
            configurationModel.NewEnvironment();
        }

        private void FileMenu_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveAsConfiguration();
        }

        private void FileMenu_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveConfiguration();
        }

        private void FileMenu_New_Click(object sender, RoutedEventArgs e)
        {
            NewConfiguration();
        }

        private void FileMenu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        #region infrastructural services 
        void IWindowsFormsEditorService.CloseDropDown()
        {
            throw new NotSupportedException();
        }

        void IWindowsFormsEditorService.DropDownControl(System.Windows.Forms.Control control)
        {
            throw new NotSupportedException();
        }

        System.Windows.Forms.DialogResult IWindowsFormsEditorService.ShowDialog(System.Windows.Forms.Form dialog)
        {
            return dialog.ShowDialog();
        }

        bool IUIService.CanShowComponentEditor(object component)
        {
            throw new NotImplementedException();
        }

        System.Windows.Forms.IWin32Window IUIService.GetDialogOwnerWindow()
        {
            throw new NotImplementedException();
        }

        void IUIService.SetUIDirty()
        {
            throw new NotImplementedException();
        }

        bool IUIService.ShowComponentEditor(object component, System.Windows.Forms.IWin32Window parent)
        {
            throw new NotImplementedException();
        }

        System.Windows.Forms.DialogResult IUIService.ShowDialog(System.Windows.Forms.Form form)
        {
            throw new NotImplementedException();
        }

        void IUIService.ShowError(Exception ex, string message)
        {
            MessageBox.Show(string.Format("{0}\n\n{1}",  message, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error));
        }

        void IUIService.ShowError(Exception ex)
        {
            throw new NotImplementedException();
        }

        void IUIService.ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        System.Windows.Forms.DialogResult IUIService.ShowMessage(string message, string caption, System.Windows.Forms.MessageBoxButtons buttons)
        {
            return System.Windows.Forms.MessageBox.Show(message, caption, buttons);
        }

        void IUIService.ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption);
        }

        void IUIService.ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        bool IUIService.ShowToolWindow(Guid toolWindow)
        {
            throw new NotImplementedException();
        }

        Dictionary<object, object> styles = new Dictionary<object, object>();
        System.Collections.IDictionary IUIService.Styles
        {
            get { return styles; }
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        #endregion

        public FileDialogResult ShowFileDialog(FileDialog dialog)
        {
            var result = dialog.ShowDialog(this);
            return new FileDialogResult { FileName = dialog.FileName, DialogResult = result };
        }


        public MessageBoxResult ShowMessageWpf(string message, string caption, MessageBoxButton buttons)
        {
            return MessageBox.Show(message, caption, buttons);
        }


        private class DelegateCommand : ICommand
        {
            Action<object> action;

            public DelegateCommand(Action<object> action)
            {
                this.action = action;
            }
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                action(parameter);
            }
        }
    }
}
