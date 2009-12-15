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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class CustomProperty<TProperty> : Property
    {
        TypeConverter converter;
        string propertyName;

        public CustomProperty(IServiceProvider serviceProvider, string propertyName)
            : this(serviceProvider, TypeDescriptor.GetConverter(typeof(TProperty)), propertyName, new Attribute[0])
        {
        }

        public CustomProperty(IServiceProvider serviceProvider, string propertyName, params Attribute[] attributes)
            : this(serviceProvider, TypeDescriptor.GetConverter(typeof(TProperty)), propertyName, attributes)
        {
        }

        public CustomProperty(IServiceProvider serviceProvider, TypeConverter converter, string propertyName)
            : this(serviceProvider, converter, propertyName, new Attribute[0])
        {
        }

        public CustomProperty(IServiceProvider serviceProvider, TypeConverter converter, string propertyName, params Attribute[] attributes)
            : base(serviceProvider, null, null, attributes)
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

        public override EditorBehavior EditorBehavior
        {
            get
            {
                return EditorBehavior.None;
            }
        }

        TProperty value;
        protected override void SetValue(object value)
        {
            this.value = (TProperty)value;
            Validate();
        }

        protected override object GetValue()
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
