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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
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
