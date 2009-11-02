using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using System.Configuration;
using Console.Wpf.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_elements_and_paths
{
    [TestClass]
    public class when_getting_element_paths : given_logging_configuration.given_logging_configuration
    {
        string[] allElementPaths;
        XmlDocument loggingSectionXml;

        protected override void Arrange()
        {
            base.Arrange();

            StringBuilder xmlContents = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(xmlContents))
            {
                LoggingSection.WriteXml(writer);
            }

            loggingSectionXml = new XmlDocument();
            loggingSectionXml.LoadXml(xmlContents.ToString());
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
                var nodes = loggingSectionXml.SelectNodes(pathForSerializedSecion);
                Assert.AreEqual(1, nodes.Count);
            }
        }
    }
}
