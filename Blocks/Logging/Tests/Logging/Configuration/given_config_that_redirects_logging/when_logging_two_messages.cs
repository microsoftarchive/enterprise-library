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

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.given_config_that_redirects_logging
{
    /// <summary>
    /// Summary description for when_logging_two_messages
    /// </summary>
    [TestClass]
    public class when_logging_two_messages : Context
    {
        protected override void Act()
        {
            LogWriter.Write("Message 1");
            LogWriter.Write("Message 2");
        }

        [TestMethod]
        public void then_log_file_is_created()
        {
            Assert.IsTrue(File.Exists(LogFileName));
        }

        [TestMethod]
        public void then_log_file_contains_both_messages()
        {
            CloseLogFile();

            var lines = File.ReadAllLines(LogFileName);

            Assert.AreEqual(2, lines.Length);
        }
    }
}
