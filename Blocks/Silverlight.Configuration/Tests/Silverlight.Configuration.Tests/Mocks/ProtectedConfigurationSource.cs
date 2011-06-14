//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

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
