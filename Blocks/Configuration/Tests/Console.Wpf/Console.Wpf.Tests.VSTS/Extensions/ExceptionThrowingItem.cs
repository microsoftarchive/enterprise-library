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

namespace Console.Wpf.Tests.VSTS.Extensions
{
    public class ExceptionThrowingItem : Item
    {
        public ExceptionThrowingItem(string name) : base(name)
        {
            
        }

        public override string ItemName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Item[] GetChildrenAsArray()
        {
            throw new InvalidOperationException("GetChildrenAsArray");
        }
    }
}
