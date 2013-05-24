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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Win32;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// ViewModel class that represents the configuration editor application.<br/>
    /// </summary>
    public class ApplicationViewModel : INotifyPropertyChanged, IApplicationModel
    {
        private readonly IUnityContainer builder;
        private readonly ElementLookup lookup;
        private readonly MenuCommandService menuCommandService;
        private readonly Profile profile;
        private readonly ConfigurationSourceModel sourceModel;
        private readonly IUIServiceWpf uiService;
        private string configurationFileName;


        private ObservableCollection<EnvironmentSourceViewModel> environments =
            new ObservableCollection<EnvironmentSourceViewModel>();

        private bool isDirty;
        private ElementViewModel selectedElement;


        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationViewModel"/>.
        /// </summary>
        /// <param name="uiService">The <see cref="IUIServiceWpf"/> that should be used to interact with the user.</param>
        /// <param name="sourceModel">The <see cref="ConfigurationSourceModel"/> that should be used to interact with the configuration schema.</param>
        /// <param name="lookup">The <see cref="ElementLookup"/> that should be used to look up <see cref="ElementViewModel"/> instances.</param>
        /// <param name="builder">The <see cref="IUnityContainer"/> instance that should be used to create view model instances with.</param>
        /// <param name="menuCommandService">The <see cref="MenuCommandService"/> that should be used to look up top-level <see cref="CommandModel"/> instances.</param>
        /// <param name="validationModel">The <see cref="ValidationModel"/> that should be used to add validation errors and warnings to.</param>
        /// <param name="profile">The <see cref="Profile"/> that should be used to filter UI elements based on the platform.</param>
        public ApplicationViewModel(IUIServiceWpf uiService, ConfigurationSourceModel sourceModel, ElementLookup lookup,
                                    IUnityContainer builder, MenuCommandService menuCommandService,
                                    ValidationModel validationModel, Profile profile)
        {
            ValidationModel = validationModel;
            this.uiService = uiService;
            this.sourceModel = sourceModel;

            this.lookup = lookup;
            this.builder = builder;
            this.menuCommandService = menuCommandService;
            this.profile = profile;

            NewConfigurationCommand = new DelegateCommand(x => New());
            SaveConfigurationCommand = new DelegateCommand(x => Save());
            SaveAsConfigurationCommand = new DelegateCommand(x => SaveAs());
            OpenConfigurationCommand = new DelegateCommand(x => OpenConfigurationSource());
            ExitCommand = new DelegateCommand(x => Close());

            var environmentCommandsEnabled = this.profile == null ? true : profile.EnvironmentCommandsEnabled;
            OpenEnvironmentCommand = new OpenEnvironmentConfigurationDeltaCommand(uiService, this, environmentCommandsEnabled);
            NewEnvironmentCommand = new DelegateCommand(x => NewEnvironment(), _ => environmentCommandsEnabled);
        }


        /// <summary>
        /// Gets the application title, including an asteriks that is used as an indicator whether the application contains unsaved changed.
        /// </summary>
        /// <value>
        /// The application title, including an asteriks that is used as an indicator whether the application contains unsaved changed.
        /// </value>
        public string ApplicationTitle
        {
            get
            {
                var applicationTitleFormat = this.profile != null && !string.IsNullOrEmpty(this.profile.ApplicationTitleFormat)
                                                 ? this.profile.ApplicationTitleFormat
                                                 : Resources.ApplicationTitleFormat;

                return string.Format(CultureInfo.CurrentCulture, applicationTitleFormat,
                                     string.IsNullOrEmpty(ConfigurationFilePath)
                                         ? Resources.ApplicationNewConfiguration
                                         : ConfigurationFilePath,
                                     IsDirty ? "*" : "");
            }
        }

        /// <summary>
        /// Gets the currently opened <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <value>
        /// The currently opened <see cref="ConfigurationSourceModel"/>.
        /// </value>
        public ConfigurationSourceModel CurrentConfigurationSource
        {
            get { return sourceModel; }
        }

        /// <summary>
        /// Gets the available <see cref="ICommand"/> instances that should be displayed in the top-level wizard menu.
        /// </summary>
        /// <value>
        /// The available <see cref="ICommand"/> instances that should be displayed in the top-level wizard menu.
        /// </value>
        public IEnumerable<ICommand> WizardCommands
        {
            get { return menuCommandService.GetCommands(CommandPlacement.WizardMenu).Cast<ICommand>(); }
        }

        /// <summary>
        /// Gets the available <see cref="ICommand"/> instances that should be displayed in the top-level blocks menu.
        /// </summary>
        /// <value>
        /// The available <see cref="ICommand"/> instances that should be displayed in the top-level blocks menu.
        /// </value>
        public IEnumerable<ICommand> BlockCommands
        {
            get { return menuCommandService.GetCommands(CommandPlacement.BlocksMenu).Cast<ICommand>(); }
        }

        #region Environment Related

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be bound to and invoked to open a Configuration Environment from a Delta Configuration File (*.dconfig).
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be bound to and invoked to open a Configuration Environment from a Delta Configuration File (*.dconfig).
        /// </value>
        public ICommand OpenEnvironmentCommand { get; private set; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be bound to and invoked to create a new Configuration Environment.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be bound to and invoked to create a new Configuration Environment.
        /// </value>
        public ICommand NewEnvironmentCommand { get; private set; }

        /// <summary>
        /// Creates a new Configuration Environment.
        /// </summary>
        public void NewEnvironment()
        {
            var environmentName = FindUniqueEnvironmentName("Environment");
            LoadEnvironment(new EnvironmentalOverridesSection() { EnvironmentName = environmentName }, string.Empty);
        }

        /// <summary>
        /// Opens a Configuration Environment from a Delta Configuration File (*.dconfig).
        /// </summary>
        /// <param name="environment">The <see cref="EnvironmentalOverridesSection"/> contained in the Delta Configuration File (*.dconfig).</param>
        /// <param name="environmentDeltaFile">The path to the source Delta Configuration File (*.dconfig).</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void LoadEnvironment(EnvironmentalOverridesSection environment, string environmentDeltaFile)
        {
            var environmentSection =
                (EnvironmentSourceViewModel)
                SectionViewModel.CreateSection(builder, EnvironmentalOverridesSection.EnvironmentallyOverriddenProperties, environment);

            environmentSection.EnvironmentDeltaFile = environmentDeltaFile;
            environmentSection.LastEnvironmentDeltaSavedFilePath = environmentDeltaFile;

            environments.Add(environmentSection);

            SectionInitializer.InitializeSection(environmentSection, new InitializeContext(null));
            lookup.AddSection(environmentSection);

            environmentSection.Select();
        }

        /// <summary>
        /// Removes a Configuration Environment from the application.
        /// </summary>
        /// <param name="environment">The <see cref="EnvironmentSourceViewModel"/> that should be removed.</param>
        public void RemoveEnvironment(EnvironmentSourceViewModel environment)
        {
            lookup.RemoveSection(environment);
            environments.Remove(environment);
        }

        /// <summary>
        /// Gets the currently loaded Configuration Environments.
        /// </summary>
        /// <value>
        /// The currently loaded Configuration Environments.
        /// </value>
        public IEnumerable<EnvironmentSourceViewModel> Environments
        {
            get { return environments; }
        }

        #endregion

        /// <summary>
        /// Re-initializes the application with a new <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <remarks>
        /// Closes the current <see cref="ConfigurationSourceModel"/> and any open Configuration Environments prior to re-initializing the application.<br/>
        /// If the current <see cref="ConfigurationSourceModel"/> or any Configuration Environments contain changes it will attempt to save these changes.<br/>
        /// </remarks>
        /// <returns>
        /// <see langword="true"/> if Re-initializing the application completed successfully, otherwise <see langword="false"/>. 
        /// E.g. The user cancelled saving the current <see cref="ConfigurationSourceModel"/>.<br/>
        /// </returns>
        public bool New()
        {
            if (!PromptSaveChangesIfDirtyAndContinue()) return false;

            sourceModel.New();
            ClearEnvironments();
            ClearSelectedElement();
            IsDirty = false;
            ConfigurationFilePath = null;
            return true;
        }

        private void ClearSelectedElement()
        {
            selectedElement = null;
            OnSelectedElementChanged(null);
        }

        private void ClearEnvironments()
        {
            foreach (var environment in environments)
            {
                environment.Dispose();
            }
            environments.Clear();
        }

        /// <summary>
        /// Saves the current <see cref="ConfigurationSourceModel"/> and any open Configuration Environments.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if saving the <see cref="ConfigurationSourceModel"/> completed successfully, otherwise <see langword="false"/>. 
        /// E.g. The user cancelled saving the current <see cref="ConfigurationSourceModel"/>.<br/>
        /// </returns>
        public bool Save()
        {
            return SaveImplementation(ConfigurationFilePath, false);
        }

        /// <summary>
        /// Ensures whether a configuration file can be saved to by the application.<br/>
        /// </summary>
        /// <remarks>
        /// This method will prompt the user if the <paramref name="configurationFile"/> is maked as readonly or has invalid content (and therefore needs to be overwritten).<br/>
        /// If the file does not exist it will create an empty configuration file that can be used to save to.
        /// </remarks>
        /// <param name="configurationFile">The path to which a configuration file is intented to be saved to.</param>
        /// <returns><see langword="true"/> if the application can save to the target configuration file. Otherwise <see langword="false"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool EnsureCanSaveConfigurationFile(string configurationFile)
        {
            if (!File.Exists(configurationFile))
            {
                using (var textWriter = File.CreateText(configurationFile))
                {
                    textWriter.WriteLine("<configuration />");
                }
            }
            else
            {
                var fileIsInvalidConfigurationFile = false;
                try
                {
                    using (var source = new FileConfigurationSource(configurationFile))
                    {
                        source.GetSection("applicationSettings");
                    }
                }
                catch (Exception ex)
                {
                    ConfigurationLogWriter.LogException(ex);
                    var warningResults = uiService.ShowMessageWpf(
                        string.Format(CultureInfo.CurrentCulture, Resources.PromptSaveOverFileThatCannotBeReadFromWarningMessage,
                                      configurationFile),
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
                        string.Format(CultureInfo.CurrentCulture, Resources.PromptOverwriteReadonlyFileMessage,
                                      configurationFile),
                        Resources.PromptOverwriteReadonlyFiletitle,
                        MessageBoxButton.YesNoCancel);

                    switch (overwriteReadonlyDialogResult)
                    {
                        case MessageBoxResult.No:

                            return SaveAs();


                        case MessageBoxResult.Yes:

                            File.SetAttributes(configurationFile, attributes ^ FileAttributes.ReadOnly);

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

        /// <summary>
        /// Gets whether the application model contains unsaved changes.<br/>
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the application model contains unsaved changes. Otherwise <see langword="false"/>.
        /// </value>
        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                isDirty = value;
                OnPropertyChanged("IsDirty");
            }
        }

        /// <summary>
        /// Gets the path to the currently opened <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <returns>
        /// The path to the currently opened <see cref="ConfigurationSourceModel"/>.
        /// </returns>
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

        /// <summary>
        /// Raises the <see cref="SelectedElementChanged"/> event.
        /// </summary>
        /// <param name="element">The newly selected <see cref="ElementViewModel"/> instance.</param>
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
                handler(this, new SelectedElementChangedEventHandlerEventArgs(element));
            }
        }

        /// <summary>
        /// Occurs when the selected <see cref="ElementViewModel"/> changed.
        /// </summary>
        public event EventHandler<SelectedElementChangedEventHandlerEventArgs> SelectedElementChanged;

        /// <summary>
        /// Sets the <see cref="IsDirty"/> property to <see langword="true"/>, indicating that the application has unsaved changes.
        /// </summary>
        public void SetDirty()
        {
            IsDirty = true;
        }

        /// <summary>
        /// Loads a new <see cref="ConfigurationSourceModel"/> from <paramref name="configurationFilePath"/>.
        /// </summary>
        /// <param name="configurationFilePath">The path to the configuration file that should be loaded.</param>
        public void Load(string configurationFilePath)
        {
            using (var source = new DesignConfigurationSource(configurationFilePath))
            {
                if (sourceModel.Load(source))
                {
                    ConfigurationFilePath = configurationFilePath;
                    IsDirty = false;
                }
                else
                {
                    sourceModel.New();
                }
            }
        }

        /// <summary>
        /// Saves the current <see cref="ConfigurationSourceModel"/> to <paramref name="configurationFilePath"/>.
        /// </summary>
        /// <param name="configurationFilePath">The path to the file to which the current <see cref="ConfigurationSourceModel"/> should be saved.</param>
        /// <returns><see langword="true"/>if the save operation succeeded, otherwise <see langword="false"/>.</returns>
        public bool Save(string configurationFilePath)
        {
            string normalizedCurrentConfigurationFile = Path.GetFullPath(this.configurationFileName);
            string normalizedTargetConfigurationFile = Path.GetFullPath(configurationFilePath);

            bool isSameFile = 0 == string.Compare(normalizedCurrentConfigurationFile, normalizedTargetConfigurationFile, StringComparison.OrdinalIgnoreCase);

            return SaveImplementation(configurationFilePath, !isSameFile);
        }

        private void Close()
        {
            if (PromptSaveChangesIfDirtyAndContinue() && OnCloseAction != null)
            {
                OnCloseAction();
            }
        }

        /// <summary>
        /// Prompts the user to save any unsaved changes.
        /// </summary>
        /// <returns>
        /// <see langword="false"/> if the user choose not to save changes or the save operation failed. Otherwise <see langword="true"/>.
        /// </returns>
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

        /// <summary>
        /// Opens a new <see cref="ConfigurationSourceModel"/> from a file the user specifies.
        /// </summary>
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
                var waitDialog = new WaitDialog
                {
                    Message = string.Format(CultureInfo.CurrentCulture, Resources.ApplicationLoadingConfigurationFileWaitMessage, result.FileName)
                };
                uiService.ShowWindow(waitDialog);
                try
                {
                    environments.Clear();
                    Load(result.FileName);
                }
                finally
                {
                    waitDialog.Close();
                }
            }
        }

        /// <summary>
        /// Saves the current <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if saving the <see cref="ConfigurationSourceModel"/> completed successfully, otherwise <see langword="false"/>. 
        /// E.g. The user cancelled saving the current <see cref="ConfigurationSourceModel"/>.<br/>
        /// </returns>
        public bool SaveMainConfiguration()
        {
            return SaveMainConfiguration(ConfigurationFilePath, false);
        }

        /// <summary>
        /// Saves the current <see cref="ConfigurationSourceModel"/> to <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The file path to which the <see cref="ConfigurationSourceModel"/> should be saved.</param>
        /// <param name="saveAs"><see langword="true"/> if a new copy of the file should be saved to <paramref name="fileName"/>.</param>
        /// <returns>
        /// <see langword="true"/> if saving the <see cref="ConfigurationSourceModel"/> completed successfully, otherwise <see langword="false"/>. 
        /// E.g. The user cancelled saving the current <see cref="ConfigurationSourceModel"/>.<br/>
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool SaveMainConfiguration(string fileName, bool saveAs)
        {
            try
            {
                if (!sourceModel.IsValidForSave() || !EnvironmentsCanSave())
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

                if (saveAs && !string.IsNullOrEmpty(this.configurationFileName) && File.Exists(this.configurationFileName))
                {
                    File.Copy(this.configurationFileName, fileName, true);

                    // make the target file writable (it may have inherited the read-only attribute from the original file)
                    FileAttributes attributes = File.GetAttributes(fileName);
                    File.SetAttributes(fileName, attributes & ~FileAttributes.ReadOnly);
                }


                using (var configurationSource = new DesignConfigurationSource(fileName))
                {
                    var saved = sourceModel.Save(configurationSource);
                    if (!saved)
                    {
                        return false;
                    }
                }
                ConfigurationFilePath = fileName;
                return true;

            }
            catch (Exception ex)
            {
                ConfigurationLogWriter.LogException(ex);
                uiService.ShowMessageWpf(string.Format(CultureInfo.CurrentCulture, Resources.SaveApplicationErrorMessage, ex.Message),
                            Resources.SaveApplicationErrorMessageTitle, MessageBoxButton.OK);

                return false;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private bool SaveImplementation(string fileName, bool saveAs)
        {
            try
            {
                if (!SaveMainConfiguration(fileName, saveAs))
                {
                    return false;
                }


                foreach (var envrionment in Environments.OfType<EnvironmentSourceViewModel>())
                {
                    if (!envrionment.SaveDelta())
                    {
                        return false;
                    }
                }

                IsDirty = false;
                return true;
            }
            catch (Exception ex)
            {
                uiService.ShowMessageWpf(string.Format(CultureInfo.CurrentCulture, Resources.SaveApplicationErrorMessage, ex.Message),
                            Resources.SaveApplicationErrorMessageTitle, MessageBoxButton.OK);

                return false;
            }
        }

        private bool EnvironmentsCanSave()
        {
            foreach (var environment in Environments)
            {
                environment.Validate();

                var errors = environment.DescendentElements()
                    .Union(new ElementViewModel[] { environment })
                       .SelectMany(e => e.Properties)
                       .SelectMany(p => p.ValidationResults);
                if (errors.Any(e => e.IsError)) return false;
            }

            return true;
        }

        /// <summary>
        /// Saves a copy of the current <see cref="ConfigurationSourceModel"/> and any loaded Configuration Environment.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if saving the <see cref="ConfigurationSourceModel"/> completed successfully, otherwise <see langword="false"/>. 
        /// E.g. The user cancelled saving the current <see cref="ConfigurationSourceModel"/>.<br/>
        /// </returns>
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

            return SaveImplementation(saveDialogResult.FileName, true);
        }

        private string FindUniqueEnvironmentName(string baseName)
        {
            var number = 1;
            while (true)
            {
                var proposedName = string.Format(CultureInfo.CurrentCulture,
                                                 Resources.NewCollectionElementNameFormat,
                                                 baseName,
                                                 number == 1 ? string.Empty : number.ToString(CultureInfo.CurrentCulture)).Trim();

                if (
                    Environments.Any(
                        x => x.NameProperty != null && x.NameProperty.BindableProperty.BindableValue == proposedName))
                    number++;
                else
                    return proposedName;
            }
        }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be invoked to re-initialize the application with a new <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be invoked to re-initialize the application with a new <see cref="ConfigurationSourceModel"/>.
        /// </value>
        /// <seealso cref="New"/>
        public ICommand NewConfigurationCommand { get; private set; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be invoked to save the current <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be invoked to save the current <see cref="ConfigurationSourceModel"/>.
        /// </value>
        /// <seealso cref="Save()"/>
        public ICommand SaveConfigurationCommand { get; private set; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be invoked to save a copy of the current <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be invoked to save a copy of the current <see cref="ConfigurationSourceModel"/>.
        /// </value>
        /// <seealso cref="SaveAs"/>
        public ICommand SaveAsConfigurationCommand { get; private set; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be invoked to open a new <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be invoked to open a new <see cref="ConfigurationSourceModel"/>.
        /// </value>
        /// <seealso cref="OpenConfigurationSource"/>
        public ICommand OpenConfigurationCommand { get; private set; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be invoked to close the application.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be invoked to close the application.
        /// </value>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="Action"/> that should be performed to close the application.
        /// </summary>
        /// <value>
        /// The <see cref="Action"/> that should be performed to close the application.
        /// </value>
        public Action OnCloseAction { get; set; }

        /// <summary>
        /// Gets the <see cref="ValidationModel"/> instance for this application.
        /// </summary>
        /// <value>
        /// The <see cref="ValidationModel"/> instance for this application.
        /// </value>
        public ValidationModel ValidationModel { get; private set; }


        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
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

        /// <summary>
        /// Occurs when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
