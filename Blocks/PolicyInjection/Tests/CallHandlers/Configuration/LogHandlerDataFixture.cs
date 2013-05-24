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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenALogCallHandlerDataWithBeforeAndAfterBehavior
    {
        private CallHandlerData callHandlerData;
        private LogWriter logWriter;

        [TestInitialize]
        public void Setup()
        {
            this.callHandlerData =
                new LogCallHandlerData("logging")
                {
                    Order = 400,

                    LogBehavior = HandlerLogBehavior.BeforeAndAfter,
                    BeforeMessage = "before",
                    AfterMessage = "after",
                    EventId = 1000,
                    IncludeCallStack = true,
                    IncludeCallTime = false,
                    IncludeParameterValues = true,
                    Priority = 500,
                    Severity = TraceEventType.Warning,
                    Categories = 
                    { 
                        new LogCallHandlerCategoryEntry("cat1"), 
                        new LogCallHandlerCategoryEntry("cat2"), 
                        new LogCallHandlerCategoryEntry("cat3")
                    }
                };

            this.logWriter = new LogWriter(new LoggingConfiguration());
            Logger.SetLogWriter(this.logWriter, false);
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenCanResolveCallHandler()
        {
            using (var container = new UnityContainer())
            {
                this.callHandlerData.ConfigureContainer(container, "-suffix");

                var handler = (LogCallHandler)container.Resolve<ICallHandler>("logging-suffix");

                Assert.AreEqual(400, handler.Order);
                Assert.AreEqual(true, handler.LogBeforeCall);
                Assert.AreEqual(true, handler.LogAfterCall);
                Assert.AreEqual("before", handler.BeforeMessage);
                Assert.AreEqual("after", handler.AfterMessage);
                Assert.AreEqual(1000, handler.EventId);
                Assert.AreEqual(true, handler.IncludeCallStack);
                Assert.AreEqual(false, handler.IncludeCallTime);
                Assert.AreEqual(true, handler.IncludeParameters);
                Assert.AreEqual(500, handler.Priority);
                Assert.AreEqual(TraceEventType.Warning, handler.Severity);
                CollectionAssert.AreEqual(new[] { "cat1", "cat2", "cat3" }, handler.Categories);

                Assert.AreNotSame(handler, container.Resolve<ICallHandler>("logging-suffix"));
            }
        }
    }

    [TestClass]
    public class GivenALogCallHandlerDataWithBeforeBehavior
    {
        private CallHandlerData callHandlerData;
        private LogWriter logWriter;

        [TestInitialize]
        public void Setup()
        {
            callHandlerData =
                new LogCallHandlerData("logging")
                {
                    Order = 400,

                    LogBehavior = HandlerLogBehavior.Before,
                    BeforeMessage = "before",
                    AfterMessage = "after",
                    EventId = 1000,
                    IncludeCallStack = true,
                    IncludeCallTime = false,
                    IncludeParameterValues = true,
                    Priority = 500,
                    Severity = TraceEventType.Warning,
                    Categories = 
                        { 
                            new LogCallHandlerCategoryEntry("cat1"), 
                            new LogCallHandlerCategoryEntry("cat2"), 
                            new LogCallHandlerCategoryEntry("cat3")
                        }
                };

            this.logWriter = new LogWriter(new LoggingConfiguration());
            Logger.SetLogWriter(this.logWriter, false);
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenCanResolveCallHandler()
        {
            using (var container = new UnityContainer())
            {
                this.callHandlerData.ConfigureContainer(container, "-suffix");

                var handler = (LogCallHandler)container.Resolve<ICallHandler>("logging-suffix");

                Assert.AreEqual(400, handler.Order);
                Assert.AreEqual(true, handler.LogBeforeCall);
                Assert.AreEqual(false, handler.LogAfterCall);
                Assert.AreEqual("before", handler.BeforeMessage);
                Assert.AreEqual("after", handler.AfterMessage);
                Assert.AreEqual(1000, handler.EventId);
                Assert.AreEqual(true, handler.IncludeCallStack);
                Assert.AreEqual(false, handler.IncludeCallTime);
                Assert.AreEqual(true, handler.IncludeParameters);
                Assert.AreEqual(500, handler.Priority);
                Assert.AreEqual(TraceEventType.Warning, handler.Severity);
                CollectionAssert.AreEqual(new[] { "cat1", "cat2", "cat3" }, handler.Categories);

                Assert.AreNotSame(handler, container.Resolve<ICallHandler>("logging-suffix"));
            }
        }
    }

    [TestClass]
    public class GivenALogCallHandlerDataWithAfterBehavior
    {
        private CallHandlerData callHandlerData;
        private LogWriter logWriter;

        [TestInitialize]
        public void Setup()
        {
            callHandlerData =
                new LogCallHandlerData("logging")
                {
                    Order = 400,

                    LogBehavior = HandlerLogBehavior.After,
                    BeforeMessage = "before",
                    AfterMessage = "after",
                    EventId = 1000,
                    IncludeCallStack = true,
                    IncludeCallTime = false,
                    IncludeParameterValues = true,
                    Priority = 500,
                    Severity = TraceEventType.Warning,
                    Categories = 
                    { 
                        new LogCallHandlerCategoryEntry("cat1"), 
                        new LogCallHandlerCategoryEntry("cat2"), 
                        new LogCallHandlerCategoryEntry("cat3")
                    }
                };

            this.logWriter = new LogWriter(new LoggingConfiguration());
            Logger.SetLogWriter(this.logWriter, false);
        }

        [TestMethod]
        public void WhenConfiguredContainer_ThenCanResolveCallHandler()
        {
            using (var container = new UnityContainer())
            {
                this.callHandlerData.ConfigureContainer(container, "-suffix");

                var handler = (LogCallHandler)container.Resolve<ICallHandler>("logging-suffix");

                Assert.AreEqual(400, handler.Order);
                Assert.AreEqual(false, handler.LogBeforeCall);
                Assert.AreEqual(true, handler.LogAfterCall);
                Assert.AreEqual("before", handler.BeforeMessage);
                Assert.AreEqual("after", handler.AfterMessage);
                Assert.AreEqual(1000, handler.EventId);
                Assert.AreEqual(true, handler.IncludeCallStack);
                Assert.AreEqual(false, handler.IncludeCallTime);
                Assert.AreEqual(true, handler.IncludeParameters);
                Assert.AreEqual(500, handler.Priority);
                Assert.AreEqual(TraceEventType.Warning, handler.Severity);
                CollectionAssert.AreEqual(new[] { "cat1", "cat2", "cat3" }, handler.Categories);

                Assert.AreNotSame(handler, container.Resolve<ICallHandler>("logging-suffix"));
            }
        }
    }
}