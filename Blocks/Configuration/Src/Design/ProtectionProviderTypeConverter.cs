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
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using System.Configuration.Provider;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    class ProtectionProviderTypeConverter : TypeConverter
    {
        private const string ProtectedConfigurationSectionName = "configProtectedData";

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> encryptionProviders = new List<string>();
            encryptionProviders.Add(Resources.NoProtectionProvider);
            
            foreach (ProviderBase provider in ProtectedConfiguration.Providers)
            {
                encryptionProviders.Add(provider.Name);
            }

            return new StandardValuesCollection(encryptionProviders);
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
