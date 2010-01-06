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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model_and_config_collection
{
    [TestClass]
    public class when_moving_collection_elements_down : SectionWithMultipleChildrenContext
    {
        private bool descendentElementsChangedFired;
        private CollectionElementViewModel secondHandler;
        private CollectionElementViewModel firstHandler;

        protected override void Arrange()
        {
            base.Arrange();
            descendentElementsChangedFired = false;
            base.ViewModel.DescendentElementsChanged += (s, e) => { descendentElementsChangedFired = true; };
        }

        protected override void Act()
        {
            firstHandler = ViewModel.DescendentElements(x => x.ConfigurationType == typeof(TestHandlerData)).OfType<CollectionElementViewModel>().Where(
                x => x.Name == "One").Single();

            firstHandler.Select();

            secondHandler = ViewModel.DescendentElements(x => x.ConfigurationType == typeof(TestHandlerData)).OfType<CollectionElementViewModel>().Where(
                x => x.Name == "Two").Single();

            secondHandler.MoveDown.Execute(null);
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
    }
}
