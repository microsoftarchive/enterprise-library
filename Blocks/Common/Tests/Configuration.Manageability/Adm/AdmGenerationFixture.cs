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

using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Adm
{
    [TestClass]
    public class AdmGenerationFixture
    {
        AdmContent content;
        StringWriter writer;

        [TestInitialize]
        public void SetUp()
        {
            writer = new StringWriter();
        }

        [TestMethod]
        public void CanWriteEmptyContent()
        {
            content = new AdmContent();

            content.Write(writer);
        }

        [TestMethod]
        public void CanWriteContentWithCategory()
        {
            content = new AdmContent();
            content.AddCategory(new AdmCategory("category"));

            content.Write(writer);
        }
    }
}
