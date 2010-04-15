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
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class DataSectionViewModel : SectionViewModel
    {
        SubordinateSectionViewModel dataSettingsViewModel;
        SubordinateSectionViewModel oracleSettingsViewModel;
        IUnityContainer builder;
        private readonly ElementLookup elementLookup;

        public DataSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
            this.builder = builder;
            elementLookup = builder.Resolve<ElementLookup>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public override void Initialize(InitializeContext context)
        {
            Guard.ArgumentNotNull(context, "context");

            if (context.WasLoadedFromSource)
            {
                InitializeSubordinateSectionViewModels(
                    (DatabaseSettings)context.LoadSource.GetLocalSection(DatabaseSettings.SectionName),
                    (OracleConnectionSettings)context.LoadSource.GetLocalSection(OracleConnectionSettings.SectionName));
            }
            else
            {
                InitializeSubordinateSectionViewModels(null, null);
            }

        }

        public override void Save(IDesignConfigurationSource configurationSource)
        {
            ProtectionProviderProperty.TypedValue = dataSettingsViewModel.ProtectionProviderProperty.TypedValue;
            oracleSettingsViewModel.ProtectionProviderProperty.TypedValue = dataSettingsViewModel.ProtectionProviderProperty.TypedValue;

            RequirePermissionProperty.TypedValue = dataSettingsViewModel.RequirePermissionProperty.TypedValue;
            oracleSettingsViewModel.RequirePermissionProperty.TypedValue = dataSettingsViewModel.RequirePermissionProperty.TypedValue;

            base.Save(configurationSource);

            if (!OracleSettingsAreEmpty((OracleConnectionSettings)oracleSettingsViewModel.ConfigurationElement))
            {
                oracleSettingsViewModel.Save(configurationSource);
            }

            if (!DaabSettingsAreEmpty((DatabaseSettings)dataSettingsViewModel.ConfigurationElement))
            {
                dataSettingsViewModel.Save(configurationSource);
            }
        }

        private void InitializeSubordinateSectionViewModels(DatabaseSettings databaseSettings, OracleConnectionSettings oracleSettings)
        {
            if (databaseSettings == null) databaseSettings = new DatabaseSettings();
            dataSettingsViewModel = CreateSubordinateModel(DatabaseSettings.SectionName, databaseSettings);
            elementLookup.AddSection(dataSettingsViewModel);

            if (oracleSettings == null) oracleSettings = new OracleConnectionSettings();
            oracleSettingsViewModel = CreateSubordinateModel(OracleConnectionSettings.SectionName, oracleSettings);
            elementLookup.AddSection(oracleSettingsViewModel);
        }

        private SubordinateSectionViewModel CreateSubordinateModel(string sectionName, ConfigurationSection section)
        {
            var model = CreateViewModelInstance<SubordinateSectionViewModel>(
                            builder,
                            Enumerable.Empty<Attribute>(),
                            new DependencyOverride<SectionViewModel>(this),
                            new DependencyOverride<string>(sectionName),
                            new DependencyOverride<ConfigurationSection>(section)
                            );

            model.Initialize(new InitializeContext());

            return model;
        }

        public override void Delete()
        {
            elementLookup.RemoveSection(dataSettingsViewModel);
            elementLookup.RemoveSection(oracleSettingsViewModel);

            oracleSettingsViewModel.Delete();
            dataSettingsViewModel.Delete();

            base.Delete();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                if (oracleSettingsViewModel != null)
                {
                    oracleSettingsViewModel.Dispose();
                    oracleSettingsViewModel = null;
                }

                if (dataSettingsViewModel != null)
                {
                    dataSettingsViewModel.Dispose();
                    dataSettingsViewModel = null;
                }
            }

            base.Dispose(disposing);
        }

        protected override IEnumerable<Property> GetAllProperties()
        {
            var dataAccessSettingProperties = this.dataSettingsViewModel.Properties;
            yield return dataAccessSettingProperties.OfType<ProtectionProviderProperty>().Single();
            yield return dataAccessSettingProperties.OfType<RequirePermissionProperty>().Single();

            var defaultDatabaseProperty = TypeDescriptor.GetProperties(typeof(DatabaseSettings)).Cast<PropertyDescriptor>().Where(x => x.Name == "DefaultDatabase").Single();
            yield return CreateProperty<ConnectionStringSettingsDefaultDatabaseElementProperty>(
                new DependencyOverride<ElementViewModel>(dataSettingsViewModel),
                new DependencyOverride<PropertyDescriptor>(defaultDatabaseProperty));
        }

        protected override object CreateBindable()
        {
            return new HorizontalListLayout(
                       new HeaderedListLayout(this.DescendentElements().Where(x => x.ConfigurationType == typeof(ConnectionStringSettingsCollection)).First()),
                       new HeaderedListLayout(this.oracleSettingsViewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(NamedElementCollection<OracleConnectionData>)).First()),
                       new HeaderedListLayout(this.dataSettingsViewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(NamedElementCollection<DbProviderMapping>)).First())
                );
        }

        private static bool DaabSettingsAreEmpty(DatabaseSettings databaseSettings)
        {
            if (!String.IsNullOrEmpty(databaseSettings.DefaultDatabase)) return false;
            if (databaseSettings.ProviderMappings.Count > 0) return false;

            return true;

        }

        private static bool OracleSettingsAreEmpty(OracleConnectionSettings oracleConnectionSettings)
        {
            if (oracleConnectionSettings.OracleConnectionsData.Count > 0) return false;

            return true;
        }

        private class SubordinateSectionViewModel : SectionViewModel
        {
            SectionViewModel containingSection;

            public SubordinateSectionViewModel(SectionViewModel containingSection, IUnityContainer builder, string sectionName, ConfigurationSection section)
                : base(builder, sectionName, section)
            {
                this.containingSection = containingSection;
            }

            public IEnumerable<Property> GetProperties()
            {
                return base.GetAllProperties();
            }

            public override void ExpandSection()
            {
                base.ExpandSection();
                containingSection.ExpandSection();
            }
        }
    }

#pragma warning restore 1591
}
