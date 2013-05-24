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
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms.Design;
using System.Windows.Input;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public sealed partial class Shell : IWindowsFormsEditorService, IUIServiceWpf, IDisposable
    {
        private ConfigurationContainer container;
        private IApplicationModel applicationModel;
        private bool closeRequested;
        private bool firstLoad = true;

        public Shell()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.AssemblyResolve += this.OnAssemblyResolve;

            container =
                new ConfigurationContainer(GetProfile(App.CommandLineParameters.ProfileFileName));

            container.RegisterInstance<IUIServiceWpf>(this);
            container.RegisterInstance<IWindowsFormsEditorService>(this);
            container.RegisterInstance<IUIService>(this);
            container.RegisterInstance<IAssemblyDiscoveryService>(new LoadedAssembliesDiscoveryService());

            container.DiscoverSubstituteTypesFromAssemblies();

            applicationModel = container.Resolve<IApplicationModel>();
            DataContext = applicationModel;

            applicationModel.New();
            applicationModel.OnCloseAction = () => RequestClose();

            InputBindings.Add(new InputBinding(applicationModel.NewConfigurationCommand, new KeyGesture(Key.N, ModifierKeys.Control)));
            InputBindings.Add(new InputBinding(applicationModel.SaveConfigurationCommand, new KeyGesture(Key.S, ModifierKeys.Control)));
            InputBindings.Add(new InputBinding(applicationModel.SaveAsConfigurationCommand, new KeyGesture(Key.A, ModifierKeys.Control)));
            InputBindings.Add(new InputBinding(applicationModel.OpenConfigurationCommand, new KeyGesture(Key.O, ModifierKeys.Control)));
        }

        private Profile GetProfile(string profileFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(profileFileName))
                {
                    return new Profile();
                }
                
                using (var stream = new FileStream(profileFileName, FileMode.Open, FileAccess.Read))
                {
                    var serializer = new XmlSerializer(typeof(Profile));
                    return (Profile)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                // TODO : Fix (the dialog is closing right away)
                ShowError(string.Format(Properties.Resources.LoadingProfileExceptionMessage, ex.Message));
            }

            return null;
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
            return form.ShowDialog();
        }

        void IUIService.ShowError(Exception ex, string message)
        {
            MessageBox.Show(
                string.Format(CultureInfo.CurrentCulture, Properties.Resources.ShowErrorMessageFormat, message, ex.Message),
                Properties.Resources.ShowErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void IUIService.ShowError(Exception ex)
        {
            throw new NotImplementedException();
        }

        void IUIService.ShowError(string message)
        {
            MessageBox.Show(message, Properties.Resources.ShowErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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

        FileDialogResult IUIServiceWpf.ShowFileDialog(FileDialog dialog)
        {
            var result = dialog.ShowDialog(this);
            return new FileDialogResult { FileName = dialog.FileName, DialogResult = result };
        }

        Nullable<bool> IUIServiceWpf.ShowDialog(Window dialog)
        {
            dialog.Owner = this;
            return dialog.ShowDialog();
        }

        MessageBoxResult IUIServiceWpf.ShowMessageWpf(string message, string caption, MessageBoxButton buttons)
        {
            return MessageBox.Show(message, caption, buttons);
        }

        void IUIServiceWpf.ShowWindow(Window window)
        {
            window.Owner = this;
            window.Show();
        }

        #endregion

        private void ShowError(string message)
        {
            MessageBox.Show(message, Properties.Resources.ShowErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RequestClose()
        {
            try
            {
                this.closeRequested = true;
                this.Close();
            }
            finally
            {
                this.closeRequested = false;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!this.closeRequested)
            {
                e.Cancel = !this.applicationModel.PromptSaveChangesIfDirtyAndContinue();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.firstLoad)
            {
                this.firstLoad = false;

                if (App.StartingFileName != null && File.Exists(App.StartingFileName))
                {
                    this.applicationModel.Load(App.StartingFileName);
                }
            }
        }

        public void Dispose()
        {
            applicationModel = null;
            if (container != null)
            {
                container.Dispose();
                container = null;
            }

            GC.SuppressFinalize(this);
        }

        private System.Reflection.Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return null;
        }
    }
}
