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
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

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
            logEntry.CallTime = TimeSpan.FromMinutes(5);

            Assert.AreEqual(typeof(TraceLogEntryFixture).GetMethod("ShouldBeCreatable").Name, logEntry.MethodName);
            Assert.AreEqual(TimeSpan.FromMinutes(5), logEntry.CallTime);
        }

        [TestMethod]
        public void NotAssignedPropertiesShouldNotThrow()
        {
            TraceLogEntry logEntry = new TraceLogEntry();

            Assert.IsNull(logEntry.MethodName);
            Assert.IsNull(logEntry.CallTime);
        }

        [TestMethod]
        public void ShouldBeAbleToAddParameterValues()
        {
            TraceLogEntry logEntry = new TraceLogEntry();
            logEntry.ExtendedProperties["x"] = 12;
            logEntry.ExtendedProperties["y"] = 43;

            Assert.AreEqual(12, logEntry.ExtendedProperties["x"]);
            Assert.AreEqual(43, logEntry.ExtendedProperties["y"]);
        }

#if !SILVERLIGHT
        [TestMethod]
        public void ShouldBeAbleToFormatLogWithExtraProperties()
        {
            TraceLogEntry logEntry = new TraceLogEntry();
            logEntry.Categories.Add("General");
            logEntry.Categories.Add("Callhandler");
            logEntry.Message = "Logging call";
            logEntry.ExtendedProperties["one"] = 1;
            logEntry.ExtendedProperties["two"] = "two";
            logEntry.TypeName = GetType().Name;
            logEntry.MethodName = "SomeMethod";
            logEntry.ReturnValue = 42.ToString();

            string template =
                @"Message logged on {timestamp}{newline}{message}{newline}
Call on type {property(TypeName)} method {property(MethodName)}{newline}
Parameter values:{newline}
{dictionary({key} = {value}{newline})}{newline}
Return value: {property(ReturnValue)}{newline}";

            var formatter = new Logging.Formatters.TextFormatter(template);

            string formatted = formatter.Format(logEntry);

            Assert.IsTrue(formatted.Contains("Logging call"));
            Assert.IsTrue(
                formatted.Contains("Call on type TraceLogEntryFixture method SomeMethod\r\n"));
            Assert.IsTrue(formatted.Contains("one = 1\r\n"));
            Assert.IsTrue(formatted.Contains("two = two\r\n"));
            Assert.IsTrue(formatted.Contains("Return value: 42\r\n"));
        }
#endif

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

#if SILVERLIGHT
        [TestMethod]
        public void ShouldBeSerializableByLogEntrySerializer()
        {
            TraceLogEntry logEntry = new TraceLogEntry();
            logEntry.Message = "Foo";
            logEntry.TypeName = typeof(TraceLogEntryFixture).FullName;
            logEntry.MethodName = typeof(TraceLogEntryFixture).GetMethod("ShouldBeCreatable").Name;
            logEntry.ReturnValue = "Foo";
            logEntry.ExtendedProperties["x"] = 12.ToString();

            var serializer = new Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.LogEntrySerializer();
            var bytes = serializer.Serialize(logEntry);

            var actual = serializer.Deserialize(bytes);

            Assert.AreEqual(logEntry.Message, actual.Message);
        }

        [TestMethod]
        public void ShouldStoreExtraPropertiesInExtendedProperties()
        {
            TraceLogEntry logEntry = new TraceLogEntry();
            logEntry.TypeName = typeof(TraceLogEntryFixture).FullName;
            logEntry.MethodName = typeof(TraceLogEntryFixture).GetMethod("ShouldBeCreatable").Name;
            logEntry.ReturnValue = "Foo";
            logEntry.CallTime = TimeSpan.FromMinutes(5);
            logEntry.ExtendedProperties["x"] = 12.ToString();

            Assert.AreEqual(typeof(TraceLogEntryFixture).FullName, logEntry.ExtendedProperties["TypeName"]);
            Assert.AreEqual(typeof(TraceLogEntryFixture).GetMethod("ShouldBeCreatable").Name, logEntry.ExtendedProperties["MethodName"]);
            Assert.AreEqual("Foo", logEntry.ExtendedProperties["ReturnValue"]);
            Assert.AreEqual(TimeSpan.FromMinutes(5).ToString(null, CultureInfo.InvariantCulture), logEntry.ExtendedProperties["CallTime"]);
            Assert.AreEqual(12.ToString(), logEntry.ExtendedProperties["x"]);
        }
#endif
    }
}
