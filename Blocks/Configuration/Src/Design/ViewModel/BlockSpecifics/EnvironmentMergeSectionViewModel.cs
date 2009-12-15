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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.RuleEditor;
using System.Windows.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    /// <summary>
    /// View model for 1 environment
    /// </summary>
    public class EnvironmentalOverridesViewModel : SectionViewModel, IElementExtendedPropertyProvider
    {
        ConfigurationSourceModel sourceModel;
        SaveMergedEnvironmentConfigurationCommand saveMergedConfigurationCommand;
        SaveEnvironmentConfigurationDeltaCommand saveEnvironmentDeltaCommand;
        ElementLookup elementLookup;
        IUnityContainer builder;
        EnvironmentMergeSection environmentSection;
        List<OverriddenElementViewModel> overriddenElements = new List<OverriddenElementViewModel>();

        public EnvironmentalOverridesViewModel(IUnityContainer builder, ConfigurationSourceModel sourceModel, ConfigurationSection section)
            : base(builder, EnvironmentMergeSection.EnvironmentMergeData, section, new Attribute[] { new EnvironmentalOverridesAttribute(false) })
        {
            this.builder = builder;
            this.environmentSection = section as EnvironmentMergeSection;
            this.elementLookup = builder.Resolve<ElementLookup>();
            this.sourceModel = sourceModel;

            foreach (EnvironmentNodeMergeElement mergeElement in environmentSection.MergeElements)
            {
                overriddenElements.Add(new OverriddenElementViewModel(elementLookup, environmentSection, mergeElement));
            }

            saveMergedConfigurationCommand = CreateCommand<SaveMergedEnvironmentConfigurationCommand>(this);
            saveEnvironmentDeltaCommand = CreateCommand<SaveEnvironmentConfigurationDeltaCommand>(this);
        }

        public override void Delete()
        {
            sourceModel.RemoveEnvironment(this);
        }

        protected override IEnumerable<CommandModel> GetAllCommands()
        {
            return base.GetAllCommands().Union(new CommandModel[] { saveMergedConfigurationCommand, saveEnvironmentDeltaCommand });
        }

        protected override IEnumerable<Property> GetAllProperties()
        {
            return base.GetAllProperties().Union(new Property[] { ContainingSection.CreateProperty<EnvironmentDeltaFileProperty>() }); ;
        }

        public string EnvironmentName
        {
            get { return environmentSection.EnvironmentName; }
        }

        public string EnvironmentDeltaFile
        {
            get { return (string)Property("EnvironmentDeltaFile").Value; }
            set { Property("EnvironmentDeltaFile").Value = value; }
        }

        public string EnvironmentConfigurationFile
        {
            get { return (string)Property("EnvironmentConfigurationFile").Value; }
            set { Property("EnvironmentConfigurationFile").Value = value; }
        }

        public bool CanExtend(ElementViewModel subject)
        {
            if (subject.Attributes.OfType<EnvironmentalOverridesAttribute>().Where(x => !x.CanOverride).Any())
            {
                return false;
            }

            return subject.Properties.Where(x => !x.Hidden && !x.Attributes.OfType<EnvironmentalOverridesAttribute>().Where(y => !y.CanOverride).Any()).Count() > 0;
        }

        public IEnumerable<Property> GetExtendedProperties(ElementViewModel subject)
        {
            OverriddenElementViewModel overrides = overriddenElements.Where(x => x.Subject == subject).FirstOrDefault();
            if (overrides == null)
            {
                overrides = new OverriddenElementViewModel(elementLookup, environmentSection, subject);
                overriddenElements.Add(overrides);
            }

            yield return ContainingSection.CreateProperty<OverridesProperty>(
                new ParameterOverride("environmentModel", this),
                new ParameterOverride("subject", subject),
                new ParameterOverride("overrides", overrides));
        }

        public OverriddenElementViewModel GetOverridesForElement(ElementViewModel element)
        {
            return overriddenElements.Where(x => x.Subject == element).FirstOrDefault();
        }

        public void SaveDelta()
        {
            saveEnvironmentDeltaCommand.Execute(null);
        }

        private class EnvironmentDeltaFileProperty : CustomProperty<string>
        {
            IApplicationModel applicationModel;

            public EnvironmentDeltaFileProperty(IServiceProvider serviceProvider, IApplicationModel applicationModel)
                : base(serviceProvider, "EnvironmentDeltaFile", new Attribute[]
                {
                    new FilteredFileNameEditorAttribute(typeof(Resources), "EnvironmentDeltaFileFilter"),
                    new EditorAttribute(CommonDesignTime.EditorTypes.FilteredFilePath, CommonDesignTime.EditorTypes.UITypeEditor)
                })
            {
                this.applicationModel = applicationModel;
            }

            public override IEnumerable<Validator> GetValidators()
            {
                return base.GetValidators().Union(new[] { new FilePathValidator() { ApplicationModel = applicationModel } });
            }
            public override string DisplayName
            {
                get
                {
                    return "Environment Delta File";
                }
            }
        }

        /// <summary>
        /// property that declares the thing that represents overrides for 1 element.
        /// </summary>
        public class OverridesProperty : Property
        {
            EnvironmentalOverridesViewModel environment;
            OverriddenElementViewModelConverter converter;
            OverriddenElementViewModel overrides;
            ElementViewModel subject;

            public OverridesProperty(IServiceProvider serviceProvider, EnvironmentalOverridesViewModel environmentModel, ElementViewModel subject, OverriddenElementViewModel overrides)
                : base(serviceProvider, subject, null, new Attribute[] { new EnvironmentalOverridesAttribute(false), new CategoryAttribute("Overrides") })
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
                get { return converter; }
            }

            public override bool SuggestedValuesEditable
            {
                get
                {
                    return false;
                }
            }

            public EnvironmentalOverridesViewModel Environment
            {
                get { return environment; }
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
                get { return typeof(bool); }
            }

            public override string Category
            {
                get { return "Overrides"; }
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

            protected override IEnumerable<Property> GetChildProperties()
            {
                return subject.Properties.Where(x => !x.Hidden)
                                         .Where(x => !x.Attributes.OfType<EnvironmentalOverridesAttribute>().Where(y => !y.CanOverride).Any())
                                         .OfType<ElementProperty>()
                                         .Select(x => ContainingSection.CreateProperty(typeof(EnvironmentOverriddenProperty),
                                                 new ParameterOverride("overridesProperty", this),
                                                 new ParameterOverride("originalProperty", x),
                                                 new ParameterOverride("overrides", overrides),
                                                 new ParameterOverride("environment", environment))).ToArray();
            }
        }

        private class OverriddenElementViewModelConverter : TypeConverter
        {
            public static string OverrideProperties = "Override Properties";
            public static string DontOverrideProperties = "Dont Override Properties";


            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return (sourceType == typeof(bool) || sourceType == typeof(OverriddenElementViewModel) || base.CanConvertFrom(context, sourceType));
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

        public class EnvironmentOverriddenProperty : Property, IElementAssociation
        {
            OverriddenElementViewModel overrides;
            OverridesProperty overridesProperty;
            ElementProperty originalProperty;
            IServiceProvider serviceProvider;
            EnvironmentalOverridesViewModel environment;
            Type customOverridesEditorType;

            public EnvironmentOverriddenProperty(IServiceProvider serviceProvider, OverridesProperty overridesProperty, OverriddenElementViewModel overrides, ElementProperty originalProperty, EnvironmentalOverridesViewModel environment)
                : base(serviceProvider, null, originalProperty.DeclaringProperty)
            {
                this.overrides = overrides;
                this.overridesProperty = overridesProperty;
                this.originalProperty = originalProperty;
                this.serviceProvider = serviceProvider;
                this.environment = environment;


                var customOverridesEditorAttribute = originalProperty.Attributes.OfType<EditorAttribute>().Where(x => Type.GetType(x.EditorBaseTypeName) == typeof(IEnvironmentalOverridesEditor)).FirstOrDefault();
                if (customOverridesEditorAttribute != null)
                {
                    customOverridesEditorType = Type.GetType(customOverridesEditorAttribute.EditorTypeName, true);
                }

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
                    if (typeof(IConvertible).IsAssignableFrom(originalProperty.Type))
                    {
                        overrides.SetValue(originalProperty, originalProperty.Value);
                    }
                    BindableProperty.ReadOnly = !overrides.OverrideProperties || originalProperty.ReadOnly;
                    OnPropertyChanged("Value");
                    Validate();
                }
            }

            public override TypeConverter Converter
            {
                get { return originalProperty.Converter; }
            }

            public override IEnumerable<Property> ChildProperties
            {
                get { return Enumerable.Empty<Property>(); }
            }

            public override string Description
            {
                get { return originalProperty.Description; }
            }

            public override FrameworkElement CreateCustomVisual()
            {
                if (customOverridesEditorType != null)
                {
                    var editor = (FrameworkElement)Activator.CreateInstance(customOverridesEditorType);

                    ((IEnvironmentalOverridesEditor)editor).Initialize(environment);
                    editor.DataContext = this.Bindable;
                    return editor;
                }

                var visual = originalProperty.CreateCustomVisual();
                if (visual != null)
                {
                    visual.DataContext = this.Bindable;
                }
                else
                {
                    visual = new ContentControl()
                    {
                        Content = this.Bindable
                    };
                }
                

                return visual;
                
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

            public ElementViewModel OriginalPropertyViewModel
            {
                get
                {
                    return originalProperty.DeclaringElement;
                }
            }

            protected override object CreateBindable()
            {
                BindableProperty prop = (BindableProperty)base.CreateBindable();
                prop.ReadOnly = !overrides.OverrideProperties || originalProperty.ReadOnly;

                return prop;
            }

            public override object Value
            {
                get
                {
                    if (!overrides.OverrideProperties) return originalProperty.Value;
                    return overrides.GetValue(originalProperty, originalProperty.Value);
                }
                set
                {
                    overrides.SetValue(originalProperty, value);
                    Validate();
                    OnPropertyChanged("Value");
                }
            }

            public override void Validate()
            {
                if (overrides.OverrideProperties)
                {
                    base.Validate();
                }
                else
                {
                    ResetValidationResults(Enumerable.Empty<Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.ValidationError>());
                }
            }


            public override IEnumerable<Validator> GetValidators()
            {
                return originalProperty.GetValidators().Select(
                    v => typeof(ElementReferenceValidator).IsAssignableFrom(v.GetType()) ? new OverridesValidatorWrapper(v) : v).Cast<Validator>();
            }

            #region IElementAssociation Members

            ElementViewModel IElementAssociation.AssociatedElement
            {
                get { return originalProperty.DeclaringElement; }
            }

            string IElementAssociation.ElementName
            {
                get
                {
                    return string.Format("{0}.{1}", originalProperty.DeclaringElement.Name,
                                         overridesProperty.DisplayName);
                }
            }

            #endregion


            private class OverridesValidatorWrapper : Validator
            {
                private readonly Validator innerValidator;

                public OverridesValidatorWrapper(Validator innerValidator)
                {
                    this.innerValidator = innerValidator;
                }

                protected override void ValidateCore(object instance, string value, IList<Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.ValidationError> errors)
                {
                    var overridesProperty = instance as EnvironmentOverriddenProperty;
                    if (overridesProperty == null) return;

                    innerValidator.Validate(overridesProperty.originalProperty, value, errors);
                }
            }
        }
    }
}

