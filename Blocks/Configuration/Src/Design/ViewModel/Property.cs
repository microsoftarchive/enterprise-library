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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="Property"/> represents a property of a single <see cref="ConfigurationElement"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="Property"/> describes the configuration property via property metadata <see cref="Attributes"/> and
    /// provides access to set and retrieve its value via <see cref="Value"/>.
    /// <br/>
    /// The value described by <see cref="Property"/> is maintained by another <see cref="object"/> provided at construction time.
    /// </remarks>
    /// <seealso cref="CustomProperty{TProperty}"/>
    /// <seealso cref="ElementProperty"/>
    [DebuggerDisplay("Name : {DisplayName} Value: {Value}")]
    public class Property : ViewModel, ITypeDescriptorContext, INotifyPropertyChanged
    {
        private readonly IServiceProvider serviceProvider;

        private readonly object component;
        private readonly PropertyDescriptor declaringProperty;
        private readonly MetadataCollection metadata;
        SectionViewModel containingSection;
        IApplicationModel appModel;
        private ObservableCollection<ValidationResult> validationResults = new ObservableCollection<ValidationResult>();
        private CompositeValidationResultsCollection compositedValidationResults;
        private IEnumerable<Property> childProperties;

        private string propertyName;
        private Type propertyType;
        private bool hidden;
        private bool propertiesShown;
        private TypeConverter converter;

        /// <summary>
        /// Intantiates a new instance of <see cref="Property"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to obtain services.  This is most-often used when the property appears in an editor.</param>
        /// <param name="component">The <see cref="object"/> that contains the property described by <paramref name="declaringProperty"/>.</param>
        /// <param name="declaringProperty">The description of the property owned by <paramref name="component"/>.</param>
        [InjectionConstructor]
        public Property(IServiceProvider serviceProvider, object component, PropertyDescriptor declaringProperty)
            : this(serviceProvider, component, declaringProperty, new Attribute[0])
        {
        }

        /// <summary>
        /// Intantiates a new instance of <see cref="Property"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to obtain services.  This is most-often used when the property appears in an editor.</param>
        /// <param name="component">The <see cref="object"/> that contains the property described by <paramref name="declaringProperty"/>.</param>
        /// <param name="declaringProperty">The description of the property owned by <paramref name="component"/>.</param>
        /// <param name="additionalAttributes">Additional, or overridden, meta-data attributes for the <paramref name="declaringProperty"/></param>
        public Property(IServiceProvider serviceProvider, object component, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additionalAttributes)
        {
            this.serviceProvider = serviceProvider;
            this.component = component;
            this.declaringProperty = declaringProperty;

            if (declaringProperty != null)
            {
                metadata = new MetadataCollection(declaringProperty.Attributes.OfType<Attribute>());
                metadata.Override(additionalAttributes);
            }
            else
            {
                metadata = new MetadataCollection(additionalAttributes);
            }

            Initialize(declaringProperty, metadata.Attributes);
        }

        private void Initialize(PropertyDescriptor declaringProperty, IEnumerable<Attribute> attributes)
        {
            this.converter = new StringConverter();

            if (declaringProperty != null)
            {
                this.propertyName = declaringProperty.Name;
                this.propertyType = declaringProperty.PropertyType;
                this.hidden = !declaringProperty.IsBrowsable;
                this.converter = declaringProperty.Converter;
            }

            TypeConverterAttribute converterAttribute = attributes.OfType<TypeConverterAttribute>().FirstOrDefault();
            if (converterAttribute != null)
            {
                Type converterType = Type.GetType(converterAttribute.ConverterTypeName);
                converter = (TypeConverter)Activator.CreateInstance(converterType);
            }
        }

        /// <summary>
        /// Property dependencies not provided through the constructor and satisfied via dependency-injection.
        /// </summary>
        /// <param name="containingSection">The <see cref="SectionViewModel"/> that contains this property.</param>
        /// <param name="applicationModel">The <see cref="IApplicationModel"/> the property resides in.</param>
        [InjectionMethod]
        public virtual void PropertyDependencyInitialization(SectionViewModel containingSection, IApplicationModel applicationModel)
        {
            this.containingSection = containingSection;
            this.appModel = applicationModel;
        }

        /// <summary>
        /// Gets the <see cref="SectionViewModel"/> containing this <see cref="Property"/>.
        /// </summary>
        /// <remarks>
        /// The containing section for this property is typically specified in the <see cref="PropertyDependencyInitialization"/>.
        /// </remarks>
        public SectionViewModel ContainingSection
        {
            get
            {
                return containingSection;
            }
        }

        /// <summary>
        /// Returns a Bindable property for the property passed in as the <paramref name="propertyToBind"/>. <br/>
        /// Note that the <paramref name="propertyToBind"/> may not be the property you override, but in the case of 
        /// environmental overrides, might be a overridden property that is based on the current one. <br/>
        /// <br/>
        /// This method translates the metadata of the current property into a <see ref="BindableProperty"/> that determines the visual layout. <br/>
        /// </summary>
        /// <param name="propertyToBind">The <see cref="Property"/> for which a <see cref="BindableProperty"/> needs to be created.</param>
        /// <returns></returns>
        public virtual BindableProperty CreateBindableProperty(Property propertyToBind)
        {
            var editorAttributes = propertyToBind.Attributes.OfType<EditorAttribute>();
            if (editorAttributes.Where(x => Type.GetType(x.EditorBaseTypeName) == typeof(FrameworkElement)).Any())
            {
                var frameworkEditorType = editorAttributes
                        .Where(x => Type.GetType(x.EditorBaseTypeName, false) == typeof(FrameworkElement))
                        .Select(x => Type.GetType(x.EditorTypeName))
                        .First();

                return new FrameworkEditorBindableProperty(propertyToBind, frameworkEditorType);
            }

            if (HasSuggestedValues)
            {
                return new SuggestedValuesBindableProperty(propertyToBind);
            }

            if (editorAttributes.Where(x => Type.GetType(x.EditorBaseTypeName) == typeof(UITypeEditor)).Any())
            {
                return new PopupEditorBindableProperty(propertyToBind);
            }

            return new BindableProperty(propertyToBind);
        }

        /// <summary>
        /// Returns an appropriate bindable property for this <see cref="Property"/> based on the <see cref="Attributes"/> meta-data.
        /// </summary>
        /// <seealso cref="CreateBindableProperty(Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Property)"/>
        /// <returns></returns>
        public virtual BindableProperty CreateBindableProperty()
        {
            return CreateBindableProperty(this);
        }

        /// <summary>
        /// Gets the attributes that were supplied to this <see cref="Property"/> instance.
        /// </summary>
        public virtual IEnumerable<Attribute> Attributes
        {
            get
            {
                return metadata.Attributes;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating that child properties should be shown.
        /// </summary>
        /// <seealso cref="ChildProperties"/>
        public bool PropertiesShown
        {
            get { return propertiesShown; }
            set { propertiesShown = value; }
        }

        /// <summary>
        /// Gets a category for the <see cref="Property"/>.
        /// </summary>
        public string Category
        {
            get { return BindableProperty.Category; }
        }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public virtual string PropertyName
        {
            get { return propertyName; }
        }

        /// <summary>
        /// The name of this property as it should appear in the UI.
        /// </summary>
        public string DisplayName
        {
            get { return BindableProperty.DisplayName; }
        }

        /// <summary>
        /// Gets a description for this property.
        /// </summary>
        public string Description
        {
            get { return BindableProperty.Description; }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the property.
        /// </summary>
        public virtual Type PropertyType
        {
            get { return propertyType; }
        }

        /// <summary>
        /// Returns <see langword="true"/> if the property should show up in the editor.
        /// Otherwise returns <see langword="false"/>.
        /// </summary>
        public virtual bool Hidden
        {
            get { return hidden; }
        }

        /// <summary>
        /// Gets the <see cref="object"/> that defines the property described by <see cref="Property"/>.
        /// </summary>
        public object Component
        {
            get { return component; }
        }


        /// <summary>
        /// Gets the <see cref="PropertyDescriptor"/> for the property defined on <see cref="Component"/>.
        /// </summary>
        public PropertyDescriptor DeclaringProperty
        {
            get { return declaringProperty; }
        }

        /// <summary>
        /// Converter that should be used to convert value to and from a string representation.
        /// </summary>
        public virtual TypeConverter Converter
        {
            get { return converter; }
        }

        /// <summary>
        /// Gets a value indiciating that this <see cref="Property"/> has child properties.
        /// </summary>
        /// <seealso cref="ChildProperties"/>
        public virtual bool HasChildProperties
        {
            get { return Converter.GetPropertiesSupported(this); }
        }

        /// <summary>
        /// Gets the child properties for this <see cref="Property"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public virtual IEnumerable<Property> ChildProperties
        {
            get
            {
                EnsureChildProperties();
                return childProperties;
            }
        }

        private void EnsureChildProperties()
        {
            if (childProperties != null) return;
            childProperties = GetChildProperties();
        }

        /// <summary>
        /// Retrieves child properties for this <see cref="Property"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<Property> GetChildProperties()
        {
            if (Converter == null) return Enumerable.Empty<Property>();

            var properties = Converter.GetProperties(this, Value);
            if (properties == null) return Enumerable.Empty<Property>();

            return properties
                .OfType<PropertyDescriptor>()
                .Select(x => ContainingSection.CreateProperty(Value, x)).ToArray();
        }

        /// <summary>
        /// Returns a value indicating that this property has suggested values.
        /// </summary>
        /// <value>Returns <see langword="true"/> if there are suggested values for this property. 
        /// Otherwise, returns <see langword="false"/>.
        /// </value>
        /// <seealso cref="SuggestedValues"/>
        public virtual bool HasSuggestedValues
        {
            get { return Converter.GetStandardValuesSupported(this); }
        }

        /// <summary>
        /// Get a list of suggested values.
        /// </summary>
        public virtual IEnumerable<object> SuggestedValues
        {
            get
            {
                if (!HasSuggestedValues) return Enumerable.Empty<object>();
                return Converter.GetStandardValues(this).OfType<object>();
            }
        }

        /// <summary>
        /// Gets a value indicating if the property is read-only.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the property is read-only.
        /// Otherwise, returns <see langword="false"/>.
        /// </value>
        public bool ReadOnly
        {
            get
            {
                return BindableProperty.ReadOnly;
            }
        }

        /// <summary>
        /// Gets a value indicating if the values must be from the <see cref="SuggestedValues"/> set
        /// or if other values an be provided.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="true"/> if the value must come from the <see cref="SuggestedValues"/> list.
        /// Otherwise, returns <see langword="false"/>.</remarks>
        public virtual bool SuggestedValuesEditable
        {
            get
            {
                return ((SuggestedValuesBindableProperty)BindableProperty).SuggestedValuesEditable;
            }
        }

        private BindableProperty bindableProproperty;

        /// <summary>
        /// Gets a <see cref="BindableProperty"/> used as the bound value in the user-interface.
        /// </summary>
        public BindableProperty BindableProperty
        {
            get { return bindableProproperty ?? (bindableProproperty = CreateBindableProperty()); }
        }

        /// <summary>
        /// Validates this property by invoking <see cref="Validate(string)"/> with the value from <see cref="BindableProperty"/>.
        /// </summary>
        public void Validate()
        {
            Validate(this.BindableProperty.BindableValue);
        }

        ///<summary>
        /// Validates the property and updates the <see cref="ValidationResults"/> collection.
        ///</summary>
        ///<param name="value">The value to run validation against.  This may be different from the internal value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public virtual void Validate(string value)
        {
            var results = new List<ValidationResult>();

            foreach (var validation in this.GetValidators())
            {
                try
                {
                    validation.Validate(this, value, results);
                }
                catch (Exception ex)
                {
                    results.Add(new PropertyValidationResult(this, string.Format(CultureInfo.CurrentCulture, Resources.ValidationErrorExceptionMessage, ex.Message)));
                }
            }

            ResetValidationResults(results);
        }

        /// <summary>
        /// Resets the validation results with the provided enumerable of <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="results"></param>
        protected void ResetValidationResults(IEnumerable<ValidationResult> results)
        {
            foreach (IDisposable error in validationResults)
            {
                error.Dispose();
            }
            if (validationResults.Any())
            {
                validationResults.Clear();
            }
            foreach (var result in results)
            {
                validationResults.Add(result);
            }
        }

        /// <summary>
        /// Converts a value from an internal value to one that can be displayed in the user-interface.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConvertToBindableValue(object value)
        {
            return Converter.ConvertToString(this, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Converts a value from a <see cref="BindableProperty"/> value to a value
        /// that can be stored internally.
        /// </summary>
        /// <param name="bindableValue"></param>
        /// <returns></returns>
        public object ConvertFromBindableValue(string bindableValue)
        {
            return Converter.ConvertFromString(this, CultureInfo.CurrentCulture, bindableValue);
        }

        /// <summary>
        /// Converts a value with no <see cref="CultureInfo"/> considerations, from a <see cref="BindableProperty"/> value to a value
        /// that can be stored internally.
        /// </summary>
        /// <param name="bindableValue"></param>
        /// <returns></returns>
        public object ConvertFromBindableValueInvariant(string bindableValue)
        {
            return Converter.ConvertFromString(this, CultureInfo.InvariantCulture, bindableValue);
        }

        /// <summary>
        /// The value of the property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public virtual object Value
        {
            get
            {
                return GetValue();
            }
            set
            {
                SetValue(value);
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// Gets the underlying, stored value.
        /// </summary>
        /// <returns></returns>
        protected virtual object GetValue()
        {
            return declaringProperty.GetValue(component);
        }


        /// <summary>
        /// Sets the underlying, stored value.
        /// </summary>
        /// <remarks>
        /// Once the value is stored, the property is <see cref="Validate()"/>.
        /// 
        /// </remarks>
        /// <param name="value"></param>
        protected virtual void SetValue(object value)
        {
            declaringProperty.SetValue(component, value);
            Validate();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoked when a property changes.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "Value")
            {
                if (appModel != null)
                {
                    appModel.SetDirty();
                }
            }

            PropertyChangedEventHandler handlers = PropertyChanged;
            if (handlers != null)
            {
                handlers(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        IContainer ITypeDescriptorContext.Container
        {
            get { return (IContainer)serviceProvider.GetService(typeof(IContainer)); }
        }

        object ITypeDescriptorContext.Instance
        {
            get { return component; }
        }

        void ITypeDescriptorContext.OnComponentChanged()
        {

        }

        bool ITypeDescriptorContext.OnComponentChanging()
        {
            return true;
        }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
        {
            get { return BindableProperty; }
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return serviceProvider.GetService(serviceType);
        }

        ///<summary>
        /// Provides an opportunity to initialize the property after creation and prior to visualization.
        ///</summary>
        ///<param name="context"></param>
        public virtual void Initialize(InitializeContext context)
        {
        }

        /// <summary>
        /// Gets a value indicating if the property is requried.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the property is required.
        /// Otherwise, returns <see langword="false"/>.
        /// </value>
        public virtual bool IsRequired
        {
            get { return false; }
        }

        ///<summary>
        /// Gets a value indicating if the property is valid.
        ///</summary>
        /// <value>
        /// Returns <see langword="true"/> if the property is valid.
        /// Otherwise, returns <see langword="false"/>.
        /// </value>

        public bool IsValid
        {
            get { return !ValidationResults.Any(); }
        }


        /// <summary>
        /// Gets any validation results for this property.
        /// </summary>
        public virtual IEnumerable<ValidationResult> ValidationResults
        {
            get { return validationResults; }
        }


        /// <summary>
        /// Gets the set of validators for this property.
        /// </summary>
        /// <remarks>
        /// Validators may be added by deriving from this and returning additional <see cref="Validator"/> objects.  
        /// Or, they can be added by providing <see cref="ValidationAttribute"/> attributes to the 
        /// underlying <see cref="Component"/> or during the construction of <see cref="Property"/>.<br/>
        /// Validators specified by <see cref="ValidationAttribute"/> are created by the containing <see cref="SectionViewModel"/>.
        /// </remarks>
        /// <returns>An <see cref="IEnumerable{Validator}"/> containing default property validators, obtained from <see cref="GetDefaultPropertyValidators"/>
        /// and additional validators provided through <see cref="ValidationAttribute"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validators")]
        public virtual IEnumerable<Validator> GetValidators()
        {
            var validations = GetDefaultPropertyValidators()
                .Union(Attributes.OfType<ValidationAttribute>()
                           .Select(v => ContainingSection.CreateValidatorInstance(v.ValidatorType)));

            return validations;
        }

        /// <summary>
        /// Gets the set of default property validators this <see cref="Property"/>.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validators")]
        protected virtual IEnumerable<Validator> GetDefaultPropertyValidators()
        {
            if (typeof(ConfigurationElementCollection).IsAssignableFrom(this.propertyType))
            {
                yield break;
            }

            yield return new DefaultPropertyValidator();
        }

        /// <summary>
        /// Indicates the object is being disposed.
        /// </summary>
        /// <param name="disposing">Indicates <see cref="ViewModel.Dispose(bool)"/> was invoked through an explicit call to <see cref="ViewModel.Dispose()"/> instead of a finalizer call.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (childProperties != null)
                {
                    foreach (var childProperty in this.childProperties)
                    {
                        childProperty.Dispose();
                    }
                }

                childProperties = null;

                if (compositedValidationResults != null)
                {
                    compositedValidationResults.Clear();
                    compositedValidationResults = null;
                }

                foreach (IDisposable error in validationResults)
                {
                    error.Dispose();
                }
                if (bindableProproperty != null)
                {
                    bindableProproperty.Dispose();
                }
                validationResults.Clear();

            }

            base.Dispose(disposing);
        }
    }
}
