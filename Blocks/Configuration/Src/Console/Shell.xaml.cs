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
        private StandAloneApplicationViewModel applicationModel;

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
            container.RegisterType<StandAloneApplicationViewModel>(new ContainerControlledLifetimeManager());

            container.RegisterInstance<IUIServiceWpf>(this);
            container.RegisterInstance<IWindowsFormsEditorService>(this);
            container.RegisterInstance<IUIService>(this);
            container.RegisterInstance<IServiceProvider>(this);

            AnnotationService annotationService = container.Resolve<AnnotationService>();
            AppSettingsDecorator.DecorateAppSettingsSection(annotationService);
            ConnectionStringsDecorator.DecorateConnectionStringsSection(annotationService);
            
            try
            {
                applicationModel = container.Resolve<StandAloneApplicationViewModel>();
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

            Binding titleBinding = new Binding("ApplicationTitle");
            titleBinding.Source = applicationModel;
            SetBinding(Window.TitleProperty, titleBinding);
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
            var service = container.Resolve<StandAloneApplicationViewModel>();
            service.OpenConfigurationSource();
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
            var shellService = container.Resolve<StandAloneApplicationViewModel>();
            shellService.SaveAs();
        }

        private void FileMenu_Save_Click(object sender, RoutedEventArgs e)
        {
            var shellService = container.Resolve<StandAloneApplicationViewModel>();
            shellService.Save();
        }

        private void FileMenu_New_Click(object sender, RoutedEventArgs e)
        {
            var service = container.Resolve<StandAloneApplicationViewModel>();
            service.New();
        }

        private void FileMenu_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            throw new NotImplementedException();
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


        public bool UIDirty
        {
            get { throw new NotImplementedException(); }
        }
    }
}
