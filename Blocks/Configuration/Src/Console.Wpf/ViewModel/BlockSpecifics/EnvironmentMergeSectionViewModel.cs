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
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Console.Wpf.ViewModel.Services;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    /// <summary>
    /// View model for 1 environment
    /// </summary>
    public class EnvironmentalOverridesViewModel : SectionViewModel, IElementExtendedPropertyProvider
    {
        ElementLookup elementLookup;
        IServiceProvider serviceProvider;
        EnvironmentMergeSection environmentSection;
        List<OverriddenElementViewModel> overriddenElements = new List<OverriddenElementViewModel>();

        public EnvironmentalOverridesViewModel(IServiceProvider serviceProvider, ConfigurationSection section)
            :base(serviceProvider, section) 
        {
            Initialize(serviceProvider, section);
        }

        public EnvironmentalOverridesViewModel(IServiceProvider serviceProvider, ConfigurationSection section, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, section, additionalAttributes)
        {
            Initialize(serviceProvider, section);
        }


        public string EnvironmentName
        {
            get { return environmentSection.EnvironmentName; }
        }


        private void Initialize(IServiceProvider serviceProvider, ConfigurationSection section)
        {
            this.serviceProvider = serviceProvider;
            this.environmentSection = section as EnvironmentMergeSection;
            this.elementLookup = serviceProvider.GetService(typeof(ElementLookup)) as ElementLookup;
            
            foreach (EnvironmentNodeMergeElement mergeElement in environmentSection.MergeElements)
            {
                overriddenElements.Add(new OverriddenElementViewModel(elementLookup, environmentSection, mergeElement));
            }
        }

        public bool CanExtend(ElementViewModel subject)
        {
            return true;
        }

        public IEnumerable<Property> GetExtendedProperties(ElementViewModel subject)
        {
            OverriddenElementViewModel overrides = overriddenElements.Where(x => x.Subject == subject).FirstOrDefault();
            if (overrides == null) overrides = new OverriddenElementViewModel(elementLookup, environmentSection);

            OverridesProperty declaringProperty = new OverridesProperty(this, overrides);
            yield return new Property(serviceProvider, subject, declaringProperty);
        }


        /// <summary>
        /// property that declares the thing that represents overrides for 1 element.
        /// </summary>
        /// 
        //TODO : can be refactored to Property.
        private class OverridesProperty : PropertyDescriptor
        {
            EnvironmentalOverridesViewModel environment;
            OverriddenElementViewModelConverter converter;
            OverriddenElementViewModel overrides;

            public OverridesProperty(EnvironmentalOverridesViewModel environmentModel, OverriddenElementViewModel overrides)
                :base(string.Format("Overrides on {0}", environmentModel.EnvironmentName), new Attribute[]{new EnvironmentalOverridesAttribute(false)})
            {
                this.environment = environmentModel;
                this.converter = new OverriddenElementViewModelConverter();
                this.overrides = overrides;
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
                return overrides;
            }

            public override void SetValue(object component, object value)
            {
                ElementViewModel elementViewModel = (ElementViewModel)component;
                overrides.SetOverrideProperties(elementViewModel, (bool)value);
            }

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override Type PropertyType
            {
                get { return typeof(bool); }
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
            public static string OverrideProperties = "Override Properties";
            public static string DontOverrideProperties = "Dont Override Properties";


            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return (sourceType == typeof(bool) || sourceType == typeof(OverriddenElementViewModel) || base.CanConvertFrom(context, sourceType)) ;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value is EnvironmentalOverridesViewModel)
                {
                    return ((OverriddenElementViewModel)value).OverrideProperties;
                }
                if (value is string)
                {
                    if (String.Equals((string)value, OverrideProperties))
                    {
                        return true;
                    }
                    return false;
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return (destinationType == typeof(string) || base.CanConvertTo(context, destinationType));
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value is bool)
                {
                    return ((bool)value) ? OverrideProperties : DontOverrideProperties;
                }
                
                if (destinationType == typeof(string) && value is OverriddenElementViewModel)
                {
                    return ((OverriddenElementViewModel)value).OverrideProperties ? OverrideProperties : DontOverrideProperties;
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                ArrayList stdValues = new ArrayList();

                stdValues.Add(OverrideProperties);
                stdValues.Add(DontOverrideProperties);

                return new StandardValuesCollection(stdValues);
            }


            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                ElementViewModel subject = context.Instance as ElementViewModel;
                OverriddenElementViewModel overriddenElement = value as OverriddenElementViewModel;

                var propertiesToOverride = subject.Properties
                                                  .Select(x => new { Property = x, OverridesAttribute = x.Attributes.OfType<EnvironmentalOverridesAttribute>().FirstOrDefault() })
                                                  .Where(x => x.OverridesAttribute == null || x.OverridesAttribute.CanOverride == true)
                                                  .Select(x => x.Property);

                var propertiesAndMetadata = propertiesToOverride.Select(x => new { Property = x, Metadata = new MetadataCollection(x.Attributes) }).ToArray();

                //override metadata.
                foreach (var propertyAndMetada in propertiesAndMetadata)
                {
                    propertyAndMetada.Metadata.Override(new Attribute[] { new ViewModelAttribute(typeof(EnvironmentOverriddenProperty)) });
                }

                var descriptors = propertiesAndMetadata.Select(x => ViewModelPropertyDescriptor.CreateProperty(x.Property, x.Metadata.Attributes.ToArray()));
                return new PropertyDescriptorCollection(descriptors.ToArray());
            }
        }

        private class EnvironmentOverriddenProperty : Property
        {
            OverriddenElementViewModel environmentalOverrides;

            public EnvironmentOverriddenProperty(IServiceProvider serviceProvider, object component, PropertyDescriptor declaringProperty)
                : base(serviceProvider, component, declaringProperty)
            {
                Initilize(component, declaringProperty);
            }

            public EnvironmentOverriddenProperty(IServiceProvider serviceProvider, object component, PropertyDescriptor declaringProperty, Attribute[] additionalAttributes)
                : base(serviceProvider, component, declaringProperty, additionalAttributes)
            {

                Initilize(component, declaringProperty);
            }

            private void Initilize(object component, PropertyDescriptor declaringProperty)
            {
                environmentalOverrides = (OverriddenElementViewModel)component;
                environmentalOverrides.PropertyChanged += new PropertyChangedEventHandler(environmentalOverrides_PropertyChanged);

                if (declaringProperty.SupportsChangeEvents)
                {
                    declaringProperty.AddValueChanged(component, (sender, args) => OnPropertyChanged("Value") );
                }
            }

            void environmentalOverrides_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "OverrideProperties")
                {
                    environmentalOverrides.SetValue(this, base.Value);

                    OnPropertyChanged("ReadOnly");
                    OnPropertyChanged("Value");
                }
            }


            public override bool ReadOnly
            {
                get
                {
                    return !environmentalOverrides.OverrideProperties;
                }
            }

            public override object Value
            {
                get
                {
                    if (!environmentalOverrides.OverrideProperties) return base.Value;
                    return environmentalOverrides.GetValue(this, base.Value);
                }
                set
                {
                    environmentalOverrides.SetValue(this, value);
                }
            }
        }
    }
}
