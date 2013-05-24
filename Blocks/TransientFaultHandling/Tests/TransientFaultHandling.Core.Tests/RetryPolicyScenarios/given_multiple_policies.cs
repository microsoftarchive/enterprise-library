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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.RetryPolicyScenarios.given_multiple_policies
{
    using System;
    using System.Threading;
    using Common.TestSupport.ContextBase;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using VisualStudio.TestTools.UnitTesting;

    public abstract class Context : ArrangeActAssert
    {
        protected RetryPolicy retryPolicy1;
        protected RetryPolicy retryPolicy2;
        protected int retryCount1;
        protected int retryCount2;
        protected TimeSpan delay1;
        protected TimeSpan delay2;
        protected TimeSpan expectedCompletionTime1;
        protected TimeSpan expectedCompletionTime2;


        protected override void Arrange()
        {
            this.delay1 = TimeSpan.FromSeconds(2);
            this.delay2 = TimeSpan.FromSeconds(.75);
            this.retryCount1 = 3;
            this.retryCount2 = 2;

            for (var i = 1; i < this.retryCount1; i++)
            {
                this.expectedCompletionTime1 = this.expectedCompletionTime1.Add(this.delay1);
            }

            for (var i = 1; i < this.retryCount2; i++)
            {
                this.expectedCompletionTime2 = this.expectedCompletionTime2.Add(this.delay2);
            }

            this.retryPolicy1 = new RetryPolicy<AlwaysTransientErrorDetectionStrategy>(this.retryCount1, this.delay1);
            this.retryPolicy2 = new RetryPolicy<AlwaysTransientErrorDetectionStrategy>(this.retryCount2, this.delay2);
        }
    }

    [TestClass]
    public class when_executing_at_the_same_time_on_different_threads : Context
    {
        DateTime start1 = DateTime.MinValue;
        DateTime end1 = DateTime.MinValue;
        DateTime start2 = DateTime.MinValue;
        DateTime end2 = DateTime.MinValue;

        protected override void Act()
        {
            var countEvent = new CountdownEvent(2);
            var retry2resetEvent = new AutoResetEvent(false);
            var retry1resetEvent = new AutoResetEvent(false);

            this.retryPolicy1.Retrying +=
                (sender, args) =>
                {
                    if (args.CurrentRetryCount == 3)
                    {
                        retry2resetEvent.Set();
                    }
                };

            ThreadPool.QueueUserWorkItem(_ =>
            {
                retry1resetEvent.WaitOne(TimeSpan.FromSeconds(15));
                start1 = DateTime.Now;
                try
                {
                    this.retryPolicy1.ExecuteAction(
                   () =>
                   {
                       throw new Exception();
                   });
                }
                catch
                {
                }
                end1 = DateTime.Now;
                countEvent.Signal();
            });

            ThreadPool.QueueUserWorkItem(_ =>
            {
                retry1resetEvent.Set();
                retry2resetEvent.WaitOne(TimeSpan.FromSeconds(15));
                start2 = DateTime.Now;

                try
                {
                    this.retryPolicy2.ExecuteAction(
                  () =>
                  {
                      throw new Exception();
                  });
                }
                catch
                {
                }
                end2 = DateTime.Now;
                countEvent.Signal();
            });

            Assert.IsTrue(countEvent.Wait(TimeSpan.FromSeconds(15)));
        }

        [TestMethod]
        public void then_second_policy_starts_on_policy_second_retry()
        {
            Assert.IsTrue(start2 - start1 >= this.delay1);
            Assert.IsTrue(start2 - start1 <= this.delay1.Add(TimeSpan.FromMilliseconds(500)));
        }

        [TestMethod]
        public void then_second_policy_ends_before_first_policy()
        {
            Assert.IsTrue(end2 - end1 >= this.expectedCompletionTime2 - this.expectedCompletionTime1);
        }

        [TestMethod]
        public void then_both_policies_finish_in_time()
        {
            Assert.IsTrue(end1 - start1 >= this.expectedCompletionTime1 && end1 - start1 <= this.expectedCompletionTime1.Add(TimeSpan.FromMilliseconds(500)));
            Assert.IsTrue(end2 - start2 >= this.expectedCompletionTime2 && end2 - start2 <= this.expectedCompletionTime2.Add(TimeSpan.FromMilliseconds(500)));
        }
    }
}
