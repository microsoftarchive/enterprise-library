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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.RetryPolicyScenarios.given_always_transient_exception_without_exception_thrown
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.TestSupport.ContextBase;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Moq;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;
    using VisualStudio.TestTools.UnitTesting;

    public abstract class Context : ArrangeActAssert
    {
        protected RetryPolicy retryPolicy;
        protected Mock<RetryStrategy> retryStrategyMock;

        protected override void Arrange()
        {
            this.retryStrategyMock = new Mock<RetryStrategy>("name", false);
            this.retryPolicy = new RetryPolicy<AlwaysTransientErrorDetectionStrategy>(retryStrategyMock.Object);
        }
    }

    [TestClass]
    public class when_executing_action : Context
    {
        private int execCount;

        protected override void Act()
        {
            this.retryPolicy.ExecuteAction(
                () =>
                    {
                        execCount++;
                    }
                );
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
            this.retryPolicy.ExecuteAction(
                () =>
                    {
                        execCount++;
                        return 0;
                    }
                );
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
                    return Task.Run<int>((Func<int>)(() => result));
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
            Assert.IsTrue(this.task.IsCompleted);
        }
    }

    [TestClass]
    public class when_executing_task_wrapped_apm_with_result : Context
    {
        private int faultCount;
        private int successCount;
        private TestAsyncOperation asyncOperation;
        private CountdownEvent countdownEvent;

        protected override void Arrange()
        {
            base.Arrange();

            this.asyncOperation = new TestAsyncOperation(null);

            this.countdownEvent = new CountdownEvent(1);
        }

        protected override void Act()
        {
            var executeAsyncTask = this.retryPolicy
                .ExecuteAsync(
                    () => Task<bool>.Factory.FromAsync(this.asyncOperation.BeginMethod, this.asyncOperation.EndMethod, null));

            executeAsyncTask.ContinueWith(t =>
            {
                successCount++;
                countdownEvent.Signal();
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            executeAsyncTask.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    faultCount++;
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
        public void then_success_action_was_called()
        {
            Assert.AreEqual(1, this.successCount);
        }

        [TestMethod]
        public void then_fault_handler_was_not_called()
        {
            Assert.AreEqual(0, this.faultCount);
        }
    }
}
