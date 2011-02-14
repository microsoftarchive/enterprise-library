using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class ResourceDictionaryConfigurationSourceFixture
    {
        [TestMethod]
        public void CanGetConfigurationFromXaml()
        {
            var uri = new Uri("/Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Tests;component/Configuration/TestConfiguration.xaml", UriKind.Relative);
            var configurationSource = ResourceDictionaryConfigurationSource.FromXaml(uri);

            var actual = configurationSource.GetSection("section1");

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void CanReadValuesFromConfigurationSection()
        {
            var uri = new Uri("/Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Tests;component/Configuration/TestConfiguration.xaml", UriKind.Relative);
            var configurationSource = ResourceDictionaryConfigurationSource.FromXaml(uri);

            var actual = configurationSource.GetSection("section1") as MockConfigurationSection;

            Assert.IsNotNull(actual);
            Assert.AreEqual("DummyText", actual.TestProperty);
        }

        [TestMethod]
        [Ignore]
        // this test does not work with test harnesses that have the Application class defined in it.
        public void WhenNoUri_ThenUsesADefaultConfigurationFile()
        {
            var configurationSource = ResourceDictionaryConfigurationSource.CreateDefault();

            var actual = configurationSource.GetSection("defaultSection") as MockConfigurationSection;

            Assert.IsNotNull(actual);
            Assert.AreEqual("This is the default configuration file.", actual.TestProperty);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void WhenInexistentFileInUri_ThenThrows()
        {
            var uri = new Uri("/Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Tests;component/NonExistent.xaml", UriKind.Relative);
            var configurationSource = ResourceDictionaryConfigurationSource.FromXaml(uri);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void WhenInvalidConfigurationFormat_ThenThrows()
        {
            var uri = new Uri("/Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Tests;component/Configuration/InvalidFormatConfiguration.xaml", UriKind.Relative);
            var configurationSource = ResourceDictionaryConfigurationSource.FromXaml(uri);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void WhenInvalidRootElement_ThenThrows()
        {
            var uri = new Uri("/Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Tests;component/Configuration/InvalidRootElementConfiguration.xaml", UriKind.Relative);
            var configurationSource = ResourceDictionaryConfigurationSource.FromXaml(uri);
        }

        [TestMethod]
        public void WhenSectionIsNotDefined_ThenReturnNull()
        {
            var uri = new Uri("/Microsoft.Practices.EnterpriseLibrary.Common.Silverlight.Tests;component/Configuration/TestConfiguration.xaml", UriKind.Relative);
            var configurationSource = ResourceDictionaryConfigurationSource.FromXaml(uri);

            var actual = configurationSource.GetSection("nonExisting");

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void WhenResourceDictionaryIsSpecifiedThroughCode_ThenRetrievesSectionsFromIt()
        {
            var section1 = new ConfigurationSection();
            var section2 = new ConfigurationSection();

            var dictionary = new ResourceDictionary()
            {
                { "section1", section1 },
                { "section2", section2 },
            };
            var configurationSource = new ResourceDictionaryConfigurationSource(dictionary);

            Assert.AreSame(section1, configurationSource.GetSection("section1"));
            Assert.AreSame(section2, configurationSource.GetSection("section2"));
            Assert.IsNull(configurationSource.GetSection("nonExisting"));
        }
    }
}
