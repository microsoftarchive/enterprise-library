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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.ViewModel;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model_and_config_collection
{
    public class SectionWithMultipleChildrenContext : ContainerContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            var section = new MockSectionWithMultipleChildCollections();
            BuildSection(section);
            ViewModel = new SectionViewModel(Container, "name", section);
        }

        protected virtual void BuildSection(MockSectionWithMultipleChildCollections section)
        {
            section.Children.Add(new TestHandlerData() { Name = "One" });
            section.Children.Add(new TestHandlerData() { Name = "Two" });
            section.Children.Add(new TestHandlerData() { Name = "Three" });
        }

        protected SectionViewModel ViewModel { get; set; }
    }

    [TestClass]
    public class when_adding_collection_elements : SectionWithMultipleChildrenContext
    {
        private bool descendentElementsChangedFired;

        protected override void Arrange()
        {
            base.Arrange();

            ViewModel.DescendentElementsChanged += (s, e) => { descendentElementsChangedFired = true; };
        }

        protected override void Act()
        {
            var collectionViewModel =
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof(NamedElementCollection<TestHandlerData>))
                    .OfType<ElementCollectionViewModel>()
                    .First();

            var command = collectionViewModel.Commands.OfType<DefaultCollectionElementAddCommand>().Where(c => c.ConfigurationElementType == typeof(TestHandlerData)).First();
            command.Execute(null);
        }

        [TestMethod]
        public void then_section_raises_change_notification()
        {
            Assert.IsTrue(descendentElementsChangedFired);
        }
    }

    [TestClass]
    public class when_moving_collection_elements_up : SectionWithMultipleChildrenContext
    {
        private bool descendentElementsChangedFired;

        protected override void Arrange()
        {
            base.Arrange();
            descendentElementsChangedFired = false;
            ViewModel.DescendentElementsChanged += (s, e) => { descendentElementsChangedFired = true; };
        }

        protected override void Act()
        {
            var handler = ViewModel.DescendentElements(x => x.ConfigurationType == typeof(TestHandlerData)).OfType<CollectionElementViewModel>()
                                .Where(x => x.Name == "Two").First();

            handler.MoveUp.Execute(null);
        }

        [TestMethod]
        public void then_can_execute_move_up_on_all_but_first_element()
        {
            var handlers =
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof(TestHandlerData))
                    .OfType<CollectionElementViewModel>();

            var handlersThatCanMoveUp = handlers.Where(x => x.MoveUp.CanExecute(null)).ToArray();
            var handlersExceptFirst = handlers.Skip(1).ToArray();

            CollectionAssert.AreEquivalent(
                handlersExceptFirst,
                handlersThatCanMoveUp);
        }

        [TestMethod]
        public void then_can_excute_move_down_on_all_but_last_element()
        {
            var handlers =
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof(TestHandlerData))
                    .OfType<CollectionElementViewModel>();

            CollectionAssert.AreEquivalent(
                handlers.Where(x => x != handlers.Last()).ToArray(),
                handlers.Where(x => x.MoveDown.CanExecute(null)).ToArray());
        }


        [TestMethod]
        public void then_elements_are_reordered()
        {
            var handlerNames =
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof(TestHandlerData)).Select(x => x.Name);

            CollectionAssert.AreEqual(new[] { "Two", "One", "Three" }, handlerNames.ToArray());
        }

        [TestMethod]
        public void then_change_notification_fires()
        {
            Assert.IsTrue(descendentElementsChangedFired);
        }
    }

    [TestClass]
    public class when_moving_collection_elements_down : SectionWithMultipleChildrenContext
    {
        private bool descendentElementsChangedFired;

        protected override void Arrange()
        {
            base.Arrange();
            descendentElementsChangedFired = false;
            base.ViewModel.DescendentElementsChanged += (s, e) => { descendentElementsChangedFired = true; };
        }

        protected override void Act()
        {
            var handler = ViewModel.DescendentElements(x => x.ConfigurationType == typeof(TestHandlerData)).OfType<CollectionElementViewModel>().Where(
                x => x.Name == "Two").First();

            handler.MoveDown.Execute(null);
        }


        [TestMethod]
        public void then_elements_are_reordered()
        {
            var handlerNames =
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof(TestHandlerData)).Select(x => x.Name);

            CollectionAssert.AreEqual(new[] { "One", "Three", "Two" }, handlerNames.ToArray());
        }

        [TestMethod]
        public void then_change_notification_fires()
        {
            Assert.IsTrue(descendentElementsChangedFired);
        }
    }

    [TestClass]
    public class when_removing_an_element : SectionWithMultipleChildrenContext
    {
        private bool deletedFired;
        private bool descendentElementsChangedFired;

        protected override void Arrange()
        {
            base.Arrange();
            descendentElementsChangedFired = false;
            base.ViewModel.DescendentElementsChanged += (s, e) => { descendentElementsChangedFired = true; };
        }

        protected override void Act()
        {
            var handler = ViewModel.DescendentElements(x => x.ConfigurationType == typeof(TestHandlerData))
                            .OfType<CollectionElementViewModel>()
                            .First(x => x.Name == "Two");

            handler.Deleted += (s, e) => { deletedFired = true; };
            handler.DeleteCommand.Execute(null);
        }

        [TestMethod]
        public void then_item_is_removed_from_descendents()
        {
            Assert.IsFalse(ViewModel.DescendentElements(x => x.ConfigurationType == typeof(TestHandlerData))
                               .OfType<CollectionElementViewModel>().Any(x => x.Name == "Two"));
        }

        [TestMethod]
        public void then_collection_change_is_raised()
        {
            Assert.IsTrue(descendentElementsChangedFired);
        }

        [TestMethod]
        public void then_deleted_was_fired_on_element()
        {
            Assert.IsTrue(deletedFired);
        }

    }

    [TestClass]
    public class when_executing_adder_on_middle_nodes : ContainerContext
    {
        private ElementCollectionViewModel childrenCollection;
        private int startingCount;

        protected override void Arrange()
        {
            base.Arrange();

            var section = new MockSectionWithSingleChild();

            section.Children.Add(CreateNewElement("One"));
            section.Children.Add(CreateNewElement("Two"));

            ViewModel = SectionViewModel.CreateSection(Container, "mock section", section);

            childrenCollection = ViewModel.DescendentElements(
                      e => e.ConfigurationType ==
                            typeof(NamedElementCollection<TestHandlerDataWithChildren>)).Cast<ElementCollectionViewModel>().First();

            startingCount =
                childrenCollection.ChildElements.Count(
                    x => typeof(TestHandlerDataWithChildren).IsAssignableFrom(x.ConfigurationType));
        }

        private static TestHandlerDataWithChildren CreateNewElement(string name)
        {
            var element = new TestHandlerDataWithChildren() { Name = name };
            element.Children.Add(new TestHandlerData() { Name = name + ".One" });
            element.Children.Add(new TestHandlerData() { Name = name + ".Two" });
            return element;
        }

        protected override void Act()
        {
            var command = childrenCollection.Commands
                            .OfType<DefaultCollectionElementAddCommand>()
                            .Where(x => x.ConfigurationElementType == typeof(TestHandlerDataWithChildren))
                            .First();
            command.Execute(null);
        }

        public SectionViewModel ViewModel { get; set; }
        [TestMethod]
        public void then_new_child_is_added_to_collection()
        {
            Assert.AreEqual(startingCount + 1,
                            childrenCollection.ChildElements.Count(
                                x => typeof(TestHandlerDataWithChildren).IsAssignableFrom(x.ConfigurationType)));
        }


        [TestMethod]
        public void then_collection_rows_are_recalculated()
        {
            ElementViewModel lastElement = childrenCollection.ChildElements.First();
            foreach (var element in childrenCollection.ChildElements.Skip(1))
            {
                var lastElementsLastHandler =
                    lastElement.DescendentElements().Last(
                        e => typeof(TestHandlerData).IsAssignableFrom(e.ConfigurationType));

                Assert.AreEqual(lastElementsLastHandler.Row + 1, element.Row);
                lastElement = element;
            }
        }

        [TestMethod]
        public void then_rowspan_for_nodes_matches_children_count()
        {
            Assert.IsTrue(
                childrenCollection.ChildElements.All(
                    x =>
                    x.RowSpan ==
                    Math.Max(1, x.DescendentElements().Count(
                                    h => typeof(TestHandlerData).IsAssignableFrom(h.ConfigurationType)))));

        }
    }
}
