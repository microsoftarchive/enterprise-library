
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    public class ConfigurationSourceFactory
    {
        public static IConfigurationSource Create()
        {
            return ResourceDictionaryConfigurationSource.CreateDefault();
        }
    }
}
