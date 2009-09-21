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
using Console.Wpf.ViewModel;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Configuration;

namespace Console.Wpf.ComponentModel.Converters
{
    public class ElementReferenceConverter : TypeConverter
    {
        IEnumerable<ElementViewModel> targetElements;
        ReferenceAttribute reference;

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType) || typeof(string) == destinationType;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            EnsureTargetElements(context);
            
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (value is string && destinationType == typeof(ElementViewModel))
            {
                string str = value == null ? string.Empty : ((string)value).Trim();

                ElementViewModel element = targetElements.Where(x => ((string)x.Property(reference.PropertyToMatch).Value) == str).FirstOrDefault();
                
                return element;
            }

            if (destinationType == typeof(ElementViewModel) && value == null) return null;
            if (destinationType == typeof(ElementViewModel) && value is ElementViewModel) return value;

            if (destinationType == typeof(string))
            {
                return ConvertFrom(context, culture, value); 
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(ElementViewModel)) || base.CanConvertFrom(context, sourceType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            EnsureTargetElements(context);
            if (value != null && value is ElementViewModel)
            {
                return ((ElementViewModel)value).Property(reference.PropertyToMatch).Value;
            }
            if (value != null && value is string)
            {
                return ConvertTo(context, culture, value, typeof(ElementViewModel));
            }
            if (value == null)
            {
                return null;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            EnsureTargetElements(context);
            return new StandardValuesCollection(targetElements.ToList());
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return context.GetService<ElementLookup>() != null;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        private void EnsureTargetElements(ITypeDescriptorContext context)
        {
            reference = context.PropertyDescriptor.Attributes.OfType<ReferenceAttribute>().FirstOrDefault();
            var instance = context.Instance as ElementViewModel;
            var lookup = context.GetService<ElementLookup>();

            if (lookup != null && reference.ScopeType != null)
            {
                targetElements = lookup.FindInstancesOfConfigurationType(reference.ScopeType, reference.TargetType);
                return;
            }

            targetElements = Enumerable.Empty<ElementViewModel>();
        }

    }
}
