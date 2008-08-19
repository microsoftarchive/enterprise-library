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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionPolicyEntryInstrumentationFixture
    {
        bool exceptionHandlerExecutedCallbackCalled;
        bool exceptionHandledCallbackCalled;
        ExceptionHandlingInstrumentationProvider instrumentationProvider;
        ExceptionPolicyEntry policyEntry;

        [TestInitialize]
        public void SetUp()
        {
            exceptionHandlerExecutedCallbackCalled = false;
            exceptionHandledCallbackCalled = false;

            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new MockExceptionHandler(new NameValueCollection()));
            policyEntry = new ExceptionPolicyEntry(PostHandlingAction.None, handlers);

            instrumentationProvider = new ExceptionHandlingInstrumentationProvider();
            instrumentationProvider.exceptionHandled += new EventHandler<EventArgs>(ExceptionHandledCallback);
            instrumentationProvider.exceptionHandlerExecuted += new EventHandler<EventArgs>(ExceptionHandlerExecutedCallback);

            policyEntry.SetInstrumentationProvider(instrumentationProvider);
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
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new MockReturnNullExceptionHandler());
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                PostHandlingAction.ThrowNewException,
                handlers);
            entry.Handle(new ApplicationException());
        }

        public void ExceptionHandlerExecutedCallback(object sender,
                                                     EventArgs e)
        {
            exceptionHandlerExecutedCallbackCalled = true;
        }

        public void ExceptionHandledCallback(object sender,
                                             EventArgs e)
        {
            exceptionHandledCallbackCalled = true;
        }
    }
}