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

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source_model
{
    [TestClass]
    public class when_initializing_new_configuration : ContainerContext
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
            sourceModel.New();
        }

        [TestMethod]
        public void then_depedency_is_refreshed()
        {
            Assert.IsTrue(refreshed);
        }
    }
}
