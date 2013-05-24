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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests
{
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;

    [TestClass]
    public class ConfigurationTests
    {
        [TestInitialize]
        public void Setup()
        {
            RetryPolicyFactory.CreateDefault();
        }

        [TestCleanup]
        public void Cleanup()
        {
            RetryPolicyFactory.SetRetryManager(null, false);
        }

        [Description("F3.1.1; F3.2.3")]
        [Priority(1)]
        [TestMethod]
        [Ignore]    // REVIEW = Negative retry counts are not allowed by configuration.
        public void NegativeRetryCount()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("NegativeRetryCount");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(1, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(0, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.2")]
        [Priority(1)]
        [TestMethod]
        [Ignore]    // REVIEW
        public void ZeroRetryCount()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("ZeroRetryCount");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(1, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(0, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.3")]
        [Priority(1)]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeRetryInterval()
        {
            RetryPolicy negativeRetryCountRetryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("NegativeRetryInterval");
            int execCount = 0;

            try
            {
                negativeRetryCountRetryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Description("F3.1.4")]
        [Priority(1)]
        [TestMethod]
        public void ZeroRetryInterval()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("ZeroRetryInterval");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(0, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.5")]
        [Priority(1)]
        [TestMethod]
        [Ignore]    // REVIEW - Negative values are not allowed by configuration
        public void NegativeRetryIncrement()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("NegativeRetryIncrement");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(270, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.6")]
        [Priority(1)]
        [TestMethod]
        public void ZeroRetryIncrement()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("ZeroRetryIncrement");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(300, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.7")]
        [Priority(1)]
        [TestMethod]
        [Ignore]    // REVIEW - Negative values are not allowed by configuration
        public void NegativeMinBackoff()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("NegativeMinBackoff");
            int execCount = 0;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
        }

        [Description("F3.1.8")]
        [Priority(1)]
        [TestMethod]
        public void ZeroMinBackoff()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("ZeroMinBackoff");
            int execCount = 0;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
        }

        [Description("F3.1.9")]
        [Priority(1)]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeMaxBackoff()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("NegativeMaxBackoff");
            int execCount = 0;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Description("F3.1.10")]
        [Priority(1)]
        [TestMethod]
        public void ZeroMaxBackoff()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("ZeroMaxBackoff");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(0, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.11")]
        [Priority(1)]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeDeltaBackoff()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("NegativeDeltaBackoff");
            int execCount = 0;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Description("F3.1.12")]
        [Priority(1)]
        [TestMethod]
        public void ZeroDeltaBackoff()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("ZeroDeltaBackoff");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(300, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.13")]
        [Priority(1)]
        [TestMethod]
        public void MinBackoffEqualsMax()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("MinBackoffEqualsMax");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(3000, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.14")]
        [Priority(1)]
        [TestMethod]
        [Ignore]    // REVIEW - Scenario not allowed by configuration
        public void MinBackoffGreaterThanMax()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("MinBackoffGreaterThanMax");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(300, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.15")]
        [Priority(1)]
        [Ignore]
        [TestMethod]
        public void LargeDeltaBackoff()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("LargeDeltaBackoff");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(3000, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.16")]
        [Priority(1)]
        [TestMethod]
        public void FixedInterval_MissingRetryInterval()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("FixedInterval_MissingRetryInterval");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(3000, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.17")]
        [Priority(1)]
        [TestMethod]
        public void IncrementalInterval_MissingRetryInterval()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("IncrementalInterval_MissingRetryInterval");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(3150, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.18")]
        [Priority(1)]
        [TestMethod]
        public void ExponentialInterval_MissingMinBackoff()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("ExponentialInterval_MissingMinBackoff");
            int execCount = 0;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
        }

        [Description("F3.1.19")]
        [Priority(1)]
        [TestMethod]
        [Ignore]    // REVIEW - This test has a random component, and since the default delta backoff is no longer 0 we cannot properly test it
        public void ExponentialInterval_MissingMaxBackoff()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("ExponentialInterval_MissingMaxBackoff");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(0, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.20")]
        [Priority(1)]
        [TestMethod]
        [Ignore]    // REVIEW - This test has a random component, and since the default delta backoff is no longer 0 we cannot properly test it
        public void ExponentialInterval_MissingDeltaBackoff()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("ExponentialInterval_MissingDeltaBackoff");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(4, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(300, totalDuration, "Unexpected duration of retry block");
        }

        [Description("F3.1.21")]
        [Priority(1)]
        [TestMethod]
        [Ignore]    // REVIEW - changed behavior - does not fall back to default
        public void NonExist()
        {
            RetryPolicy retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>("NonExist");
            int execCount = 0;
            double totalDuration = 0;

            retryPolicy.Retrying += (sender, args) => totalDuration += args.Delay.TotalMilliseconds;

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(11, execCount, "The action was not executed the expected amount of times");
            Assert.AreEqual(1000, totalDuration, "Unexpected duration of retry block");
        }
    }
}
