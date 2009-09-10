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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    ///<summary>
    /// Indicates if a collection property is orderable
    ///</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class OrderableCollectionAttribute : Attribute
    {
        ///<summary>
        /// Indicates the colleciton is orderable.
        ///</summary>
        public bool IsOrderable { get; private set; }

        ///<summary>
        /// Initializes a <see cref="OrderableCollectionAttribute"/>
        ///</summary>
        public OrderableCollectionAttribute() : this(false)
        {
        }

        ///<summary>
        /// Initializes a <see cref="OrderableCollectionAttribute"/>
        ///</summary>
        ///<param name="isOrderable"></param>
        public OrderableCollectionAttribute(bool isOrderable)
        {
            IsOrderable = isOrderable;
        }
    }
}
