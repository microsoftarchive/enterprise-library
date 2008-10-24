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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class TextExceptionFormatterFixture
    {
        const string message = "message";
        const string innerException = "Inner Exception";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstrutingWithNullWriterThrows()
        {
            new TextExceptionFormatter(null, new Exception());
        }

        [TestMethod]
        public void CreateTest()
        {
            TextWriter writer = new StringWriter();
            Exception exception = new Exception();
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);

            Assert.AreSame(writer, formatter.Writer);
            Assert.AreSame(exception, formatter.Exception);
        }

        [TestMethod]
        public void VerifyInnerExceptionGetsFormatted()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception exception = new MockException(message, new MockException());

            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);
            Assert.IsTrue(sb.Length == 0);
            formatter.Format();

            Assert.IsTrue(sb.ToString().Contains(innerException));
        }

        [TestMethod]
        public void SimpleFormatterTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            Exception exception = new MockException();

            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);

            // Nothing should be written until Format() is called
            Assert.IsTrue(sb.Length == 0);

            // Format the exception
            formatter.Format();

            // Not much of a test, but at least we can tell if _something_ got written
            // to the underlying StringBuilder
            Assert.IsTrue(sb.Length > 0);
        }

        [TestMethod]
        public void WritesHandlingInstanceIdIfNotEmpty()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Guid testGuid = Guid.NewGuid();

            Exception exception = new MockException();

            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception, testGuid);

            // Format the exception
            formatter.Format();

            string[] lines = sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string line = lines.First(l => l.StartsWith("HandlingInstanceID", StringComparison.Ordinal));

            Assert.IsNotNull(line);
            Assert.IsTrue(line.IndexOf(testGuid.ToString("D", CultureInfo.InvariantCulture)) >= 0);
        }

        [TestMethod]
        public void SkipsHandlingInstanceIdIfEmpty()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);

            Exception exception = new MockException();

            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception, Guid.Empty);

            // Format the exception
            formatter.Format();

            string[] lines = sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.IsFalse(lines.Any(l => l.StartsWith("HandlingInstanceID", StringComparison.Ordinal)));
        }
    }
}
