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
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Windows.Input;
using Console.Wpf.ViewModel.ComponentModel;
using System.Configuration;
using Microsoft.Practices.Unity;
using Console.Wpf.ViewModel.Services;

namespace Console.Wpf.ViewModel
{
    public class Property : ViewModel, ITypeDescriptorContext, INotifyPropertyChanged
    {
        private readonly IServiceProvider serviceProvider;

        private readonly object component;
        private readonly PropertyDescriptor declaringProperty;
        private readonly MetadataCollection metadata;
        SectionViewModel containingSection;
        IPropertyDirtyStateListener propertyDirtyStateListener;
        
        private TypeConverter designTimeTypeConverter;
        private string propertyName;
        private string displayName;
        private string description;
        private Type propertyType;
        private Type designTimeType;
        private bool hidden;
        private bool @readonly;
        private bool? designTimeReadOnly = null;
        private TypeConverter converter;
        private UITypeEditor popupEditor;
        private FrameworkElement dropdownEditor;
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
        	LaunchEditor = new LaunchEditorCommand();
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
                this.designTimeType = declaringProperty.PropertyType;
                this.@readonly = declaringProperty.IsReadOnly;
                this.hidden = !declaringProperty.IsBrowsable;
                this.converter = declaringProperty.Converter;
                if (declaringProperty.Category != "Misc") // TODO: check
                {
                    this.category = declaringProperty.Category;
                }
            }

            DesignTimeTypeAttribute designtimeTypeAttributes = attributes.OfType<DesignTimeTypeAttribute>().FirstOrDefault();

            if (designtimeTypeAttributes != null)
            {
                designTimeType = designtimeTypeAttributes.DesignTimeType;
                designTimeTypeConverter = Activator.CreateInstance(designtimeTypeAttributes.DesignTimeTypeConverter) as TypeConverter;
            }

            TypeConverterAttribute converterAttribute = attributes.OfType<TypeConverterAttribute>().FirstOrDefault();
            if (converterAttribute != null)
            {
                Type converterType = Type.GetType(converterAttribute.ConverterTypeName);
                converter = (TypeConverter)Activator.CreateInstance(converterType);
            }

            CategoryAttribute categoryAttribute = attributes.OfType<CategoryAttribute>().FirstOrDefault();
            if (categoryAttribute != null)
            {
                category = categoryAttribute.Category;
            }

