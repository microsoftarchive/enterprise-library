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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class ExceptionHandlingSectionViewModel : SectionViewModel
    {
        public ExceptionHandlingSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
        }
        
        protected override object CreateBindable()
        {
            var policiesCollection = DescendentElements().Where(x => x.ConfigurationType == typeof(NamedElementCollection<ExceptionPolicyData>)).First();

            return new HorizontalListLayout(
                        new HeaderLayout(policiesCollection.Name, policiesCollection.AddCommands),
                        new HeaderLayout(Resources.ExceptionHandlingExceptionTypesHeader),
                        new HeaderLayout(Resources.ExceptionHandlingHandlersHeader))
                        {
                            Contained = new ElementListLayout(policiesCollection.ChildElements)
                        };
        }
    }
#pragma warning restore 1591
}
