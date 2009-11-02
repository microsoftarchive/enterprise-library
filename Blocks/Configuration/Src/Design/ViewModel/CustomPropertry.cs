using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Console.Wpf.ViewModel
{
    public class CustomPropertry<TProperty> : Property
    {
        TypeConverter converter;
        string propertyName;

        public CustomPropertry(IServiceProvider serviceProvider, string propertyName)
            : this(serviceProvider, TypeDescriptor.GetConverter(typeof(TProperty)), propertyName)
        {
        }

        public CustomPropertry(IServiceProvider serviceProvider, TypeConverter converter, string propertyName)
            :base(serviceProvider, null, null)
        {
            this.converter = converter;
            this.propertyName = propertyName;
        }


        public override Type PropertyType
        {
            get
            {
                return typeof(TProperty);
            }
        }

        public override TypeConverter Converter
        {
            get
            {
                return converter;
            }
        }

        public override string PropertyName
        {
            get
            {
                return propertyName;
            }
        }

        public override string DisplayName
        {
            get
            {
                return propertyName;
            }
        }

        public override bool HasEditor
        {
            get
            {
                return false;
            }
        }

        public override EditorBehavior EditorBehavior
        {
            get
            {
                return EditorBehavior.None;
            }
        }

        TProperty value;
        protected override void SetConvertedValueDirect(object value)
        {
            this.value = (TProperty)value;
        }

        protected override object GetUnConvertedValueDirect()
        {
            return value;
        }

        public TProperty TypedValue
        {
            get
            {
                return (TProperty)Value;
            }
            set
            {
                Value = value;
            }
        }

    }
}
