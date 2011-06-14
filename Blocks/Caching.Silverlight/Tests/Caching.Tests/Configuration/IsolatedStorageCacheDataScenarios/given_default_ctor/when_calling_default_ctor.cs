//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.IsolatedStorageCacheDataScenarios.given_default_ctor
{
    [TestClass]
    public class when_calling_default_ctor : Context
    {
        protected override void Act()
        {
            base.Act();

            Data = new IsolatedStorageCacheData();
        }

        [TestMethod]
        public void then_default_values_are_setted_properly()
        {
            Assert.AreNotEqual(0, Data.MaxSizeInKilobytes);
            Assert.AreNotEqual(0, Data.PercentOfQuotaUsedBeforeScavenging);
            Assert.AreNotEqual(0, Data.PercentOfQuotaUsedAfterScavenging);
            Assert.IsNotNull(Data.ExpirationPollingInterval);
            Assert.IsNotNull(Data.SerializerType);
        }
    }
}
