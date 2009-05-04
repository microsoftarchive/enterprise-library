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

using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    /// <summary>
    /// Summary description for LogEntryXmlFixture
    /// </summary>
    [TestClass]
    public class LogEntryXmlFixture
    {
        [TestMethod]
        public void CanDeserializeLogEntryXmlUsingBinaryFormatter()
        {
            XmlLogEntry entry = CommonUtil.CreateXmlLogEntry();

            string serializedLogEntryXmlText = new BinaryLogFormatter().Format(entry);
            XmlLogEntry desiaralizedLogEntryXml = (XmlLogEntry)BinaryLogFormatter.Deserialize(serializedLogEntryXmlText);

            Assert.IsNotNull(desiaralizedLogEntryXml);
            CommonUtil.AssertXmlLogEntries(entry, desiaralizedLogEntryXml);
        }
    }
}
