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
using Microsoft.Practices.Unity.Configuration.Design.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.ComponentModel;

namespace Microsoft.Practices.Unity.Configuration.Design.ViewModel
{
    public class ValueElementProperty : CustomProperty<Type>
    {
        public static Type[] KnownParameterValueTypes;

        static ValueElementProperty()
        {
            KnownParameterValueTypes = new Type[] { typeof(DependencyElement), typeof(OptionalElement), typeof(ValueElement), typeof(ArrayElement) };
        }

        ElementViewModel parentViewModel;
        ElementViewModel valueElementViewModel;
        PropertyDescriptor valueElementPropertyDescriptor;


        public ValueElementProperty(IServiceProvider serviceProvider, ElementViewModel parentViewModel, PropertyDescriptor valueElementPropertyDescriptor)
            : base(serviceProvider, new KnownTypeNameConverter(KnownParameterValueTypes), valueElementPropertyDescriptor.Name,
                    new ResourceCategoryAttribute(typeof(DesignResources), "ParameterValueCategory"))
        {
            this.parentViewModel = parentViewModel;
            this.valueElementPropertyDescriptor = valueElementPropertyDescriptor;
            RecreateParameterValueViewModel(parentViewModel);
        }

        private void RecreateParameterValueViewModel(ElementViewModel parentViewModel)
        {
            valueElementViewModel = parentViewModel.ContainingSection.
                                            CreateElement(parentViewModel, valueElementPropertyDescriptor);
        }

        public ElementViewModel ValueElementViewModel
        {
            get { return valueElementViewModel; }
        }

        public override object Value
        {
            get
            {
                return valueElementPropertyDescriptor.GetValue(parentViewModel.ConfigurationElement).GetType();
            }
            set
            {
                var parameterValueElement = Activator.CreateInstance((Type)value) as ParameterValueElement;
                valueElementPropertyDescriptor.SetValue(parentViewModel.ConfigurationElement, parameterValueElement);
                RecreateParameterValueViewModel(parentViewModel);

                OnPropertyChanged("ChildProperties");
            }
        }

        public override bool HasChildProperties
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<Property> ChildProperties
        {
            get
            {
                return valueElementViewModel.Properties;
            }
        }
    }
}
