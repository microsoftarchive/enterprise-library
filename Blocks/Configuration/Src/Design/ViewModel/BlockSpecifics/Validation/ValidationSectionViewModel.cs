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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using System.Windows;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ValidationSectionViewModel : SectionViewModel
    {
        public ValidationSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section) 
        {
        }

        protected override object CreateBindable()
        {
            var validationTypes = DescendentElements().Where(x => x.ConfigurationType == typeof(ValidatedTypeReferenceCollection)).First();

            return new HorizontalListViewModel(
                            new HeaderViewModel(validationTypes.Name, validationTypes.AddCommands),
                            new HeaderViewModel("Rule Sets"),
                            new HeaderViewModel("Validation Targets"),
                            new HeaderViewModel("Rules"),
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null
                        )
                        {
                            Contained = new ElementListViewModel(validationTypes.ChildElements)
                        };
        }
    }
}
