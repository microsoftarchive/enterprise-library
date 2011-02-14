using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    public class MockConfigurationSection : ConfigurationSection
    {
        public string TestProperty { get; set; }
    }
}
