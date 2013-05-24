#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.EventListeners
{
    [TestClass]
    public class TallyKeepingFileStreamWriterTests
    {
        string fileName = Guid.NewGuid().ToString();

        [TestCleanup]
        public void TearDown()
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void InitializesTallyForNewFile()
        {
            using (var writer
                = new TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                Assert.AreEqual(0L, writer.Tally);
            }
        }

        [TestMethod]
        public void InitializesTallyForExistingFile()
        {
            File.WriteAllText(fileName, "12345");
            using (var writer
                = new TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                Assert.AreEqual(5L, writer.Tally);
            }

            File.WriteAllText(fileName, "12345");
            using (var writer
                = new TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read)))
            {
                Assert.AreEqual(0L, writer.Tally);
            }

            File.WriteAllText(fileName, "12345");
            using (var writer
                = new TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Truncate, FileAccess.Write, FileShare.Read)))
            {
                Assert.AreEqual(0L, writer.Tally);
            }
        }

        [TestMethod]
        public void WritingToFileUpdatesTally()
        {
            using (var writer
                = new TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                writer.Write("12345");
                writer.Flush();

                Assert.AreEqual(5L, writer.Tally);
            }
        }

        [TestMethod]
        public void WritingToFileWithEncodingUpdatesTally()
        {
            using (var writer
                = new TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read),
                                                                                Encoding.UTF32))
            {
                writer.Write("12345");
                writer.Flush();

                Assert.AreEqual(20L, writer.Tally); // BOM is not part of tally - minimal fidelity loss on new files.
            }
        }
    }
}
