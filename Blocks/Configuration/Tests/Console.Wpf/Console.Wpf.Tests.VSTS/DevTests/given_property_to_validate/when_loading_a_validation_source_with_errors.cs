using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_a_validation_service
{
    [TestClass]
    public class when_loading_a_validation_source_with_errors : ContainerContext
    {
        protected SectionViewModel Section { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();

            var locator = new Mock<ConfigurationSectionLocator>();
            locator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { "testSection" });
            Container.RegisterInstance(locator.Object);

            var section = new ElementForValidation();
            section.DefaultTestElement = "IsAnInvalidReference";

            var source = new DesignDictionaryConfigurationSource();
            source.Add("testSection", section);

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);

            Section = sourceModel.Sections.Where(s => s.SectionName == "testSection").Single();
        }

        [TestMethod]
        public void then_after_load_validation_errors_populated()
        {
            var property = Section.Property("DefaultTestElement");
            Assert.IsTrue(property.ValidationErrors.Any());
        }
    }
}
