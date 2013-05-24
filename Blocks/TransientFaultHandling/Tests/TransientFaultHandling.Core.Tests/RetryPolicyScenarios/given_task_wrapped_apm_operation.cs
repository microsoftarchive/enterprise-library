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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.RetryPolicyScenarios.given_task_wrapped_apm_operation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    public class Context : ArrangeActAssert
    {
        protected TestAsyncOperation asyncOperation;
        protected AutoResetEvent asyncOperationFinished;
        protected Mock<ITransientErrorDetectionStrategy> detectionStrategyMock;
        protected RetryPolicy retryPolicy;

        protected override void Arrange()
        {
            base.Arrange();

            this.asyncOperation = new TestAsyncOperation();

            this.asyncOperationFinished = new AutoResetEvent(false);

            this.detectionStrategyMock = new Mock<ITransientErrorDetectionStrategy>();
            this.detectionStrategyMock.Setup(ds => ds.IsTransient(It.Is<Exception>(e => e is InvalidOperationException))).Returns(true);

            this.retryPolicy = new RetryPolicy(this.detectionStrategyMock.Object, 2);
        }

        protected override void Teardown()
        {
            base.Teardown();

            this.asyncOperationFinished.Dispose();
        }

        protected class TestAsyncOperation
        {
            public TestAsyncOperation()
            {
                this.CheckBegin = i => { };
                this.CheckEnd = i => { };
            }

            public int BeginCalls { get; private set; }

            public int EndCalls { get; private set; }

            public Action<int> CheckBegin
            {
                get;
                set;
            }

            public Action<int> CheckEnd
            {
                get;
                set;
            }

            public IAsyncResult BeginAsyncOperation(object value, AsyncCallback c, object state)
            {
                this.CheckBegin(++this.BeginCalls);

                var result = new TestAsyncResult { AsyncState = state, Value = value };

                ThreadPool.QueueUserWorkItem(r => c((IAsyncResult)r), result);

                return result;
            }

            public object EndAsyncOperation(IAsyncResult result)
            {
                var testResult = (TestAsyncResult)result;

                this.CheckEnd(++this.EndCalls);

                return testResult.Value;
            }

            private class TestAsyncResult : IAsyncResult
            {
                public object AsyncState { get; internal set; }

                public WaitHandle AsyncWaitHandle { get { return null; } }

                public bool CompletedSynchronously { get { return false; } }

                public bool IsCompleted { get; internal set; }

                public object Value { get; internal set; }
            }
        }
    }

    [TestClass]
    public class when_invoking_successful_operation : Context
    {
        private object value;
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.value = null;
            this.exception = null;
        }

        protected override void Act()
        {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"))
                .ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        this.value = t.Result; this.asyncOperationFinished.Set();
                    }
                });

            Assert.IsTrue(this.asyncOperationFinished.WaitOne(TimeSpan.FromSeconds(20)));
        }

        [TestMethod]
        public void then_gets_return_value()
        {
            Assert.AreSame(this, this.value);
        }

        [TestMethod]
        public void then_does_not_get_exception()
        {
            Assert.IsNull(this.exception);
        }

        [TestMethod]
        public void then_invokes_begin_once()
        {
            Assert.AreEqual(1, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_once()
        {
            Assert.AreEqual(1, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_no_exceptions_are_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.Never());
        }
    }

    [TestClass]
    public class when_operation_fails_on_begin_with_non_transient_exception : Context
    {
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.exception = null;

            this.asyncOperation.CheckBegin = i => { throw new Exception("non transient"); };
        }

        protected override void Act()
        {
            try
            {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"));
            }
            catch (Exception ex)
            {
                this.exception = ex;
            }
        }

        [TestMethod]
        public void then_gets_synchronous_exception()
        {
            Assert.AreEqual("non transient", this.exception.Message);
        }

        [TestMethod]
        public void then_invokes_begin_once()
        {
            Assert.AreEqual(1, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_zero_times()
        {
            Assert.AreEqual(0, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_one_exception_is_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.Exactly(1));
        }
    }

    [TestClass]
    public class when_operation_fails_on_end_with_non_transient_exception : Context
    {
        private object value;
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.value = null;
            this.exception = null;

            this.asyncOperation.CheckEnd = i => { throw new Exception("non transient"); };
        }

        protected override void Act()
        {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"))
                .ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        this.value = t.Result;
                    }
                    else
                    {
                        this.exception = t.Exception.InnerException;
                    }

                    this.asyncOperationFinished.Set();
                });

            Assert.IsTrue(this.asyncOperationFinished.WaitOne(TimeSpan.FromSeconds(20)));
        }

        [TestMethod]
        public void then_does_not_get_value()
        {
            Assert.IsNull(this.value);
        }

        [TestMethod]
        public void then_gets_exception()
        {
            Assert.AreEqual("non transient", this.exception.Message);
        }

        [TestMethod]
        public void then_invokes_begin_once()
        {
            Assert.AreEqual(1, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_once()
        {
            Assert.AreEqual(1, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_one_exception_is_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.Exactly(1));
        }
    }

    [TestClass]
    public class when_operation_fails_on_begin_with_transient_exception_in_first_attempt : Context
    {
        private object value;
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.value = null;
            this.exception = null;

            this.asyncOperation.CheckBegin = i => { if (i == 1) throw new InvalidOperationException("transient"); };
        }

        protected override void Act()
        {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"))
                .ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        this.value = t.Result;
                    }
                    else
                    {
                        this.exception = t.Exception.InnerException;
                    }

                    this.asyncOperationFinished.Set();
                });

            Assert.IsTrue(this.asyncOperationFinished.WaitOne(TimeSpan.FromSeconds(20)));
        }

        [TestMethod]
        public void then_gets_value()
        {
            Assert.AreSame(this, this.value);
        }

        [TestMethod]
        public void then_does_not_get_exception()
        {
            Assert.IsNull(this.exception);
        }

        [TestMethod]
        public void then_invokes_begin_twice()
        {
            Assert.AreEqual(2, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_once()
        {
            Assert.AreEqual(1, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_one_exception_is_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.AtLeast(1));
        }
    }

    [TestClass]
    public class when_operation_fails_on_begin_with_transient_exception_in_first_and_second_attempts : Context
    {
        private object value;
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.value = null;
            this.exception = null;

            this.asyncOperation.CheckBegin = i => { if (i <= 2) throw new InvalidOperationException("transient"); };
        }

        protected override void Act()
        {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"))
                .ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        this.value = t.Result;
                    }
                    else
                    {
                        this.exception = t.Exception.InnerException;
                    }

                    this.asyncOperationFinished.Set();
                });

            Assert.IsTrue(this.asyncOperationFinished.WaitOne(TimeSpan.FromSeconds(20)));
        }

        [TestMethod]
        public void then_gets_value()
        {
            Assert.AreSame(this, this.value);
        }

        [TestMethod]
        public void then_does_not_get_exception()
        {
            Assert.IsNull(this.exception);
        }

        [TestMethod]
        public void then_invokes_begin_three_times()
        {
            Assert.AreEqual(3, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_once()
        {
            Assert.AreEqual(1, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_two_exceptions_are_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.AtLeast(2));
        }
    }

    [TestClass]
    public class when_operation_fails_on_begin_with_transient_exception_in_all_attempts : Context
    {
        private object value;
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.value = null;
            this.exception = null;

            this.asyncOperation.CheckBegin = i => { if (i <= 3) throw new InvalidOperationException("transient"); };
        }

        protected override void Act()
        {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"))
                .ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        this.value = t.Result;
                    }
                    else
                    {
                        this.exception = t.Exception.InnerException;
                    }

                    this.asyncOperationFinished.Set();
                });

            Assert.IsTrue(this.asyncOperationFinished.WaitOne(TimeSpan.FromSeconds(20)));
        }

        [TestMethod]
        public void then_does_not_gets_value()
        {
            Assert.IsNull(this.value);
        }

        [TestMethod]
        public void then_gets_exception()
        {
            Assert.AreEqual("transient", this.exception.Message);
        }

        [TestMethod]
        public void then_invokes_begin_three_times()
        {
            Assert.AreEqual(3, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_zero_times()
        {
            Assert.AreEqual(0, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_three_exceptions_are_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.AtLeast(3));
        }
    }

    [TestClass]
    public class when_operation_fails_on_end_with_transient_exception_in_first_attempt : Context
    {
        private object value;
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.value = null;
            this.exception = null;

            this.asyncOperation.CheckEnd = i => { if (i == 1) throw new InvalidOperationException("transient"); };
        }

        protected override void Act()
        {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"))
                .ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        this.value = t.Result;
                    }
                    else
                    {
                        this.exception = t.Exception.InnerException;
                    }

                    this.asyncOperationFinished.Set();
                });

            Assert.IsTrue(this.asyncOperationFinished.WaitOne(TimeSpan.FromSeconds(20)));
        }

        [TestMethod]
        public void then_gets_value()
        {
            Assert.AreSame(this, this.value);
        }

        [TestMethod]
        public void then_does_not_get_exception()
        {
            Assert.IsNull(this.exception);
        }

        [TestMethod]
        public void then_invokes_begin_twice()
        {
            Assert.AreEqual(2, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_twice()
        {
            Assert.AreEqual(2, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_one_exception_is_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.Exactly(1));
        }
    }

    [TestClass]
    public class when_operation_fails_on_end_with_transient_exception_in_first_and_second_attempts : Context
    {
        private object value;
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.value = null;
            this.exception = null;

            this.asyncOperation.CheckEnd = i => { if (i <= 2) throw new InvalidOperationException("transient"); };
        }

        protected override void Act()
        {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"))
                .ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        this.value = t.Result;
                    }
                    else
                    {
                        this.exception = t.Exception.InnerException;
                    }

                    this.asyncOperationFinished.Set();
                });

            Assert.IsTrue(this.asyncOperationFinished.WaitOne(TimeSpan.FromSeconds(20)));
        }

        [TestMethod]
        public void then_gets_value()
        {
            Assert.AreSame(this, this.value);
        }

        [TestMethod]
        public void then_does_not_get_exception()
        {
            Assert.IsNull(this.exception);
        }

        [TestMethod]
        public void then_invokes_begin_three_times()
        {
            Assert.AreEqual(3, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_three_times()
        {
            Assert.AreEqual(3, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_two_exceptions_are_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.Exactly(2));
        }
    }

    [TestClass]
    public class when_operation_fails_on_end_with_transient_exception_in_all_attempts : Context
    {
        private object value;
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.value = null;
            this.exception = null;

            this.asyncOperation.CheckEnd = i => { if (i <= 3) throw new InvalidOperationException("transient"); };
        }

        protected override void Act()
        {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"))
                .ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        this.value = t.Result;
                    }
                    else
                    {
                        this.exception = t.Exception.InnerException;
                    }

                    this.asyncOperationFinished.Set();
                });

            Assert.IsTrue(this.asyncOperationFinished.WaitOne(TimeSpan.FromSeconds(20)));
        }

        [TestMethod]
        public void then_does_not_gets_value()
        {
            Assert.IsNull(this.value);
        }

        [TestMethod]
        public void then_gets_exception()
        {
            Assert.AreEqual("transient", this.exception.Message);
        }

        [TestMethod]
        public void then_invokes_begin_three_times()
        {
            Assert.AreEqual(3, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_three_times()
        {
            Assert.AreEqual(3, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_three_exceptions_are_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.Exactly(3));
        }
    }

    [TestClass]
    public class when_operation_fails_on_second_begin_with_non_transient_exception : Context
    {
        private object value;
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.value = null;
            this.exception = null;

            this.asyncOperation.CheckEnd = i => { if (i == 1) throw new InvalidOperationException("transient"); };
            this.asyncOperation.CheckBegin = i => { if (i == 2) throw new Exception("non transient"); };
        }

        protected override void Act()
        {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"))
                .ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        this.value = t.Result;
                    }
                    else
                    {
                        this.exception = t.Exception.InnerException;
                    }

                    this.asyncOperationFinished.Set();
                });

            Assert.IsTrue(this.asyncOperationFinished.WaitOne(TimeSpan.FromSeconds(20)));
        }

        [TestMethod]
        public void then_does_not_gets_value()
        {
            Assert.IsNull(this.value);
        }

        [TestMethod]
        public void then_gets_exception()
        {
            Assert.AreEqual("non transient", this.exception.Message);
        }

        [TestMethod]
        public void then_invokes_begin_two_times()
        {
            Assert.AreEqual(2, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_one_time()
        {
            Assert.AreEqual(1, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_two_exceptions_are_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.Exactly(2));
        }
    }

    [TestClass]
    public class when_operation_fails_on_second_end_with_non_transient_exception : Context
    {
        private object value;
        private Exception exception;

        protected override void Arrange()
        {
            base.Arrange();

            this.value = null;
            this.exception = null;

            this.asyncOperation.CheckEnd =
                i =>
                {
                    if (i == 1) throw new InvalidOperationException("transient");
                    if (i == 2) throw new Exception("non transient");
                };
        }

        protected override void Act()
        {
            this.retryPolicy
                .ExecuteAsync(
                    () => Task<object>.Factory.FromAsync(
                        this.asyncOperation.BeginAsyncOperation,
                        this.asyncOperation.EndAsyncOperation,
                        this,
                        "state"))
                .ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        this.value = t.Result;
                    }
                    else
                    {
                        this.exception = t.Exception.InnerException;
                    }

                    this.asyncOperationFinished.Set();
                });

            Assert.IsTrue(this.asyncOperationFinished.WaitOne(TimeSpan.FromSeconds(20)));
        }

        [TestMethod]
        public void then_does_not_gets_value()
        {
            Assert.IsNull(this.value);
        }

        [TestMethod]
        public void then_gets_exception()
        {
            Assert.AreEqual("non transient", this.exception.Message);
        }

        [TestMethod]
        public void then_invokes_begin_two_times()
        {
            Assert.AreEqual(2, this.asyncOperation.BeginCalls);
        }

        [TestMethod]
        public void then_invokes_end_two_times()
        {
            Assert.AreEqual(2, this.asyncOperation.EndCalls);
        }

        [TestMethod]
        public void then_two_exceptions_are_tested_for_transient()
        {
            this.detectionStrategyMock.Verify(ds => ds.IsTransient(It.IsAny<Exception>()), Times.AtLeast(2));
        }
    }
}