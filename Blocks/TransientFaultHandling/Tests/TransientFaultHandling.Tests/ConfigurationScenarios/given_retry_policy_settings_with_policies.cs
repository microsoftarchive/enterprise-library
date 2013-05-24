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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.ConfigurationScenarios.given_retry_policy_settings_with_policies
{
    using System;
    using Common.TestSupport.ContextBase;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;

    using VisualStudio.TestTools.UnitTesting;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    public class Context : ArrangeActAssert
    {
        protected RetryPolicyConfigurationSettings settings;

        protected override void Arrange()
        {
            this.settings = new RetryPolicyConfigurationSettings()
            {
                //DefaultRetryStrategy = "defaultPolicy",
                //DefaultSqlConnectionRetryStrategy = "defaultSqlConnectionPolicy",
                //DefaultSqlCommandRetryStrategy = "defaultSqlCommandPolicy",
                //DefaultAzureStorageRetryStrategy = "defaultAzureStoragePolicy",
                //DefaultAzureServiceBusRetryStrategy = "defaultAzureServiceBusStoragePolicy",
                //DefaultAzureCachingRetryStrategy = "defaultAzureCachingStoragePolicy",
                RetryStrategies = 
                {
                    new ExponentialBackoffData()
                    {
                        Name = "first",
                        MaxRetryCount = 1,
                        MinBackoff = TimeSpan.FromMilliseconds(2),
                        MaxBackoff = TimeSpan.FromMilliseconds(3),
                        DeltaBackoff = TimeSpan.FromMilliseconds(4)
                    },
                    new IncrementalData()
                    {
                        Name = "second",
                        MaxRetryCount = 1,
                        InitialInterval = TimeSpan.FromMilliseconds(2),
                        RetryIncrement = TimeSpan.FromMilliseconds(3)
                    },
                    new FixedIntervalData()
                    {
                        Name = "third",
                        MaxRetryCount = 1,
                        RetryInterval = TimeSpan.FromMilliseconds(2)
                    },  
                    new CustomRetryStrategyData(
                        "Test custom retry strategy",
                        "Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport.TestRetryStrategy, Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
                        {
                            Name = "fourth",
                            FirstFastRetry = false,
                            Attributes = { { "customProperty", 10.ToString() } }
                        }
                    }
            };
        }
    }

    [TestClass]
    public class when_getting_retry_manager : Context
    {
        private RetryManager retryManager;

        protected override void Act()
        {
            retryManager = settings.BuildRetryManager();
        }

        [TestMethod]
        public void then_has_4_strategies_defined()
        {
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("first"), typeof(ExponentialBackoff));
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("second"), typeof(Incremental));
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("third"), typeof(FixedInterval));
            Assert.IsInstanceOfType(retryManager.GetRetryStrategy("fourth"), typeof(TestRetryStrategy));
        }

        [TestMethod]
        public void custom_strategy_has_custom_parameters()
        {
            var strategy = ((TestRetryStrategy)retryManager.GetRetryStrategy("fourth"));
            Assert.AreEqual(10, strategy.CustomProperty);
        }
    }
}
