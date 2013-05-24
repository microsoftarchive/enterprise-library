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
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class XmlExceptionFormatterFixture
    {
        [TestMethod]
        public void CreateXmlWriterTest()
        {
            StringBuilder sb = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
            Exception ex = new MockException();
            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex, Guid.Empty);

            Assert.AreSame(writer, formatter.Writer);
            Assert.AreSame(ex, formatter.Exception);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithNullXmlWriterThrows()
        {
            new XmlExceptionFormatter((XmlWriter)null, new Exception(), Guid.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithNullTextWriterThrows()
        {
            new XmlExceptionFormatter((TextWriter)null, new Exception(), Guid.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithNullExceptionThrows()
        {
            StringBuilder sb = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
            new XmlExceptionFormatter(writer, null, Guid.Empty);
        }

        [TestMethod]
        public void VerifyInnerExceptionGetsFormatted()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception exception = new MockException("exception message", new MockException());

            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, exception, Guid.Empty);
            Assert.IsTrue(sb.Length == 0);
            formatter.Format();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            XmlNode element = doc.DocumentElement.SelectSingleNode("//InnerException");
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void CreateTextWriterTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception ex = new MockException();
            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex, Guid.Empty);

            // TextWriter won't be the same so we can only test the Exception
            Assert.AreSame(ex, formatter.Exception);
        }

        [TestMethod]
        public void SimpleXmlWriterFormatterTest()
        {
            StringBuilder sb = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb));
            Exception ex = new MockException();
            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex, Guid.Empty);

            Assert.IsTrue(sb.Length == 0);

            formatter.Format();

            Assert.IsTrue(sb.Length > 0);
        }

        [TestMethod]
        public void SimpleTextWriterFormatterTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception ex = new MockException();
            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex, Guid.Empty);

            Assert.IsTrue(sb.Length == 0);

            formatter.Format();

            Assert.IsTrue(sb.Length > 0);
        }

        [TestMethod]
        public void WellFormedTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception ex = new MockException();
            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex, Guid.Empty);
            formatter.Format();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
        }

        [TestMethod]
        public void FormatsHandlingInstanceIdIfAvailable()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception exception = new MockException("exception message", new MockException());
            Guid testGuid = Guid.NewGuid();

            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, exception, testGuid);
            Assert.IsTrue(sb.Length == 0);
            formatter.Format();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            XmlNode element = doc.DocumentElement.SelectSingleNode("/Exception/@handlingInstanceId");
            Assert.IsNotNull(element);
            Assert.AreEqual(testGuid.ToString("D", CultureInfo.InvariantCulture), element.InnerText);
        }

        [TestMethod]
        public void DoesNotFormatHandlingInstanceIdIfEmpty()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception exception = new MockException("exception message", new MockException());

            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, exception, Guid.Empty);
            Assert.IsTrue(sb.Length == 0);
            formatter.Format();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            XmlNode element = doc.DocumentElement.SelectSingleNode("/Exception/@handlingInstanceId");
            Assert.IsNull(element);
        }
    }
}
