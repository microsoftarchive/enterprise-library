//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// This class supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public static class DataAccessDesignTime
    {
        /// <summary>
        /// Name of the connection strings settings configuration section.
        /// </summary>
        public const string ConnectionStringSettingsSectionName = "connectionStrings";

        internal static class ConverterTypeNames
        {
            public const string SystemDataConverter = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters.SystemDataProviderConverter, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        internal static class ViewModelTypeNames
        {
            public const string ConnectionStringPropertyViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.ConnectionStringPropertyViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
            
            public const string DataSectionViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.DataSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string OraclePackageDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.OraclePackageDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
            
        }

        internal static class CommandTypeNames
        {
            public const string AddDataAccessBlockCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddDatabaseBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        /// <summary>
        /// This class supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public static class MetadataTypes
        {
            /// <summary>
            /// This class supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
            /// </summary>
            [RegisterAsMetadataType(typeof(DbProviderMapping))]
            public abstract class DbProviderMappingMetadata
            {
                /// <summary>
                /// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
                /// </summary>
                [TypeConverter(ConverterTypeNames.SystemDataConverter)]
                public string Name
                {
                    get;
                    set;
                }
            }

            /// <summary>
            /// This class supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
            /// </summary>
            [ViewModel(ViewModelTypeNames.DataSectionViewModel)]
            [ResourceDisplayName(typeof(DesignResources), "ConnectionStringsSectionMetadataDisplayName")]
            [ResourceDescription(typeof(DesignResources), "ConnectionStringsSectionMetadataDescription")]
            [RegisterAsMetadataType(typeof(ConnectionStringsSection))]
            public abstract class ConnectionStringsSectionMetadata
            {

            }

            /// <summary>
            /// This class supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
            /// </summary>
            [ResourceDisplayName(typeof(DesignResources), "ConnectionStringSettingsCollectionMetadataDisplayName")]
            [ResourceDescription(typeof(DesignResources), "ConnectionStringSettingsCollectionMetadataDescription")]
            [RegisterAsMetadataType(typeof(ConnectionStringSettingsCollection))]
            public abstract class ConnectionStringSettingsCollectionMetadata
            {
            }

            /// <summary>
            /// This class supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
            /// </summary>
            [NameProperty("Name")]
            [ResourceDisplayName(typeof(DesignResources), "ConnectionStringSettingsMetadataDisplayName")]
            [ResourceDescription(typeof(DesignResources), "ConnectionStringSettingsMetadataDescription")]
            [RegisterAsMetadataType(typeof(ConnectionStringSettings))]
            public abstract class ConnectionStringSettingsMetadata
            {

                /// <summary>
                /// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
                /// </summary>
                [ResourceCategory(typeof(DesignResources), "CategoryName")]
                [ResourceDisplayName(typeof(DesignResources), "ConnectionStringSettingsMetadataNameDisplayName")]
                [ResourceDescription(typeof(DesignResources), "ConnectionStringSettingsMetadataNameDescription")]
                [EnvironmentalOverrides(false)]
                public string Name
                {
                    get;
                    set;
                }


                /// <summary>
                /// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
                /// </summary>
                [ResourceDisplayName(typeof(DesignResources), "ConnectionStringSettingsMetadataConnectionStringDisplayName")]
                [ResourceDescription(typeof(DesignResources), "ConnectionStringSettingsMetadataConnectionStringDescription")]
                [Editor(CommonDesignTime.EditorTypes.PopupTextEditor, CommonDesignTime.EditorTypes.UITypeEditor)]
                [EditorWithReadOnlyText(true)]
                [ViewModel(DataAccessDesignTime.ViewModelTypeNames.ConnectionStringPropertyViewModel)]
                public string ConnectionString
                {
                    get;
                    set;
                }


                /// <summary>
                /// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
                /// </summary>
                [ResourceDisplayName(typeof(DesignResources), "ConnectionStringSettingsMetadataProviderNameDisplayName")]
                [ResourceDescription(typeof(DesignResources), "ConnectionStringSettingsMetadataProviderNameDescription")]
                [TypeConverter(ConverterTypeNames.SystemDataConverter)]
                public string ProviderName
                {
                    get;
                    set;
                }
            }
        }
    }

}
