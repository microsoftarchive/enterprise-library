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
}
