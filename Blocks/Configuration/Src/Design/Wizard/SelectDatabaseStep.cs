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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard
{
    /// <summary>
    /// The <see cref="SelectDatabaseStep"/> collections information for and configures the current <see cref="ConfigurationSourceModel"/> for 
    /// a selected database.
    /// </summary>
    public class SelectDatabaseStep : ConfigurationWizardStep
    {
        const string propertyConnectionString = "ConnectionString";
        const string propertyName = "Name";
        const string propertyProviderName = "ProviderName";

        private readonly ConfigurationSourceModel sourceModel;
        private readonly SelectDatabaseData wizardData = new SelectDatabaseData();

        ///<summary>
        /// Initializes a new instance of <see cref="SelectDatabaseStep"/>.
        ///</summary>
        ///<param name="serviceProvider">The service provider to use for service location.</param>
        ///<param name="sourceModel">The <see cref="ConfigurationSourceModel"/> to update on <see cref="WizardStep.Execute"/></param>
        ///<param name="validatorFactory">The factory creating new <see cref="Validator"/> instances.</param>
        ///<param name="elementLookup">The service for locating registered elements.</param>
        public SelectDatabaseStep(IServiceProvider serviceProvider,
                                ConfigurationSourceModel sourceModel,
                                IResolver<Validator> validatorFactory,
                                ElementLookup elementLookup
                                )
            : base(serviceProvider, validatorFactory, elementLookup)
        {
            this.sourceModel = sourceModel;

            var name = AddReflectedProperty(wizardData, propertyName);

            PropertyList.Add(new AssociatedWizardProperty(serviceProvider,
                                                          wizardData,
                                                          TypeDescriptor.GetProperties(wizardData)[propertyConnectionString],
                                                          validatorFactory,
                                                          elementLookup,
                                                          name));

            
            PropertyList.Add(new AssociatedWizardProperty(serviceProvider,
                                                          wizardData,
                                                          TypeDescriptor.GetProperties(wizardData)[propertyProviderName],
                                                          validatorFactory,
                                                          elementLookup,
                                                          name));

            SetDefaultDatabase(elementLookup);
        }

        private void SetDefaultDatabase(ElementLookup elementLookUp)
        {
            var element = elementLookUp.FindInstancesOfConfigurationType(typeof(DatabaseSettings)).FirstOrDefault();

            if (element == null)
            {
                return;
            }

            var elementProperty = element.Properties.FirstOrDefault(p => p.PropertyName == "DefaultDatabase");

            if (elementProperty == null)
            {
                return;
            }

            if (elementProperty.Value != null)
            {
                Name.Value = elementProperty.Value;
            }
        }

        /// <summary>
        /// Gets the title of the wizard.
        /// </summary>
        public override string Title
        {
            get { return Resources.WizardSelectDatabaseStepTitle; }
        }

        /// <summary>
        /// Gets step instructions to display to the user.
        /// </summary>
        public override string Instruction
        {
            get { return Resources.WizardSelectDatabaseStepInstructions; }
        }
        
        /// <summary>
        /// Gets the database connection string <see cref="Property"/>.
        /// </summary>
        public Property ConnectionString
        {
            get { return PropertyList[propertyConnectionString]; }
        }

        /// <summary>
        /// Gets the database name <see cref="Property"/>.
        /// </summary>
        public Property Name
        {
            get { return PropertyList[propertyName]; }
        }

        /// <summary>
        /// Gets the database provider <see cref="Property"/>.
        /// </summary>
        public Property DatabaseProvider
        {
            get { return PropertyList[propertyProviderName]; }
        }

        /// <summary>
        /// Invoked when the wizard should apply its changes.
        /// </summary>
        public override void Execute()
        {
            new DatabaseSectionBuilder(sourceModel, wizardData).Build();
        }

        private class DatabaseSectionBuilder : ConfigurationBuilder
        {
            private readonly SelectDatabaseData wizardData;

            public DatabaseSectionBuilder(ConfigurationSourceModel sourceModel, SelectDatabaseData wizardData)
                : base(sourceModel)
            {
                this.wizardData = wizardData;
            }

            public override void Build()
            {
                var connectionSection = EnsureConnectionStringsSection();
                AddConnection(connectionSection);
            }

            private void AddConnection(SectionViewModel connectionSection)
            {
                var connection = connectionSection.DescendentConfigurationsOfType<ConnectionStringSettings>()
                                    .Where(s => (string)s.Property("Name").Value == wizardData.Name).FirstOrDefault();

                if (connection == null)
                {
                    var connectionStrings = GetCollectionOfType<ConnectionStringSettingsCollection>(connectionSection);
                    connection = connectionStrings.AddNewCollectionElement(typeof(ConnectionStringSettings));
                    connection.Property("Name").Value = wizardData.Name;
                    connection.Property("ConnectionString").Value = wizardData.ConnectionString;
                    connection.Property("ProviderName").Value = wizardData.ProviderName;
                }
            }

            private SectionViewModel EnsureConnectionStringsSection()
            {
                var connectionStringsSection = GetSectionOfType<ConnectionStringsSection>();
                if (connectionStringsSection == null)
                {
                    connectionStringsSection = SourceModel.AddSection("connectionStrings", new ConnectionStringsSection());
                }

                return connectionStringsSection;
            }
        }

        internal class SelectDatabaseData
        {
            [Validation(typeof(RequiredFieldValidator))]
            [ResourceDisplayName(ResourceName="SelectDatabaseStepConnectionStringDisplayName", ResourceType=typeof(Resources))]
            public string ConnectionString { get; set; }

            [Validation(typeof(RequiredFieldValidator))]
            [Reference(typeof(ConnectionStringSettingsCollection), typeof(ConnectionStringSettings))]
            [ResourceDisplayName(ResourceName = "SelectDatabaseStepConnectionNameDisplayName", ResourceType = typeof(Resources))]
            public string Name { get; set; }

            [Validation(typeof(RequiredFieldValidator))]
            [TypeConverter(typeof(SystemDataProviderConverter))]
            [ResourceDisplayName(ResourceName = "SelectDatabaseStepConnectionProviderNameDisplayName", ResourceType = typeof(Resources))]
            public string ProviderName { get; set; }
        }
    }
}
