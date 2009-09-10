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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Tests.Configuration
{

    public abstract class Given_ExceptionTypeInConfigurationSourceBuilder : ArrangeActAssert
    {
        protected ConfigurationSourceBuilder configurationSourceBuilder;
        protected IExceptionConfigurationForExceptionType policy;
        protected IExceptionConfigurationAddExceptionHandlers exception;
        protected Type exceptionType = typeof(ArgumentException);

        protected string policyName = "Some Policy";

        protected override void Arrange()
        {

            configurationSourceBuilder = new ConfigurationSourceBuilder();
            policy = configurationSourceBuilder
                        .ConfigureExceptionHandling()
                            .GivenPolicyWithName(policyName);

            exception = policy.ForExceptionType(exceptionType);
        }

        protected ExceptionPolicyData GetExceptionPolicyData()
        {
            var source = new DictionaryConfigurationSource();
            configurationSourceBuilder.UpdateConfigurationWithReplace(source);

            return ((ExceptionHandlingSettings)source.GetSection(ExceptionHandlingSettings.SectionName))
                .ExceptionPolicies.Get(policyName);
        }

        protected ExceptionTypeData GetExceptionTypeData()
        {
            return GetExceptionPolicyData().ExceptionTypes.Where(x => x.Type == exceptionType).First();
        }
    }

    [TestClass]
    public class When_AddingLogExceptionHandlerToExceptionTypePassingNullForCategory : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_LogToCategory_ThrowsArgumentException()
        {
            exception.LogToCategory(null);
        }
    }

    [TestClass]
    public class When_AddingLogExceptionHandlerToExceptionType : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            exception.LogToCategory("Category");
        }

        [TestMethod]
        public void Then_ExceptionTypeContainsLoggingHandler()
        {
            Assert.IsTrue(
                base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType<LoggingExceptionHandlerData>()
                .Any());
        }


        [TestMethod]
        public void Then_ExceptionTypeHasDefaultExceptionTypeformatter()
        {
            var logExceptionHandler = base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType <LoggingExceptionHandlerData>()
                .First();

            Assert.AreEqual(typeof(TextExceptionFormatter), logExceptionHandler.FormatterType);
        }


        [TestMethod]
        public void Then_LoggingHandlerHasDefaultSeverity()
        {
            var logExceptionHandler = base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType<LoggingExceptionHandlerData>()
                .First();

            Assert.AreEqual(TraceEventType.Error, logExceptionHandler.Severity);
        }


        [TestMethod]
        public void Then_LoggingHandlerHasDefaultTitle()
        {
            var logExceptionHandler = base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType<LoggingExceptionHandlerData>()
                .First();

            Assert.AreEqual("Enterprise Library Exception Handling", logExceptionHandler.Title);
        }



        [TestMethod]
        public void Then_LoggingHandlerHasDefaultEventId()
        {
            var logExceptionHandler = base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType<LoggingExceptionHandlerData>()
                .First();

            Assert.AreEqual(100, logExceptionHandler.EventId);
        }
    }


    [TestClass]
    public class When_AddingLogExceptionHandlerToExceptionTypeAndOverridingAllValues : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        LoggingExceptionHandlerData loggingHandlerData;

        protected override void Arrange()
        {
            base.Arrange();

            exception.LogToCategory("Category")
                        .UsingTitle("title")
                        .UsingEventId(12)
                        .WithPriority(23)
                        .WithSeverity(TraceEventType.Stop)
                        .UsingExceptionFormatter<XmlExceptionFormatter>();

            loggingHandlerData = base.GetExceptionTypeData()
                .ExceptionHandlers
                .OfType<LoggingExceptionHandlerData>()
                .First();

        }

        [TestMethod]
        public void Then_LoggingHandlerOverridenEventId()
        {
            Assert.AreEqual(12, loggingHandlerData.EventId);
        }

        [TestMethod]
        public void Then_LoggingHandlerOverridenPriority()
        {
            Assert.AreEqual(23, loggingHandlerData.Priority);
        }

        [TestMethod]
        public void Then_LoggingHandlerOverridenSeverity()
        {
            Assert.AreEqual(TraceEventType.Stop, loggingHandlerData.Severity);
        }

        [TestMethod]
        public void Then_LoggingHandlerOverridenExceptionFormatter()
        {
            Assert.AreEqual(typeof(XmlExceptionFormatter), loggingHandlerData.FormatterType);
        }

        [TestMethod]
        public void Then_LoggingHandlerOverridenTitle()
        {
            Assert.AreEqual("title", loggingHandlerData.Title);
        }
    }

    [TestClass]
    public class When_SettingTitletoNullForOnLoggingHandler : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_UsingTitle_ThrowsArgumentException()
        {
            exception.LogToCategory("Category")
                        .UsingTitle(null);
        }
    }


    [TestClass]
    public class When_SettingExceptionFormattertoNullForOnLoggingHandler : Given_ExceptionTypeInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_UsingExceptionFormatter_ThrowsArgumentNullException()
        {
            exception.LogToCategory("Category")
                        .UsingExceptionFormatter(null);
        }
    }

}
