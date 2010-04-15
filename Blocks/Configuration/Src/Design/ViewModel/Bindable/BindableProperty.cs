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
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Collections.Specialized;


namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="BindableProperty"/> class contains all the UI (User Interface) related information for a <see cref="Property"/> instance.
    /// </summary>
    /// <remarks>
    /// <see cref="Property"/> instances are shown in the designer as well in Visual Studio's property grid.<br/>
    /// Threrefore the <see cref="BindableProperty"/> class derives from <see cref="PropertyDescriptor"/>.
    /// </remarks>
    /// <seealso cref="Property"/>
    public class BindableProperty : PropertyDescriptor, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        EventHandler changedHandler;
        bool hasUncommittedValue;
        string uncommittedValue;

        bool @readonly;
        readonly string category;
        readonly string displayName;
        readonly string description;
        readonly INotifyCollectionChanged validationResultsChanged;

        readonly Property property;

        /// <summary>
        /// Initializes a new instance of <see cref="BindableProperty"/>.
        /// </summary>
        /// <param name="property">The <see cref="Design.ViewModel.Property"/> that should be displayed through a UI (User Interface).</param>
        public BindableProperty(Property property)
            : base(property.PropertyName, property.Attributes.ToArray())
        {
            this.property = property;
            this.property.PropertyChanged += PropertyPropertyChanged;
            displayName = property.PropertyName;

            if (property.DeclaringProperty != null)
            {
                displayName = property.DeclaringProperty.DisplayName;
                category = property.DeclaringProperty.Category == "Misc" ? ResourceCategoryAttribute.General.Category : property.DeclaringProperty.Category;
                description = property.DeclaringProperty.Description;
                @readonly = property.DeclaringProperty.IsReadOnly;
            }

            var displayNameAttribute = this.property.Attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayNameAttribute != null)
            {
                displayName = displayNameAttribute.DisplayName;
            }

            var categoryAttribute = this.property.Attributes.OfType<CategoryAttribute>().FirstOrDefault();
            if (categoryAttribute != null)
            {
                category = categoryAttribute.Category;
            }

            var descriptionAttribute = this.property.Attributes.OfType<DescriptionAttribute>().FirstOrDefault();
            if (descriptionAttribute != null)
            {
                description = descriptionAttribute.Description;
            }

            var readonlyAttribute = this.property.Attributes.OfType<ReadOnlyAttribute>().FirstOrDefault();
            if (readonlyAttribute != null)
            {
                @readonly = readonlyAttribute.IsReadOnly;
            }

            var designtimeReadOnlyAttribute = this.property.Attributes.OfType<DesignTimeReadOnlyAttribute>().FirstOrDefault();
            if (designtimeReadOnlyAttribute != null)
            {
                @readonly = designtimeReadOnlyAttribute.ReadOnly;
            }

            validationResultsChanged = property.ValidationResults as INotifyCollectionChanged;
            if (validationResultsChanged != null)
            {
                validationResultsChanged.CollectionChanged += ValidationResultsChanged;
            }
        }

        void ValidationResultsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //this is needed to reset the red rectangle around a property.
            // review: is there a better way to do this?
            // note: there is a test that verifies this behavior which is written in a symptomatic way.
            OnPropertyChanged("BindableValue");
        }

        void PropertyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                hasUncommittedValue = false;

                OnPropertyChanged("Value");
                OnPropertyChanged("BindableValue");
            }
        }

        /// <summary>
        /// Gets the name that can be displayed in a window, such as a Properties window.
        /// </summary>
        /// <returns>
        /// The name to display for the property.
        /// </returns>
        public override string DisplayName
        {
            get { return displayName; }
        }

        /// <summary>
        /// Gets the name of the category to which the member belongs.
        /// </summary>
        /// <returns>
        /// The name of the category to which the member belongs.
        /// </returns>
        public override string Category
        {
            get { return category; }
        }

        /// <summary>
        /// Gets the description of the property.
        /// </summary>
        /// <returns>
        /// The description of the property.
        /// </returns>
        public override string Description
        {
            get { return description; }
        }
        
        /// <summary>
        /// Gets whether the <see cref="BindableValue"/> was comitted to the underlying <see cref="Design.ViewModel.Property"/>.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the value set to <see cref="BindableValue"/> was comitted to the underlying <see cref="Design.ViewModel.Property"/>; Otheriwse <see langword="false"/>.
        /// </returns>
        public bool IsBindableValueCommitted
        {
            get { return !hasUncommittedValue; }
        }

        /// <summary>
        /// Gets or sets a textual representing the underlying <see cref="Design.ViewModel.Property"/>'s value.
        /// </summary>
        /// <remarks>
        /// <para>The value that is gotten or set through this property is expected to be in the client's culture.<br/></para>
        /// If validation errors occur when setting the value, the value will not be comitted to the underlying <see cref="Design.ViewModel.Property"/>.<br/>
        /// Instead, the <see cref="Design.ViewModel.Property"/>'s <see cref="Design.ViewModel.Property.ValidationResults"/> can be used to display an error message.
        /// </remarks>
        /// <value>
        /// A textual representation of the underlying <see cref="Design.ViewModel.Property"/>'s value.
        /// </value>
        public string BindableValue
        {
            get
            {
                if (hasUncommittedValue) return uncommittedValue;
                return property.ConvertToBindableValue(property.Value);
            }
            set
            {
                uncommittedValue = value;
                hasUncommittedValue = true;

                property.Validate(uncommittedValue);
                if (!property.ValidationResults.Any(x => x.IsError))
                {
                    property.Value = property.ConvertFromBindableValue(value);
                    hasUncommittedValue = false;
                }
                OnPropertyChanged("BindableValue");
            }
        }

        /// <summary>
        /// Gets the underlying <see cref="Design.ViewModel.Property"/>'s value.
        /// </summary>
        /// <returns>
        /// The underlying <see cref="Design.ViewModel.Property"/>'s value.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public object Value
        {
            get { return property.Value; }
            set { property.Value = value; }
        }

        /// <summary>
        /// Gets the underlying <see cref="Design.ViewModel.Property"/>.
        /// </summary>
        /// <returns>
        /// The underlying <see cref="Design.ViewModel.Property"/>.
        /// </returns>
        public Property Property
        {
            get { return property; }
        }

        /// <summary>
        /// Gets whether the underlying <see cref="Design.ViewModel.Property"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the property is read-only; otherwise, false.
        /// </returns>
        public bool ReadOnly
        {
            get { return @readonly; }
            set
            {
                @readonly = value;
                OnPropertyChanged("ReadOnly");
            }
        }

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get
            {
                return string.Join(Environment.NewLine, property.ValidationResults.Select(e => e.Message).ToArray());
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (columnName == "BindableValue")
                {
                    return ((IDataErrorInfo)this).Error;
                }
                return string.Empty;
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the <see cref="propertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "BindableValue" && changedHandler != null)
            {
                changedHandler(this, EventArgs.Empty);
            }
            var handler = propertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        PropertyChangedEventHandler propertyChanged;


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        #endregion

        /// <summary>
        /// Returns whether resetting an object changes its value.
        /// </summary>
        /// <returns>
        /// Always returns <see langword="false"/>.
        /// </returns>
        public override bool CanResetValue(object component)
        {
            return false;
        }

        /// <summary>
        /// Gets the type of the component this property is bound to.
        /// </summary>
        /// <returns>
        /// Always returns <see cref="ElementViewModel"/>.
        /// </returns>
        public override Type ComponentType
        {
            get { return typeof(ElementViewModel); }
        }

        /// <summary>
        /// Gets the current value of the property on a component.
        /// </summary>
        /// <returns>
        /// The value of a property for a given component.
        /// </returns>
        public override object GetValue(object component)
        {
            return property.Value;
        }

        /// <summary>
        /// Gets whether this property is read-only.
        /// </summary>
        /// <returns>
        /// true if the property is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly
        {
            get { return ReadOnly; }
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/> that represents the type of the property.
        /// </returns>
        public override Type PropertyType
        {
            get { return property.PropertyType; }
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public override void ResetValue(object component)
        {

        }

        /// <summary>
        /// Gets a <see cref="TypeConverter"/> implementation that can be used to interact with the underlying <see cref="Design.ViewModel.Property"/>.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="BindablePropertyConverter"/> that is used to display the underlying <see cref="Design.ViewModel.Property"/>.
        /// </returns>
        public override TypeConverter Converter
        {
            get { return new BindablePropertyConverter(property); }
        }

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set. </param><param name="value">The new value. </param>
        public override void SetValue(object component, object value)
        {
            property.Value = value;
        }

        /// <summary>
        /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
        /// </summary>
        /// <returns>
        /// Always returns <see langword="false"/>.
        /// </returns>
        /// <param name="component">The component with the property to be examined for persistence. </param>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether value change notifications for this property may originate from outside the property descriptor.
        /// </summary>
        /// <returns>
        /// Always returns <see langword="true"/>.
        /// </returns>
        public override bool SupportsChangeEvents
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="BindableProperty"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (validationResultsChanged != null)
                {
                    validationResultsChanged.CollectionChanged -= ValidationResultsChanged;
                }
                
                property.PropertyChanged -= PropertyPropertyChanged;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Enables other objects to be notified when this property changes.
        /// </summary>
        /// <param name="component">The component to add the handler for. </param><param name="handler">The delegate to add as a listener. </param><exception cref="T:System.ArgumentNullException"><paramref name="component"/> or <paramref name="handler"/> is null.</exception>
        public override void AddValueChanged(object component, EventHandler handler)
        {
            changedHandler += handler;
        }

        /// <summary>
        /// Enables other objects to be notified when this property changes.
        /// </summary>
        /// <param name="component">The component to remove the handler for. </param><param name="handler">The delegate to remove as a listener. </param><exception cref="T:System.ArgumentNullException"><paramref name="component"/> or <paramref name="handler"/> is null.</exception>
        public override void RemoveValueChanged(object component, EventHandler handler)
        {
            changedHandler -= handler;
        }

        /// <summary>
        /// A <see cref="TypeConverter"/> implementation that can be used to display and interpret a <see cref="Property"/>'s Value.
        /// </summary>
        protected class BindablePropertyConverter : TypeConverter
        {
            readonly Property property;

            /// <summary>
            /// Initializes a new instance of <see cref="BindablePropertyConverter"/>.
            /// </summary>
            /// <param name="property">The <see cref="Property"/> instance this <see cref="BindablePropertyConverter"/> is created for.</param>
            public BindablePropertyConverter(Property property)
            {
                this.property = property;
            }

            /// <summary>
            /// Returns whether this object supports properties, using the specified context.
            /// </summary>
            /// <returns>
            /// true if <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)"/> should be called to find the properties of this object; otherwise, false.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param>
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return property.HasChildProperties;
            }

            /// <summary>
            /// Returns a collection of properties for the type of array specified by the value parameter, using the specified context and attributes.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> with the properties that are exposed for this data type, or null if there are no properties.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param><param name="value">An <see cref="T:System.Object"/> that specifies the type of array for which to get properties. </param><param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter. </param>
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return new PropertyDescriptorCollection(property.ChildProperties.Select(x => x.BindableProperty).Cast<PropertyDescriptor>().ToArray());
            }

            /// <summary>
            /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
            /// </summary>
            /// <returns>
            /// true if this converter can perform the conversion; otherwise, false.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param><param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from. </param>
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return property.Converter.CanConvertFrom(context, sourceType);
            }

            /// <summary>
            /// Returns whether this converter can convert the object to the specified type, using the specified context.
            /// </summary>
            /// <returns>
            /// true if this converter can perform the conversion; otherwise, false.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param><param name="destinationType">A <see cref="T:System.Type"/> that represents the type you want to convert to. </param>
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return property.Converter.CanConvertTo(context, destinationType);
            }

            /// <summary>
            /// Converts the given object to the type of this converter, using the specified context and culture information.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Object"/> that represents the converted value.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param><param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture. </param><param name="value">The <see cref="T:System.Object"/> to convert. </param><exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var bindableValue = (string)value;

                property.Validate(bindableValue);
                var errors = property.ValidationResults.Where(x => x.IsError);
                if (errors.Any())
                {
                    throw new ArgumentException(string.Join(Environment.NewLine, property.ValidationResults.Select(e => e.Message).ToArray()));
                }

                return property.Converter.ConvertFrom(context, culture, value);
            }

            /// <summary>
            /// Converts the given value object to the specified type, using the specified context and culture information.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Object"/> that represents the converted value.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param><param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current culture is assumed. </param><param name="value">The <see cref="T:System.Object"/> to convert. </param><param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to. </param><exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/> parameter is null. </exception><exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                return property.Converter.ConvertTo(context, culture, value, destinationType);
            }

            /// <summary>
            /// Creates an instance of the type that this <see cref="T:System.ComponentModel.TypeConverter"/> is associated with, using the specified context, given a set of property values for the object.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Object"/> representing the given <see cref="T:System.Collections.IDictionary"/>, or null if the object cannot be created. This method always returns null.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param><param name="propertyValues">An <see cref="T:System.Collections.IDictionary"/> of new property values. </param>
            public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
            {
                return property.Converter.CreateInstance(context, propertyValues);
            }

            /// <summary>
            /// Returns whether changing a value on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"/> to create a new value, using the specified context.
            /// </summary>
            /// <returns>
            /// true if changing a property on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"/> to create a new value; otherwise, false.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param>
            public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
            {
                return property.Converter.GetCreateInstanceSupported(context);
            }

            /// <summary>
            /// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"/> that holds a standard set of valid values, or null if the data type does not support a standard set of values.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null. </param>
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return property.Converter.GetStandardValues(context);
            }

            /// <summary>
            /// Returns whether the collection of standard values returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> is an exclusive list of possible values, using the specified context.
            /// </summary>
            /// <returns>
            /// true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"/> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> is an exhaustive list of possible values; false if other values are possible.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param>
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return property.Converter.GetStandardValuesExclusive(context);
            }

            /// <summary>
            /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
            /// </summary>
            /// <returns>
            /// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> should be called to find a common set of values the object supports; otherwise, false.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param>
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return property.Converter.GetStandardValuesSupported(context);
            }

            /// <summary>
            /// Returns whether the given value object is valid for this type and for the specified context.
            /// </summary>
            /// <returns>
            /// true if the specified value is valid for this object; otherwise, false.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param><param name="value">The <see cref="T:System.Object"/> to test for validity. </param>
            public override bool IsValid(ITypeDescriptorContext context, object value)
            {
                return property.Converter.IsValid(context, value);
            }
        }
    }
}
