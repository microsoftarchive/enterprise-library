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

namespace Console.Wpf.ViewModel.ComponentModel
{
    public class ViewModelPropertyDescriptor : PropertyDescriptor
    {
        EventHandler changedHandler;
        Property property;

        private ViewModelPropertyDescriptor(Property property, Attribute[] attributes)
            : base(property.PropertyName, attributes)
        {
            this.property = property;
            this.property.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "Value")
                    {
                        var handler = changedHandler;
                        if (handler != null)
                        {
                            handler(this, EventArgs.Empty);
                        }
                    }
                };
        }

        public override Type ComponentType
        {
            get { return typeof(ElementViewModel); }
        }

        public override bool IsReadOnly
        {
            get { return property.ReadOnly; }
        }

        public override Type PropertyType
        {
            get { return property.Type; }
        }

        public override TypeConverter Converter
        {
            get{ return property.Converter; }
        }

        public override string Description
        {
            get { return property.Description; }
        }

        public override string Name
        {
            get{ return property.PropertyName; }
        }

        public override string DisplayName
        {
            get { return property.DisplayName; }
        }

        public override bool IsBrowsable
        {
            get { return !property.Hidden; }
        }

        public override object GetEditor(Type editorBaseType)
        {
            return property.GetEditor(editorBaseType);
        }

        public override string Category
        {
            get{ return property.Category; }
        }

        public override object GetValue(object component)
        {
            return property.Value;
        }

        public override void SetValue(object component, object value)
        {
            property.Value = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }


        public override void ResetValue(object component)
        {

        }

        public override void AddValueChanged(object component, EventHandler handler)
        {
            changedHandler += handler;
        }

        public override void RemoveValueChanged(object component, EventHandler handler)
        {
            changedHandler -= handler;
        }

        public override bool SupportsChangeEvents
        {
            get
            {
                return true;
            }
        }

        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
        {
            return base.GetChildProperties(instance, filter);
        }

        public static ViewModelPropertyDescriptor CreateProperty(Property property)
        {
            return CreateProperty(property, new Attribute[0]);
        }

        public static ViewModelPropertyDescriptor CreateProperty(Property property, Attribute[] attributes)
        {
            return new ViewModelPropertyDescriptor(property, attributes);
        }

    }
}
