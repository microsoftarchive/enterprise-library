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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.ComponentModel;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using System.Windows.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    [DebuggerDisplay("Name : {DisplayName} Value: {Value}")]
    public class Property : ViewModel, ITypeDescriptorContext, INotifyPropertyChanged, INeedInitialization
    {
        private readonly IServiceProvider serviceProvider;

        private readonly object component;
        private readonly PropertyDescriptor declaringProperty;
        private readonly MetadataCollection metadata;
        SectionViewModel containingSection;
        IApplicationModel appModel;
        private ObservableCollection<ValidationError> validationErrors = new ObservableCollection<ValidationError>();
        private CompositeErrorsCollection compositedErrors;
        private IEnumerable<Property> childProperties;

        private string propertyName;
        private string displayName;
        private string description;
        private Type propertyType;
        private bool hidden;
        private bool @readonly;
        private TypeConverter converter;
        private string category;

        [InjectionConstructor]
        public Property(IServiceProvider serviceProvider, object component, PropertyDescriptor declaringProperty)
            : this(serviceProvider, component, declaringProperty, new Attribute[0])
        {
        }

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
            this.category = ResourceCategoryAttribute.General.Category;
            this.converter = new StringConverter();

            if (declaringProperty != null)
            {
                this.propertyName = declaringProperty.Name;
                this.displayName = declaringProperty.DisplayName;
                this.description = declaringProperty.Description;
                this.propertyType = declaringProperty.PropertyType;
                this.@readonly = declaringProperty.IsReadOnly;
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

        [InjectionMethod]
        public virtual void PropertyDependencyInitialization(SectionViewModel containingSection, IApplicationModel applicationModel)
        {
            this.containingSection = containingSection;
            this.appModel = applicationModel;
        }

        public SectionViewModel ContainingSection
        {
            get
            {
                return containingSection;
            }
        }

        protected override object CreateBindable()
        {
            if (HasSuggestedValues)
            {
                return new SuggestedValuesBindableProperty(this);
            }
            else if (EditorBehavior == EditorBehavior.ModalPopup)
            {
                return new PopupEditorBindableProperty(this);
            }
            else
            {
                return new BindableProperty(this);
            }
        }

        public override FrameworkElement CreateCustomVisual()
        {
            var customVisual = base.CreateCustomVisual();
            if (customVisual != null)
            {
                return customVisual;
            }

            if (Attributes.OfType<EditorAttribute>().Where(x => Type.GetType(x.EditorBaseTypeName) == typeof(FrameworkElement)).Any())
            {
                var editor = Attributes.OfType<EditorAttribute>().Where(x => Type.GetType(x.EditorBaseTypeName) == typeof(FrameworkElement)).First();
                var editorType = Type.GetType(editor.EditorTypeName, true);
                FrameworkElement editorInstance = (FrameworkElement)Activator.CreateInstance(editorType);
                editorInstance.DataContext = Bindable;
                return editorInstance;
            }

            return null;
        }

        /// <summary>
        /// Gets the attributes that where supplied to this <see cref="Property"/> instance.
        /// </summary>
        public virtual IEnumerable<Attribute> Attributes
        {
            get
            {
                return metadata.Attributes;
            }
        }

        public virtual string Category
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
        public virtual string DisplayName
        {
            get { return BindableProperty.DisplayName; }
        }

        /// <summary>
        /// The description for this property.
        /// </summary>
        public virtual string Description
        {
            get { return description; }
        }

        /// <summary>
        /// The type of the property.
        /// </summary>
        public virtual Type PropertyType
        {
            get { return propertyType; }
        }

        /// <summary>
        /// The type of the value.
        /// </summary>
        public virtual Type Type
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

        public object Component
        {
            get { return component; }
        }


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
        /// 
        /// </summary>
        public virtual EditorBehavior EditorBehavior
        {
            get
            {
                if (Attributes.OfType<EditorAttribute>().Where(x => Type.GetType(x.EditorBaseTypeName) == typeof(UITypeEditor)).Any()) return EditorBehavior.ModalPopup;

                return EditorBehavior.None;
            }
        }

        /// <summary/>
        public virtual bool HasChildProperties
        {
            get { return Converter.GetPropertiesSupported(this); }
        }

        /// <summary/>
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
        /// returns <see langword="true"/> if there are suggested values for this property. 
        /// otherwise <see langword="false"/>.
        /// </summary>
        public virtual bool HasSuggestedValues
        {
            get { return Converter.GetStandardValuesSupported(this); }
        }

        /// <summary>
        /// returns a list of suggested values.
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
        /// returns <see langword="true"/> if the property is readonly.
        /// otherwise <see langword="false"/>.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return BindableProperty.ReadOnly;
            }
        }

        public virtual bool SuggestedValuesEditable
        {
            get
            {
                return ((SuggestedValuesBindableProperty)BindableProperty).SuggestedValuesEditable;
            }
        }

        public BindableProperty BindableProperty
        {
            get { return Bindable as BindableProperty; }
        }

        public IEnumerable<ValidationError> ValidateWithResults(string value)
        {
             var results = new List<ValidationError>();

            foreach (var validation in this.GetValidators())
            {
                validation.Validate(this, value, results);
            }

            return results;
        }

        ///<summary>
        /// Validates the property and updates the <see cref="ValidationErrors"/> collection.
        ///</summary>
        public virtual void Validate()
        {
            ResetValidationResults(ValidateWithResults(this.BindableProperty.BindableValue));
        }

        public void ResetValidationResults(IEnumerable<ValidationError> results)
        {
            validationErrors.Clear();
            foreach (var result in results)
            {
                validationErrors.Add(result);
            }
        }

        public string ConvertToBindableValue(object value)
        {
            return Converter.ConvertToString(this, CultureInfo.CurrentUICulture, value);
        }

        public object ConvertFromBindableValue(string bindableValue)
        {
            return Converter.ConvertFromString(this, CultureInfo.CurrentUICulture, bindableValue);
        }

        /// <summary>
        /// The value of the property.
        /// </summary>
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

        protected virtual object GetValue()
        {
            return declaringProperty.GetValue(component);
        }

        protected virtual void SetValue(object value)
        {
            declaringProperty.SetValue(component, value);
            Validate();
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

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
            get { return declaringProperty; }
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

        public virtual bool IsRequired
        {
            get{ return false; }
        }

        ///<summary>
        /// Returns true if valid.
        ///</summary>
        public bool IsValid
        {
            get { return !ValidationErrors.Any(); }
        }


        protected IList<ValidationError> InternalErrors
        {
            get { return validationErrors; }
        }

        ///<summary>
        /// The validation errors for this property.
        ///</summary>
        public virtual IEnumerable<ValidationError> ValidationErrors
        {
            get
            {
                EnsureCompositeErrors();
                return compositedErrors;
            }
        }

        private void EnsureCompositeErrors()
        {
            if (compositedErrors == null)
            {
                compositedErrors = new CompositeErrorsCollection();
                compositedErrors.Add(validationErrors);

                foreach(var childProperty in ChildProperties)
                {
                    compositedErrors.Add(childProperty.ValidationErrors);
                }
            }
        }

        public virtual IEnumerable<Validator> GetValidators()
        {
            var validations = GetDefaultPropertyValidators()
                .Union(Attributes.OfType<ValidationAttribute>()
                           .Select(v => ContainingSection.CreateValidatorInstance(v.ValidatorType)));

            return validations;
        }

        protected virtual IEnumerable<Validator> GetDefaultPropertyValidators()
        {
            if (this.Converter is CollectionConverter)
            {
                yield break;
            }

            yield return new DefaultPropertyValidator();
        }
    }


    public enum EditorBehavior
    {
        None = 0,
        DropDown,
        ModalPopup
    }
}
