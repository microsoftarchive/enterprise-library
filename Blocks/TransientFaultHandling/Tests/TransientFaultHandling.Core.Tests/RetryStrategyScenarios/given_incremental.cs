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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.RetryStrategyScenarios.given_incremental
{
    using System;
    using Common.TestSupport.ContextBase;

    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    using VisualStudio.TestTools.UnitTesting;

    public abstract class Context : ArrangeActAssert
    {
        protected override void Arrange()
        {
        }
    }

    [TestClass]
    public class when_using_default_values : Context
    {
        protected RetryStrategy retryStrategy;
        protected ShouldRetry shouldRetry;

        protected override void Act()
        {
            this.retryStrategy = new Incremental();
            this.shouldRetry = this.retryStrategy.GetShouldRetry();
        }

        [TestMethod]
        public void then_default_values_are_used()
        {
            Assert.IsNull(retryStrategy.Name);

            TimeSpan delay;
            Assert.IsTrue(shouldRetry(0, null, out delay));
            Assert.AreEqual(TimeSpan.FromSeconds(1), delay);

            Assert.IsTrue(shouldRetry(9, null, out delay));
            Assert.AreEqual(TimeSpan.FromSeconds(10), delay);

            Assert.IsFalse(shouldRetry(10, null, out delay));
            Assert.AreEqual(TimeSpan.Zero, delay);
        }
    }

    [TestClass]
    public class when_using_custom_values : Context
    {
        protected RetryStrategy retryStrategy;
        protected ShouldRetry shouldRetry;

        protected override void Act()
        {
            this.retryStrategy = new Incremental("name", 5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(2));
            this.shouldRetry = this.retryStrategy.GetShouldRetry();
        }

        [TestMethod]
        public void then_default_values_are_used()
        {
            Assert.AreEqual("name", retryStrategy.Name);

            TimeSpan delay;
            Assert.IsTrue(shouldRetry(0, null, out delay));
            Assert.AreEqual(TimeSpan.FromSeconds(5), delay);

            Assert.IsTrue(shouldRetry(4, null, out delay));
            Assert.AreEqual(TimeSpan.FromSeconds(13), delay);

            Assert.IsFalse(shouldRetry(5, null, out delay));
            Assert.AreEqual(TimeSpan.Zero, delay);
        }
    }
}
