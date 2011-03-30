using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.InMemoryCacheDataScenarios.given_default_ctor
{
    [TestClass]
    public class when_calling_default_ctor : Context
    {
        protected override void Act()
        {
            base.Act();

            Data = new InMemoryCacheData();
        }

        [TestMethod]
        public void then_default_values_are_setted_properly()
        {
            Assert.IsNotNull(Data.ExpirationPollingInterval);
            Assert.AreNotEqual(0, Data.MaxItemsBeforeScavenging);
            Assert.AreNotEqual(0, Data.ItemsLeftAfterScavenging);
            Assert.AreEqual(TimeSpan.FromMinutes(2), Data.ExpirationPollingInterval);
        }

    }
}
