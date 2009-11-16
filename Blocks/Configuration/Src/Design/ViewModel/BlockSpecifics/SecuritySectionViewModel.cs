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
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class SecuritySectionViewModel : PositionedSectionViewModel
    {
        public SecuritySectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
            var authZProviders = Positioning.PositionCollection("Authorization Providers", 
                typeof(NameTypeConfigurationElementCollection<AuthorizationProviderData, CustomAuthorizationProviderData>), 
                typeof(AuthorizationProviderData), 
                new PositioningInstructions { FixedColumn = 0, FixedRow = 0 });

            authZProviders.PositionNestedCollection(typeof(AuthorizationRuleData));


            Positioning.PositionCollection("Security Cache Providers",
                            typeof(NameTypeConfigurationElementCollection<SecurityCacheProviderData, CustomSecurityCacheProviderData>),
                            typeof(SecurityCacheProviderData),
                            new PositioningInstructions { FixedColumn = 0, RowAfter = authZProviders});

            Positioning.PositionHeader("Auhtorization Rules", new PositioningInstructions { FixedColumn = 1, FixedRow = 0 });
        }

    }
}
