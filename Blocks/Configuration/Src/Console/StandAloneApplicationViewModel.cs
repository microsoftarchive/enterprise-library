using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console.Properties;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Windows;

namespace Console.Wpf.ViewModel.Services
{
    public class StandAloneApplicationViewModel : INotifyPropertyChanged, IPropertyDirtyStateListener
    {
        ObservableCollection<string> mostRecentlyUsed;
        IUIServiceWpf uiService;
        ConfigurationSourceModel sourceModel;
        ElementLookup lookup;
        string currentFileName;
        bool isDirty;

        public StandAloneApplicationViewModel(IUIServiceWpf uiService, ConfigurationSourceModel sourceModel, ElementLookup lookup)
        {
            this.uiService = uiService;
            this.sourceModel = sourceModel;
            this.mostRecentlyUsed = new ObservableCollection<string>();
            this.lookup = lookup;

            this.lookup.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(lookup_CollectionChanged);
        }

        void lookup_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!IsDirty) IsDirty = true;
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
                using (var source = new DesignConfigurationSource(result.FileName))
                {
                    sourceModel.Load(source);

                    CurrentFilePath = result.FileName;
                    IsDirty = false;
                    
                    mostRecentlyUsed.Add(currentFileName);
                }
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
                    Save();
                }
            }
            return true;
        }

        public void New()
        {
            if (!PromptSaveChangesIfDirtyAndContinue()) return;

            sourceModel.New();
            IsDirty = false;
        }

        public void Save()
        {
            SaveImplementation(CurrentFilePath);
        }

        private void SaveImplementation(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                SaveAs();
                return;
            }

            if (!File.Exists(fileName))
            {
                using (var textWriter = File.CreateText(fileName))
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
                    using (FileConfigurationSource source = new FileConfigurationSource(fileName))
                    {
                        source.GetSection("applicationSettings");
                    }

                }
                catch
                {
                    var warningResults = uiService.ShowMessageWpf(
                        string.Format(Resources.Culture, Resources.PromptSaveOverFileThatCannotBeReadFromWarningMessage, fileName),
                        Resources.PromptSaveOverFileThatCannotBeReadFromWarningTitle, 
                        MessageBoxButton.OKCancel);

                    if (warningResults == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                    fileIsInvalidConfigurationFile = true;
                }


                var attributes = File.GetAttributes(fileName);
                if ((attributes | FileAttributes.ReadOnly) == attributes)
                {
                    var overwriteReadonlyDialogResult = uiService.ShowMessageWpf(
                            Resources.PromptSaveOverFileThatCannotBeReadFromWarningMessage, 
                            Resources.PromptSaveOverFileThatCannotBeReadFromWarningTitle, 
                            MessageBoxButton.YesNoCancel);

                    switch (overwriteReadonlyDialogResult)
                    {
                        case MessageBoxResult.No:

                            SaveAs();
                            return;

                        case MessageBoxResult.Yes:

                            File.SetAttributes(currentFileName, attributes ^ FileAttributes.ReadOnly);

                            break;

                        default:

                            return;
                    }
                }

                if (fileIsInvalidConfigurationFile)
                {
                    File.Delete(fileName);
                    using (var textWriter = File.CreateText(fileName))
                    {
                        textWriter.WriteLine("<configuration />");
                    }
                }
            }
            using (DesignConfigurationSource configurationSource = new DesignConfigurationSource(fileName))
            {
                sourceModel.Save(configurationSource);
            }

            CurrentFilePath = fileName;
            
            IsDirty = false;
        }

        public void SaveAs()
        {
            var dialog = new SaveFileDialog()
            {
                Title = Resources.SaveConfigurationFileDialogTitle,
                DefaultExt = "*.config",
                Filter = Resources.SaveConfigurationFileDialogFilter,
                FilterIndex = 0
            };

            var saveDialogResult = uiService.ShowFileDialog(dialog);

            if (saveDialogResult.DialogResult != true) return;

            SaveImplementation(saveDialogResult.FileName);
        }

        public IEnumerable<string> MostRecentlyUsed
        {
            get { return mostRecentlyUsed; }
        }

        public bool IsDirty
        {
            get { return isDirty; }
            private set { isDirty = value; OnPropertyChanged("IsDirty"); }
        }

        public string CurrentFilePath
        {
            get { return currentFileName; }
            private set { currentFileName = value; OnPropertyChanged("CurrentFilePath"); }
        }

        public string ApplicationTitle
        {
            get
            {
                return string.Format("Enterprise Library Configuration - {0} {1}",
                                     string.IsNullOrEmpty(CurrentFilePath) ? "New Configuration" : CurrentFilePath,
                                     IsDirty ? "*" : "");
            }

        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "IsDirty") OnPropertyChanged("ApplicationTitle");
            if (propertyName == "CurrentFilePath") OnPropertyChanged("ApplicationTitle");

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetDirty()
        {
            IsDirty = true;
        }
    }
}
