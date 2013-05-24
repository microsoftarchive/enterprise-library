using System;
using System.Collections.Specialized;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    internal static class CustomProviderBuildHelper
    {
        public static T Build<T, TC>(TC customData, Func<string> noHandlerType, Func<string> noValidType, Func<string> typeNotAssignable, Func<string> noConstructor)
            where TC : NameTypeConfigurationElement, ICustomProviderData
        {
            if (string.IsNullOrEmpty(customData.TypeName))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, noHandlerType(), customData.ElementInformation.Source, customData.ElementInformation.LineNumber, customData.Name));
            }

            Type providerType;

            try
            {
                providerType = customData.Type;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, noValidType(), customData.ElementInformation.Source, customData.ElementInformation.LineNumber, customData.Name, customData.TypeName),
                    e);
            }

            if (!typeof(T).IsAssignableFrom(providerType))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, typeNotAssignable(), customData.ElementInformation.Source, customData.ElementInformation.LineNumber, customData.Name, customData.TypeName));
            }

            var ctor = providerType.GetConstructor(new[] { typeof(NameValueCollection) });
            if (ctor == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, noConstructor(), customData.ElementInformation.Source, customData.ElementInformation.LineNumber, customData.Name, customData.TypeName));
            }

            return (T)Activator.CreateInstance(providerType, customData.Attributes);
        }
    }
}
