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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.ConfigurationScenarios.given_retry_policy_factory
{
    using System;
    using Common.Configuration;
    using Common.TestSupport.ContextBase;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    using VisualStudio.TestTools.UnitTesting;

    public abstract class Context : ArrangeActAssert
    {
        protected override void Arrange()
        {
            RetryPolicyFactory.SetRetryManager(GetSettings().BuildRetryManager(), false);
        }

        protected override void Teardown()
        {
            RetryPolicyFactory.SetRetryManager(null, false);
        }

        protected abstract RetryPolicyConfigurationSettings GetSettings();
    }

    [TestClass]
    public class when_getting_existing_default_sql_connection_policy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultSqlConnectionRetryPolicy();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                DefaultSqlConnectionRetryStrategy = "defaultSql",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) }, 
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                    new FixedIntervalData("defaultSql") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(retryPolicy);
            Assert.IsInstanceOfType(retryPolicy.ErrorDetectionStrategy, typeof(SqlDatabaseTransientErrorDetectionStrategy));
            Assert.AreEqual("defaultSql", retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_inexisting_default_sql_connection_policy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultSqlConnectionRetryPolicy();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) }, 
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                    new FixedIntervalData("defaultSql") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(retryPolicy);
            Assert.IsInstanceOfType(retryPolicy.ErrorDetectionStrategy, typeof(SqlDatabaseTransientErrorDetectionStrategy));
            Assert.AreEqual("default", retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_existing_default_sql_command_policy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultSqlCommandRetryPolicy();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                DefaultSqlCommandRetryStrategy = "defaultSql",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) }, 
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                    new FixedIntervalData("defaultSql") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(retryPolicy);
            Assert.IsInstanceOfType(retryPolicy.ErrorDetectionStrategy, typeof(SqlDatabaseTransientErrorDetectionStrategy));
            Assert.AreEqual("defaultSql", retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_inexisting_default_sql_command_policy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultSqlCommandRetryPolicy();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) }, 
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                    new FixedIntervalData("defaultSql") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(retryPolicy);
            Assert.IsInstanceOfType(retryPolicy.ErrorDetectionStrategy, typeof(SqlDatabaseTransientErrorDetectionStrategy));
            Assert.AreEqual("default", retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_existing_default_azure_service_bus_policy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultAzureServiceBusRetryPolicy();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                DefaultAzureServiceBusRetryStrategy = "defaultAzureServiceBus",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) }, 
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                    new FixedIntervalData("defaultAzureServiceBus") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(retryPolicy);
            Assert.IsInstanceOfType(retryPolicy.ErrorDetectionStrategy, typeof(ServiceBusTransientErrorDetectionStrategy));
            Assert.AreEqual("defaultAzureServiceBus", retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_inexisting_default_azure_service_bus_policy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultAzureServiceBusRetryPolicy();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) }, 
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                    new FixedIntervalData("defaultAzureServiceBus") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(retryPolicy);
            Assert.IsInstanceOfType(retryPolicy.ErrorDetectionStrategy, typeof(ServiceBusTransientErrorDetectionStrategy));
            Assert.AreEqual("default", retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_existing_default_azure_caching_policy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultAzureCachingRetryPolicy();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                DefaultAzureCachingRetryStrategy = "defaultAzureCaching",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) }, 
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                    new FixedIntervalData("defaultAzureCaching") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(retryPolicy);
            Assert.IsInstanceOfType(retryPolicy.ErrorDetectionStrategy, typeof(CacheTransientErrorDetectionStrategy));
            Assert.AreEqual("defaultAzureCaching", retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_inexisting_default_azure_caching_policy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultAzureCachingRetryPolicy();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) }, 
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                    new FixedIntervalData("defaultAzureCaching") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(retryPolicy);
            Assert.IsInstanceOfType(retryPolicy.ErrorDetectionStrategy, typeof(CacheTransientErrorDetectionStrategy));
            Assert.AreEqual("default", retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_existing_default_azure_storage_policy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultAzureStorageRetryPolicy();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                DefaultAzureStorageRetryStrategy = "defaultAzureStorage",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) }, 
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                    new FixedIntervalData("defaultAzureStorage") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(retryPolicy);
            Assert.IsInstanceOfType(retryPolicy.ErrorDetectionStrategy, typeof(StorageTransientErrorDetectionStrategy));
            Assert.AreEqual("defaultAzureStorage", retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_inexisting_default_azure_storage_policy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetDefaultAzureStorageRetryPolicy();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) }, 
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                    new FixedIntervalData("defaultAzureStorage") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(retryPolicy);
            Assert.IsInstanceOfType(retryPolicy.ErrorDetectionStrategy, typeof(StorageTransientErrorDetectionStrategy));
            Assert.AreEqual("default", retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_retry_policy_with_default_strategy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetRetryPolicy<AlwaysTransientErrorDetectionStrategy>();
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "default",
                RetryStrategies =
                {
                    new FixedIntervalData("default") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(this.retryPolicy);
            Assert.IsInstanceOfType(this.retryPolicy.ErrorDetectionStrategy, typeof(AlwaysTransientErrorDetectionStrategy));
            Assert.AreEqual("default", this.retryPolicy.RetryStrategy.Name);
        }
    }

    [TestClass]
    public class when_getting_retry_policy_with_named_strategy : Context
    {
        private RetryPolicy retryPolicy;

        protected override void Act()
        {
            this.retryPolicy = RetryPolicyFactory.GetRetryPolicy<AlwaysTransientErrorDetectionStrategy>("other");
        }

        protected override RetryPolicyConfigurationSettings GetSettings()
        {
            return new RetryPolicyConfigurationSettings()
            {
                DefaultRetryStrategy = "other",
                RetryStrategies =
                {
                    new FixedIntervalData("other") { MaxRetryCount = 5, RetryInterval = TimeSpan.FromMilliseconds(10) },
                }
            };
        }

        [TestMethod]
        public void then_get_value()
        {
            Assert.IsNotNull(this.retryPolicy);
            Assert.IsInstanceOfType(this.retryPolicy.ErrorDetectionStrategy, typeof(AlwaysTransientErrorDetectionStrategy));
            Assert.AreEqual("other", this.retryPolicy.RetryStrategy.Name);
        }
    }
}