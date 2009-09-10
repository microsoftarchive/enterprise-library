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
using System.ComponentModel;
using Console.Wpf.ViewModel.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    //should be moved to Envrionmental Overrides project, i think.
    //requires us to split up this project in Shell and Design.
    public class EnvironmentMergeSectionViewModel : SectionViewModel, IElementExtendedPropertyProvider
    {
        IServiceProvider serviceProvider;
        ConfigurationSection section;
        Dictionary<ElementViewModel, OverriddenElementViewModel> overrides = new Dictionary<ElementViewModel, OverriddenElementViewModel>();

        public EnvironmentMergeSectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section)
            :base(serviceProvider, section) 
        {
            this.serviceProvider = serviceProvider;
            this.section = section;
        }

        public EnvironmentMergeSectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, section, additionalAttributes)
        {
            this.serviceProvider = serviceProvider;
        }

        public bool CanExtend(ElementViewModel subject)
        {
            return true;
        }

        public IEnumerable<Property> GetExtendedProperties(ElementViewModel subject)
        {
            var declaringProperty = CreateDeclaringProperty(subject);
            yield return new Property(serviceProvider, subject, declaringProperty);
        }

        private PropertyDescriptor CreateDeclaringProperty(ElementViewModel subject)
        {
            return new OverridesProperty(this);
        }

        private class OverridesProperty : PropertyDescriptor
        {
            EnvironmentMergeSectionViewModel environment;
            OverriddenElementViewModelConverter converter;
            
            public OverridesProperty(EnvironmentMergeSectionViewModel environment)
                :base(string.Format("Overrides on {0}", environment.Name), new Attribute[0])
            {
                this.environment = environment;
                this.converter = new OverriddenElementViewModelConverter();
            }

            public override TypeConverter Converter
            {
                get{ return converter; }
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get { return typeof(ElementViewModel); }
            }

            public override object GetValue(object component)
            {
                ElementViewModel subject = component as ElementViewModel;

                OverriddenElementViewModel overriddes;
                environment.overrides.TryGetValue(subject, out overriddes);
                return overriddes;
            }

            public override void SetValue(object component, object value)
            {
                ElementViewModel subject = component as ElementViewModel;

                OverriddenElementViewModel overriddes = value as OverriddenElementViewModel;
                environment.overrides[subject] = overriddes;
            }

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override Type PropertyType
            {
                get { return typeof(OverriddenElementViewModel); }
            }

            public override void ResetValue(object component)
            {
                
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }

        private class OverriddenElementViewModelConverter : TypeConverter
        {
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                ElementViewModel subject = context.Instance as ElementViewModel;
                OverriddenElementViewModel overriddenElement = value as OverriddenElementViewModel;

                var Pds = subject.Properties.Select(x => ViewModelPropertyDescriptor.CreateProperty(x, new []{ new ViewModelAttribute(typeof(EnvironmentOverriddenProperty)) }));
                return new PropertyDescriptorCollection(Pds.ToArray());
            }
        }

        private class EnvironmentOverriddenProperty : Property
        {
            public EnvironmentOverriddenProperty(IServiceProvider serviceProvider, object component, PropertyDescriptor declaringProperty)
                : base(serviceProvider, component, declaringProperty)
            {
            }

            public EnvironmentOverriddenProperty(IServiceProvider serviceProvider, object component, PropertyDescriptor declaringProperty, Attribute[] additionalAttributes)
                : base(serviceProvider, component, declaringProperty, additionalAttributes)
            {
            }


            public override bool ReadOnly
            {
                get
                {
                    return true;
                }
            }
        }
    }

}
