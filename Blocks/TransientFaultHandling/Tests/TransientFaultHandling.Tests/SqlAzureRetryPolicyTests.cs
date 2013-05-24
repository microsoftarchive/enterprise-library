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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests
{
    #region Using statements

    using System;
    using System.Diagnostics;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    /// <summary>
    /// Summary description for RetryPolicyTests
    /// </summary>
    [TestClass]
    public class SqlAzureRetryPolicyTests
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

        [TestMethod]
        public void TestNoRetryPolicy()
        {
            RetryPolicy noRetryPolicy = RetryPolicy.NoRetry;
            int execCount = 0;

            try
            {
                noRetryPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new ApplicationException("Forced Exception");
                });
            }
            catch (ApplicationException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(1, execCount, "The action was not executed the expected amount of times");
        }

        [TestMethod]
        public void TestDefaultRetryPolicyWithNonRetryableError()
        {
            RetryPolicy defaultPolicy = RetryPolicyFactory.GetDefaultSqlConnectionRetryPolicy();
            int execCount = 0;

            try
            {
                defaultPolicy.ExecuteAction(() =>
                {
                    execCount++;
                    throw new ApplicationException("Forced Exception");
                });
            }
            catch (ApplicationException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(1, execCount, "The action was not executed the expected amount of times");
        }

        [TestMethod]
        public void TestDefaultRetryPolicyWithRetryableError()
        {
            RetryPolicy defaultPolicy = RetryPolicyFactory.GetDefaultSqlConnectionRetryPolicy();

            RetryPolicyConfigurationSettings retryPolicySettings = RetryPolicyConfigurationSettings.GetRetryPolicySettings(new SystemConfigurationSource());
            var retryStrategyData = retryPolicySettings.RetryStrategies.Get(retryPolicySettings.DefaultSqlConnectionRetryStrategy) as FixedIntervalData;

            int execCount = 0;

            try
            {
                defaultPolicy.ExecuteAction(() =>
                {
                    execCount++;

                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.IsNotNull(retryStrategyData);
            Assert.AreEqual<int>(retryStrategyData.MaxRetryCount, execCount - 1, "The action was not retried using the expected amount of times");
        }

        [TestMethod]
        public void TestBackoffRetryPolicyWithRetryableError()
        {
            const int MaxRetryCount = 4;
            RetryPolicy retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(MaxRetryCount, TimeSpan.FromMilliseconds(500), TimeSpan.FromSeconds(1));
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

            Assert.AreEqual<int>(MaxRetryCount, execCount - 1, "The action was not retried using the expected amount of times");
        }

        [TestMethod]
        public void TestProgressiveIncrementRetryPolicyWithRetryableError()
        {
            const int MaxRetryCount = 4;
            TimeSpan initialInterval = TimeSpan.FromMilliseconds(500);
            TimeSpan increment = TimeSpan.FromMilliseconds(500);

            RetryPolicy retryPolicy =
                new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(
                    new Incremental("", MaxRetryCount, initialInterval, increment, false));

            int execCount = 0;
            TimeSpan totalDelay;
            Stopwatch stopwatch = Stopwatch.StartNew();

            GeneralRetryPolicyTests.TestRetryPolicy(retryPolicy, out execCount, out totalDelay);

            stopwatch.Stop();

            Assert.AreEqual<int>(MaxRetryCount, execCount, "The action was not retried using the expected amount of times");
            Assert.IsTrue(stopwatch.ElapsedMilliseconds >= totalDelay.TotalMilliseconds, "Unexpected duration of retry block");
        }

        [TestMethod]
        public void TestRetryCallback()
        {
            RetryPolicy retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategy.DefaultClientRetryCount);
            int callbackCount = 0;

            retryPolicy.Retrying += (sender, args) =>
            {
                callbackCount++;

                Trace.WriteLine(String.Format("Current Retry Count: {0}", args.CurrentRetryCount));
                Trace.WriteLine(String.Format("Last Exception: {0}", args.LastException.Message));
                Trace.WriteLine(String.Format("Delay (ms): {0}", args.Delay.TotalMilliseconds));
            };

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            Assert.AreEqual<int>(RetryStrategy.DefaultClientRetryCount, callbackCount, "The callback has not been made using the expected amount of times");

        }

        [TestMethod]
        public void TestRetryStrategyInExtensionMethods()
        {
            int maxRetryCount = 5;
            RetryPolicy retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(maxRetryCount, TimeSpan.FromMilliseconds(500));
            int callbackCount = 0;

            retryPolicy.Retrying += (sender, args) =>
            {
                callbackCount++;

                Trace.WriteLine(String.Format("Current Retry Count: {0}", args.CurrentRetryCount));
                Trace.WriteLine(String.Format("Last Exception: {0}", args.LastException.Message));
                Trace.WriteLine(String.Format("Delay (ms): {0}", args.Delay.TotalMilliseconds));
            };

            try
            {
                // Action #1
                retryPolicy.ExecuteAction(() =>
                {
                    SimulateFailure(retryPolicy);
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            // NO LONGER VALID STRATEGY (initial implementation)
            // We expect to call the SimulateFailure method (maxRetryCount + 1) number of times (first pass + maxRetryCount attempts).
            // Each time we call SimulateFailure, we are going to retry for maxRetryCount times. The callback method is shared, hence it will be invoked
            // whenever we retry inside SimulateFailure as well as outside. The outer retry will impose extra hits on the callback method.
            // Therefore we should account for these extra calls and add further maxRetryCount number of attempts. Hence, the formula below.
            // int expectedRetryCount = (maxRetryCount + 1) * maxRetryCount + maxRetryCount;

            int expectedRetryCount = maxRetryCount;

            Assert.AreEqual<int>(expectedRetryCount, callbackCount, "The action was not retried using the expected amount of times");
        }

        [TestMethod]
        public void TestFastFirstRetry()
        {
            RetryPolicy retryPolicy =
                new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(new FixedInterval("", 1, TimeSpan.FromMinutes(10), true));

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    SimulateFailure(retryPolicy);
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            stopwatch.Stop();
            Assert.IsFalse(stopwatch.Elapsed.TotalSeconds > 5, "FastFirstRetry does not seem to work correctly");

            retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(2, TimeSpan.FromSeconds(1));
            stopwatch.Start();

            try
            {
                retryPolicy.ExecuteAction(() =>
                {
                    SimulateFailure(retryPolicy);
                });
            }
            catch (TimeoutException ex)
            {
                Assert.AreEqual("Forced Exception", ex.Message);
            }

            stopwatch.Stop();
            Assert.IsTrue(stopwatch.Elapsed.TotalSeconds >= 1 && stopwatch.Elapsed.TotalSeconds < 2, "FastFirstRetry does not seem to work correctly");
        }

        #region Private methods
        private static void SimulateFailure(RetryPolicy retryPolicy)
        {
            try
            {
                // Action #2
                retryPolicy.ExecuteAction(() =>
                {
                    throw new TimeoutException("Forced Exception");
                });
            }
            catch (Exception ex)
            {
                // By now, we exhausted all retry attempts when establishing a connection. This means we should also
                // stop retrying the SQL command. Should we not do that, we will continue retrying substantially 
                // increasing the command execution time. The strategy here is that we wrap the transient error into an 
                // RetryLimitExceededException and re-throw. This will ensure that the command will not be retried.
#pragma warning disable 0618
                throw new RetryLimitExceededException(ex);
#pragma warning restore 0618
            }
        }
        #endregion
    }
}
