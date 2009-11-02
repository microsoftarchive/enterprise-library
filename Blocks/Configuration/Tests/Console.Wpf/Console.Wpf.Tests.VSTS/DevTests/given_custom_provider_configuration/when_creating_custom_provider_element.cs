using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System.ComponentModel.Design;
using Console.Wpf.ViewModel;
using Console.Wpf.ViewModel.Services;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.Tests.VSTS.DevTests.given_custom_provider_configuration
{
    [TestClass]
    public class when_creating_custom_provider_element : ContainerContext
    {
        SectionViewModel viewModel;
        ConfigurationSectionWithCustomProvider section;

        Property attributesProperty;

        protected override void Arrange()
        {
            base.Arrange();
            section = new ConfigurationSectionWithCustomProvider();

            var elementLookup = Container.Resolve<ElementLookup>();
            elementLookup.AddCustomElement(new CustomAttributesPropertyExtender(Container.Resolve<IServiceProvider>()));
        }

        protected override void Act()
        {
            viewModel = SectionViewModel.CreateSection(Container, "mockSection", section);
            var providerElement = viewModel.DescendentElements(x => x.ConfigurationType == typeof(CustomProviderConfigurationElement)).First();
            attributesProperty = providerElement.Property("Attributes");
        }

        [TestMethod]
        public void then_has_attributes_property()
        {
            Assert.IsNotNull(attributesProperty);
        }

        [TestMethod]
        public void then_attributes_property_has_drop_down_editor()
        {
            Assert.IsTrue(attributesProperty.HasEditor);
            Assert.AreEqual(EditorBehavior.DropDown, attributesProperty.EditorBehavior);
        }

        [TestMethod]
        public void then_attributes_property_is_not_readonly()
        {
            Assert.IsFalse(attributesProperty.ReadOnly);
        }

        [TestMethod]
        public void then_property_is_not_envrionment_overridable()
        {
            Assert.IsFalse(attributesProperty.Attributes.OfType<EnvironmentalOverridesAttribute>().First().CanOverride);
        }
    }
}
