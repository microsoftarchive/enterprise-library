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

namespace Microsoft.Practices.Unity.Configuration.Design.ComponentModel
{
    public class KnownTypeNameConverter : TypeConverter
    {
        KeyValuePair<string, Type>[] knownTypes;
        public KnownTypeNameConverter(IEnumerable<Type> knownTypes)
        {
            this.knownTypes = knownTypes.Select(x => 
                                new KeyValuePair<string, Type>(
                                        KnownTypeNameConverter.GetDisplayNameFallBackOnLocalName(x), 
                                        x))
                                .ToArray();
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                return knownTypes.Where(x => x.Key == (string)value).Select(x => x.Value).FirstOrDefault();
            }
            return null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is Type)
            {
                return knownTypes.Where(x => x.Value == value).Select(x => x.Key).FirstOrDefault();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(knownTypes.Select(x => x.Value).ToArray());
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        private static string GetDisplayNameFallBackOnLocalName(Type t)
        {
            var displayNameAttribute = TypeDescriptor.GetAttributes(t).OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayNameAttribute != null) return displayNameAttribute.DisplayName;
            return t.Name;
        }
    }
}
