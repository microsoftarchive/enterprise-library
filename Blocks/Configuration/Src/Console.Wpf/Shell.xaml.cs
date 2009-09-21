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
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using System.Windows.Forms.Design;
using Console.Wpf.ViewModel;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;

namespace Console.Wpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Shell : Window, IWindowsFormsEditorService, IUIService, IServiceProvider
    {
        private string currentFile;
        private ConfigurationSourceViewModel configModel;
        private ServiceContainer serviceContainer = new ServiceContainer();

        public Shell()
        {
            InitializeComponent();
            var locator = new BinPathProbingAssemblyLocator();
            serviceContainer.AddService(typeof (AssemblyLocator), locator);
            serviceContainer.AddService(typeof (ConfigurationSectionLocator),
                                        new AssemblyAttributeSectionLocator(locator));
            serviceContainer.AddService(typeof (IWindowsFormsEditorService), this);
            serviceContainer.AddService(typeof (IUIService), this);
        	serviceContainer.AddService(typeof(DiscoverDerivedConfigurationTypesService),
        	                            new DiscoverDerivedConfigurationTypesService(locator));
        	serviceContainer.AddService(typeof(MergeableConfigurationCollectionService),
        	                            new MergeableConfigurationCollectionService());

            serviceContainer.AddService(typeof(ElementLookup), new ElementLookup());

            FileMenu_NewEnvironment_Click(this, null);

        }

        private void FileMenu_Open_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
                             {
                                 Title = "Select Configuration File",
                                 InitialDirectory = @"f:\temp",
                                 DefaultExt = "*.config",
                                 Multiselect = false,
                                 Filter = "Configure files (*.config)|*.config|All Files (*.*)|*.*",
                                 FilterIndex = 0
                             };

            if (dialog.ShowDialog(this) == true)
            {
                currentFile = dialog.FileName;

                configModel = new ConfigurationSourceViewModel(serviceContainer);
                
                using (var configSource = new FileConfigurationSource(currentFile))
                {
                    configModel.Open(configSource);
                }

                this.DataContext = configModel;
            }
        }

        List<SectionViewModel> environments = new List<SectionViewModel>();
        private void FileMenu_NewEnvironment_Click(object sender, RoutedEventArgs e)
        {
            EnvironmentMergeSection section = new EnvironmentMergeSection() { EnvironmentName = "Test environment" };
            var environmentViewModel = SectionViewModel.CreateSection(serviceContainer, section);
            environments.Add(environmentViewModel);
        }


        private void FileMenu_OpenEnvironment_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Select Delta Configuration File",
                InitialDirectory = Environment.CurrentDirectory,
                DefaultExt = "*.dconfig",
                Multiselect = false,
                Filter = "Delta configuration file (*.dconfig)|*.dconfig|All Files (*.*)|*.*",
                FilterIndex = 0
            };

            if (dialog.ShowDialog(this) == true)
            {
                MessageBox.Show("asdasd");
            }

        }

        private void FileMenu_Save_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationSectionCloner cloner = new ConfigurationSectionCloner();
            using (var configSource = new FileConfigurationSource(currentFile))
            {
                foreach(var section in configModel.Sections)
                {
                    var sectionName = ((ConfigurationSection)section.ConfigurationElement).SectionInformation.Name;
                    var savableSection = cloner.Clone((ConfigurationSection)section.ConfigurationElement);
                    configSource.Remove(sectionName);
                    configSource.Add(sectionName, savableSection);
                }
            }
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        void IUIService.ShowMessage(string message, string caption)
        {
            throw new NotImplementedException();
        }

        void IUIService.ShowMessage(string message)
        {
            throw new NotImplementedException();
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
            return serviceContainer.GetService(serviceType);
        }

        #endregion
    }
}
