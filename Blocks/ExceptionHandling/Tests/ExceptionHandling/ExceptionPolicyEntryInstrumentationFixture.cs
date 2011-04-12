//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionPolicyEntryInstrumentationFixture
    {
        bool exceptionHandlerExecutedCallbackCalled;
        bool exceptionHandledCallbackCalled;
        ExceptionPolicyEntry policyEntry;

        class TestInstrumentationProvider : IExceptionHandlingInstrumentationProvider
        {
            private readonly ExceptionPolicyEntryInstrumentationFixture outer;

            public TestInstrumentationProvider(ExceptionPolicyEntryInstrumentationFixture outer)
            {
                this.outer = outer;
            }

            public void FireExceptionHandledEvent()
            {
                outer.exceptionHandledCallbackCalled = true;
            }

            public void FireExceptionHandlerExecutedEvent()
            {
                outer.exceptionHandlerExecutedCallbackCalled = true;
            }

            public void FireExceptionHandlingErrorOccurred(string errorMessage)
            {
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            exceptionHandlerExecutedCallbackCalled = false;
            exceptionHandledCallbackCalled = false;

            var handlers = new List<IExceptionHandler> { new MockExceptionHandler() };

            var instrumentationProvider = new TestInstrumentationProvider(this);
            policyEntry = new ExceptionPolicyEntry(typeof(Exception), PostHandlingAction.None, handlers, instrumentationProvider);
        }

        [TestMethod]
        public void ExceptionHandlerExecutedRaisedWhenEachHandlerIsExecuted()
        {
            policyEntry.Handle(new Exception());

            Assert.IsTrue(exceptionHandlerExecutedCallbackCalled);
        }

        [TestMethod]
        public void ExceptionHandledRaisedWhenExceptionSuccessfullyHandled()
        {
            policyEntry.Handle(new ArgumentException());

            Assert.IsTrue(exceptionHandledCallbackCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void ExceptionHandlerInChainReturnsNullThrows()
        {
            var handlers = new List<IExceptionHandler> { new MockReturnNullExceptionHandler() };
            var entry = new ExceptionPolicyEntry(
                typeof(Exception),
                PostHandlingAction.ThrowNewException,
                handlers);
            entry.Handle(new ApplicationException());
        }
    }
}
