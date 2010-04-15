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
using System.Text;
using System.Collections.Generic;

namespace Console.Wpf.Tests.VSTS.Extensions
{
    public class Item
    {
        private readonly string itemName;
        private readonly List<Item> children = new List<Item>();

        public Item(string itemName)
        {
            this.itemName = itemName;
        }

        public virtual string ItemName
        {
            get { return itemName; }
        }

        public List<Item> Children
        {
            get { return children; }
        }

        public virtual Item[] GetChildrenAsArray()
        {
            return children.ToArray();
        }
    }
}
