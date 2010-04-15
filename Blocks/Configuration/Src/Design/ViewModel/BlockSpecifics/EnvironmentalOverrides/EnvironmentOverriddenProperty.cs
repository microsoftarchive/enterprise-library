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
using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class EnvironmentOverriddenProperty : Property, ILogicalPropertyContainerElement
    {
        readonly EnvironmentOverriddenElementPayload overrides;
        readonly EnvironmentOverriddenElementProperty overridesProperty;
        readonly Property originalProperty;
        readonly EnvironmentSourceViewModel environment;
        readonly Type customOverridesEditorType;

        public EnvironmentOverriddenProperty(IServiceProvider serviceProvider, EnvironmentOverriddenElementProperty overridesProperty, EnvironmentOverriddenElementPayload overrides, Property originalProperty, EnvironmentSourceViewModel environment)
            : base(serviceProvider, null, originalProperty.DeclaringProperty)
        {
            this.overrides = overrides;
            this.overridesProperty = overridesProperty;
            this.originalProperty = originalProperty;
            this.environment = environment;


            var customOverridesEditorAttribute = originalProperty.Attributes.OfType<EditorAttribute>().Where(x => Type.GetType(x.EditorBaseTypeName) == typeof(IEnvironmentalOverridesEditor)).FirstOrDefault();
            if (customOverridesEditorAttribute != null)
            {
                customOverridesEditorType = Type.GetType(customOverridesEditorAttribute.EditorTypeName, true);
            }

            this.originalProperty.PropertyChanged += OriginalPropertyPropertyChanged;
            this.overridesProperty.PropertyChanged += OverridesPropertyPropertyChanged;
            this.overridesProperty.Subject.PropertyChanged += SubjectPropertyChanged;
        }

        void OriginalPropertyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        void OverridesPropertyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                BindableProperty.ReadOnly = !overrides.Exists;
                OnPropertyChanged("Value");
                Validate();
            }
        }

        void SubjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                OnPropertyChanged("ContainingElementDisplayName");
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

        public override bool SuggestedValuesEditable
        {
            get { return originalProperty.SuggestedValuesEditable; }
        }

        public override bool HasChildProperties
        {
            get { return false; }
        }

        public override IEnumerable<object> SuggestedValues
        {
            get { return originalProperty.SuggestedValues; }
        }

        public override bool HasSuggestedValues
        {
            get { return originalProperty.HasSuggestedValues; }
        }

        public override Type PropertyType
        {
            get { return originalProperty.PropertyType; }
        }

        public Property OriginalProperty
        {
            get { return originalProperty; }
        }

        public override string PropertyName
        {
            get { return originalProperty.PropertyName; }
        }

        public override BindableProperty CreateBindableProperty()
        {
            var prop = customOverridesEditorType == null ?
                                                             originalProperty.CreateBindableProperty(this) :
                                                                                                               new EnvironmentAwareFrameworkEditorBindableProperty(this, customOverridesEditorType, environment);

            prop.ReadOnly = !overrides.Exists;

            return prop;
        }

        public override object Value
        {
            get
            {
                return overrides.GetValue(originalProperty);
            }
            set
            {
                overrides.SetValue(originalProperty, value);
                Validate();
                OnPropertyChanged("Value");
            }
        }

        public override void Validate(string value)
        {
            if (overrides.Exists)
            {
                base.Validate(value);
            }
            else
            {
                ResetValidationResults(Enumerable.Empty<ValidationResult>());
            }
        }


        public override IEnumerable<Validator> GetValidators()
        {
            return originalProperty.GetValidators().Select(
                v => typeof(ElementReferenceValidator).IsAssignableFrom(v.GetType()) ? new OverridesValidatorWrapper(v) : v).Cast<Validator>();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.overridesProperty.Subject.PropertyChanged -= SubjectPropertyChanged;
            }
        }

        #region IElementAssociation Members

        ElementViewModel ILogicalPropertyContainerElement.ContainingElement
        {
            get
            {
                return overridesProperty.Subject;
            }
        }

        string ILogicalPropertyContainerElement.ContainingElementDisplayName
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}.{1}", overridesProperty.Subject.Name,
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

            protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
            {
                var overridesProperty = instance as EnvironmentOverriddenProperty;
                if (overridesProperty == null) return;

                innerValidator.Validate(overridesProperty.originalProperty, value, results);
            }
        }
    }

#pragma warning restore 1591
}
