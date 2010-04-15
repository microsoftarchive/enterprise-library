//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Console.Wpf.Tests.VSTS.Extensions
{
    [TestClass]
    public class when_wrapped_enumerable_raises_exception_not_filtered : ArrangeActAssert
    {
        private List<ActionExecuter> list = new List<ActionExecuter>();
        private TestTraceListener testListener;
        private Exception lastCaughtException;

        protected override void Arrange()
        {
            base.Arrange();

            list.Add(new ActionExecuter());

            ConfigurationLogWriter.LoggingSwitch.Level = TraceLevel.Warning;

            testListener = new TestTraceListener();
            Trace.Listeners.Add(testListener);
        }

        protected override void Act()
        {
            try
            {
                list.FilterSelectSafe(x => x.DoAction(() => { throw new Exception("Unfiltered Exception"); })).ToArray();
            }
            catch(Exception ex)
            {
                lastCaughtException = ex;
            }
        }

        [TestMethod]
        public void then_exception_should_be_rethrown()
        {
            Assert.AreEqual(typeof (Exception), lastCaughtException.GetType());
        }

        [TestMethod]
        public void then_exception_is_not_logged()
        {
            Assert.IsFalse(testListener.Messages.Any());
        }
    }

    [TestClass]
    public class when_wrapped_enumerable_raises_exception_in_filterlist : ArrangeActAssert
    {
        private List<ActionExecuter> list = new List<ActionExecuter>();
        private TestTraceListener testListener;

        protected override void Arrange()
        {
            base.Arrange();

            list.Add(new ActionExecuter());
            list.Add(new ActionExecuter());
            list.Add(new ActionExecuter());

            ConfigurationLogWriter.LoggingSwitch.Level = TraceLevel.Warning;

            testListener = new TestTraceListener();
            Trace.Listeners.Add(testListener);
        }

        protected override void Act()
        {
            list.FilterSelectSafe(x => x.DoAction(() => { throw new FileNotFoundException("FileNotFoundAction"); })).ToArray();
        }

        [TestMethod]
        public void then_should_log_messages()
        {
            Assert.AreEqual(list.Count(), testListener.Messages.Count());
        }


    }

    [TestClass]
    public class when_filtered_exceptions_raised_they_are_logged : ArrangeActAssert
    {
        private List<Func<bool>> list = new List<Func<bool>>();
        private TestTraceListener testListener;

        protected override void Arrange()
        {
            base.Arrange();

            list.Add(() =>{ throw new FileNotFoundException("FileNotFound"); });
            list.Add(() => { throw new TypeLoadException("TypeLoadException"); });
            list.Add(() => { throw new FileLoadException("FileLoadException"); });
            list.Add(() => { throw new ReflectionTypeLoadException(new Type[0], new Exception[0], "ReflectionTypeLoadException"); });

            ConfigurationLogWriter.LoggingSwitch.Level = TraceLevel.Warning;

            testListener = new TestTraceListener();
            Trace.Listeners.Add(testListener);
        }

        protected override void Act()
        {
            list.FilterSelectSafe(x => x.Invoke()).ToArray();
        }

        [TestMethod]
        public void then_all_exception_types_logged()
        {
            string[] containsList = {"FileNotFound", "TypeLoadException", "FileLoadException", "ReflectionTypeLoadException"};
            foreach(var error in containsList)
            {
                Assert.IsTrue(testListener.Messages.Any(m => m.Contains(error)));
            }
        }

    }
    class TestTraceListener : TraceListener
    {
        private List<string> messages = new List<string>();

        public override void Write(string message)
        {
            messages.Add(message);
        }

        public override void WriteLine(string message)
        {
            messages.Add(message);
        }

        public IEnumerable<string> Messages
        {
            get { return messages; }
        }
    }

    class ActionExecuter
    {
        public ActionExecuter()
        {

        }
        public bool DoAction(Action action)
        {
            action();
            return true;
        }
    }

}
