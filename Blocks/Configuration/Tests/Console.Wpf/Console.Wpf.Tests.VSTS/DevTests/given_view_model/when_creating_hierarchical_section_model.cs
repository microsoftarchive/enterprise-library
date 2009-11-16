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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model
{
    [TestClass]
    public class when_creating_hierarchical_section_model : ExceptionHandlingSettingsContext
    {
        private SectionViewModel sectionViewModel;

        protected override void Act()
        {
            sectionViewModel = SectionViewModel.CreateSection(Container, ExceptionHandlingSettings.SectionName, Section);
            sectionViewModel.UpdateLayout();
        }

        [TestMethod]
        public void then_all_policies_in_first_column()
        {
            var policies = ElementsFromType<ExceptionPolicyData>();
            Assert.IsTrue(policies.Any());
            Assert.IsTrue(policies.All(x => x.Column == 0));
        }

        [TestMethod]
        public void then_all_exception_types_in_second_column()
        {
            var exceptionTypes = ElementsFromType<ExceptionTypeData>();
            Assert.IsTrue(exceptionTypes.Any());
            Assert.IsTrue(exceptionTypes.All(x => x.Column == 1));
            
        }

        [TestMethod]
        public void then_all_handlers_in_third_column()
        {
            var handlers = ElementsFromType<ExceptionHandlerData>();
            Assert.IsTrue(handlers.Any());
            Assert.IsTrue(handlers.All(x => x.Column == 2));
        }

        [TestMethod]
        public void then_first_policy_in_first_row()
        {
            var policy = ElementsFromType<ExceptionPolicyData>().First();

            Assert.AreEqual(1, policy.Row);
        }

        [TestMethod]
        public void then_first_exception_type_in_first_row()
        {
            var exceptionType = ElementsFromType<ExceptionTypeData>().First();

            Assert.AreEqual(1, exceptionType.Row);
        }

        [TestMethod]
        public void then_first_handler_type_in_first_row()
        {
            var handler = ElementsFromType<ExceptionHandlerData>().First();

            Assert.AreEqual(1, handler.Row);
        }

        [TestMethod]
        public void then_last_handler_row_is_summation_of_all_handlers()
        {
            var handlers = ElementsFromType<ExceptionHandlerData>();

            Assert.AreEqual(handlers.Count(), handlers.Last().Row);
        }

        [TestMethod]
        public void then_policy_rowspan_is_summation_of_descendent_handlers()
        {
            var policy = ElementsFromType<ExceptionPolicyData>().First();

            Assert.AreEqual(
                policy.DescendentElements(x => typeof (ExceptionHandlerData).IsAssignableFrom(x.ConfigurationType)).
                    Count(), policy.RowSpan);
        }

        [TestMethod]
        public void then_leaf_rowspans_are_one()
        {
            var handlers = ElementsFromType<ExceptionHandlerData>();
            Assert.IsTrue(handlers.All(x => x.RowSpan == 1));
        }

        [TestMethod]
        public void then_get_related_elements_returns_children_of_policy()
        {
            var policy = ElementsFromType<ExceptionPolicyData>().First();
            var relatedelements = sectionViewModel.GetRelatedElements(policy);

            CollectionAssert.AreEquivalent(
                policy.ChildElements.SelectMany(x => x.ChildElements).ToArray(),
                relatedelements.ToArray());
        }

        [TestMethod]
        public void then_get_related_element_returns_parent_and_children_of_exception_type()
        {
            var exceptionType = ElementsFromType<ExceptionTypeData>().First();
            var relatedElements = sectionViewModel.GetRelatedElements(exceptionType);
            CollectionAssert.AreEquivalent(
                exceptionType.ChildElements.SelectMany(x => x.ChildElements).Concat(new [] {exceptionType.ParentElement.ParentElement}).ToArray(),
                relatedElements.ToArray()
                );
        }

        [TestMethod]
        public void then_visual_contain_headers()
        {
            var headerNames = sectionViewModel.GetGridVisuals().OfType<HeaderViewModel>().Select(h => h.Name);
            CollectionAssert.AreEqual(new []{"Policies","Exception Types", "Handlers"}, headerNames.ToArray());
        }

        [TestMethod]
        public void then_headers_are_in_the_zeroth_row()
        {
            var headers = sectionViewModel.GetGridVisuals().OfType<HeaderViewModel>().ToArray();
            Assert.IsTrue(headers.All(h => h.Row == 0));
        }

        [TestMethod]
        public void then_headers_are_in_correct_columns()
        {
            var headers = sectionViewModel.GetGridVisuals().OfType<HeaderViewModel>();
            
            for(int i = 0; i < headers.Count(); i++)
            {
                Assert.IsTrue(headers.ElementAt(i).Column == i, string.Format("Header is not in expected column {0}", i));
            }            
        }

        [TestMethod]
        public void then_returns_only_visible_elements()
        {
            var collectionElements = sectionViewModel.GetGridVisuals().OfType<ElementCollectionViewModel>().ToArray();
            Assert.IsFalse(collectionElements.Any());
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
        public void then_top_level_headers_offer_add_commands()
        {
            var topLevelHeaders =
                sectionViewModel.GetGridVisuals().OfType<ElementHeaderViewModel>().Where(
                    x => x.Column == 0);
            Assert.IsTrue(topLevelHeaders.All(x => x.Commands.Count() > 0));
        }

        [TestMethod]
        public void then_non_top_level_headers_do_not_offer_add_commands()
        {
            var nonTopLevelHeaders =
               sectionViewModel.GetGridVisuals().OfType<ElementHeaderViewModel>().Where(
                   x => x.Column != 0);
            Assert.IsTrue(nonTopLevelHeaders.All(x => x.Commands.Count() == 0));
        }

        [TestMethod]
        public void then_has_add_command()
        {
            var exceptionPolicy = sectionViewModel.GetDescendentsOfType<ExceptionPolicyData>().First();
            Assert.IsTrue(exceptionPolicy.Commands.Where(x=>x.Placement == CommandPlacement.ContextAdd).Any());
        }



        private IEnumerable<ElementViewModel> ElementsFromType<T>()
        {
            return sectionViewModel.GetDescendentsOfType<T>();
        }
    }
}
