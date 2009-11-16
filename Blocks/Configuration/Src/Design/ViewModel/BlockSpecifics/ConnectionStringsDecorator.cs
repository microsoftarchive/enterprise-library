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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
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
