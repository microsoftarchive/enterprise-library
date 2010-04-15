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

using System.Collections.Generic;
using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model
{
    [TestClass]
    public class when_creating_hierarchical_section_model : ExceptionHandlingSettingsContext
    {
        private SectionViewModel sectionViewModel;
        private Mock<IAssemblyDiscoveryService> discoveryService;

        protected override void Arrange()
        {
            base.Arrange();
            this.discoveryService = new Mock<IAssemblyDiscoveryService>();
            this.Container.RegisterInstance(this.discoveryService.Object);
        }

        protected override void Act()
        {
            sectionViewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, Section);
        }

        [TestMethod]
        public void then_get_related_elements_returns_children_of_policy()
        {
            var policy = ElementsFromType<ExceptionPolicyData>().First();
            var relatedelements = sectionViewModel.GetRelatedElements(policy);

            CollectionAssert.IsSubsetOf(
                policy.ChildElements.SelectMany(x => x.ChildElements).ToArray(),
                relatedelements.ToArray());
        }

        [TestMethod]
        public void then_get_related_element_returns_parent_and_children_of_exception_type()
        {
            var exceptionType = ElementsFromType<ExceptionTypeData>().First();
            var relatedElements = sectionViewModel.GetRelatedElements(exceptionType);
            var parentAndChildren = exceptionType.ChildElements.SelectMany(x => x.ChildElements).Concat(new[] { exceptionType.ParentElement.ParentElement }).ToArray();

            CollectionAssert.IsSubsetOf(
                parentAndChildren,
                relatedElements.ToArray()
                );
        }


        [TestMethod]
        public void then_child_adders_returns_collections_add_commands()
        {
            var policyElement = ElementsFromType<ExceptionPolicyData>().First();

            var childAdders =
                policyElement.ChildElements.SelectMany(x => x.Commands)
                        .OfType<DefaultCollectionElementAddCommand>()
                        .Select(x => x.Title);


            var policyAdders = policyElement.Commands
                                .OfType<DefaultCollectionElementAddCommand>()
                                .Select(x => x.Title);

            CollectionAssert.AreEquivalent(childAdders.ToArray(),
                                            policyAdders.ToArray());
        }

        [TestMethod]
        public void then_has_add_command()
        {
            var exceptionPolicy = sectionViewModel.GetDescendentsOfType<ExceptionPolicyData>().First();
            Assert.IsTrue(exceptionPolicy.Commands.Where(x => x.Placement == CommandPlacement.ContextAdd).Any());
        }



        private IEnumerable<ElementViewModel> ElementsFromType<T>()
        {
            return sectionViewModel.GetDescendentsOfType<T>();
        }
    }
}
