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
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Enables interaction with the user interface of the development environment object that is hosting the designer.
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// When implemented by a class, notifies the user interface that an update is occuring.
        /// </summary>
        void BeginUpdate();

        /// <summary>
        /// When implemented by a class, notifies the user interface that an update has ended.
        /// </summary>
        void EndUpdate();

        /// <summary>
        /// When implemented by a class, displays a <see cref="SaveFileDialog"/> in the user interface.
        /// </summary>
        /// <param name="dialog">
        /// The <see cref="SaveFileDialog"/> to display.
        /// </param>
        /// <returns>
        /// One of the <see cref="DialogResult"/> values.
        /// </returns>
        DialogResult ShowSaveDialog(SaveFileDialog dialog);

        /// <summary>
        /// When implemented by a class, displays an <see cref="OpenFileDialog"/> in the user interface.
        /// </summary>
        /// <param name="dialog">
        /// The <see cref="OpenFileDialog"/> to display.
        /// </param>
        /// <returns>
        /// One of the <see cref="DialogResult"/> values.
        /// </returns>
        DialogResult ShowOpenDialog(OpenFileDialog dialog);

        /// <summary>
        /// When implemented by a class, gets the owner window.
        /// </summary>
        /// <value>
        /// The owner window.
        /// </value>
        IWin32Window OwnerWindow { get; }

        /// <summary>
        /// When implemented by a class, avtivates a node.
        /// </summary>
        /// <param name="node">
        /// The <see cref="ConfigurationNode"/> to activate.
        /// </param>
        void ActivateNode(ConfigurationNode node);

        /// <summary>
        /// When implemented by a class, displays the validation errors.
        /// </summary>
        /// <param name="errorLogService">
        /// The errorLogToDisplay.
        /// </param>
        void DisplayErrorLog(IErrorLogService errorLogService);

        /// <summary>
        /// When implemented by a class, sets the hierarchy dirty indicating the UI should indicate a model change.
        /// </summary>
        /// <param name="hierarchy">
		/// The <see cref="IConfigurationUIHierarchy"/> to set dirty in the <see cref="IUIService"/>.
        /// </param>
        void SetUIDirty(IConfigurationUIHierarchy hierarchy);

        /// <summary>
        /// When implemented by a class, determines if the given <paramref name="hierarchy"/> has been modified.
        /// </summary>
		/// <param name="hierarchy">A <see cref="IConfigurationUIHierarchy"/> object.</param>
        /// <returns><see langword="true"/> if the given <paramref name="hierarchy"/> has been modified; otherwise, <see langword="false"/>.</returns>
		bool IsDirty(IConfigurationUIHierarchy hierarchy);

        /// <summary>
        /// When implemented by a class, updates the status text in the user interface.
        /// </summary>
        /// <param name="status">
        /// The status to display.
        /// </param>
        void SetStatus(string status);

        /// <summary>
        /// Clear the errors in the user interface.
        /// </summary>
        void ClearErrorDisplay();

        /// <summary>
        /// When implemented by a class, displays the specified exception and information about the exception.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Exception"/> to display.
        /// </param>
        void ShowError(Exception e);

        /// <summary>
        /// When implemented by a class, displays the specified exception and information about the exception.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Exception"/> to display.
        /// </param>
        /// <param name="message">
        /// A message to display that provides information about the exception
        /// </param>
        void ShowError(Exception e, string message);

        /// <summary>
        /// When implemented by a class, displays the specified exception and information about the exception.
        /// </summary>
        /// <param name="e">
        /// The <see cref="Exception"/> to display.
        /// </param>
        /// <param name="message">
        /// A message to display that provides information about the exception
        /// </param>
        /// <param name="caption">
        /// The caption for the message.
        /// </param>
        void ShowError(Exception e, string message, string caption);

        /// <summary>
        /// When implemented by a class, displays the specified error message.
        /// </summary>
        /// <param name="message">
        /// The error message to display.
        /// </param>
        void ShowError(string message);

        /// <summary>
        /// When implemented by a class, displays the specified error message.
        /// </summary>
        /// <param name="message">
        /// The error message to display.
        /// </param>
        /// <param name="caption">
        /// The caption for the message.
        /// </param>
        void ShowError(string message, string caption);

        /// <summary>
        /// When implemented by a class, displays the specified message.
        /// </summary>
        /// <param name="message">
        /// The message to display.
        /// </param>
        void ShowMessage(string message);

        /// <summary>
        /// When implemented by a class, displays the specified message with the specified caption.
        /// </summary>
        /// <param name="message">
        /// The message to display.
        /// </param>
        /// <param name="caption">
        /// The caption for the message.
        /// </param>
        void ShowMessage(string message, string caption);

        /// <summary>
        /// When implemented by a class, displays the specified message in a message box with the specified caption and buttons to place on the dialog box.
        /// </summary>
        /// <param name="message">
        /// The message to display.
        /// </param>
        /// <param name="caption">
        /// The caption for the message.
        /// </param>
        /// <param name="buttons">
        /// One of the <see cref="MessageBoxButtons"/> values.
        /// </param>
        /// <returns>
        /// One of the <see cref="DialogResult"/> values.
        /// </returns>
        DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons);

        /// <summary>
        /// refreshes the currenlty selected node in the designers propertygrid
        /// </summary>
        void RefreshPropertyGrid();
    }
}
