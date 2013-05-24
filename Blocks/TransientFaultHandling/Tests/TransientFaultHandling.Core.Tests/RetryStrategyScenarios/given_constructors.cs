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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.RetryStrategyScenarios.given_constructors
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class Context : ArrangeActAssert
    {
    }

    [TestClass]
    public class when_creating_a_fixed_interval_strategy : Context
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void then_throws_if_retry_count_is_negative()
        {
            new FixedInterval("", -1, TimeSpan.FromSeconds(1), true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void then_throws_if_retry_interval_is_negative()
        {
            new FixedInterval("", 1, TimeSpan.FromSeconds(-1), true);
        }
    }
}
