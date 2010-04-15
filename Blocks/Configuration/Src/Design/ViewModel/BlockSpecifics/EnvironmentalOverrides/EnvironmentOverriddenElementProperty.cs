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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides;
using Microsoft.Practices.Unity;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// property that declares the thing that represents overrides for 1 element.
    /// </summary>
    public class EnvironmentOverriddenElementProperty : Property
    {
        readonly EnvironmentSourceViewModel environment;
        readonly OverriddenElementViewModelConverter converter;
        readonly EnvironmentOverriddenElementPayload overrides;
        readonly ElementViewModel subject;
        readonly IUIServiceWpf uiService;
        private CompositeValidationResultsCollection compositedValidationResults;

        public EnvironmentOverriddenElementProperty(IServiceProvider serviceProvider, IUIServiceWpf uiService, EnvironmentSourceViewModel environmentModel, ElementViewModel subject, EnvironmentOverriddenElementPayload overrides)
            : base(serviceProvider, subject, null, new Attribute[]
               {
                   new EnvironmentalOverridesAttribute(false), 
                   new ResourceCategoryAttribute(typeof(DesignResources), "CategoryOverrides")
               })
        {
            this.uiService = uiService;
            this.overrides = overrides;
            this.environment = environmentModel;
            this.converter = new OverriddenElementViewModelConverter();
            this.overrides = overrides;
            this.subject = subject;

            this.environment.PropertyChanged += EnvironmentPropertyChanged;
        }

        void EnvironmentPropertyChanged(object sender, PropertyChangedEventArgs e)
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
            get { return converter; }
        }

        public override bool SuggestedValuesEditable
        {
            get { return false; }
        }

        public EnvironmentSourceViewModel Environment
        {
            get { return environment; }
        }
        
        public ElementViewModel Subject
        {
            get { return subject; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void SetValue(object value)
        {
            try
            {
                if (!(bool)value)
                {
                    if (overrides.Exists)
                    {
                        overrides.Delete();
                    }
                }
                else
                {
                    if (!overrides.Exists)
                    {
                        overrides.Create();

                        overrides.CopyInitialValues(subject.Properties.Where(EnvironmentSourceViewModel.CanOverrideProperty));
                    }
                }
            }
            catch (Exception e)
            {
                uiService.ShowError(e, Resources.UnexpectedErrorOverwritingProperties);
            }
        }

        protected override object GetValue()
        {
            return overrides.Exists;
        }

        public override Type PropertyType
        {
            get { return typeof(bool); }
        }

        public override string PropertyName
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, DesignResources.OverridesPropertyNameTemplate, environment.EnvironmentName);
            }
        }

        public override bool HasChildProperties
        {
            get { return true; }
        }

        protected override IEnumerable<Property> GetChildProperties()
        {
            var properties = subject.Properties.Where(EnvironmentSourceViewModel.CanOverrideProperty)
                                     .Select(x => ContainingSection.CreateProperty(typeof(EnvironmentOverriddenProperty),
                                             new ParameterOverride("overridesProperty", this),
                                             new ParameterOverride("originalProperty", x),
                                             new ParameterOverride("overrides", new InjectionParameter<EnvironmentOverriddenElementPayload>(overrides)),
                                             new ParameterOverride("environment", environment))).ToArray();

            SectionViewModel subjectAsSectionViewModel = subject as SectionViewModel;
            if (subjectAsSectionViewModel != null)
            {
                var protectionProviderProperty = subjectAsSectionViewModel.Properties.OfType<ProtectionProviderProperty>().FirstOrDefault();

                var overriddenProtectionProviderProperty = ContainingSection.CreateProperty<OverriddenProtectionProviderProperty>(
                        new DependencyOverride<ProtectionProviderProperty>(protectionProviderProperty),
                        new DependencyOverride<EnvironmentOverriddenElementPayload>(overrides),
                        new DependencyOverride<EnvironmentOverriddenElementProperty>(this));

                properties = properties.Union(new Property[] {overriddenProtectionProviderProperty}).ToArray();
            }
            return properties;
        }

        public override IEnumerable<Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.ValidationResult> ValidationResults
        {
            get
            {
                EnsureCompositeErrors();
                return compositedValidationResults;
            }
        }

        private void EnsureCompositeErrors()
        {
            if (compositedValidationResults != null) return;

            compositedValidationResults = new CompositeValidationResultsCollection();
            compositedValidationResults.Add(base.ValidationResults);

            foreach (var childProperty in ChildProperties)
            {
                compositedValidationResults.Add(childProperty.ValidationResults);
            }
        }

        #region TypeConverter to translate bool value to Override | Dont Override strings in UI

        private class OverriddenElementViewModelConverter : TypeConverter
        {
            public static string OverrideProperties = DesignResources.OverridesPropertyOverrideProperties;
            public static string DontOverrideProperties = DesignResources.OverridesPropertyDontOverrideProperties;

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return (sourceType == typeof(string) || base.CanConvertFrom(context, sourceType));
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
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
                return (destinationType == typeof(bool) || base.CanConvertTo(context, destinationType));
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value is bool)
                {
                    return ((bool)value) ? OverrideProperties : DontOverrideProperties;
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

                stdValues.Add(false);
                stdValues.Add(true);

                return new StandardValuesCollection(stdValues);
            }
        }

        #endregion

    }
#pragma warning restore 1591
}
