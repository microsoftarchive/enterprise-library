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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class PiabSectionViewModel : SectionViewModel
    {
        public PiabSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section) 
        {
          
        }

        protected override object CreateBindable()
        {
            var policies = DescendentElements().Where(x => x.ConfigurationType == typeof(NamedElementCollection<PolicyData>)).First();

            return new HorizontalListViewModel(
                new HeaderViewModel("Matching Rules"),
                new HeaderViewModel(policies.Name, policies.AddCommands),
                new HeaderViewModel("Call Handlers"))
                {
                    Contained = new ElementListViewModel(policies.ChildElements)
                };
        }
    }
}
