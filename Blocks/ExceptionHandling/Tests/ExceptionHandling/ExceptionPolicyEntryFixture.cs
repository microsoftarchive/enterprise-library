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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionPolicyEntryFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void ExceptionHandlerInChainReturnsNullThrows()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new MockReturnNullExceptionHandler());
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                typeof(Exception),
                PostHandlingAction.ThrowNewException,
                handlers);
            entry.Handle(new ApplicationException());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructExceptionPolicyEntryWithNullHandlersThrows()
        {
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                typeof(Exception),
                PostHandlingAction.ThrowNewException,
                null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructExceptionPolicyEntryWithNullExceptionTypeThrows()
        {
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                null,
                PostHandlingAction.ThrowNewException,
                new List<IExceptionHandler>());
        }

        [TestMethod]
        public void HandleExceptionWithNoPostHandling()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new WrapHandler("message", typeof(ArgumentException)));
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                typeof(Exception),
                PostHandlingAction.None,
                handlers);
            bool handled = entry.Handle(new ApplicationException());
            Assert.IsFalse(handled);
        }

        [TestMethod]
        public void HandleExceptionWithNotifyRethrow()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new WrapHandler("message", typeof(ArgumentException)));
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                typeof(Exception),
                PostHandlingAction.NotifyRethrow,
                handlers);
            bool handled = entry.Handle(new ApplicationException());
            Assert.IsTrue(handled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandleExceptionWithThrowNewException()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new WrapHandler("message", typeof(ArgumentException)));
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                typeof(Exception),
                PostHandlingAction.ThrowNewException,
                handlers);
            entry.Handle(new InvalidOperationException());
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void HandleExceptionWithBadExceptionHandlerThatThrows()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new MockThrowingExceptionHandler());
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                typeof(Exception),
                PostHandlingAction.None,
                handlers);
            entry.Handle(new ApplicationException());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleExceptionWithNullExceptionThrows()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new WrapHandler("message", typeof(ArgumentException)));
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                typeof(Exception),
                PostHandlingAction.ThrowNewException,
                handlers);
            entry.Handle(null);
        }
    }
}
