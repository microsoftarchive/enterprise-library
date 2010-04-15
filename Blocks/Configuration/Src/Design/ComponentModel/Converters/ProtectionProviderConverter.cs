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
using System.Configuration;
using System.Configuration.Provider;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters
{
    class ProtectionProviderTypeConverter : StringConverter
    {
        public readonly static string NoProtectionValue = Resources.NoProtectionProviderDisplayName;

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> encryptionProviders = new List<string>();
            encryptionProviders.Add(string.Empty);

            foreach (ProviderBase provider in ProtectedConfiguration.Providers)
            {
                encryptionProviders.Add(provider.Name);
            }

            return new System.ComponentModel.TypeConverter.StandardValuesCollection(encryptionProviders);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (0 == String.Compare(NoProtectionValue, value.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) &&  (string.IsNullOrEmpty((string)value)))
            {
                return NoProtectionValue;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
