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
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{



    public class DataSectionViewModel : SectionViewModel
    {
        SubordinateSectionViewModel dataSettingsViewModel;
        SubordinateSectionViewModel oracleSettingsViewModel;
        IUnityContainer builder;

        public DataSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
            this.builder = builder;
        }


        public override void Initialize(InitializeContext context)
        {
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
            var elementLookup = builder.Resolve<ElementLookup>();

            if (databaseSettings == null) databaseSettings = new DatabaseSettings();
            dataSettingsViewModel = CreateSubordinateModel(DatabaseSettings.SectionName, databaseSettings);
            elementLookup.AddSection(dataSettingsViewModel);

            if (oracleSettings == null) oracleSettings = new OracleConnectionSettings();
            oracleSettingsViewModel = CreateSubordinateModel(OracleConnectionSettings.SectionName, oracleSettings);
            elementLookup.AddSection(oracleSettingsViewModel);
        }

        private SubordinateSectionViewModel CreateSubordinateModel(string sectionName, ConfigurationSection section)
        {
            return CreateViewModelInstance<SubordinateSectionViewModel>(
                builder,
                Enumerable.Empty<Attribute>(),
                new DependencyOverride<SectionViewModel>(this),
                new DependencyOverride<string>(sectionName),
                new DependencyOverride<ConfigurationSection>(section)
                );
        }

        public override void Delete()
        {
            base.Delete();

            oracleSettingsViewModel.Delete();
            dataSettingsViewModel.Delete();
        }

        protected override IEnumerable<Property> GetAllProperties()
        {
            return dataSettingsViewModel.GetProperties();
        }

       
        protected override object CreateBindable()
        {
            return new HorizontalListViewModel(
                       new HeaderedListViewModel(this.DescendentElements().Where(x=>x.ConfigurationType == typeof(ConnectionStringSettingsCollection)).First()),
                       new HeaderedListViewModel(this.oracleSettingsViewModel.DescendentElements().Where(x => x.ConfigurationType == typeof(NamedElementCollection<OracleConnectionData>)).First()),
                       new HeaderedListViewModel(this.dataSettingsViewModel.DescendentElements().Where(x=>x.ConfigurationType == typeof(NamedElementCollection<DbProviderMapping>)).First())
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
            SectionViewModel containerModel;
            public SubordinateSectionViewModel(SectionViewModel containerModel, IUnityContainer builder, string sectionName, ConfigurationSection section)
                : base(builder, sectionName, section)
            {
                this.containerModel = containerModel;
            }

            public IEnumerable<Property> GetProperties()
            {
                return base.GetAllProperties();
            }
        }
    }
}
