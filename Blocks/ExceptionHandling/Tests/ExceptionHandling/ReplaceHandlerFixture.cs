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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ReplaceHandlerFixture
    {
        const string message = "message";

        [TestInitialize]
        public void Initialize()
        {
            ExceptionPolicy.SetExceptionManager(new ExceptionPolicyFactory().CreateManager(), false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            ExceptionPolicy.Reset();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandlerThrowsWhenNotReplaceingAnException()
        {
            ReplaceHandler handler = new ReplaceHandler(message, typeof(object));
            handler.HandleException(new ApplicationException(), Guid.NewGuid());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructingWithNullExceptionTypeThrows()
        {
            ReplaceHandler handler = new ReplaceHandler(message, null);
        }

        [TestMethod]
        public void CanWrapException()
        {
            ReplaceHandler handler = new ReplaceHandler(message, typeof(ApplicationException));
            Exception ex = handler.HandleException(new InvalidOperationException(), Guid.NewGuid());

            Assert.AreEqual(typeof(ApplicationException), ex.GetType());
            Assert.AreEqual(typeof(ApplicationException), handler.ReplaceExceptionType);
            Assert.AreEqual(message, ex.Message);
            Assert.IsNull(ex.InnerException);
        }

        [TestMethod]
        public void ReplaceExceptionReturnsLocalizedMessage()
        {
            Exception exceptionToWrap = new Exception();
            Exception thrownException;
            ExceptionPolicy.HandleException(exceptionToWrap, "LocalizedReplacePolicy", out thrownException);

            Assert.AreEqual(Resources.ExceptionMessage, thrownException.Message);
        }
    }
}
