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
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionPolicyFixture
    {
        [TestInitialize]
        public void Initialize()
        {
            ExceptionPolicy.SetExceptionManager(new ExceptionPolicyFactory().CreateManager(), false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            ExceptionPolicy.Reset();
            MockExceptionHandler.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandleExceptionWithNullPolicyThrows()
        {
            ExceptionPolicy.HandleException(new UnauthorizedAccessException(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandleExceptionWithNullPolicyThrows2()
        {
            Exception exceptionToThrow;
            ExceptionPolicy.HandleException(new UnauthorizedAccessException(), null, out exceptionToThrow);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleExceptionWithNullExceptionThrows()
        {
            ExceptionPolicy.HandleException(null, "Wrap Policy");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleExceptionWithNullExceptionThrows2()
        {
            Exception exceptionToThrow;
            ExceptionPolicy.HandleException(null, "Wrap Policy", out exceptionToThrow);
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void UndefinedPolicyRequestedThrows()
        {
            ExceptionPolicy.HandleException(new MockException(), "Undefined Policy");
        }

        [TestMethod]
        public void WrapHandlerTest()
        {
            Exception originalException = new ArgumentNullException();
            Exception wrappedException = null;

            try
            {
                ExceptionPolicy.HandleException(originalException, "Wrap Policy");
            }
            catch (Exception ex)
            {
                wrappedException = ex;
            }

            Assert.IsNotNull(wrappedException);
            Assert.AreEqual("Test Message", wrappedException.Message);
            Assert.AreEqual(typeof(ApplicationException), wrappedException.GetType());
            Assert.IsNotNull(wrappedException.InnerException);
        }

        [TestMethod]
        public void ReplaceHandlerTest()
        {
            Exception originalException = new ArgumentNullException();
            Exception replacedException = null;

            try
            {
                ExceptionPolicy.HandleException(originalException, "Replace Policy");
            }
            catch (Exception ex)
            {
                replacedException = ex;
            }

            Assert.IsNotNull(replacedException);
            Assert.AreEqual("Test Message", replacedException.Message);
            Assert.AreEqual(typeof(ApplicationException), replacedException.GetType());
            Assert.IsNull(replacedException.InnerException);
        }

        [TestMethod]
        public void CustomHandlerTest()
        {
            Exception originalException = new ArgumentNullException();
            Exception customException = null;

            try
            {
                ExceptionPolicy.HandleException(originalException, "Custom Policy");
            }
            catch (Exception ex)
            {
                customException = ex;
            }

            Assert.IsNotNull(customException);
            Assert.AreEqual(2, MockExceptionHandler.attributes.Count);
            Assert.AreEqual("32", MockExceptionHandler.attributes["Age"]);
            Assert.AreEqual(typeof(ArgumentNullException), customException.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void InvalidExceptionTypeInConfigurationTest()
        {
            try
            {
                throw new SecurityException("ExceptionType in Config File is not available");
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, "InvalidExceptionTypeInConfiguration");
                if (rethrow)
                {
                    throw;
                }

            }
        }

        [TestMethod]
        public void OutExceptionReturnsExceptionWhenThrowNewException()
        {
            Exception exceptionToThrow;
            ExceptionPolicy.HandleException(new Exception(), "ThrowNewExceptionPolicy", out exceptionToThrow);

            Assert.IsNotNull(exceptionToThrow);
            Assert.AreSame(typeof(ApplicationException), exceptionToThrow.GetType());
        }
    }
}
