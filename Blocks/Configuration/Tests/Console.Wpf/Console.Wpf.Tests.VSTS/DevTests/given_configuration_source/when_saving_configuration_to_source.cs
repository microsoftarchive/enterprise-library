using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.ViewModel;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source
{
    [TestClass]
    [DeploymentItem("temp.config")]
    public class when_saving_configuration_to_source : ExceptionHandlingSettingsContext
    {
        ConfigurationSection clonedSection;
        ConfigurationSectionCloner cloner = new ConfigurationSectionCloner();
        FileConfigurationSource source = new FileConfigurationSource("temp.config");

        protected override void Arrange()
        {
            base.Arrange();
            clonedSection = cloner.Clone(Section);
        }

        [TestMethod]
        public void then_file_contains_no_clear_elements()
        {
            source.Remove(ExceptionHandlingSettings.SectionName);
            source.Add(ExceptionHandlingSettings.SectionName, clonedSection);

            string text = File.ReadAllText("temp.config");
            Assert.IsTrue(text.Contains(ExceptionHandlingSettings.SectionName));
            Assert.IsFalse(text.Contains("<clear/>"));
            
        }
    }
}
