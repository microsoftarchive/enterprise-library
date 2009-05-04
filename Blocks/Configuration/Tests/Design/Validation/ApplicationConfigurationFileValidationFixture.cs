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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestClass]
    public class ApplicationConfigurationFileValidationFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void ValidatingInvalidPathCharactersFails()
        {
            ApplicationConfigurationFileNode appConfigurationFileNode = new ApplicationConfigurationFileNode("def|abc");
            DoValidation(appConfigurationFileNode, 1);
        }

        void DoValidation(ApplicationConfigurationFileNode fileNode,
                          int expectedErrors)
        {
            PropertyInfo fileNameInfo = typeof(ApplicationConfigurationFileNode).GetProperty("FileName");
            ApplicationConfigurationFileValidationAttribute attr = new ApplicationConfigurationFileValidationAttribute();
            List<ValidationError> errors = new List<ValidationError>();
            attr.Validate(fileNode, fileNameInfo, errors, ServiceProvider);
            Assert.AreEqual(expectedErrors, errors.Count);
        }

        class ApplicationConfigurationFileNode : ConfigurationNode
        {
            string fileName;

            public ApplicationConfigurationFileNode(string fileName)
            {
                this.fileName = fileName;
            }

            [ApplicationConfigurationFileValidation]
            public string FileName
            {
                get { return fileName; }
            }
        }
    }
}
