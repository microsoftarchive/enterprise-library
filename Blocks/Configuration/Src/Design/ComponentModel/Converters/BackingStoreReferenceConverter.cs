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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters
{
    public class BackingStoreReferenceConverter : StringConverter
    {
        private readonly string NoReference = "<none>";
        private readonly string nullBackingStoreName;

        public BackingStoreReferenceConverter(string nullBackingStoreName)
        {
            this.nullBackingStoreName = nullBackingStoreName;
            this.NoReference = Resources.ReferencePropertyNoReference;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string && (string.IsNullOrEmpty((string)value) || 0 == String.Compare(NoReference, (string)value)))
            {
                return nullBackingStoreName;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is string && (string.IsNullOrEmpty((string)value)  || 0 == string.Compare(nullBackingStoreName, (string)value)))
            {
                return NoReference;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
