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
using System.Windows.Input;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// Service interface used to interact with the Enterprise Library Configuration Designtime.
    /// </summary>
    /// <remarks>
    /// In order to get an instance of this interface, declare it as a constructor argument on the consuming component or use the <see cref="IUnityContainer"/> to obtain an instance from code.
    /// </remarks>
    public interface IApplicationModel
    {
        /// <summary>
        /// Gets the path to the currently opened <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <returns>
        /// The path to the currently opened <see cref="ConfigurationSourceModel"/>.
        /// </returns>
        string ConfigurationFilePath { get; }

        /// <summary>
        /// Gets whether the application model contains unsaved changes.<br/>
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the application model contains unsaved changes. Otherwise <see langword="false"/>.
        /// </value>
        bool IsDirty { get; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be invoked to re-initialize the application with a new <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be invoked to re-initialize the application with a new <see cref="ConfigurationSourceModel"/>.
        /// </value>
        /// <seealso cref="New"/>
        ICommand NewConfigurationCommand { get; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be invoked to save the current <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be invoked to save the current <see cref="ConfigurationSourceModel"/>.
        /// </value>
        /// <seealso cref="Save()"/>
        ICommand SaveConfigurationCommand { get; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be invoked to save a copy of the current <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be invoked to save a copy of the current <see cref="ConfigurationSourceModel"/>.
        /// </value>
        ICommand SaveAsConfigurationCommand { get; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be invoked to open a new <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be invoked to open a new <see cref="ConfigurationSourceModel"/>.
        /// </value>
        ICommand OpenConfigurationCommand { get; }

        /// <summary>
        /// Gets an <see cref="ICommand"/> instance that can be invoked to close the application.
        /// </summary>
        /// <value>
        /// An <see cref="ICommand"/> instance that can be invoked to close the application.
        /// </value>
        ICommand ExitCommand { get; }

        /// <summary>
        /// Gets or sets the <see cref="Action"/> that should be performed to close the application.
        /// </summary>
        /// <value>
        /// The <see cref="Action"/> that should be performed to close the application.
        /// </value>
        Action OnCloseAction { get; set; }
        
        /// <summary>
        /// Gets the <see cref="ValidationModel"/> instance for this application.
        /// </summary>
        /// <value>
        /// The <see cref="ValidationModel"/> instance for this application.
        /// </value>
        ValidationModel ValidationModel { get; }

        /// <summary>
        /// Sets the <see cref="IsDirty"/> property to <see langword="true"/>, indicating that the application has unsaved changes.
        /// </summary>
        void SetDirty();

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
        bool New();

        /// <summary>
        /// Saves the current <see cref="ConfigurationSourceModel"/> and any open Configuration Environments.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if saving the <see cref="ConfigurationSourceModel"/> completed successfully, otherwise <see langword="false"/>. 
        /// E.g. The user cancelled saving the current <see cref="ConfigurationSourceModel"/>.<br/>
        /// </returns>
        bool Save();

        /// <summary>
        /// Saves the current <see cref="ConfigurationSourceModel"/> to <paramref name="configurationFilePath"/>.
        /// </summary>
        /// <param name="configurationFilePath">The path to the file to which the current <see cref="ConfigurationSourceModel"/> should be saved.</param>
        /// <returns>
        /// <see langword="true"/> if saving the <see cref="ConfigurationSourceModel"/> completed successfully, otherwise <see langword="false"/>. 
        /// E.g. The user cancelled saving the current <see cref="ConfigurationSourceModel"/>.<br/>
        /// </returns>
        bool Save(string configurationFilePath);

        /// <summary>
        /// Loads a new <see cref="ConfigurationSourceModel"/> from <paramref name="configurationFilePath"/>.
        /// </summary>
        /// <param name="configurationFilePath">The path to the configuration file that should be loaded.</param>
        void Load(string configurationFilePath);

        /// <summary>
        /// Ensures whether a configuration file can be saved to by the application.<br/>
        /// </summary>
        /// <remarks>
        /// This method will prompt the user if the <paramref name="configurationFile"/> is maked as readonly or has invalid content (and therefore needs to be overwritten).<br/>
        /// If the file does not exist it will create an empty configuration file that can be used to save to.
        /// </remarks>
        /// <param name="configurationFile">The path to which a configuration file is intented to be saved to.</param>
        /// <returns><see langword="true"/> if the application can save to the target configuration file. Otherwise <see langword="false"/>.</returns>
        bool EnsureCanSaveConfigurationFile(string configurationFile);

        /// <summary>
        /// Prompts the user to save any unsaved changes.
        /// </summary>
        /// <returns>
        /// <see langword="false"/> if the user choose not to save changes or the save operation failed. Otherwise <see langword="true"/>.
        /// </returns>
        bool PromptSaveChangesIfDirtyAndContinue();

        /// <summary>
        /// Raises the <see cref="SelectedElementChanged"/> event.
        /// </summary>
        /// <param name="element">The newly selected <see cref="ElementViewModel"/> instance.</param>
        void OnSelectedElementChanged(ElementViewModel element);

        /// <summary>
        /// Occurs when the selected <see cref="ElementViewModel"/> changed.
        /// </summary>
        event EventHandler<SelectedElementChangedEventHandlerEventArgs> SelectedElementChanged;
    }

    /// <summary>
    /// Provides data for the <see cref="IApplicationModel.SelectedElementChanged"/> event.
    /// </summary>
    public class SelectedElementChangedEventHandlerEventArgs : EventArgs
    {
        private readonly ElementViewModel selectedElement;

        /// <summary>
        /// Creates a new instance of the <see cref="SelectedElementChangedEventHandlerEventArgs"/> class.
        /// </summary>
        /// <param name="selectedElement">The newly selected <see cref="ElementViewModel"/> instance.</param>
        public SelectedElementChangedEventHandlerEventArgs(ElementViewModel selectedElement)
        {
            this.selectedElement = selectedElement;
        }

        /// <summary>
        /// Gets the newly selected <see cref="ElementViewModel"/> instance.
        /// </summary>
        /// <value>
        /// The newly selected <see cref="ElementViewModel"/> instance.
        /// </value>
        public ElementViewModel SelectedElement
        {
            get { return selectedElement; }
        }
    }
}
