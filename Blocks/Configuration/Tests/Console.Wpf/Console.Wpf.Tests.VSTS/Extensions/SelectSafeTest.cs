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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Extensions
{
    [TestClass]
    public class when_selecting_from_nonthrowing_collection : TestItemHierarchyContext
    {
        private IEnumerable<string> result;
        private bool exceptionThrown;

        protected override void Act()
        {
            result = item.Children.SelectSafe(c => c.ItemName, (e) => exceptionThrown = true).ToArray();
        }

        [TestMethod]
        public void then_all_items_appear_in_results()
        {
            CollectionAssert.AreEquivalent(item.Children.Select(c => c.ItemName).ToArray(),
                                           result.ToArray());
        }

        [TestMethod]
        public void then_no_exception_thrown()
        {
            Assert.IsFalse(exceptionThrown);
        }
    }

    [TestClass]
    public class when_selecting_from_throwing_collection : TestItemHierarchyContext
    {
        private bool exceptionThrown;
        private IEnumerable<string> result;

        protected override void Arrange()
        {
            base.Arrange();

            item.Children.Insert(0, new ExceptionThrowingItem("Child throwing"));
        }

        protected override void Act()
        {
            result = item.Children.SelectSafe(c => c.ItemName, (e) => exceptionThrown = true).ToArray();
        }

        [TestMethod]
        public void then_only_returns_non_throwing_elements()
        {
            CollectionAssert.AreEquivalent(
                item.Children.Where(c => c.GetType() == typeof (Item)).Select(x => x.ItemName).ToArray(),
                result.ToArray());
        }

        [TestMethod]
        public void then_exceptionaction_invoked()
        {
            Assert.IsTrue(exceptionThrown);
        }
    }

    [TestClass]
    public class when_selecting_from_throwing_collection_with_empty_results : TestItemHierarchyContext
    {
        private bool exceptionThrown;
        private IEnumerable<string> result;

        protected override void Arrange()
        {
            base.Arrange();
            
            item.Children.Clear();
            item.Children.Add(new ExceptionThrowingItem("Child throwing 1"));
            item.Children.Add(new ExceptionThrowingItem("Child throwing 2"));
            item.Children.Add(new ExceptionThrowingItem("Child throwing 3"));
        }

        protected override void Act()
        {
            result = item.Children.SelectSafe(c => c.ItemName, (e) => exceptionThrown = true).ToArray();
        }

        [TestMethod]
        public void then_result_is_empty_enumerable()
        {
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void then_exceptionaction_invoked()
        {
            Assert.IsTrue(exceptionThrown);
        }
    }
}
