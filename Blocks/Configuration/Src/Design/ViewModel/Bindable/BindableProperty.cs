using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class BindableProperty : PropertyDescriptor, IDataErrorInfo, INotifyPropertyChanged
    {
        EventHandler changedHandler;
        bool hasUncommittedValue;
        string uncommittedValue;

        bool @readonly;
        string category;
        string displayName;
        string description;

        Property property;

        public BindableProperty(Property property)
            :base(property.PropertyName, property.Attributes.ToArray())
	    {
            this.property = property;
            this.property.PropertyChanged += new PropertyChangedEventHandler(property_PropertyChanged);
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
	    }

        void property_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                OnPropertyChanged("Value");
                OnPropertyChanged("BindableValue");
            }
        }

        public override string DisplayName
        {
            get { return displayName; }
        }

        public override string Category
        {
            get { return category; }
        }

        public override string Description
        {
            get { return description; }
        }

        public string BindableValue
        {
            get
            {
                if (hasUncommittedValue) return uncommittedValue;
                return property.Converter.ConvertToString(property, CultureInfo.CurrentUICulture, property.Value);
            }
            set
            {
                uncommittedValue = value;
                hasUncommittedValue = true;

                var results = property.ValidateWithResults(uncommittedValue);
                property.ResetValidationResults(results);
                if (!results.Any(x => x.IsError))
                {
                    property.Value = property.Converter.ConvertFromString(property, CultureInfo.CurrentUICulture, value);
                    hasUncommittedValue = false;
                }
                OnPropertyChanged("BindableValue");
            }
        }

        public object Value
        {
            get { return property.Value; }
            set { property.Value = value; }
        }

        public Property Property
        {
            get { return property; }
        }

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
                return string.Join(Environment.NewLine, property.ValidationErrors.Select(e => e.Message).ToArray());
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
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        #endregion

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
            return BindableValue;
        }

        public override bool IsReadOnly
        {
            get { return ReadOnly; }
        }

        public override Type PropertyType
        {
            get { return typeof(string); }
        }

        public override void ResetValue(object component)
        {

        }


        public override TypeConverter Converter
        {
            get { return new BindablePropertyConverter(property); }
        }

        public override void SetValue(object component, object value)
        {
            BindableValue = (string)value;
            var errors = property.ValidationErrors.Where(x => x.IsError);
            if (errors.Any())
            {
                throw new ArgumentException(string.Join(Environment.NewLine, property.ValidationErrors.Select(e => e.Message).ToArray()));
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override bool SupportsChangeEvents
        {
            get
            {
                return true;
            }
        }

        public override void AddValueChanged(object component, EventHandler handler)
        {
            changedHandler += handler;
        }

        public override void RemoveValueChanged(object component, EventHandler handler)
        {
            changedHandler -= handler;
        }

        object cachedEditor;
        public override object GetEditor(Type editorBaseType)
        {
            if (cachedEditor != null) return cachedEditor;

            var editorType = property.Attributes
                               .OfType<EditorAttribute>()
                               .Where(x => Type.GetType(x.EditorBaseTypeName, false) == typeof(FrameworkElement))
                               .Select(x => Type.GetType(x.EditorTypeName)).FirstOrDefault();

            if (editorType != null)
            {
                return cachedEditor = new FrameworkElementUITypeEditor(property);
            }
            return null;
        }

        protected class BindablePropertyConverter : StringConverter
        {
            Property property;
            public BindablePropertyConverter(Property property)
            {
                this.property = property;
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return property.HasChildProperties;
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return new PropertyDescriptorCollection(property.ChildProperties.Select(x => x.BindableProperty).Cast<PropertyDescriptor>().ToArray());
            }
        }
    }
}
