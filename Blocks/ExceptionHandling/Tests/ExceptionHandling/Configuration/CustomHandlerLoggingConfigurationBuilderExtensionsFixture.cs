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
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Configuration
{
    [TestClass]
    public class When_AddingCustomHandlerWithNullTypeToException : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_HandleCustomThrowsArgumentNullException()
        {
            exception.HandleCustom(null);
        }
    }

    [TestClass]
    public class When_AddingCustomHandlerWithNullAttributesToException : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_HandleCustomThrowsArgumentNullException()
        {
            exception.HandleCustom(typeof(MockCustomHandler), null);
        }
    }

    [TestClass]
    public class When_AddingCustomHandlerWithNonHandlerTypeToException : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_HandleCustom_ThrowsArgumentException()
        {
            exception.HandleCustom(typeof(object));
        }
    }



    [TestClass]
    public class When_AddingCustomHandlerToExceptionTypeInConfigurationSourceBuilder : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        private Type customHandlerType = typeof(MockCustomHandler);

        protected override void Act()
        {
            exception.HandleCustom(customHandlerType);
        }

        [TestMethod]
        public void ThenExceptionTypeContainsCustomHandler()
        {
            Assert.IsTrue(GetExceptionTypeData().ExceptionHandlers.Any(x => x.GetType() == typeof(CustomHandlerData)));
        }

        [TestMethod]
        public void ThenCustomHandlerHasSpecifiedType()
        {
            CustomHandlerData customHandler = (CustomHandlerData) GetExceptionTypeData()
                .ExceptionHandlers
                .Where(x => x.GetType() == typeof(CustomHandlerData))
                .First();

            Assert.AreEqual(customHandlerType, customHandler.Type);
        }

        [TestMethod]
        public void ThenCustomHandlerHasNoAttributes()
        {
            CustomHandlerData customHandler = (CustomHandlerData)GetExceptionTypeData()
                .ExceptionHandlers
                .Where(x => x.GetType() == typeof(CustomHandlerData))
                .First();

            Assert.AreEqual(0, customHandler.Attributes.Count);
        }
    }

    [TestClass]
    public class When_AddingCustomHandlerWithAttributesToExceptionTypeInConfigurationSourceBuilder : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        private Type customHandlerType = typeof(MockCustomHandler);
        private NameValueCollection attributes = new NameValueCollection();

        protected override void Act()
        {
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            exception.HandleCustom(customHandlerType, attributes);
        }

        [TestMethod]
        public void ThenCustomHandlerContainsAllAttributes()
        {
            CustomHandlerData customHandler = (CustomHandlerData)GetExceptionTypeData()
                  .ExceptionHandlers
                  .Where(x => x.GetType() == typeof(CustomHandlerData))
                  .First();

            Assert.AreEqual(attributes.Count, customHandler.Attributes.Count);
            foreach (string key in attributes)
            {
                Assert.AreEqual(attributes[key], customHandler.Attributes[key]);
            }
        }
    }

    class MockCustomHandler : IExceptionHandler
    {
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            throw new NotImplementedException();
        }
    }
}
