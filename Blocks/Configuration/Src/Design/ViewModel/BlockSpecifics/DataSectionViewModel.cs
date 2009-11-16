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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{



    public class DataSectionViewModel : PositionedSectionViewModel
    {
        SobordinateSectionViewModel dataSettingsViewModel;
        SobordinateSectionViewModel oracleSettingsViewModel;
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

            InitializeGridPosition();
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
            dataSettingsViewModel = new SobordinateSectionViewModel(this, builder, DatabaseSettings.SectionName, databaseSettings);
            elementLookup.AddSection(dataSettingsViewModel);


            if (oracleSettings == null) oracleSettings = new OracleConnectionSettings();
            oracleSettingsViewModel = new SobordinateSectionViewModel(this, builder, OracleConnectionSettings.SectionName, oracleSettings);
            elementLookup.AddSection(oracleSettingsViewModel);
        }

        public override void Delete()
        {
            base.Delete();

            oracleSettingsViewModel.Delete();
            dataSettingsViewModel.Delete();
        }

        private void InitializeGridPosition()
        {
            this.Positioning.PositionCollection("Connection Strings",
                            typeof(ConnectionStringSettingsCollection),
                            typeof(ConnectionStringSettings),
                            new PositioningInstructions { FixedColumn = 0, FixedRow = 0 });


            this.dataSettingsViewModel.Positioning.PositionCollection("Provider Mappings",
                            typeof(NamedElementCollection<DbProviderMapping>),
                            typeof(DbProviderMapping),
                            new PositioningInstructions { FixedColumn = 2, FixedRow = 0 });

            this.oracleSettingsViewModel.Positioning.PositionCollection("Oracle connection Settings",
                            typeof(NamedElementCollection<OracleConnectionData>),
                            typeof(OracleConnectionData),
                            new PositioningInstructions { FixedColumn = 1, FixedRow = 0 });
        }

        public override IEnumerable<ViewModel> GetGridVisuals()
        {
            return this.Positioning.GetGridVisuals().Union(this.dataSettingsViewModel.Positioning.GetGridVisuals()).Union(this.oracleSettingsViewModel.Positioning.GetGridVisuals());
        }

        public override int Columns
        {
            get { return new[] { Positioning.EndColumn, dataSettingsViewModel.Positioning.EndColumn, oracleSettingsViewModel.Positioning.EndColumn }.Max(); }
        }

        public override int Rows
        {
            get { return new[] { Positioning.EndRow, dataSettingsViewModel.Positioning.EndRow, oracleSettingsViewModel.Positioning.EndRow }.Max(); }
        }

        protected override IEnumerable<Property> GetAllProperties()
        {
            return dataSettingsViewModel.GetProperties();
        }

        int lastNumberOfGridVisuals = 0;
        public override void UpdateLayout()
        {
            Positioning.Update(this);
            dataSettingsViewModel.Positioning.Update(dataSettingsViewModel);
            oracleSettingsViewModel.Positioning.Update(oracleSettingsViewModel);

            var numberOfVisuals = GetGridVisuals().Count();

            if (lastNumberOfGridVisuals != numberOfVisuals)
            {
                lastNumberOfGridVisuals = numberOfVisuals;

                OnUpdateVisualGrid();
            }
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

        private class SobordinateSectionViewModel : SectionViewModel
        {
            SectionViewModel containerModel;
            public SobordinateSectionViewModel(SectionViewModel containerModel, IUnityContainer builder, string sectionName, ConfigurationSection section)
                : base(builder, sectionName, section)
            {
                this.containerModel = containerModel;
            }

            public override void UpdateLayout()
            {
                containerModel.UpdateLayout();
            }

            public IEnumerable<Property> GetProperties()
            {
                return base.GetAllProperties();
            }
        }
    }
}
