using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{

    ///<summary/>
    public static class AppSettingsDesignTime
    {

        ///<summary/>
        public const string AppSettingsSectionName = "appSettings";

        ///<summary/>
        public static class ViewModelTypeNames
        {
            ///<summary/>
            public const string AppSettingsSectionViewModel = "Console.Wpf.ViewModel.BlockSpecifics.AppSettingsViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        ///<summary/>
        public static class MetadataTypes
        {
            ///<summary/>
            [ViewModel(ViewModelTypeNames.AppSettingsSectionViewModel)]
            [ResourceDisplayName(typeof(DesignResources), "AppSettingsSectionMetadataDisplayName")]
            [ResourceDescription(typeof(DesignResources), "AppSettingsSectionMetadataDescription")]
            public abstract class AppSettingsSectionMetadata
            {

            }

            ///<summary/>
            [ResourceDisplayName(typeof(DesignResources), "KeyValueConfigurationCollectionMetadataDisplayName")]
            [ResourceDescription(typeof(DesignResources), "KeyValueConfigurationCollectionMetadataDescription")]
            public abstract class KeyValueConfigurationCollectionMetadata
            {
            }

            ///<summary/>
            [NameProperty("Key", NamePropertyDisplayFormat = "Setting : '{0}'")]
            [ResourceDisplayName(typeof(DesignResources), "KeyValueConfigurationElementMetadataDisplayName")]
            [ResourceDescription(typeof(DesignResources), "KeyValueConfigurationElementMetadataDescription")]
            public abstract class KeyValueConfigurationElementMetadata
            {
                ///<summary/>
                [ResourceDisplayName(typeof(DesignResources), "KeyValueConfigurationElementMetadataKeyDisplayName")]
                [ResourceDescription(typeof(DesignResources), "KeyValueConfigurationElementMetadataKeyDescription")]
                [EnvironmentalOverridesAttribute(false)]
                [ViewModel(CommonDesignTime.ViewModelTypeNames.ConfigurationPropertyViewModel)]
                public abstract string Key { get; set; }

                ///<summary/>
                [ResourceDisplayName(typeof(DesignResources), "KeyValueConfigurationElementMetadataValueDisplayName")]
                [ResourceDescription(typeof(DesignResources), "KeyValueConfigurationElementMetadataValueDescription")]
                public abstract string Value { get; set; }
            }
        }
    }
}
