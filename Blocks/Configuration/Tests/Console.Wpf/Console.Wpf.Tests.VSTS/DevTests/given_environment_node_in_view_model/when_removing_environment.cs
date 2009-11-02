using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.ViewModel;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_environment_node_in_view_model
{
    [TestClass]
    public class when_removing_environment : ContainerContext
    {
        ConfigurationSourceModel configurationSourceModel;

        protected override void Arrange()
        {
            base.Arrange();
            configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            configurationSourceModel.NewEnvironment();
        }

        protected override void Act()
        {
            configurationSourceModel.Environments.First().Delete();
        }

        [TestMethod]
        public void then_override_is_no_longer_in_section_view_model()
        {
            Assert.AreEqual(0, configurationSourceModel.Environments.Count());
        }

        [TestMethod]
        public void then_override_is_no_longer_in_section_view_model_sections()
        {
            Assert.AreEqual(0, configurationSourceModel.Sections.Count());
        }
    }
}
