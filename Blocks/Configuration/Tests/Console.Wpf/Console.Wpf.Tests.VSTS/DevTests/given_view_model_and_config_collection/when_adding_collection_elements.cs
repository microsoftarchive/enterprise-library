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

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model_and_config_collection
{
    public class SectionWithMultipleChildrenContext : ServiceProviderContext
    {
        protected override void Arrange()
        {
            base.Arrange();
            
            ChangedSource = new CollectionChangedSource();
            ServiceProvider.AddService(typeof(CollectionChangedSource), ChangedSource);

            var section = new MockSectionWithMultipleChildCollections();
            section.Children.Add(new TestHandlerData(){Name = "One"});
            section.Children.Add(new TestHandlerData(){Name = "Two"});
            section.Children.Add(new TestHandlerData(){Name = "Three"});

            ViewModel = new SectionViewModel(ServiceProvider, section);
        }

        protected CollectionChangedSource ChangedSource { get; set; }

        protected SectionViewModel ViewModel { get; set; }
    }

    [TestClass]
    public class when_adding_collection_elements : SectionWithMultipleChildrenContext
    {
        private NotifyCollectionChangedEventArgs lastCollectionChangedEventArguments;
        private bool childrenChangedFired;

        protected override void Arrange()
        {
            base.Arrange();

            childrenChangedFired = false;
            ViewModel.ChildrenChangedEvent += (s, e) => {childrenChangedFired = true;};

            lastCollectionChangedEventArguments = null;
            ChangedSource.CollectionChanged += (s, e) => { lastCollectionChangedEventArguments = e;};
        }

        protected override void Act()
        {
            var collectionViewModel =
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof (NamedElementCollection<TestHandlerData>))
                    .OfType<ElementCollectionViewModel>()
                    .First();

            collectionViewModel.ChildAdders.First().Execute(null);
        }

        [TestMethod]
        public void then_change_notification_is_fired()
        {
            Assert.IsNotNull(lastCollectionChangedEventArguments);
        }
    
        [TestMethod]
        public void then_section_raises_change_notification()
        {
            Assert.IsTrue(childrenChangedFired);
        }
    }

    [TestClass]
    public class when_moving_collection_elements_up : SectionWithMultipleChildrenContext
    {
        private bool collectionChangedFired;

        protected override void Arrange()
        {
            base.Arrange();
            collectionChangedFired = false;
            ChangedSource.CollectionChanged += (s, e) => { collectionChangedFired = true; };
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
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof (TestHandlerData))
                    .OfType<CollectionElementViewModel>();

            CollectionAssert.AreEquivalent(
                handlers.Skip(1).ToArray(),
                handlers.Where(x => x.MoveUp.CanExecute(null)).ToArray());
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
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof (TestHandlerData)).Select(x => x.Name);

            CollectionAssert.AreEqual(new[] { "Two", "One", "Three" }, handlerNames.ToArray());
        }

        [TestMethod]
        public void then_change_notification_fires()
        {
            Assert.IsTrue(collectionChangedFired);
        }
    }

    [TestClass]
    public class when_moving_collection_elements_down : SectionWithMultipleChildrenContext
    {
        private bool collectionChangedFired;

        protected override void Arrange()
        {
            base.Arrange();
            collectionChangedFired = false;
            ChangedSource.CollectionChanged += (s, e) => { collectionChangedFired = true; };
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
            Assert.IsTrue(collectionChangedFired);
        }
    }

    [TestClass]
    public class when_removing_an_element : SectionWithMultipleChildrenContext
    {
        private bool deletedFired;
        private bool collectionChangedFired;

        protected override void Arrange()
        {
            base.Arrange();
            collectionChangedFired = false;
            ChangedSource.CollectionChanged += (s, e) => { collectionChangedFired = true; };
        }

        protected override void Act()
        {
            var handler = ViewModel.DescendentElements(x => x.ConfigurationType == typeof (TestHandlerData))
                            .OfType<CollectionElementViewModel>()
                            .First(x => x.Name == "Two");

            handler.Deleted += (s, e) => { deletedFired = true; };
            handler.Delete.Execute(null);
        }

        [TestMethod]
        public void then_item_is_removed_from_descendents()
        {
            Assert.IsFalse(ViewModel.DescendentElements(x => x.ConfigurationType == typeof (TestHandlerData))
                               .OfType<CollectionElementViewModel>().Any(x => x.Name == "Two"));
        }

        [TestMethod]
        public void then_collection_change_is_raised()
        {
            Assert.IsTrue(collectionChangedFired);
        }

        [TestMethod]
        public void then_deleted_was_fired_on_element()
        {
            Assert.IsTrue(deletedFired);
        }

    }
}
