using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.Tests.VSTS.Mocks;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source_model
{
    [TestClass]
    public class when_loading_configuration : ContainerContext
    {
        bool refreshed;
        ConfigurationSourceDependency dependency;

        protected override void Arrange()
        {
            base.Arrange();

            dependency = Container.Resolve<ConfigurationSourceDependency>();
            dependency.Cleared += (sender, args) => refreshed = true;

        }

        protected override void Act()
        {
            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(new DesignDictionaryConfigurationSource());
        }

        [TestMethod]
        public void then_depedency_is_refreshed()
        {
            Assert.IsTrue(refreshed);
        }
    }
}
