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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.RetryPolicyScenarios.given_retry_limit_exceeded_exception_without_inner_exception
{
    using System;
    using System.Threading;
    using Common.TestSupport.ContextBase;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Moq;
    using VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

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
        private Exception exception;

        protected override void Act()
        {
            try
            {
                this.retryPolicy.ExecuteAction(
                    () =>
                        {
                            execCount++;
                            throw new RetryLimitExceededException(new Exception("my exception"));
                        }
                    );
            }
            catch (Exception e)
            {
                this.exception = e;
            }
        }

        [TestMethod]
        public void then_does_not_retry()
        {
            Assert.AreEqual(1, this.execCount);
        }

        [TestMethod]
        public void then_exception_is_not_retry_limit_exceeded()
        {
            Assert.IsNotInstanceOfType(this.exception, typeof(RetryLimitExceededException));
            Assert.AreEqual("my exception", this.exception.Message);
        }
    }

    [TestClass]
    public class when_executing_func : Context
    {
        private int execCount;
        private Exception exception;

        protected override void Act()
        {
            try
            {
                this.retryPolicy.ExecuteAction<int>(
                    () =>
                    {
                        execCount++;
                        throw new RetryLimitExceededException(new Exception("my exception"));
                    }
                    );
            }
            catch (Exception e)
            {
                this.exception = e;
            }
        }

        [TestMethod]
        public void then_does_not_retry()
        {
            Assert.AreEqual(1, this.execCount);
        }

        [TestMethod]
        public void then_exception_is_not_retry_limit_exceeded()
        {
            Assert.IsNotInstanceOfType(this.exception, typeof(RetryLimitExceededException));
            Assert.AreEqual("my exception", this.exception.Message);
        }
    }

    [TestClass]
    public class when_executing_async : Context
    {
        private int timesStarted;
        private Task task;
        private AggregateException exception;

        protected override void Act()
        {
            this.task = this.retryPolicy
                .ExecuteAsync(() =>
                {
                    int result = ++timesStarted;
                    return Task.Run<int>((Func<int>)(() => { throw new RetryLimitExceededException(new Exception("my exception")); }));
                });

            try
            {
                task.Wait(TimeSpan.FromSeconds(2));
            }
            catch (AggregateException e)
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

        [TestMethod]
        public void then_exception_is_not_retry_limit_exceeded()
        {
            Assert.IsNotInstanceOfType(this.exception.InnerException, typeof(RetryLimitExceededException));
            Assert.AreEqual("my exception", this.exception.InnerException.Message);
        }
    }
}
