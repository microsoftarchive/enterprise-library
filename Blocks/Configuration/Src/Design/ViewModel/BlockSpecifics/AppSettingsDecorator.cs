using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Services;
using System.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class AppSettingsDecorator
    {
        public static void DecorateAppSettingsSection(AnnotationService service)
        {
            service.RegisterSubstituteTypeForMetadata(typeof(AppSettingsSection), typeof(AppSettingsDesignTime.MetadataTypes.AppSettingsSectionMetadata));
            service.RegisterSubstituteTypeForMetadata(typeof(KeyValueConfigurationElement), typeof(AppSettingsDesignTime.MetadataTypes.KeyValueConfigurationElementMetadata));
            service.RegisterSubstituteTypeForMetadata(typeof(KeyValueConfigurationCollection), typeof(AppSettingsDesignTime.MetadataTypes.KeyValueConfigurationCollectionMetadata));
        }

    }
}
