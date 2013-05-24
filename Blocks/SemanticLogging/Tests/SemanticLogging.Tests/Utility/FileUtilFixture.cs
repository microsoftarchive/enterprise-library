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
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.Utility
{
    [TestClass]
    public class FileUtilFixture
    {
        [TestMethod]
        public void ThrowOnInvalidFileChars()
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                AssertEx.Throws<ArgumentException>(() => FileUtil.ValidFile(c.ToString()));
            }

            foreach (var c in Path.GetInvalidPathChars())
            {
                AssertEx.Throws<ArgumentException>(() => FileUtil.ValidFile(c.ToString()));
            }
        }

        [TestMethod]
        public void ThrowOnInvalidOSFileNames()
        {
            AssertEx.Throws<ArgumentException>(() => FileUtil.ValidFile("PRN.log"));
            AssertEx.Throws<ArgumentException>(() => FileUtil.ValidFile("AUX.log"));
            AssertEx.Throws<ArgumentException>(() => FileUtil.ValidFile("CON.log"));
        }

        [TestMethod]
        public void ThrowOnPathNavigationFileName()
        {
            AssertEx.Throws<ArgumentException>(() => FileUtil.ValidFile("."));
            AssertEx.Throws<ArgumentException>(() => FileUtil.ValidFile(@"..\"));
            AssertEx.Throws<ArgumentException>(() => FileUtil.ValidFile(@"..\..\.."));
            AssertEx.Throws<ArgumentException>(() => FileUtil.ValidFile(@"C:\Test\..\"));
        }
    }
}
