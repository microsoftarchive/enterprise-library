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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport
{
    using System;
    using System.Collections.Specialized;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;

    [ConfigurationElementType(typeof(CustomRetryStrategyData))]
    public class TestRetryStrategy : RetryStrategy
    {
        public TestRetryStrategy()
            : base("TestRetryStrategy", true)
        {
            this.CustomProperty = 1;
        }

        public TestRetryStrategy(string name, bool firstFastRetry, NameValueCollection attributes)
            : base(name, firstFastRetry)
        {
            this.CustomProperty = int.Parse(attributes["customProperty"]);
        }

        public int CustomProperty { get; private set; }

        public int ShouldRetryCount { get; private set; }

        public override ShouldRetry GetShouldRetry()
        {
            return delegate(int currentRetryCount, Exception lastException, out TimeSpan interval)
            {
                if (this.CustomProperty == currentRetryCount)
                {
                    interval = TimeSpan.Zero;
                    return false;
                }

                this.ShouldRetryCount++;

                interval = TimeSpan.FromMilliseconds(1);
                return true;
            };
        }
    }
}
