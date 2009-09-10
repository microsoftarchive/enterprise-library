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
using System.Collections;
using System.Linq;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Console.Wpf.ComponentModel.Converters;

namespace Console.Wpf.ViewModel
{
    public class ElementReferenceProperty : ElementProperty
    {
        static readonly Attribute[] ReferencePropertyAttributes = new Attribute[]
                                    {
                                        new DesignTimeTypeAttribute(typeof(ElementViewModel), typeof(ElementReferenceConverter)),
                                        new TypeConverterAttribute(typeof(ElementReferenceConverter))
                                    };


        private ReferenceAttribute reference;
        private ElementViewModel linkedElement;
        private Property linkedElementMatchProperty;

        private PropertyChangedEventHandler linkedElementNameChangedHandler;
        private EventHandler linkedElementDeletedHandler;

        public ElementReferenceProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty, ReferencePropertyAttributes)
        {
            if (declaringProperty.PropertyType != typeof(string)) throw new ArgumentException("TODO");

            reference = declaringProperty.Attributes.OfType<ReferenceAttribute>().FirstOrDefault();
            if (reference == null) throw new InvalidOperationException("TODO");

            linkedElementNameChangedHandler = new PropertyChangedEventHandler(linkedElementName_PropertyChanged);
            linkedElementDeletedHandler = new EventHandler(linkedElement_Deleted);
        }

        public override object Value
        {
            get
            {
                var value = base.Value;
                if (value != linkedElement)
                {
                    LinkElement(value as ElementViewModel);
                }
                return value;
            }
            set
            {
                base.Value = value;
                LinkElement(value as ElementViewModel);
            }
        }

        void linkedElementName_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                SetConvertedValueDirect(linkedElement.Property("Name").Value);
            }
        }

        void linkedElement_Deleted(object sender, EventArgs e)
        {
            SetConvertedValueDirect(null);
        }


        private void ClearLinkedElement()
        {
            if (linkedElementMatchProperty != null)
            {
                linkedElementMatchProperty.PropertyChanged -= linkedElementNameChangedHandler;
                linkedElementMatchProperty = null;
            }
            if (linkedElement != null)
            {
                linkedElement.Deleted -= linkedElementDeletedHandler;
                linkedElement = null;
            }
        }

        private void LinkElement(ElementViewModel element)
        {
            ClearLinkedElement();

            if (element != null)
            {
                linkedElement = element;
                linkedElement.Deleted += new EventHandler(linkedElement_Deleted);

                linkedElementMatchProperty = element.Property(reference.PropertyToMatch);
                linkedElementMatchProperty.PropertyChanged += linkedElementNameChangedHandler;
                
            }
        }
    }
}
