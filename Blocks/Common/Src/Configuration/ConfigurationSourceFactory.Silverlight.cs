
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Contains factory methods to create configuration sources.
    /// </summary>
    public class ConfigurationSourceFactory
    {
        /// <summary>
        /// Creates a default configuration source.
        /// </summary>
        /// <returns>The default configuration source.</returns>
        /// <seealso cref="ResourceDictionaryConfigurationSource.CreateDefault"/>
        public static IConfigurationSource Create()
        {
            return ResourceDictionaryConfigurationSource.CreateDefault();
        }
    }
}
