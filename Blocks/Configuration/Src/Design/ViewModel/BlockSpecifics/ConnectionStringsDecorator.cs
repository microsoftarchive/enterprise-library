using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Services;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;
using Console.Wpf.ComponentModel.Converters;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class ConnectionStringsDecorator
    {
        public static void DecorateConnectionStringsSection(AnnotationService service)
        {
            service.RegisterSubstituteTypeForMetadata(typeof(ConnectionStringsSection), typeof(DataAccessDesignTime.MetadataTypes.ConnectionStringsSectionMetadata));
            service.RegisterSubstituteTypeForMetadata(typeof(ConnectionStringSettingsCollection), typeof(DataAccessDesignTime.MetadataTypes.ConnectionStringSettingsCollectionMetadata));
            service.RegisterSubstituteTypeForMetadata(typeof(ConnectionStringSettings), typeof(DataAccessDesignTime.MetadataTypes.ConnectionStringSettingsMetadata));
        }

    }
}
