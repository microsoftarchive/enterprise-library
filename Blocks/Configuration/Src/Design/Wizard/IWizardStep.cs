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

using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// The <see cref="IWizardStep"/> is a single step in a <see cref="WizardModel"/> sequence.
    /// </summary>
    public interface IWizardStep : INotifyPropertyChanged
    {
        /// <summary>
        /// Returns <see langword="true"/> if the step is valid.
        /// Otherwise, returns <see langword="false"/>.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Gets the title of the wizard.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the instructions for this wizard step.
        /// </summary>
        string Instruction { get; }

        /// <summary>
        /// Invoked when the wizard should apply its changes.
        /// </summary>
        void Execute();
    }
}