            DesignTimeReadOnlyAttribute designTimeReadOnlyAttribute =
                attributes.OfType<DesignTimeReadOnlyAttribute>().FirstOrDefault();
            if (designTimeReadOnlyAttribute != null)
            {
                designTimeReadOnly = designTimeReadOnlyAttribute.ReadOnly;
            }
        }

        [InjectionMethod]
        public void PropertyDependencyInitialization(SectionViewModel containingSection, IPropertyDirtyStateListener propertyChangedListener)
        {
            this.containingSection = containingSection;
            this.propertyDirtyStateListener = propertyChangedListener;
        }

        public SectionViewModel ContainingSection
        {
            get
            {
                return containingSection;
            }
        }

        private bool editorsAreInitialized = false;

        protected void EnsureEditors()
        {
            if (editorsAreInitialized) return;
            if (declaringProperty != null)
            {
                this.popupEditor = declaringProperty.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
                this.dropdownEditor = declaringProperty.GetEditor(typeof(FrameworkElement)) as FrameworkElement;
                
                if (dropdownEditor != null)
                {
                    //todo: need to figure this out proper, 
                    //... maybe even moving away from TypeDescriptor, as it seems get in our way.
                    this.dropdownEditor = (FrameworkElement)Activator.CreateInstance(dropdownEditor.GetType());
                    dropdownEditor.DataContext = this;
                }
            }
            editorsAreInitialized = true;
        }

        public virtual IEnumerable<Attribute> Attributes
        {
            get
            {
                return metadata.Attributes;
            }
        }

        public virtual string Category
        {
            get { return category; }
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
            get { return displayName; }
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
            get { return designTimeType; }
        }

        public virtual bool TextReadOnly
        {
            get { return false; }
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
        /// Returns <see langword="true"/> if there is an editor availble for this property
        /// otherwise <see langword="false"/>.
        /// </summary>
        public virtual bool HasEditor
        {
            get
            {
                EnsureEditors();
                return popupEditor != null || dropdownEditor != null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual FrameworkElement Editor
        {
            get
            {
                EnsureEditors();
                return dropdownEditor;
            }
        }

        public virtual UITypeEditor PopupEditor
        {
            get
            {
                EnsureEditors();
                return popupEditor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual EditorBehavior EditorBehavior
        {
            get
            {
                EnsureEditors();
                if (dropdownEditor != null) return EditorBehavior.DropDown;
                if (popupEditor != null) return EditorBehavior.ModalPopup;

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
                var properties = Converter.GetProperties(this, Value);
                if (properties == null) return Enumerable.Empty<Property>();

                return properties
                        .OfType<PropertyDescriptor>()
                        .Select(x => ContainingSection.CreateProperty(Value, x));    
            }
        }

        /// <summary>
        /// returns <see langword="true"/> if there are suggested values for this property. 
        /// otherwise <see langword="false"/>.
        /// </summary>
        public virtual bool HasSuggestedValues
        {
            get { return !HasEditor && Converter.GetStandardValuesSupported(this); }
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

        public IEnumerable<string> BindableSuggestedValues
        {
            get { return SuggestedValues.Select(x=>Converter.ConvertToString(this, CultureInfo.CurrentUICulture, x)); }
        }


        /// <summary>
        /// returns <see langword="true"/> if the property is readonly.
        /// otherwise <see langword="false"/>.
        /// </summary>
        public virtual bool ReadOnly
        {
            get
            {
                return @readonly;
            }
        }

        public virtual bool DesignTimeReadOnly
        {
            get
            {
                if (designTimeReadOnly.HasValue)
                    return designTimeReadOnly.Value;

                return ReadOnly;
            }
        }

        public virtual bool SuggestedValuesEditable
        {
            get
            {
                if (HasSuggestedValues && !Converter.GetStandardValuesExclusive(this))
                {
                    return true;
                }
                return false;
            }
        }

        public string BindableValue
        {
            get { return Converter.ConvertToString(this, CultureInfo.CurrentUICulture, Value); }
            set { Value = Converter.ConvertFromString(this, CultureInfo.CurrentUICulture, value); }
        }

        /// <summary>
        /// The value of the property.
        /// </summary>
        public virtual object Value
        {
            get
            {
                object value = GetUnConvertedValueDirect();

                return  ConvertValueForRead(value);
            }
            set
            {
                object v = ConvertValueForWrite(value);

                SetConvertedValueDirect(v);

            }
        }

        protected virtual object GetUnConvertedValueDirect()
        {
            return declaringProperty.GetValue(component);
        }

        protected virtual void SetConvertedValueDirect(object value)
        {
            declaringProperty.SetValue(component, value);
            OnPropertyChanged("Value");
        }

        private object ConvertValueForWrite(object value)
        {
            if (designTimeType != null && designTimeTypeConverter != null)
            {
                value = designTimeTypeConverter.ConvertFrom(this, CultureInfo.CurrentUICulture, value);
            }
            return value;
        }

        private object ConvertValueForRead(object value)
        {
            if (designTimeType != null && designTimeTypeConverter != null)
            {
                value = designTimeTypeConverter.ConvertTo(this, CultureInfo.CurrentUICulture, value, designTimeType);
            }
            return value;
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "Value")
            {
                OnPropertyChanged("BindableValue");
                if (propertyDirtyStateListener != null)
                {
                    propertyDirtyStateListener.SetDirty();
                }
            }
            if (propertyName == "SuggestedValues") OnPropertyChanged("BindableSuggestedValues");
            if (propertyName == "ReadOnly") OnPropertyChanged("DesignTimeReadOnly");

            PropertyChangedEventHandler handlers = PropertyChanged;
            if (handlers != null)
            {
                handlers(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        //ICommand for ease of Binding
        public LaunchEditorCommand LaunchEditor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual void ShowUITypeEditor()
        {
            if (PopupEditor != null) Value = PopupEditor.EditValue(this, serviceProvider, Value);
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

        internal object GetEditor(Type editorBaseType)
        {
            if (editorBaseType == typeof(UITypeEditor))
            {
                if (popupEditor != null) return popupEditor;
                if (dropdownEditor != null) return null;
                //todo: wrap dropdown editor in UITypeEditor
            }

            return null;
        }
           
    }


    public class LaunchEditorCommand : ICommand
    {
        public void Execute(object parameter)
        {
            var propertyModel = parameter as Property;
            if (propertyModel != null && propertyModel.EditorBehavior == EditorBehavior.ModalPopup)
            {
                propertyModel.ShowUITypeEditor();
            }
            else if (propertyModel != null && propertyModel.EditorBehavior == EditorBehavior.DropDown)
            {
                
            }
        }

        public bool CanExecute(object parameter)
        {
            //var propertyModel = parameter as ConfigElementPropertyModel;
            //if (propertyModel != null)
            //{
            //    return propertyModel.HasEditor && propertyModel.EditorBehavior == EditorBehavior.ModalPopup;
            //}

            return true;
        }

        public event EventHandler CanExecuteChanged;
    }

    public enum EditorBehavior
    {
        None = 0,
        DropDown,
        ModalPopup
    }
}
