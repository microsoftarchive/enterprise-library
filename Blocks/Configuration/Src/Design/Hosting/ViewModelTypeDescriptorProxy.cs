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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.ComponentModel
{

    class ViewModelTypeDescriptorProxy : ICustomTypeDescriptor
    {
        ElementViewModel elementViewModel;

        public ViewModelTypeDescriptorProxy(ElementViewModel elementViewModel)
        {
            this.elementViewModel = elementViewModel;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(elementViewModel, true);
        }

        public string GetClassName()
        {
            return elementViewModel.Name;
        }

        public string GetComponentName()
        {
            return elementViewModel.Name;
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(elementViewModel, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(elementViewModel, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(elementViewModel, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(elementViewModel, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(elementViewModel, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(elementViewModel, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(
                        elementViewModel.
                            Properties.
                            Where(x => !x.Hidden).
                            Select(x => x.BindableProperty).
                            OfType<PropertyDescriptor>().
                            ToArray());
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(new Attribute[0]);
        }

        public object GetPropertyOwner(PropertyDescriptor propertyDescriptor)
        {
            return elementViewModel;
        }
    }
}
