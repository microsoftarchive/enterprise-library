using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.Mocks
{
    class ProtectedConfigurationSource : DesignDictionaryConfigurationSource
    {
        public string protectionProviderNameOnLastCall;
        public int ProtectedAddCallCount;

        public override void Add(string sectionName, System.Configuration.ConfigurationSection configurationSection, string protectionProviderName)
        {
            ProtectedAddCallCount++;
            protectionProviderNameOnLastCall = protectionProviderName;   
        }
    }
}
