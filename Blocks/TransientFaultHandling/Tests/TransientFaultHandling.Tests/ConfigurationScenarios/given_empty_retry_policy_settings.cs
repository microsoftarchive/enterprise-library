#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.ConfigurationScenarios.given_empty_retry_policy_settings
{
    using Common.TestSupport.ContextBase;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using VisualStudio.TestTools.UnitTesting;

    public class Context : ArrangeActAssert
    {
        protected RetryPolicyConfigurationSettings settings;

        protected override void Arrange()
        {
            settings = new RetryPolicyConfigurationSettings()
                           {
                               //DefaultRetryStrategy = "defaultPolicy",
                               //DefaultSqlConnectionRetryStrategy = "defaultSqlConnectionPolicy",
                               //DefaultSqlCommandRetryStrategy = "defaultSqlCommandPolicy",
                               //DefaultAzureServiceBusRetryStrategy = "defaultAzureServiceBusStoragePolicy",
                               //DefaultAzureCachingRetryStrategy = "defaultAzureCachingStoragePolicy",
                               //DefaultAzureStorageRetryStrategy = "defaultAzureStoragePolicy"
                           };
        }
    }

    [TestClass]
    public class when_building_retry_manager : Context
    {
        private RetryManager retryManager;

        protected override void Act()
        {
            retryManager = settings.BuildRetryManager();
        }

        [TestMethod]
        public void then_is_not_null()
        {
            Assert.IsNotNull(retryManager);
            Assert.IsInstanceOfType(retryManager, typeof(RetryManager));
        }
    }
}
