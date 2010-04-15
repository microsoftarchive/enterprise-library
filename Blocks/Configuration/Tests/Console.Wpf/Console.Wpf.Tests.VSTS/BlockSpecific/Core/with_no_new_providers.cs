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

using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Core
{
    [TestClass]
    public class when_manageable_configuration_source_view_model_is_constructed_for_source_with_existing_providers_and_with_no_new_providers : ContainerContext
    {
        private string targetFilePath;

        protected override void Arrange()
        {
            base.Arrange();

            Mock<AssemblyLocator> assemblyLocator = new Mock<AssemblyLocator>();
            assemblyLocator.Setup(x => x.Assemblies).Returns(new Assembly[0]);
            this.Container.RegisterType<ManageableConfigurationSourceViewModel>(
                new InjectionConstructor(
                    typeof(ElementCollectionViewModel),
                    typeof(ConfigurationElement),
                    new InjectionParameter<AssemblyLocator>(assemblyLocator.Object)));

            targetFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "manageableSourcetext.config");
            File.Create(targetFilePath).Dispose();
            File.WriteAllText(targetFilePath, "<configuration/>");

            var source = new DesignConfigurationSource(targetFilePath);

            ConfigurationSourceElement =
                new ManageableConfigurationSourceElement
                {
                    Name = "manageable",
                    FilePath = "file.config",
                    ConfigurationManageabilityProviders = 
                    {
                        new ConfigurationSectionManageabilityProviderData
                        {
                            Name = "provider",
                            TypeName = typeof(object).AssemblyQualifiedName
                        }
                    }
                };
            var section =
                new ConfigurationSourceSection
                {
                    SelectedSource = "manageable",
                    Sources =
                    {
                        ConfigurationSourceElement
                    }
                };

            source.AddLocalSection(ConfigurationSourceSection.SectionName, section);


            var sourceModel = this.Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);

            SectionViewModel =
                sourceModel.Sections.Where(x => x.ConfigurationType == typeof(ConfigurationSourceSection)).Single();

            ConfigurationSourceViewModel = SectionViewModel.GetDescendentsOfType<ManageableConfigurationSourceElement>().FirstOrDefault();
            ConfigurationSourceElement = (ManageableConfigurationSourceElement) ConfigurationSourceViewModel.ConfigurationElement;
        }

        protected SectionViewModel SectionViewModel { get; private set; }
        protected ElementViewModel ConfigurationSourceViewModel { get; private set; }
        protected ManageableConfigurationSourceElement ConfigurationSourceElement { get; private set; }

        [TestMethod]
        public void then_source_model_is_custom_view_model()
        {
            Assert.IsInstanceOfType(ConfigurationSourceViewModel, typeof(ManageableConfigurationSourceViewModel));
        }

        [TestMethod]
        public void then_managenable_configuration_source_element_is_cleared()
        {
            Assert.AreEqual(0, ConfigurationSourceElement.ConfigurationManageabilityProviders.Count);
        }

        protected override void Teardown()
        {
            if (File.Exists(targetFilePath)) File.Delete(targetFilePath);
        }
    }
}
