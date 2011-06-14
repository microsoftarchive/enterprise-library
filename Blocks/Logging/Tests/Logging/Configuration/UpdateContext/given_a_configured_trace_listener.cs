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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class given_a_configured_trace_listener : ArrangeActAssert
    {
        protected LogWriterImpl LogWriter;
        protected CustomTraceListener listener2;

        protected override void Arrange()
        {
            base.Arrange();
            MockTraceListener.Reset();

            var listener1 = new MockTraceListener("listener1");
            listener2 = new CustomTraceListener { Name = "listener2" };

            LogWriter =
                new LogWriterImpl(
                    new ILogFilter[0],
                    new LogSource[]
                    {
                        new LogSource("category1", new TraceListener[] { listener1 }, SourceLevels.Error, true),
                        new LogSource("category2", new TraceListener[] { listener2 }, SourceLevels.Error, true),
                    },
                    new LogSource(""),
                    new LogSource(""),
                    new LogSource(""),
                    "category1",
                    true,
                    false);
        }

        protected override void Teardown()
        {
            base.Teardown();
            LogWriter.Dispose();
            LogWriter = null;
        }

        [TestMethod]
        public void then_can_list_trace_listeners()
        {
            var context = LogWriter.GetUpdateContext();
            Assert.AreEqual(2, context.Listeners.Count);
            Assert.IsTrue(context.Listeners.Any(x => x.Name == "listener1"));
            Assert.IsTrue(context.Listeners.OfType<ICustomTraceListenerUpdateContext>().Any(x => x.Name == "listener2"));
        }

        [TestMethod]
        public void then_can_update_custom_trace_listeners()
        {
            var context = LogWriter.GetUpdateContext();
            Assert.AreEqual(2, context.Listeners.Count);
            ((ICustomTraceListenerUpdateContext)context.Listeners.First(x => x.Name == "listener2")).Foo = "Bar";

            Assert.AreNotEqual("Bar", listener2.Foo);

            context.ApplyChanges();

            Assert.AreEqual("Bar", listener2.Foo);
        }

        public interface ICustomTraceListenerUpdateContext : ITraceListenerUpdateContext
        {
            string Foo { get; set; } 
        }

        public class CustomTraceListener : TraceListener
        {
            public string Foo { get; set; }

            public override void Write(string message)
            {
            }

            public override void WriteLine(string message)
            {
            }

            protected override ITraceListenerUpdateContext GetUpdateContext()
            {
                return new CustomTraceListenerUpdateContext(this);
            }

            protected class CustomTraceListenerUpdateContext : TraceListenerUpdateContext, ICustomTraceListenerUpdateContext
            {
                public CustomTraceListenerUpdateContext(TraceListener traceListener)
                    : base(traceListener)
                {
                }

                public string Foo { get; set; }

                protected override void ApplyChanges()
                {
                    base.ApplyChanges();

                    ((CustomTraceListener)base.TraceListener).Foo = this.Foo;
                }
            }
        }
    }


}
