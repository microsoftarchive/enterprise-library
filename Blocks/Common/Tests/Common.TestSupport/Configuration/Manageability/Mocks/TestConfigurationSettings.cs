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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks
{
    public class TestConfigurationSettings : ConfigurationSetting
    {
        public TestConfigurationSettings(string value)
        {
            this.Value = value;
        }

        public string Value;

        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            ((TestsConfigurationSection)sourceElement).Value = this.Value;
            return true;	// required by tests
        }

        public override void Publish()
        {
            throw new System.NotImplementedException();
        }

        public override void Revoke()
        {
            throw new System.NotImplementedException();
        }
    }
}
