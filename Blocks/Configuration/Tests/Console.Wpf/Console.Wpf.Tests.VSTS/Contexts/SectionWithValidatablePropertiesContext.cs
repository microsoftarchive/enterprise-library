//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

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
