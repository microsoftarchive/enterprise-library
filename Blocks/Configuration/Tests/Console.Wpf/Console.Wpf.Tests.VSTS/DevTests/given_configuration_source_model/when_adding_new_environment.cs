using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source_model
{
    [TestClass]
    public class when_adding_new_environment : ContainerContext
    {
        protected override void Act()
        {
            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.NewEnvironment();
        }

        [TestMethod]
        public void then_environment_is_contained_in_sections()
        {
            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            Assert.IsTrue(sourceModel.Sections.Where(x=>x.ConfigurationType == typeof(EnvironmentMergeSection)).Any());
        }
    }
}
