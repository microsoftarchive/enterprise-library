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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Console.Wpf.Tests.VSTS.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Console.Wpf.Tests.VSTS.DevTests.given_elements_and_paths
{
    [TestClass]
    public class when_getting_element_paths : LoggingConfigurationContext
    {
        string[] allElementPaths;
        XDocument loggingSectionXml;

        protected override void Arrange()
        {
            base.Arrange();

            StringBuilder xmlContents = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(xmlContents))
            {
                LoggingSection.WriteXml(writer);
            }

            loggingSectionXml = XDocument.Parse(xmlContents.ToString());
        }

        protected override void Act()
        {
            SectionViewModel loggingSection = SectionViewModel.CreateSection(Container, LoggingSettings.SectionName, LoggingSection);

            allElementPaths = loggingSection.DescendentElements().Select(x=>x.Path).ToArray();
        }

        [TestMethod]
        public void then_paths_can_be_used_as_xpath()
        {
            foreach (string path in allElementPaths)
            {
                var pathForSerializedSecion = path.Replace("/configuration/loggingConfiguration/", "/SerializableConfigurationSection/");
                var nodes = loggingSectionXml.XPathSelectElements(pathForSerializedSecion);
                Assert.AreEqual(1, nodes.Count());
            }
        }
    }
}
