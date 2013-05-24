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

#pragma warning disable 618

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.RetryPolicyScenarios.given_always_transient_exception_and_retry_strategy_should_not_retry
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    public abstract class Context : ArrangeActAssert
    {
        protected RetryPolicy retryPolicy;
        protected Mock<RetryStrategy> retryStrategyMock;

        protected override void Arrange()
        {
            this.retryStrategyMock = new Mock<RetryStrategy>("name", false);
            this.retryStrategyMock.Setup(x => x.GetShouldRetry())
                .Returns(
                    () =>
                        delegate(int currentRetryCount, Exception lastException, out TimeSpan interval)
                        {
                            interval = TimeSpan.Zero;
                            return false;
                        }
                    );

            this.retryPolicy = new RetryPolicy<AlwaysTransientErrorDetectionStrategy>(retryStrategyMock.Object);
        }
    }

    [TestClass]
    public class when_executing_action : Context
    {
        private int execCount;

        protected override void Act()
        {
            try
            {
                this.retryPolicy.ExecuteAction(
                    () =>
                    {
                        execCount++;
                        throw new Exception();
                    }
                    );

                Assert.Fail();
            }
            catch
            {

            }
        }

        [TestMethod]
        public void then_does_not_retry()
        {
            Assert.AreEqual(1, this.execCount);
        }
    }

    [TestClass]
    public class when_executing_func : Context
    {
        private int execCount;

        protected override void Act()
        {
            try
            {
                this.retryPolicy.ExecuteAction(
                    () =>
                    {
                        execCount++;
                        throw new Exception();
                    });
            }
            catch
            {

            }
        }

        [TestMethod]
        public void then_does_not_retry()
        {
            Assert.AreEqual(1, this.execCount);
        }
    }

    [TestClass]
    public class when_executing_async : Context
    {
        private int timesStarted;
        private Task<int> task;
        private Exception exception;

        protected override void Act()
        {
            this.task = this.retryPolicy
                .ExecuteAsync(() =>
                {
                    int result = ++timesStarted;
                    return Task.Run<int>((Func<int>)(() => { throw new Exception(); }));
                });

            try
            {
                task.Wait(TimeSpan.FromSeconds(2));
            }
            catch (Exception e)
            {
                this.exception = e;
            }
        }

        [TestMethod]
        public void then_does_not_retry()
        {
            Assert.AreEqual(1, this.timesStarted);
        }

        [TestMethod]
        public void then_is_faulted()
        {
            Assert.IsTrue(this.task.IsFaulted);
        }
    }

    [TestClass]
    public class when_executing_task_wrapped_apm_with_result : Context
    {
        private int successCount;
        private int faultCount;
        private TestAsyncOperation asyncOperation;
        private CountdownEvent countdownEvent;

        protected override void Arrange()
        {
            base.Arrange();

            this.asyncOperation = new TestAsyncOperation(new Exception());

            this.countdownEvent = new CountdownEvent(1);
        }

        protected override void Act()
        {
            var executeAsyncTask = this.retryPolicy
                .ExecuteAsync(
                    () => Task<bool>.Factory.FromAsync(this.asyncOperation.BeginMethod, this.asyncOperation.EndMethod, null));

            executeAsyncTask.ContinueWith(t => successCount++, TaskContinuationOptions.OnlyOnRanToCompletion);
            executeAsyncTask.ContinueWith(t =>
                {
                    if (t.Exception != null)
                    {
                        faultCount++;
                        this.countdownEvent.Signal();
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);

            Assert.IsTrue(this.countdownEvent.Wait(TimeSpan.FromSeconds(2)));
        }

        [TestMethod]
        public void then_does_not_retry()
        {
            Assert.AreEqual(1, this.asyncOperation.BeginMethodCount);
            Assert.AreEqual(1, this.asyncOperation.EndMethodCount);
        }

        [TestMethod]
        public void then_success_action_was_not_called()
        {
            Assert.AreEqual(0, this.successCount);
        }

        [TestMethod]
        public void then_fault_handler_was_called()
        {
            Assert.AreEqual(1, this.faultCount);
        }
    }
}
