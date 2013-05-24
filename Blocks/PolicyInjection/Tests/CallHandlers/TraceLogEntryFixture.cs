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
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.PolicyInjection
{
    [TestClass]
    public class TraceLogEntryFixture
    {
        [TestMethod]
        public void ShouldBeCreatable()
        {
            TraceLogEntry logEntry = new TraceLogEntry();
            logEntry.TypeName = typeof(TraceLogEntryFixture).FullName;
            logEntry.MethodName = typeof(TraceLogEntryFixture).GetMethod("ShouldBeCreatable").Name;
            logEntry.ReturnValue = "Foo";
        }

        [TestMethod]
        public void ShouldBeAbleToAddParameterValues()
        {
            Dictionary<string, object> parameterValues = new Dictionary<string, object>();
            parameterValues["x"] = 12;
            parameterValues["y"] = 43;

            TraceLogEntry logEntry = new TraceLogEntry();
            logEntry.ExtendedProperties = parameterValues;

            Assert.AreEqual(12, logEntry.ExtendedProperties["x"]);
            Assert.AreEqual(43, logEntry.ExtendedProperties["y"]);
        }

        [TestMethod]
        public void ShouldBeAbleToFormatLogWithExtraProperties()
        {
            Dictionary<string, object> parameterValues = new Dictionary<string, object>();
            parameterValues["one"] = 1;
            parameterValues["two"] = "two";

            TraceLogEntry logEntry = new TraceLogEntry();
            logEntry.Categories.Add("General");
            logEntry.Categories.Add("Callhandler");
            logEntry.Message = "Logging call";
            logEntry.ExtendedProperties = parameterValues;
            logEntry.TypeName = GetType().Name;
            logEntry.MethodName = "SomeMethod";
            logEntry.ReturnValue = 42.ToString();

            string template =
                @"Message logged on {timestamp}{newline}{message}{newline}
Call on type {property(TypeName)} method {property(MethodName)}{newline}
Parameter values:{newline}
{dictionary({key} = {value}{newline})}{newline}
Return value: {property(ReturnValue)}{newline}";

            TextFormatter formatter = new TextFormatter(template);

            string formatted = formatter.Format(logEntry);

            Assert.IsTrue(formatted.Contains("Logging call"));
            Assert.IsTrue(
                formatted.Contains("Call on type TraceLogEntryFixture method SomeMethod\r\n"));
            Assert.IsTrue(formatted.Contains("one = 1\r\n"));
            Assert.IsTrue(formatted.Contains("two = two\r\n"));
            Assert.IsTrue(formatted.Contains("Return value: 42\r\n"));
        }

        [TestMethod]
        public void ShouldReturnElapsedTimeZeroIfCallTimeNull()
        {
            TraceLogEntry logEntry = new TraceLogEntry();
            logEntry.CallTime = null;

            Assert.AreEqual(TimeSpan.Zero, logEntry.ElapsedTime);
        }

        [TestMethod]
        public void ElapsedTimeShouldMatchAllTimeIfNotNull()
        {
            TraceLogEntry logEntry = new TraceLogEntry();
            var timespan = TimeSpan.FromMinutes(5);
            logEntry.CallTime = timespan;
            Assert.AreEqual(timespan, logEntry.ElapsedTime);
        }
    }
}
