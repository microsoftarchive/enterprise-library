using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Configuration.Provider;
using System.ComponentModel;

namespace Console.Wpf.ComponentModel.Converters
{
    class ProtectionProviderTypeConverter : StringConverter
    {
        public const string NoProtectionValue = "(no protection)"; //todo: needs to be localizable

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> encryptionProviders = new List<string>();
            encryptionProviders.Add(NoProtectionValue);

            foreach (ProviderBase provider in ProtectedConfiguration.Providers)
            {
                encryptionProviders.Add(provider.Name);
            }

            return new System.ComponentModel.TypeConverter.StandardValuesCollection(encryptionProviders);
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
