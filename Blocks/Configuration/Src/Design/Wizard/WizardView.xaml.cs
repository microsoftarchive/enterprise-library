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

using System.Windows;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// The <see cref="WizardView"/> provides the layout and user-interface for a <see cref="WizardModel"/>.
    /// </summary>
    public partial class WizardView : Window
    {
        private WizardModel wizardModel;

        /// <summary>
        /// Intantiates a new instance of <see cref="WizardView"/>
        /// </summary>
        public WizardView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the <see cref="WizardModel"/> for this view instance.
        /// </summary>
        /// <param name="wizard">The <see cref="WizardModel"/> to display in <see cref="WizardView"/>.</param>
        /// <remarks>
        /// Setting the <see cref="WizardModel"/> sets the model as <see cref="FrameworkElement.DataContext"/> for this view and
        /// adds an action to the <see cref="WizardModel.OnCloseAction"/></remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public void SetWizard(WizardModel wizard)
        {
            Guard.ArgumentNotNull(wizard, "wizard");

            if (this.wizardModel != null)
            {
                this.wizardModel.OnCloseAction = null;
            }

            this.wizardModel = wizard;
            this.DataContext = wizardModel;
            wizard.OnCloseAction = () => this.Close();
        }
    }
}
