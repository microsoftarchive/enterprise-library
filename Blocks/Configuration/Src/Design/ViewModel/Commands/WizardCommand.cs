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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands
{
    /// <summary>
    /// Initiates a <see cref="WizardModel"/>, usually from a user action.
    /// </summary>
    public class WizardCommand : CommandModel
    {
        private readonly IResolver<WizardModel> resolver;
        private Type attributedClassType;

        /// <summary>
        /// Initializes a new instance of the WizardCommand.
        /// </summary>
        /// <param name="attribute">The <see cref="WizardCommandAttribute"/> used to define this command.</param>
        /// <param name="uiService">The service to use when displaying dialogs.</param>
        /// <param name="resolver">The service to use to get instances of new <see cref="WizardModel"/> based on <paramref name="attribute"/></param>
        public WizardCommand(WizardCommandAttribute attribute
            , IUIServiceWpf uiService
            , IResolver<WizardModel> resolver)
            : base(attribute, uiService)
        {
            this.resolver = resolver;
            this.attributedClassType = attribute.WizardType;

            if(!typeof(WizardModel).IsAssignableFrom(attributedClassType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidWizardTypeMessage, attributedClassType.Name));
            }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Not used</param>
        protected override bool InnerCanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Not used.  May be null.</param>
        /// <remarks>
        /// Execution of wizard command creates a new instance of the <see cref="WizardCommand"/> and attaches it
        /// to a <see cref="WizardView"/>.
        /// </remarks>
        protected override void InnerExecute(object parameter)
        {
            var wizard = new WizardView();
            var wizardModel = resolver.Resolve(this.attributedClassType);
            wizard.SetWizard(wizardModel);
            UIService.ShowDialog(wizard);
        }

        /// <summary>
        /// The type of the Wizard.
        /// </summary>
        public Type WizardType { get { return this.attributedClassType; } }
    }
}
