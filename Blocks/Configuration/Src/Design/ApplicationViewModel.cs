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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    public class ApplicationViewModel : INotifyPropertyChanged, IApplicationModel
    {
        ObservableCollection<string> mostRecentlyUsed;
        IUIServiceWpf uiService;
        ConfigurationSourceModel sourceModel;
        ElementLookup lookup;
        string configurationFileName;
        bool isDirty;

        public ApplicationViewModel(IUIServiceWpf uiService, ConfigurationSourceModel sourceModel, ElementLookup lookup)
        {
            this.uiService = uiService;
            this.sourceModel = sourceModel;
            this.mostRecentlyUsed = new ObservableCollection<string>();
            this.lookup = lookup;
        }

        public void OpenConfigurationSource()
        {
            if (!PromptSaveChangesIfDirtyAndContinue()) return;

            var dialog = new OpenFileDialog()
                             {
                                 Title = Resources.OpenConfigurationFileDialogTitle,
                                 DefaultExt = "*.config",
                                 Multiselect = false,
                                 Filter = Resources.OpenConfigurationFileDialogFilter,
                                 FilterIndex = 0
                             };

            var result = uiService.ShowFileDialog(dialog);

            if (result.DialogResult == true)
            {
                Load(result.FileName);
                
            }
        }

        public bool PromptSaveChangesIfDirtyAndContinue()
        {
            if (IsDirty)
            {
                var confirmationResult = uiService.ShowMessageWpf(
                    Resources.PromptContinueOperationDiscardChangesWarningMessage,
                    Resources.PromptContinueOperationDiscardChangesWarningTitle, MessageBoxButton.YesNoCancel);
                if (confirmationResult == MessageBoxResult.Cancel)
                {
                    return false;
                }
                if (confirmationResult == MessageBoxResult.Yes)
                {
                    return Save();
                }
            }
            return true;
        }

        public bool New()
        {
            if (!PromptSaveChangesIfDirtyAndContinue()) return false;

            sourceModel.New();
            IsDirty = false;
            ConfigurationFilePath = null;
            return true;
        }

        public bool Save()
        {
            return SaveImplementation(ConfigurationFilePath);
        }
        
        public bool EnsureCanSaveConfigurationFile(string configurationFile)
        {
            if (!File.Exists(configurationFile))
            {
                using (var textWriter = File.CreateText(configurationFile))
                {
                    textWriter.WriteLine("<configuration />");
                    textWriter.Close();
                }
            }
            else
            {
                bool fileIsInvalidConfigurationFile = false;
                try
                {
                    using (FileConfigurationSource source = new FileConfigurationSource(configurationFile))
                    {
                        source.GetSection("applicationSettings");
                    }

                }
                catch
                {
                    var warningResults = uiService.ShowMessageWpf(
                        string.Format(Resources.Culture, Resources.PromptSaveOverFileThatCannotBeReadFromWarningMessage, configurationFile),
                        Resources.PromptSaveOverFileThatCannotBeReadFromWarningTitle,
                        MessageBoxButton.OKCancel);

                    if (warningResults == MessageBoxResult.Cancel)
                    {
                        return false;
                    }
                    fileIsInvalidConfigurationFile = true;
                }


                var attributes = File.GetAttributes(configurationFile);
                if ((attributes | FileAttributes.ReadOnly) == attributes)
                {
                    var overwriteReadonlyDialogResult = uiService.ShowMessageWpf(
                        string.Format(Resources.Culture, Resources.PromptSaveOverFileThatCannotBeReadFromWarningMessage, configurationFile),
                        Resources.PromptSaveOverFileThatCannotBeReadFromWarningTitle,
                        MessageBoxButton.YesNoCancel);

                    switch (overwriteReadonlyDialogResult)
                    {
                        case MessageBoxResult.No:

                            return SaveAs();


                        case MessageBoxResult.Yes:

                            File.SetAttributes(configurationFileName, attributes ^ FileAttributes.ReadOnly);

                            break;

                        default:

                            return false;
                    }
                }

                if (fileIsInvalidConfigurationFile)
                {
                    File.Delete(configurationFile);
                    using (var textWriter = File.CreateText(configurationFile))
                    {
                        textWriter.WriteLine("<configuration />");
                    }
                }
            }
            return true;
        }

        private bool SaveImplementation(string fileName)
        {
            if (!sourceModel.IsValidForSave())
            {
                uiService.ShowError(Resources.SaveConfigurationInvalidError);
                return false;
            }
            
            if (String.IsNullOrEmpty(fileName))
            {
                return SaveAs();
            }

            if (!EnsureCanSaveConfigurationFile(fileName))
            {
                return false;
            }
            
            using (var configurationSource = new DesignConfigurationSource(fileName))
            {
                sourceModel.Save(configurationSource);
            }

            ConfigurationFilePath = fileName;
            IsDirty = false;

            sourceModel.SaveEnvironmentDeltaFiles();

            return true;
        }

        public bool SaveAs()
        {
            var dialog = new SaveFileDialog()
                             {
                                 Title = Resources.SaveConfigurationFileDialogTitle,
                                 DefaultExt = "*.config",
                                 Filter = Resources.SaveConfigurationFileDialogFilter,
                                 FilterIndex = 0
                             };

            var saveDialogResult = uiService.ShowFileDialog(dialog);

            if (saveDialogResult.DialogResult != true) return false;

            return SaveImplementation(saveDialogResult.FileName);
        }

        public IEnumerable<string> MostRecentlyUsed
        {
            get { return mostRecentlyUsed; }
        }

        public bool IsDirty
        {
            get { return isDirty; }
            set { isDirty = value; OnPropertyChanged("IsDirty"); }
        }

        public string ConfigurationFilePath
        {
            get { return configurationFileName; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !Path.IsPathRooted(value))
                {
                    configurationFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value);
                }
                else
                {
                    configurationFileName = value;
                }
                OnPropertyChanged("ConfigurationFilePath");
            }
        }

        public string ApplicationTitle
        {
            get
            {
                return string.Format("Enterprise Library Configuration - {0} {1}",
                                     string.IsNullOrEmpty(ConfigurationFilePath) ? "New Configuration" : ConfigurationFilePath,
                                     IsDirty ? "*" : "");
            }

        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "IsDirty") OnPropertyChanged("ApplicationTitle");
            if (propertyName == "ConfigurationFilePath") OnPropertyChanged("ApplicationTitle");

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ElementViewModel selectedElement;

        public event PropertyChangedEventHandler PropertyChanged;

        #region IApplicationModel Members
       
        public void OnSelectedElementChanged(ElementViewModel element)
        {
            if (selectedElement != null)
            {
                selectedElement.IsSelected = false;
            }

            selectedElement = element;
            
            var handler = SelectedElementChanged;
            if (handler != null)
            {
                handler(this, new SelectedElementChangedEventHandlerArgs(element));
            }
        }



        public event EventHandler<SelectedElementChangedEventHandlerArgs> SelectedElementChanged;

        public void SetDirty()
        {
            IsDirty = true;
        }


        public void Load(string configurationFile)
        {
            using (var source = new DesignConfigurationSource(configurationFile))
            {
                sourceModel.Load(source);

                ConfigurationFilePath = configurationFile;
                IsDirty = false;

                mostRecentlyUsed.Add(configurationFileName);
            }
        }

        public bool Save(string configurationFile)
        {
            return SaveImplementation(configurationFile);
        }

        #endregion
    }

}
