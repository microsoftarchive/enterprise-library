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
    /// A single step in a <see cref="WizardModel"/> sequence.
    /// </summary>
    /// <remarks>
    /// A <see cref="WizardStep"/> is reponsible prior to execution
    /// for the steps validity via the <see cref="IsValid"/> property.
    /// If validity changes, the step should raise the 
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> event as 
    /// this is monitored by <see cref="WizardModel"/> to determine
    /// overall wizard validation.
    /// </remarks>
    /// <seealso cref="WizardView"/>
    /// <seealso cref="WizardModel"/>
    /// <seealso cref="IWizardStep"/>
    public abstract class WizardStep : IWizardStep, INotifyPropertyChanged
    {
        /// <summary>
        /// Returns true if the step is valid, false otherwise.
        /// </summary>
        public abstract bool IsValid { get; }


        /// <summary>
        /// Gets the title of the wizard.
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Gets step instructions to display to the user.
        /// </summary>
        public virtual string Instruction
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Invoked when the wizard should apply its changes.
        /// </summary>
        public virtual void Execute() { }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
