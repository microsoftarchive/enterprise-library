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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary/>
    public static class DataAccessDesignTime
    {
        /// <summary/>
        public const string ConnectionStringSettingsSectionName = "connectionStrings";

        /// <summary/>
        public static class ConverterTypeNames
        {
            /// <summary/>
            public const string SystemDataConverter = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters.SystemDataProviderConverter, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        ///<summary/>
        public static class ViewModelTypeNames
        {
            /// <summary/>
            public const string DataSectionViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.DataSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string OraclePackageDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.OraclePackageDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddDataAccessBlockCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddDatabaseBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        /// <summary/>
        public static class MetadataTypes
        {

            /// <summary/>
            [ViewModel(ViewModelTypeNames.DataSectionViewModel)]
            [ResourceDisplayName(typeof(DesignResources), "ConnectionStringsSectionMetadataDisplayName")]
            [ResourceDescription(typeof(DesignResources), "ConnectionStringsSectionMetadataDescription")]
            public abstract class ConnectionStringsSectionMetadata
            {

            }

            /// <summary/>
            [ResourceDisplayName(typeof(DesignResources), "ConnectionStringSettingsCollectionMetadataDisplayName")]
            [ResourceDescription(typeof(DesignResources), "ConnectionStringSettingsCollectionMetadataDescription")]
            public abstract class ConnectionStringSettingsCollectionMetadata
            {
            }

            /// <summary/>
            [NameProperty("Name")]
            [ResourceDisplayName(typeof(DesignResources), "ConnectionStringSettingsMetadataDisplayName")]
            [ResourceDescription(typeof(DesignResources), "ConnectionStringSettingsMetadataDescription")]
            public abstract class ConnectionStringSettingsMetadata
            {
                /// <summary/>
                [Category("(name)")]
                [ResourceDisplayName(typeof(DesignResources), "ConnectionStringSettingsMetadataNameDisplayName")]
                [ResourceDescription(typeof(DesignResources), "ConnectionStringSettingsMetadataNameDescription")]
                [EnvironmentalOverrides(false)]
                public string Name
                {
                    get;
                    set;
                }

                /// <summary/>
                [DisplayName("Connection String")]
                [ResourceDisplayName(typeof(DesignResources), "ConnectionStringSettingsMetadataConnectionStringDisplayName")]
                [ResourceDescription(typeof(DesignResources), "ConnectionStringSettingsMetadataConnectionStringDescription")]
                //[Editor(EditorTypes.ConnectionStringEditor, EditorTypes.UITypeEditor)]
                public string ConnectionString
                {
                    get;
                    set;
                }

                /// <summary/>
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
