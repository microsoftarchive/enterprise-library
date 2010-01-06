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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
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

            return new HorizontalListViewModel(
                    new HeaderViewModel(authorizationProviders.Name, authorizationProviders.Commands),
                    new HeaderViewModel("Authorization Rules")
                )
                {
                    Contained = new TwoVerticalVisualsViewModel(
                        new ElementListViewModel(authorizationProviders.ChildElements),
                        new TwoColumnsViewModel(new HeaderedListViewModel(securityCacheProviders), null) { ColumnName = "Column0" }
                    )
                };
        }
    }
}
