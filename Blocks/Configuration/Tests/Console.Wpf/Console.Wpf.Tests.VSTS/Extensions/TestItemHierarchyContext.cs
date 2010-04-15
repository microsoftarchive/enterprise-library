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

using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Console.Wpf.Tests.VSTS.Extensions
{
    public abstract class TestItemHierarchyContext : ArrangeActAssert
    {
        protected Item item;

        protected override void Arrange()
        {
            base.Arrange();
            item = BuildTestItem();

        }
        protected Item BuildTestItem()
        {
            var item = new Item("ParentItem");
            AddChildren(item);
            return item;
        }

        protected void AddChildren(Item item)
        {
            for (int i = 0; i < 10; i++)
            {
                var childItem = new Item(string.Format("ChildItem {0}", i));
                item.Children.Add(childItem);
            }
        }
    }
}
