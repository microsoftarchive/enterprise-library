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

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestClass]
    public class FileValidationAttributeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void EnsureThatCanCreateFileWithValidPath()
        {
            FileNode fileNode = new FileNode(Path.GetTempPath() + @"\foo.config");
            DoValidation(fileNode, 0);
        }

        [TestMethod]
        public void EnsureCanWriteToAValidFile()
        {
            string fileName = Path.GetTempFileName();
            FileNode fileNode = new FileNode(fileName);
            using (File.Create(fileName)) {}
            DoValidation(fileNode, 0);
            if (File.Exists(fileName)) File.Delete(fileName);
        }

        [TestMethod]
        public void IfFileIsNullValidationSucceeds()
        {
            FileNode fileNode = new FileNode(null);
            DoValidation(fileNode, 0);
        }

        [TestMethod]
        public void ValidationSucceedsWhenValueIsAnEmptyString()
        {
            FileNode fileNode = new FileNode(string.Empty);
            DoValidation(fileNode, 0);
        }

        void DoValidation(FileNode fileNode,
                          int expectedErrors)
        {
            PropertyInfo fileNameInfo = typeof(FileNode).GetProperty("FileName");
            FileValidationAttribute attr = new FileValidationAttribute();
            List<ValidationError> errors = new List<ValidationError>();
            attr.Validate(fileNode, fileNameInfo, errors, ServiceProvider);
            Assert.AreEqual(expectedErrors, errors.Count);
        }

        class FileNode
        {
            string fileName;

            public FileNode(string fileName)
            {
                this.fileName = fileName;
            }

            [FileValidation]
            public string FileName
            {
                get { return fileName; }
            }
        }
    }
}