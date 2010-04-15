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
using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard
{
    /// <summary>
    /// The <see cref="LogExceptionsToDatabase"/> wizard collections and updates 
    /// the current <see cref="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.ConfigurationSourceModel"/>
    /// to log exceptions to a database.
    /// </summary>
    /// <seealso cref="LogExceptionsToDatabaseCommand"/>
    /// <seealso cref="WizardModel"/>
    public class LogExceptionsToDatabase : WizardModel
    {
        private static readonly Uri ResourcesUri =
            new Uri(String.Format(CultureInfo.InvariantCulture, "/{0};component/Wizard/WizardDataTemplates.xaml",
                                  typeof(LogExceptionsToDatabase).Assembly.GetName().Name), UriKind.Relative);

        /// <summary>
        /// Initializes a new instance of <see cref="LogExceptionsToDatabaseCommand"/>
        /// </summary>
        /// <remarks>
        /// Resources needed by this wizard are added to the <see cref="GlobalResources"/> extended dictionary.</remarks>
        /// <param name="uiService">The user-interface service to use.</param>
        /// <param name="wizardSteps">The steps for this wizard that are added to <see cref="WizardModel"/></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        protected LogExceptionsToDatabase(IUIServiceWpf uiService, params IWizardStep[] wizardSteps)
            : base(uiService)
        {
            Guard.ArgumentNotNull(wizardSteps, "wizardSteps");

            foreach (var step in wizardSteps)
            {
                AddStep(step);
            }

            var dictionary = (ResourceDictionary)Application.LoadComponent(ResourcesUri);
            GlobalResources.AddExtendedDictionary(dictionary);
        }

        /// <summary>
        /// Initializes a new instances of <see cref="LogExceptionsToDatabase"/>.
        /// </summary>
        /// <param name="uiService">The user-interface service to use.</param>
        /// <param name="exceptionStep">The <see cref="PickExceptionStep"/> step.</param>
        /// <param name="databaseStep">The <see cref="SelectDatabaseStep"/> step.</param>
        [InjectionConstructor]
        public LogExceptionsToDatabase(
            IUIServiceWpf uiService,
            PickExceptionStep exceptionStep,
            SelectDatabaseStep databaseStep)
            : this(uiService, new IWizardStep[] { exceptionStep, databaseStep })
        {
        }

        /// <summary>
        /// Gets the wizard title.
        /// </summary>
        protected override string GetWizardTitle()
        {
            return Resources.WizardLogExceptionsToDatabaseTitle;
        }

        /// <summary>
        /// Executes each <see cref="IWizardStep"/> in the wizard to log exceptions to a database based
        /// on the values collected from the user.
        /// </summary>
        protected override void Execute()
        {
            var exceptionStep = Steps.OfType<PickExceptionStep>().First();
            var databaseStep = Steps.OfType<SelectDatabaseStep>().First();
            exceptionStep.ReferencedDatabaseName = databaseStep.Name.Value.ToString();

            foreach (var step in Steps)
            {
                step.Execute();
            }
        }
    }
}
