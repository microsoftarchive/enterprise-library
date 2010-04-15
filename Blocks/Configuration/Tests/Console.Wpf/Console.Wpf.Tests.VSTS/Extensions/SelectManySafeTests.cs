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
using Console.Wpf.Tests.VSTS.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Hosting
{
    [TestClass]
    public class when_selectingmany_from_non_throwing_hierarchy : TestItemHierarchyContext
    {
        private IEnumerable<Item> result;

        protected override void Arrange()
        {
            base.Arrange();

            item.Children.First().Children.Add(new Item("Grandchild 1"));
            item.Children.First().Children.Add(new Item("Grandchild 2"));
            item.Children.Last().Children.Add(new Item("Grandchild 1"));
            item.Children.Last().Children.Add(new Item("Grandchild 2"));
        }

        protected override void Act()
        {
            result = item.Children.SelectManySafe(x => x.GetChildrenAsArray()).Where(x => x.ItemName.StartsWith("Grand"));
        }

        [TestMethod]
        public void then_count_is_right()
        {
            Assert.AreEqual(4, result.Count());
        }
    }

    [TestClass]
    public class when_selectingmany_with_first_child_throwing_method : TestItemHierarchyContext
    {
        private IEnumerable<Item> result;

        protected override void Arrange()
        {
            base.Arrange();

            item.Children.First().Children.Add(new Item("Grandchild Nonthrowing 1"));
            item.Children.First().Children.Add(new Item("Grandchild Nonthrowing 2"));
            item.Children.Last().Children.Add(new Item("Grandchild Nonthrowing 3"));

            var throwingChild = new ExceptionThrowingItem("Child throwing 1");
            throwingChild.Children.Add(new Item("Grandchild Nonthrowing 4"));
            item.Children.Insert(0, throwingChild);
        }

        protected override void Act()
        {
            result = item.Children.SelectManySafe(x => x.GetChildrenAsArray());
        }

        [TestMethod]
        public void then_gets_all_grandchildren()
        {
            Assert.AreEqual(3, result.Count());
        }
    }

    [TestClass]
    public class when_error_occurs_when_action_delegate_supplied : TestItemHierarchyContext
    {
        private IEnumerable<Item> result;
        private bool actionInvoked;

        protected override void Arrange()
        {
            base.Arrange();

            item.Children.Add(new ExceptionThrowingItem("Child throwing"));
        }

        protected override void Act()
        {
            result = item.Children.SelectManySafe(x => x.GetChildrenAsArray(), (e) => actionInvoked = true);
            result.Count();
        }

        [TestMethod]
        public void then_action_invoked()
        {
            Assert.IsTrue(actionInvoked);   
        }
    }

    [TestClass]
    public class when_selectingmany_with__child_throwing_method : TestItemHierarchyContext
    {
        private IEnumerable<Item> result;

        protected override void Arrange()
        {
            base.Arrange();

            item.Children.First().Children.Add(new Item("Grandchild Nonthrowing 1"));
            item.Children.First().Children.Add(new Item("Grandchild Nonthrowing 2"));
            item.Children.Last().Children.Add(new Item("Grandchild Nonthrowing 3"));

            var throwingChild = new ExceptionThrowingItem("Child throwing 1");
            throwingChild.Children.Add(new Item("Grandchild Nonthrowing 4"));
            item.Children.Add(throwingChild);
        }

        protected override void Act()
        {
            result = item.Children.SelectManySafe(x => x.GetChildrenAsArray());
        }

        [TestMethod]
        public void then_gets_all_grandchildren()
        {
            Assert.AreEqual(3, result.Count());
        }
    }

    [TestClass]
    public class when_selectingmany_with_grandchild_throwing : TestItemHierarchyContext
    {
        private IEnumerable<string> result;

        protected override void Arrange()
        {
            base.Arrange();

            item.Children.First().Children.Add(new Item("Grandchild Nonthrowing 1"));
            item.Children.First().Children.Add(new Item("Grandchild Nonthrowing 2"));
            item.Children.First().Children.Add(new ExceptionThrowingItem("Grandchild throwing 1"));
            item.Children.Last().Children.Add(new Item("Grandchild Nonthrowing 3"));

            var throwingChild = new ExceptionThrowingItem("Child throwing 1");
            throwingChild.Children.Add(new Item("Grandchild Nonthrowing 4"));
            throwingChild.Children.Add(new ExceptionThrowingItem("Grandchild throwing 2"));
            item.Children.Insert(0, throwingChild);
        }

        protected override void Act()
        {
            result = item.Children.SelectManySafe(x => x.GetChildrenAsArray().Select(i => i.ItemName));
        }

        [TestMethod]
        public void then_nonthrowing_grandchild_count_correct()
        {
            Assert.AreEqual(3, result.Count());
        }

    }

    [TestClass]
    public class when_selectingmany_with_no_results : ArrangeActAssert
    {
        private IEnumerable<Item> result;
        private Item item;

        protected override void Arrange()
        {
            item = new Item("Parent");
        }

        protected override void Act()
        {
            result = item.Children.SelectManySafe(x => x.GetChildrenAsArray());
        }

        [TestMethod]
        public void then_correctly_returns_empty_resultset()
        {
            Assert.IsFalse(result.Any());
        }
    }

    [TestClass]
    public class when_selectingmany_with_no_results_from_children : ArrangeActAssert
    {
        private IEnumerable<Item> result;
        private Item item;

        protected override void Arrange()
        {
            item = new Item("Parent");
            item.Children.Add(new Item("Child 1"));
            item.Children.Add(new Item("Child 2"));
        }

        protected override void Act()
        {
            result = item.Children.SelectManySafe(x => x.GetChildrenAsArray());
        }

        [TestMethod]
        public void then_correctly_returns_empty_resultset()
        {
            Assert.IsFalse(result.Any());
        }
    }
}
