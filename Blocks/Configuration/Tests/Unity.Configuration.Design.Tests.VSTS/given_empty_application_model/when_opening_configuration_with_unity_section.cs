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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Unity.given_empty_application_model
{
    [TestClass]
    public class when_opening_configuration_with_unity_section : given_empty_application_model_unity
    {
        DesignDictionaryConfigurationSource configurationSource;
        ConfigurationSourceModel configurationSourceModel;

        protected override void Arrange()
        {
            base.Arrange();

            configurationSource = new DesignDictionaryConfigurationSource();
            configurationSource.Add(UnityConfigurationSection.SectionName, new UnityConfigurationSection());
            Container.RegisterInstance(new Mock<IAssemblyDiscoveryService>().Object);

            configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
        }

        protected override void Act()
        {
            configurationSourceModel.Load(configurationSource);
        }

        [TestMethod]
        public void then_section_is_added_to_configuration_source()
        {
            Assert.IsTrue(configurationSourceModel.Sections.Any(x => x.SectionName == UnityConfigurationSection.SectionName));
        }

        [TestMethod]
        public void then_unity_section_has_unity_section_view_model()
        {
            SectionViewModel unitySectionVM = configurationSourceModel.Sections.Single(x => x.SectionName == UnityConfigurationSection.SectionName);

            Assert.IsInstanceOfType(unitySectionVM, typeof(UnitySectionViewModel));
        }

        [TestMethod]
        public void then_unity_section_is_displayed_as_horizontal_list()
        {
            SectionViewModel unitySectionVM = configurationSourceModel.Sections.Single(x => x.SectionName == UnityConfigurationSection.SectionName);

            Assert.IsInstanceOfType(unitySectionVM.Bindable, typeof(HorizontalListLayout));
        }

        [TestMethod]
        public void then_first_header_in_bindable_horizontal_list_has_add_commands()
        {
            SectionViewModel unitySectionVM = configurationSourceModel.Sections.Single(x => x.SectionName == UnityConfigurationSection.SectionName);
            HorizontalListLayout unitySectionBindable = (HorizontalListLayout)unitySectionVM.Bindable;

            HeaderLayout firstHeaderViewModel = unitySectionBindable.Current as HeaderLayout;

            Assert.IsNotNull(firstHeaderViewModel);
            Assert.IsNotNull(firstHeaderViewModel.Commands);
            Assert.IsTrue(firstHeaderViewModel.Commands.Any());
        }
    }
}
