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
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model_and_config_collection
{
    [TestClass]
    public class when_moving_collection_elements_up : SectionWithMultipleChildrenContext
    {
        private bool descendentElementsChangedFired;
        private CollectionElementViewModel firstHandler;
        private CollectionElementViewModel secondHandler;
        private PropertyChangedListener firstElementChangeListener;

        protected override void Arrange()
        {
            base.Arrange();
            descendentElementsChangedFired = false;
            ViewModel.DescendentElementsChanged += (s, e) => { descendentElementsChangedFired = true; };
        }

        protected override void Act()
        {
            firstHandler =
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof (TestHandlerData)).OfType
                    <CollectionElementViewModel>()
                    .Where(x => x.Name == "One").Single();

            firstHandler.Select();
            firstElementChangeListener = new PropertyChangedListener(firstHandler);

            secondHandler =
                ViewModel.DescendentElements(x => x.ConfigurationType == typeof (TestHandlerData)).OfType
                    <CollectionElementViewModel>()
                    .Where(x => x.Name == "Two").Single();

            secondHandler.MoveUp.Execute(null);
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

        [TestMethod]
        public void then_relocated_element_gains_selection()
        {
            Assert.IsTrue(secondHandler.IsSelected);
        }

        [TestMethod]
        public void then_previosly_selected_element_loses_selection()
        {
            Assert.IsFalse(firstHandler.IsSelected);
        }

        [TestMethod]
        public void then_property_changed_event_was_raised_when_losing_selection()
        {
            Assert.IsTrue(firstElementChangeListener.ChangedProperties.Contains("IsSelected"));
        }
    }
}
