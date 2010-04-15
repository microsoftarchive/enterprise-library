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
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;


namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard
{

    /// <summary>
    /// The <see cref="PickExceptionStep"/> wizard step collects data for and configures the exception policy as part of the 
    /// <see cref="LogExceptionsToDatabase"/> wizard.
    /// </summary>
    public class PickExceptionStep : ConfigurationWizardStep
    {
        const string propertyExceptionType = "ExceptionType";
        const string propertyPolicy = "Policy";

        private readonly ConfigurationSourceModel sourceModel;
        private readonly PickExceptionTypeData wizardData = new PickExceptionTypeData();


        /// <summary>
        /// Initializes an instance of <see cref="PickExceptionStep"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="sourceModel"></param>
        /// <param name="validatorFactory"></param>
        /// <param name="elementLookup"></param>
        public PickExceptionStep(IServiceProvider serviceProvider,
                                ConfigurationSourceModel sourceModel,
                                IResolver<Validator> validatorFactory,
                                ElementLookup elementLookup
                                ) : base(serviceProvider, validatorFactory, elementLookup)
        {
            this.sourceModel = sourceModel;

            AddReflectedProperty(wizardData, propertyExceptionType);
            AddReflectedProperty(wizardData, propertyPolicy);
        }

        /// <summary>
        /// Returns true if all properties are valid.
        /// </summary>
        public override bool IsValid
        {
            get { return PropertyList.All(x => x.IsValid); }
        }

        /// <summary>
        /// Gets the title of the wizard.
        /// </summary>
        public override string Title
        {
            get { return Resources.WizardPickExceptionTypeStepTitle; }
        }

        /// <summary>
        /// Gets step instructions to display to the user.
        /// </summary>
        public override string Instruction
        {
            get{ return Resources.WizardPickExceptionTypeInstructions; }
        }

        /// <summary>
        /// Gets the exception type <see cref="Property"/> to be configured as part of this step.
        /// </summary>
        public Property ExceptionType
        {
            get { return PropertyList[propertyExceptionType];} 
        }


        /// <summary>
        /// Gets the policy <see cref="Property"/> to be configured as part of this step.
        /// </summary>
        public Property Policy
        {
            get { return PropertyList[propertyPolicy]; }
        }

        /// <summary>
        /// Gets or sets the database reference to log exceptions to.
        /// </summary>
        public string ReferencedDatabaseName
        {
            get { return wizardData.DatabaseName; }
            set { wizardData.DatabaseName = value; }
        }

        /// <summary>
        /// Invoked when the wizard should apply its changes.
        /// </summary>
        public override void Execute()
        {
            new LoggingSectionBuilder(sourceModel, wizardData).Build();
            new ExceptionHandlingBuilder(sourceModel, wizardData).Build();
        }

        internal PickExceptionTypeData WizardData
        {
            get { return wizardData; }
        }

        internal class PickExceptionTypeData
        {
            [EditorAttribute(typeof(TypeSelectionEditor), typeof(UITypeEditor))]
            [BaseType(typeof(Exception), TypeSelectorIncludes.AbstractTypes | TypeSelectorIncludes.BaseType)]
            [Validation(typeof(RequiredFieldValidator))]
            [ResourceDisplayName(ResourceName = "PickExceptionTypeStepExceptionTypeDisplayName", ResourceType = typeof(Resources))]
            [EditorWithReadOnlyText(true)]
            public string ExceptionType { get; set; }

            [Validation(typeof(RequiredFieldValidator))]
            [Reference(typeof(ExceptionHandlingSettings), typeof(ExceptionPolicyData))]
            [ResourceDisplayName(ResourceName = "PickExceptionTypeStepPolicyDisplayName", ResourceType = typeof(Resources))]
            public string Policy { get; set; }

            public string DatabaseName { get; set; }

            public string LogCategory { get; set; }
        }

        private class LoggingSectionBuilder : ConfigurationBuilder
        {
            private PickExceptionTypeData wizardData;
            private SectionViewModel loggingSection;
            private bool loggingSectionIsNew;
            private ElementViewModel formatter;

            public LoggingSectionBuilder(ConfigurationSourceModel sourceModel, PickExceptionTypeData wizardData)
                : base(sourceModel)
            {
                this.wizardData = wizardData;
            }

            public override void Build()
            {
                EnsureLoggingSection();
                EnsureLogFormatter();
                CreateNewPolicy();

            }

            private void EnsureLoggingSection()
            {
                loggingSection = GetSectionOfType<LoggingSettings>();
                if (loggingSection == null)
                {
                    loggingSectionIsNew = true;
                    loggingSection = SourceModel.AddSection(LoggingSettings.SectionName, new LoggingSettings());
                }
            }

            private void EnsureLogFormatter()
            {
                formatter = loggingSection.DescendentConfigurationsOfType<FormatterData>().FirstOrDefault();
                if (formatter == null)
                {
                    var formatters = GetCollectionOfType<NamedElementCollection<FormatterData>>(loggingSection);
                    formatter = formatters.AddNewCollectionElement(typeof(TextFormatterData));
                    formatter.Property("Template").Value = TextFormatterData.DefaultTemplate;
                }
            }

            private void CreateNewPolicy()
            {
                var categories = GetCollectionOfType<NamedElementCollection<TraceSourceData>>(loggingSection);
                var category = categories.AddNewCollectionElement(typeof(TraceSourceData));
                wizardData.LogCategory = (string)category.Property("Name").Value;

                if (loggingSectionIsNew)
                {
                    loggingSection.Property("DefaultCategory").Value = category.Property("Name").Value;
                }

                var listenerReferences = GetCollectionOfType<NamedElementCollection<TraceListenerReferenceData>>(category);
                var listenerReference = listenerReferences.AddNewCollectionElement(typeof(TraceListenerReferenceData));

                var listeners = GetCollectionOfType<NamedElementCollection<TraceListenerData>>(loggingSection);
                var databaseListener = listeners.AddNewCollectionElement(typeof(FormattedDatabaseTraceListenerData));
                databaseListener.Property("DatabaseInstanceName").Value = wizardData.DatabaseName;
                databaseListener.Property("Formatter").Value = formatter.Property("Name").Value;

                listenerReference.Property("Name").Value = databaseListener.Property("Name").Value;
            }

           
        }
        private class ExceptionHandlingBuilder : ConfigurationBuilder
        {
            private PickExceptionTypeData wizardData;
            private SectionViewModel exceptionSettingsSection;
            private ElementViewModel exceptionType;
            private ElementViewModel policyModel;

            public ExceptionHandlingBuilder(ConfigurationSourceModel sourceModel, PickExceptionTypeData wizardData)
                : base(sourceModel)
            {
                this.wizardData = wizardData;
            }

            public override void Build()
            {
                EnsureExceptionHandlingSection();
                EnsurePolicy();
                EnsureExceptionType();
                AddLoggingHandler();
                

                exceptionSettingsSection.IsExpanded = true;
                policyModel.Select();
            }

         

            private void AddLoggingHandler()
            {
                var handlersCollection = GetCollectionOfType<NamedElementCollection<ExceptionHandlerData>>(exceptionType);

                var loggingHandler = handlersCollection.AddNewCollectionElement(typeof(LoggingExceptionHandlerData));
                loggingHandler.Property("LogCategory").Value = wizardData.LogCategory;               
            }

            private void EnsureExceptionType()
            {
                var exceptionTypes = GetCollectionOfType<NamedElementCollection<ExceptionTypeData>>(policyModel);

                exceptionType = exceptionTypes.DescendentConfigurationsOfType<ExceptionTypeData>()
                    .Where(x => (string)x.Property("TypeName").Value == wizardData.ExceptionType)
                    .FirstOrDefault();

                if (exceptionType == null)
                {
                    exceptionType = exceptionTypes.AddNewCollectionElement(typeof(ExceptionTypeData));
                    exceptionType.Property("Name").Value = TypeNameParserHelper.ParseTypeName(wizardData.ExceptionType);
                    exceptionType.Property("TypeName").Value = wizardData.ExceptionType;
                }
            }

            private void EnsurePolicy()
            {
                policyModel = exceptionSettingsSection.DescendentConfigurationsOfType<ExceptionPolicyData>()
                                    .Where(x => (string)x.Property("Name").Value == wizardData.Policy).FirstOrDefault();

                if (policyModel == null)
                {
                    var policies = GetCollectionOfType<NamedElementCollection<ExceptionPolicyData>>(exceptionSettingsSection);
                    policyModel = policies.AddNewCollectionElement(typeof(ExceptionPolicyData));
                    policyModel.Property("Name").Value = wizardData.Policy;
                }
            }

            private void EnsureExceptionHandlingSection()
            {
                exceptionSettingsSection = GetSectionOfType<ExceptionHandlingSettings>();

                if (exceptionSettingsSection == null)
                {
                    var section = new ExceptionHandlingSettings();
                    exceptionSettingsSection = SourceModel.AddSection(ExceptionHandlingSettings.SectionName, section);
                }
            }
        }
    }
}
