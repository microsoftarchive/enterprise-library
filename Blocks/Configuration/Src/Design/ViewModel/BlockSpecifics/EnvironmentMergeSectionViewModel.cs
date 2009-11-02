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
using Microsoft.Practices.Unity;
using System.Windows;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    /// <summary>
    /// View model for 1 environment
    /// </summary>
    public class EnvironmentalOverridesViewModel : SectionViewModel, IElementExtendedPropertyProvider
    {
        ElementLookup elementLookup;
        IUnityContainer builder;
        EnvironmentMergeSection environmentSection;
        List<OverriddenElementViewModel> overriddenElements = new List<OverriddenElementViewModel>();
        
        [InjectionConstructor]
        public EnvironmentalOverridesViewModel(IUnityContainer builder, ConfigurationSection section)
            : base(builder, EnvironmentMergeSection.EnvironmentMergeData, section) 
        {
            Initialize(builder, section);
        }

        public EnvironmentalOverridesViewModel(IUnityContainer builder, ConfigurationSection section, IEnumerable<Attribute> additionalAttributes)
            : base(builder, EnvironmentMergeSection.EnvironmentMergeData, section, additionalAttributes)
        {
            Initialize(builder, section);
        }

        public override void Delete()
        {
            ConfigurationSourceModel sourceModel = builder.Resolve<ConfigurationSourceModel>();
            sourceModel.RemoveEnvironment(this);
        }

        public string EnvironmentName
        {
            get { return environmentSection.EnvironmentName; }
        }

        private void Initialize(IUnityContainer builder, ConfigurationSection section)
        {
            this.builder = builder;
            this.environmentSection = section as EnvironmentMergeSection;
            this.elementLookup = builder.Resolve<ElementLookup>();
            
            foreach (EnvironmentNodeMergeElement mergeElement in environmentSection.MergeElements)
            {
                overriddenElements.Add(new OverriddenElementViewModel(elementLookup, environmentSection, mergeElement));
            }
        }

        public bool CanExtend(ElementViewModel subject)
        {
            return subject.Properties.Where(x => !x.Hidden && !x.Attributes.OfType<EnvironmentalOverridesAttribute>().Where(y=>!y.CanOverride).Any()).Count() > 0;
        }

        public IEnumerable<Property> GetExtendedProperties(ElementViewModel subject)
        {
            OverriddenElementViewModel overrides = overriddenElements.Where(x => x.Subject == subject).FirstOrDefault();
            if (overrides == null) overrides = new OverriddenElementViewModel(elementLookup, environmentSection);

            yield return ContainingSection.CreateProperty<OverridesProperty>(
                new ParameterOverride("environmentModel", this),
                new ParameterOverride("subject", subject),
                new ParameterOverride("overrides", overrides));
        }

        /// <summary>
        /// property that declares the thing that represents overrides for 1 element.
        /// </summary>
        private class OverridesProperty : Property
        {
            EnvironmentalOverridesViewModel environment;
            OverriddenElementViewModelConverter converter;
            OverriddenElementViewModel overrides;
            ElementViewModel subject;

            public OverridesProperty(IServiceProvider serviceProvider, EnvironmentalOverridesViewModel environmentModel, ElementViewModel subject, OverriddenElementViewModel overrides)
                : base(serviceProvider, subject, null, new Attribute[] { new EnvironmentalOverridesAttribute(false) })
            {
                this.environment = environmentModel;
                this.converter = new OverriddenElementViewModelConverter();
                this.overrides = overrides;
                this.subject = subject;

                this.environment.PropertyChanged += new PropertyChangedEventHandler(environment_PropertyChanged);
            }

            void environment_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Name")
                {
                    OnPropertyChanged("PropertyName");
                }
            }

            protected override void OnPropertyChanged(string propertyName)
            {
                if (propertyName == "PropertyName") OnPropertyChanged("DisplayName");

                base.OnPropertyChanged(propertyName);
            }

            public override TypeConverter Converter
            {
                get{ return converter; }
            }

            public override bool SuggestedValuesEditable
            {
                get
                {
                    return false;
                }
            }

            public override object Value
            {
                get
                {
                    return overrides.OverrideProperties;
                }
                set
                {
                    overrides.SetOverrideProperties(subject, (bool)value);
                    OnPropertyChanged("Value");
                }
            }

            public override Type PropertyType
            {
                get{ return typeof(bool); }
            }

            public override string Category
            {
                get{ return "Overrides" ; }
            }

            public override string PropertyName
            {
                get
                {
                    return string.Format("Overrides on {0}", environment.EnvironmentName);
                }
            }

            public override string DisplayName
            {
                get
                {
                    return PropertyName;
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
                    return subject.Properties.Where( x => !x.Hidden)
                                             .Where( x => !x.Attributes.OfType<EnvironmentalOverridesAttribute>().Where(y=>!y.CanOverride).Any())
                                             .Select( x => ContainingSection.CreateProperty(typeof(EnvironmentOverriddenProperty), 
                                                     new ParameterOverride("overridesProperty", this),
                                                     new ParameterOverride("originalProperty", x),
                                                     new ParameterOverride("overrides", overrides)));
                }
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
        }

        private class EnvironmentOverriddenProperty : Property
        {
            OverriddenElementViewModel overrides;
            OverridesProperty overridesProperty;
            Property originalProperty;
            IServiceProvider serviceProvider;

            public EnvironmentOverriddenProperty(IServiceProvider serviceProvider, OverridesProperty overridesProperty, OverriddenElementViewModel overrides, Property originalProperty)
                : base(serviceProvider, null, originalProperty.DeclaringProperty)
            {
                this.overrides = overrides;
                this.overridesProperty = overridesProperty;
                this.originalProperty = originalProperty;
                this.serviceProvider = serviceProvider;

                this.originalProperty.PropertyChanged += new PropertyChangedEventHandler(originalProperty_PropertyChanged);
                this.overrides.PropertyChanged += new PropertyChangedEventHandler(overridesProperty_PropertyChanged);
            }

            void originalProperty_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged(e.PropertyName);
            }

            void overridesProperty_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "OverrideProperties")
                {
                    overrides.SetValue(this, originalProperty.Value);

                    OnPropertyChanged("ReadOnly");
                    OnPropertyChanged("Value");
                }
            }

            public override TypeConverter Converter
            {
                get { return originalProperty.Converter; }
            }

            public override string Category
            {
                get{ return originalProperty.Category; }
            }

            public override IEnumerable<Property> ChildProperties
            {
                get { return Enumerable.Empty<Property>(); }
            }

            public override string Description
            {
                get{ return originalProperty.Description; }
            }

            public override System.Windows.FrameworkElement Editor
            {
                get
                {
                    if (originalProperty.Editor == null) return null;

                    FrameworkElement editor = (FrameworkElement)Activator.CreateInstance(originalProperty.Editor.GetType());
                    editor.DataContext = this;

                    return editor;
                }
            }

            public override bool SuggestedValuesEditable
            {
                get
                {
                    return originalProperty.SuggestedValuesEditable;
                }
            }

            public override EditorBehavior EditorBehavior
            {
                get
                {
                    return originalProperty.EditorBehavior;
                }
            }

            public override bool HasChildProperties
            {
                get
                {
                    return false;
                }
            }

            public override IEnumerable<object> SuggestedValues
            {
                get
                {
                    return originalProperty.SuggestedValues;
                }
            }

            public override bool HasSuggestedValues
            {
                get
                {
                    return originalProperty.HasSuggestedValues;
                }
            }

            public override bool HasEditor
            {
                get
                {
                    return originalProperty.HasEditor;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return originalProperty.PropertyType;
                }
            }

            public override string PropertyName
            {
                get
                {
                    return originalProperty.PropertyName;
                }
            }

            public override string DisplayName
            {
                get
                {
                    return originalProperty.DisplayName;
                }
            }


            public override bool ReadOnly
            {
                get
                {
                    return !overrides.OverrideProperties || originalProperty.ReadOnly;
                }
            }

            public override void ShowUITypeEditor()
            {
                if (ReadOnly) return;
                if (originalProperty.PopupEditor != null) Value = originalProperty.PopupEditor.EditValue(this, serviceProvider, Value);        
            }

            public override object Value
            {
                get
                {
                    if (!overrides.OverrideProperties) return originalProperty.Value;
                    return overrides.GetValue(this, originalProperty.Value);
                }
                set
                {
                    overrides.SetValue(this, value);
                    OnPropertyChanged("Value");
                }
            }
        }
    }
}
