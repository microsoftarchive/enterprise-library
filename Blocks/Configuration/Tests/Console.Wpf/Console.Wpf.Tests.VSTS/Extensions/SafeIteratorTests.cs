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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Extensions
{
    [TestClass]
    public class when_filter_nonthrowing_enumerable : TestItemHierarchyContext
    {
        private IEnumerable<Item> results;
        private bool exceptionthrown;

        protected override void Act()
        {
            results = item.Children.WithNoExceptions((e) => exceptionthrown = true).ToArray();
        }

        [TestMethod]
        public void then_returns_all_values()
        {
            CollectionAssert.AreEquivalent(item.Children.ToArray(), results.ToArray());
        }

        [TestMethod]
        public void then_exception_was_not_thrown()
        {
            Assert.IsFalse(exceptionthrown);
        }
    }

    [TestClass]
    public class when_child_throws_exception : TestItemHierarchyContext
    {
        private Item[] originalChildList;
        private IEnumerable<Item> newChildList;

        protected override void Arrange()
        {
            base.Arrange();

            originalChildList = item.Children.ToArray();
            item.Children.Add(new ExceptionThrowingItem("ChildItem Throwing"));
        }

        protected override void Act()
        {
            newChildList = item.Children.Where(x => x.ItemName == x.ItemName).WithNoExceptions().ToArray();
        }

        [TestMethod]
        public void then_filters_out_exception_items()
        {
            CollectionAssert.AreEquivalent(this.originalChildList.ToArray(), newChildList.ToArray());
        }
        
    }

    [TestClass]
    public class when_child_and_grandchild_throw_exception : TestItemHierarchyContext
    {
        private IEnumerable<Item> result;

        protected override void Arrange()
        {
            base.Arrange();

            var childItem = item.Children[0];
            childItem.Children.Add(new Item("Grandchild Nonthrowing"));
            childItem.Children.Add(new ExceptionThrowingItem("Grandchild Throwing"));

            item.Children.Add(new ExceptionThrowingItem("ChildItem Throwing"));
        }

        protected override void Act()
        {
            result =
                item.Children.Where(x => x.Children.Any(y => y.ItemName.StartsWith("Grandchild"))).WithNoExceptions().ToArray();
        }

        [TestMethod]
        public void then_returns_child_item_with_grandchild()
        {
            Assert.AreEqual(1, result.Count());
            Assert.AreSame(item.Children[0], result.ElementAt(0));
        }
    }

    [TestClass]
    public class when_selecting_throwing_items : TestItemHierarchyContext
    {
        private IEnumerable<string> results;
        private IEnumerable<string> nonthrowingItems;

        protected override void Arrange()
        {
            base.Arrange();
            nonthrowingItems = item.Children.Where(x => x.GetType() == typeof(Item)).Select(x => x.ItemName);
            item.Children.Insert(0, new ExceptionThrowingItem("Child throwing"));
        }

        protected override void Act()
        {
            results = item.Children.Select(x => x.ItemName).WithNoExceptions().ToArray();
        }

        [TestMethod]
        public void then_filters_out_throwing_items()
        {
            CollectionAssert.AreEquivalent(nonthrowingItems.ToArray(), results.ToArray());
        }
    }

    [TestClass]
    public class when_selectingmany : TestItemHierarchyContext
    {
        private IEnumerable<Item> grandchildren;

        protected override void Arrange()
        {
            base.Arrange();
            item.Children.First().Children.Add(new ExceptionThrowingItem("Grandchild throwing 1"));
            item.Children.First().Children.Add(new Item("Grandchild Nonthrowing 1"));
            item.Children.First().Children.Add(new Item("Grandchild Nonthrowing 2"));
            item.Children.Last().Children.Add(new ExceptionThrowingItem("Grandchild throwing 2"));
            item.Children.Last().Children.Add(new Item("Grandchild Nonthrowing 3"));
        }

        protected override void Act()
        {
            grandchildren =
                item.Children.SelectMany(x => x.Children.Where(c => c.ItemName.StartsWith("Grandchild")).WithNoExceptions()).WithNoExceptions();
        }

        [TestMethod]
        public void then_gets_only_good_grandchildren()
        {
            Assert.AreEqual(3, grandchildren.Count());
        }
    }
}
