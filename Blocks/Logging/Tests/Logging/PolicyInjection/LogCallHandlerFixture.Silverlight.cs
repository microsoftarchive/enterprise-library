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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.PolicyInjection
{
    [TestClass]
    public class LogCallHandlerFixture
    {
        LogWriter writer;
        LogCallHandler callHandler;
        private NotificationTraceListener Listener;
        private IList<TraceLogEntry> LogEntriesMessagesSent;
        private LoggingTarget TestTarget;

        const string beforeMessage = "@@@ Logging before calling target @@@";
        const string afterMessage = "@@@ Logging after calling target @@@";

        private const string TestListenerName = "listenerName";

        [TestInitialize]
        public void SetupLogWriter()
        {
            LogEntriesMessagesSent = new List<TraceLogEntry>();

            var traceDispatcher = new Mock<ITraceDispatcher>();
            traceDispatcher.Setup(
                x =>
                x.ReceiveTrace(It.IsAny<TraceEventCache>(), It.IsAny<string>(), It.IsAny<TraceEventType>(),
                               It.IsAny<int>(), It.IsAny<object>(), It.IsAny<string>()))
                .Callback(
                    (TraceEventCache traceEventCache, string source, TraceEventType traceEventType, int id, object data,
                     string tag) =>
                    LogEntriesMessagesSent.Add((TraceLogEntry)data));

            Listener = new NotificationTraceListener(traceDispatcher.Object) { Name = TestListenerName };

            var logSource = new LogSource("Logging");
            logSource.Listeners.Add(Listener);

            writer = new LogWriterImpl(new ILogFilter[0], new[] {logSource},
                                       logSource, logSource, logSource, "General", true, true);

            TestTarget = CreateTarget();
        }

        [TestMethod]
        public void CallGetsLogged()
        {
            Assert.AreEqual(0, LogEntriesMessagesSent.Count);

            TestTarget.DoSomething(1, "two", 3.0);

            Assert.AreEqual(2, LogEntriesMessagesSent.Count);
        }

        [TestMethod]
        public void ShouldLogBeforeAndAfter()
        {
            TestTarget.DoSomething(3, "Not two", 6.02e23);

            Assert.IsTrue(LogEntriesMessagesSent.Any(x => x.Message == beforeMessage));
            Assert.IsTrue(LogEntriesMessagesSent.Any(x => x.Message == afterMessage));
        }

        [TestMethod]
        public void ShouldLogBeforeOnly()
        {
            callHandler.LogAfterCall = false;

            TestTarget.DoSomething(5, "Why", 42.0);

            Assert.IsTrue(LogEntriesMessagesSent.Any(x => x.Message == beforeMessage));
            Assert.IsFalse(LogEntriesMessagesSent.Any(x => x.Message == afterMessage));
        }

        [TestMethod]
        public void ShouldLogAfterOnly()
        {
            callHandler.LogBeforeCall = false;

            TestTarget.DoSomething(5, "Why", 42.0);

            Assert.IsFalse(LogEntriesMessagesSent.Any(x => x.Message == beforeMessage));
            Assert.IsTrue(LogEntriesMessagesSent.Any(x => x.Message == afterMessage));
        }

        [TestMethod]
        public void ShouldLogReturnValue()
        {
            callHandler.LogBeforeCall = false;

            int one = 5;
            string two = "xy";
            double three = 8.0;

            TestTarget.DoSomething(one, two, three);

            Assert.IsTrue(
                LogEntriesMessagesSent.All(x => x.ReturnValue.Equals(string.Format("{1}: {0} {2}", one, two, three))));
        }

        [TestMethod]
        public void ShouldIncludeParametersByDefault()
        {
            int one = 5;
            string two = "xy";
            double three = 8.0;

            TestTarget.DoSomething(one, two, three);

#if SILVERLIGHT
            Assert.IsTrue(LogEntriesMessagesSent.All(x => x.ExtendedProperties["param-one"].Equals(5.ToString())));
            Assert.IsTrue(LogEntriesMessagesSent.All(x => x.ExtendedProperties["param-two"].Equals("xy")));
            Assert.IsTrue(LogEntriesMessagesSent.All(x => x.ExtendedProperties["param-three"].Equals(8.0.ToString())));
#else
            Assert.IsTrue(LogEntriesMessagesSent.All(x => x.ExtendedProperties["one"].Equals(5)));
            Assert.IsTrue(LogEntriesMessagesSent.All(x => x.ExtendedProperties["two"].Equals("xy")));
            Assert.IsTrue(LogEntriesMessagesSent.All(x => x.ExtendedProperties["three"].Equals(8.0)));
#endif
        }

        [TestMethod]
        public void ShouldExcludeParameters()
        {
            callHandler.IncludeParameters = false;

            int one = 5;
            string two = "xy";
            double three = 8.0;

            TestTarget.DoSomething(one, two, three);

#if SILVERLIGHT
            Assert.IsFalse(LogEntriesMessagesSent.Any(x => x.ExtendedProperties.ContainsKey("param-one")));
            Assert.IsFalse(LogEntriesMessagesSent.Any(x => x.ExtendedProperties.ContainsKey("param-two")));
            Assert.IsFalse(LogEntriesMessagesSent.Any(x => x.ExtendedProperties.ContainsKey("param-three")));
#else
            Assert.IsFalse(LogEntriesMessagesSent.Any(x => x.ExtendedProperties.ContainsKey("one")));
            Assert.IsFalse(LogEntriesMessagesSent.Any(x => x.ExtendedProperties.ContainsKey("two")));
            Assert.IsFalse(LogEntriesMessagesSent.Any(x => x.ExtendedProperties.ContainsKey("three")));
#endif
        }

        [TestMethod]
        public void ShouldNotIncludeCallStackByDefault()
        {
            TestTarget.DoSomething(5, "xy", 8.0);

            Assert.IsTrue(LogEntriesMessagesSent.All(x => string.IsNullOrEmpty(x.CallStack)));
        }

        [TestMethod]
        public void ShouldIncludeCallStackBefore()
        {
            callHandler.IncludeCallStack = true;
            callHandler.LogBeforeCall = true;
            callHandler.LogAfterCall = false;

            TestTarget.DoSomething(2, "foo", 42.0);

            Assert.IsTrue(LogEntriesMessagesSent.All(x => !string.IsNullOrEmpty(x.CallStack)));
        }

        [TestMethod]
        public void ShouldIncludeCallStackAfter()
        {
            callHandler.IncludeCallStack = true;
            callHandler.LogBeforeCall = false;
            callHandler.LogAfterCall = true;

            TestTarget.DoSomething(2, "foo", 42.0);

            Assert.IsTrue(LogEntriesMessagesSent.All(x => !string.IsNullOrEmpty(x.CallStack)));
        }

        [TestMethod]
        public void ShouldIncludeCallTimeByDefault()
        {
            callHandler.LogBeforeCall = false;
            callHandler.LogAfterCall = true;

            TestTarget.DoSomething(3, "bar", 43.2);

            Assert.IsTrue(LogEntriesMessagesSent.All(x => x.CallTime.HasValue));
        }

        [TestMethod]
        public void ShouldBeAbleToTurnCallTimeOff()
        {
            callHandler.LogBeforeCall = false;
            callHandler.LogAfterCall = true;

            callHandler.IncludeCallTime = false;

            TestTarget.DoSomething(5, "zzz", 6.02e23);

            Assert.IsFalse(LogEntriesMessagesSent.Any(x => x.CallTime.HasValue));
        }

        [TestMethod]
        public void ShouldBeAbleToRetrieveProperCallTime()
        {
            callHandler.LogBeforeCall = true;
            callHandler.LogAfterCall = true;

            callHandler.IncludeCallTime = true;

            TestTarget.DoSomething(5, "zzz", 6.02e23);
            
            Assert.IsTrue(LogEntriesMessagesSent[0].CallTime.GetValueOrDefault().Ticks == 0);
            Assert.IsTrue(LogEntriesMessagesSent[1].CallTime.GetValueOrDefault().Ticks > 0);
        }

        [TestMethod]
        public void ShouldBeAbleToSetPriority()
        {
            callHandler.Priority = 15;

            TestTarget.DoSomething(14, "yyy", 543.0);

            Assert.IsTrue(LogEntriesMessagesSent.All(x => x.Priority == 15));
        }

        [TestMethod]
        public void ShouldbeAbleToSetSeverity()
        {
            callHandler.Severity = TraceEventType.Critical;

            TestTarget.DoSomething(15, "xxx", 654.0);

            Assert.IsTrue(LogEntriesMessagesSent.All(x => x.Severity == TraceEventType.Critical));
        }

        [TestMethod]
        public void ShouldDefaultSeverityToInformation()
        {
            TestTarget.DoSomething(15, "xxx", 654.0);

            Assert.IsTrue(LogEntriesMessagesSent.All(x => x.Severity == TraceEventType.Information));
        }

        [TestMethod]
        public void ShouldFormatCategoryReplacements()
        {
            callHandler.Categories.Add("Type {type}");
            callHandler.Categories.Add("Namespace {namespace}");
            TestTarget.DoSomething(54, "werw", 5340.34);

            Assert.IsTrue(
                LogEntriesMessagesSent.All(
                    x =>
                    x.ToString().Contains(
                        "Category: General, PIAB, Type LoggingTarget, Namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.PolicyInjection")));
        }

        [TestMethod]
        public void ShouldNotReportReturnValueWhenTargetMethodIsVoid()
        {
            TestTarget.DoSomethingElse("dummy");

            Assert.IsTrue(LogEntriesMessagesSent.All(x => string.IsNullOrEmpty(x.ReturnValue)));
        }

        [TestMethod]
        public void ShouldRecordExceptionInLog()
        {
            callHandler.LogBeforeCall = false;

            try
            {
                TestTarget.DoSomethingBad();
                Assert.Fail("Should not get here, should have exception");
            }
            catch (Exception) { }

            Assert.IsTrue(LogEntriesMessagesSent.All(x => !string.IsNullOrEmpty(x.Exception)));
        }

        [TestMethod]
        public void ShouldNotHaveExceptionWhereThereIsntOne()
        {
            TestTarget.DoSomethingElse("hey there");

            Assert.IsTrue(LogEntriesMessagesSent.All(x => string.IsNullOrEmpty(x.Exception)));
        }

        [TestMethod]
        public void ShouldNotLogReturnValueIfIncludeParametersIsFalse()
        {
            callHandler.LogBeforeCall = false;
            callHandler.IncludeParameters = false;
            TestTarget.DoSomething(1, "two", 3.0);

            Assert.IsTrue(LogEntriesMessagesSent.All(x => string.IsNullOrEmpty(x.ReturnValue)));
        }

        [TestMethod]
        public void ShouldDefaultTo0ForEventId()
        {
            TestTarget.DoSomethingElse("boo!");
            Assert.IsTrue(LogEntriesMessagesSent.Any(x => x.EventId == 0));
        }

        [TestMethod]
        public void ShouldBeAbleToChangeEventId()
        {
            callHandler.EventId = 42;
            TestTarget.DoSomething(2, "three", 4.0);
            Assert.IsTrue(LogEntriesMessagesSent.Any(x => x.EventId == 42));
        }

        [TestMethod]
        public void CreatesHandlerProperlyFromAttributes()
        {
            MethodInfo method = typeof(LoggingTarget).GetMethod("DoSomethingElse");

            Assert.IsNotNull(method);

            object[] attributes = method.GetCustomAttributes(typeof(LogCallHandlerAttribute), false);

            Assert.AreEqual(1, attributes.Length);

            LogCallHandlerAttribute att = attributes[0] as LogCallHandlerAttribute;

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            container.RegisterInstance(writer);
            ICallHandler callHandler = att.CreateHandler(container);

            Assert.IsNotNull(callHandler);
            Assert.AreEqual(9, callHandler.Order);
        }

        [TestCleanup]
        public void TeardownLogWriter()
        {
            Listener.Flush();
            writer.Dispose();
            writer = null;
            Listener = null; // Disposing the log also disposes the trace listener
        }

        LoggingTarget CreateTarget()
        {
            callHandler = new LogCallHandler(writer);
            callHandler.LogBeforeCall = callHandler.LogAfterCall = true;
            callHandler.BeforeMessage = beforeMessage;
            callHandler.AfterMessage = afterMessage;
            callHandler.Categories.Add("General");
            callHandler.Categories.Add("PIAB");

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            container.RegisterType<LoggingTarget>(new Interceptor<VirtualMethodInterceptor>());
            
            container.Configure<Interception>()
                .SetDefaultInterceptorFor<LoggingTarget>(new VirtualMethodInterceptor())
                .AddPolicy("Logging")
                .AddMatchingRule(new TypeMatchingRule("LoggingTarget"))
                .AddCallHandler(callHandler);
            container.RegisterInstance(this.writer);

            return container.Resolve<LoggingTarget>();
        }

        private static MethodImplementationInfo GetMethodImpl(MethodBase method)
        {
            return new MethodImplementationInfo(null, ((MethodInfo)method));
        }
    }

    public class LoggingTarget
    {
        public virtual string DoSomething(int one,
                                  string two,
                                  double three)
        {
            Thread.Sleep(1); // Required to properly test the CallTime
            return string.Format("{1}: {0} {2}", one, two, three);
        }

        public virtual void DoSomethingBad()
        {
            throw new Exception("Exception thrown here");
        }

        [LogCallHandler(Order = 9)]
        public virtual void DoSomethingElse(string message) { }
    }
}
