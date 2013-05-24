#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestSupport
{
    internal class PollingHelper
    {
        public static T WaitUntil<T>(Func<T> action, Func<T, bool> condition, TimeSpan timeout)
        {
            var waitTime = (int)Math.Max(timeout.TotalMilliseconds / 20.5d, 150d);
            var deadline = DateTime.UtcNow.Add(timeout);
            while (true)
            {
                T result = action();
                if (condition(result))
                {
                    return result;
                }

                if (DateTime.UtcNow > deadline)
                {
                    Assert.Fail("Timeout time exceeded and condition was not met. Current result: {0}", result);
                }

                Thread.Sleep(waitTime);
            }
        }
    }
}
