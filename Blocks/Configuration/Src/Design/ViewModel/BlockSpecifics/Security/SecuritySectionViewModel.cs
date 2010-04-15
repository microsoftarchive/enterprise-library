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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591

    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class SecuritySectionViewModel : SectionViewModel
    {
        public SecuritySectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {

        }

        protected override object CreateBindable()
        {
            var securityCacheProviders = DescendentElements().Where(x => x.ConfigurationType == typeof(NameTypeConfigurationElementCollection<SecurityCacheProviderData, CustomSecurityCacheProviderData>)).First();
            var authorizationProviders = DescendentElements().Where(x => x.ConfigurationType == typeof(NameTypeConfigurationElementCollection<AuthorizationProviderData, CustomAuthorizationProviderData>)).First();

            return new HorizontalListLayout(
                    new HeaderLayout(authorizationProviders.Name, authorizationProviders.Commands),
                    new HeaderLayout(Resources.SecurityAuthorizationRulesHeader)
                )
                {
                    Contained = new TwoVerticalsLayout(
                        new ElementListLayout(authorizationProviders.ChildElements),
                        new HorizontalColumnBindingLayout(new HeaderedListLayout(securityCacheProviders), 0)
                    )
                };
        }
    }
#pragma warning restore 1591
}
