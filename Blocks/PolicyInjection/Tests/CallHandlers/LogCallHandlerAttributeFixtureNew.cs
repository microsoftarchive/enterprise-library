//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.PolicyInjection
{
    [TestClass]
    public class LogCallHandlerAttributeFixture
    {
        private IUnityContainer container;
        private LogWriter logWriter;

        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);

            this.logWriter = new LogWriter(new ILogFilter[0], new LogSource[0], new LogSource("name"), "default");
            this.container = new UnityContainer();

            Logger.SetLogWriter(this.logWriter, false);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Logger.Reset();
        }

        [TestMethod]
        public void ShouldCreateDefaultLogHandler()
        {
            LogCallHandlerAttribute attribute = new LogCallHandlerAttribute();
            LogCallHandler handler = GetHandlerFromAttribute(attribute);
            Assert.AreEqual(LogCallHandlerDefaults.EventId, handler.EventId);
            Assert.AreEqual(LogCallHandlerDefaults.AfterMessage, handler.AfterMessage);
            Assert.AreEqual(LogCallHandlerDefaults.BeforeMessage, handler.BeforeMessage);
            Assert.AreEqual(0, handler.Categories.Count);
            Assert.AreEqual(LogCallHandlerDefaults.IncludeCallStack, handler.IncludeCallStack);
            Assert.AreEqual(LogCallHandlerDefaults.IncludeCallTime, handler.IncludeCallTime);
            Assert.AreEqual(LogCallHandlerDefaults.IncludeParameters, handler.IncludeParameters);
            Assert.AreEqual(LogCallHandlerDefaults.LogAfterCall, handler.LogAfterCall);
            Assert.AreEqual(LogCallHandlerDefaults.LogBeforeCall, handler.LogBeforeCall);
            Assert.AreEqual(LogCallHandlerDefaults.Priority, handler.Priority);
            Assert.AreEqual(LogCallHandlerDefaults.Severity, handler.Severity);
        }

        [TestMethod]
        public void ShouldCreateWithMultipleCategories()
        {
            LogCallHandlerAttribute attribute = new LogCallHandlerAttribute();
            attribute.Categories = new string[] { "This", "That", "The Other" };

            LogCallHandler handler = GetHandlerFromAttribute(attribute);
            Assert.AreEqual(attribute.Categories.Length, handler.Categories.Count);
            for (int i = 0; i < attribute.Categories.Length; ++i)
            {
                Assert.AreEqual(attribute.Categories[i], handler.Categories[i]);
            }
        }

        [TestMethod]
        public void ShouldCreateWithEveryOtherSetting()
        {
            LogCallHandlerAttribute attribute = new LogCallHandlerAttribute();
            attribute.EventId = 67;
            attribute.LogBeforeCall = false;
            attribute.LogAfterCall = false;
            attribute.BeforeMessage = "Before call log";
            attribute.AfterMessage = "After call log";
            attribute.Categories = new string[] { "Lots", "of", "categories", "here" };
            attribute.IncludeParameters = false;
            attribute.IncludeCallStack = true;
            attribute.IncludeCallTime = false;
            attribute.Priority = 762;
            attribute.Severity = TraceEventType.Critical;

            LogCallHandler handler = GetHandlerFromAttribute(attribute);

            Assert.AreEqual(attribute.EventId, handler.EventId);
            Assert.AreEqual(attribute.LogBeforeCall, handler.LogBeforeCall);
            Assert.AreEqual(attribute.LogAfterCall, handler.LogAfterCall);
            Assert.AreEqual(attribute.BeforeMessage, handler.BeforeMessage);
            Assert.AreEqual(attribute.AfterMessage, handler.AfterMessage);
            Assert.AreEqual(attribute.Categories.Length, handler.Categories.Count);
            for (int i = 0; i < attribute.Categories.Length; ++i)
            {
                Assert.AreEqual(attribute.Categories[i], handler.Categories[i], "Category mismatch at index {0}", i);
            }
            Assert.AreEqual(attribute.IncludeParameters, handler.IncludeParameters);
            Assert.AreEqual(attribute.IncludeCallStack, handler.IncludeCallStack);
            Assert.AreEqual(attribute.IncludeCallTime, handler.IncludeCallTime);
            Assert.AreEqual(attribute.Priority, handler.Priority);
            Assert.AreEqual(attribute.Severity, handler.Severity);
        }

        LogCallHandler GetHandlerFromAttribute(LogCallHandlerAttribute attribute)
        {
            return (LogCallHandler)attribute.CreateHandler(this.container);
        }
    }
 }
