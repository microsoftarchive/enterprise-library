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

using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.Tests
{
    [TestClass]
    public class AppSettingsNodeFixture
    {
        [TestMethod]
        public void AppSettingsNodeHasProperName()
        {
            AppSettingsNode appSettingsNode = new AppSettingsNode();

            Assert.AreEqual("Application Settings", appSettingsNode.Name);
        }

        [TestMethod]
        public void FilePropertyWillBeDisplayedOnNode()
        {
            string fileName = "a:\random\filename.txt";
            AppSettingsSection appSettingsSection = new AppSettingsSection();
            appSettingsSection.File = fileName;

            AppSettingsNode appSettingsNode = new AppSettingsNode(appSettingsSection);

            Assert.AreEqual(fileName, appSettingsNode.File);
        }
    }
}
