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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ExceptionHandlingSectionViewModel : SectionViewModel
    {
        public ExceptionHandlingSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
        }
        
        protected override object CreateBindable()
        {
            var policiesCollection = DescendentElements().Where(x => x.ConfigurationType == typeof(NamedElementCollection<ExceptionPolicyData>)).First();

            return new HorizontalListViewModel(
                        new HeaderViewModel(policiesCollection.Name, policiesCollection.AddCommands),
                        new HeaderViewModel("Exception Types"),
                        new HeaderViewModel("Handlers"))
                        {
                            Contained = new ElementListViewModel(policiesCollection.ChildElements)
                        };
        }
    }
}
