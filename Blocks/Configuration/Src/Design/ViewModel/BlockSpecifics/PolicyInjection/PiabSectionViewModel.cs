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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PolicyInjection
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class PolicyInjectionSectionViewModel : SectionViewModel
    {
        public PolicyInjectionSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section) 
        {
          
        }

        protected override object CreateBindable()
        {
            var policies = DescendentElements().Where(x => x.ConfigurationType == typeof(NamedElementCollection<PolicyData>)).First();

            return new HorizontalListLayout(
                new HeaderLayout(Resources.PolicyInjectionMatchingRulesHeader),
                new HeaderLayout(policies.Name, policies.AddCommands),
                new HeaderLayout(Resources.PolicyInjectionCallHandlersHeader))
                       {
                           Contained = new ElementListLayout(policies.ChildElements)
                       };
        }
    }
#pragma warning restore 1591

}
