using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Moq;

namespace Console.Wpf.Tests.VSTS.Contexts
{
    public abstract class SectionWithValidatablePropertiesContext : ContainerContext
    {
        protected SectionViewModel Section { get; private set; }
        protected ElementProperty property { get; private set; }

        protected override void Arrange()
        {
            base.Arrange();

            var locator = new Mock<ConfigurationSectionLocator>();
            locator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { "testSection" });
            Container.RegisterInstance(locator.Object);

            var section = new ElementForValidation();

            var source = new DesignDictionaryConfigurationSource();
            source.Add("testSection", section);

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);

            Section = sourceModel.Sections.Where(s => s.SectionName == "testSection").Single();

            property = (ElementProperty)Section.Property(ArrangePropertyName());
        }

        protected abstract string ArrangePropertyName();
    }
}