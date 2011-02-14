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
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="IObjectWithName"/> objects.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IObjectWithName"/>.</typeparam>
    public class NamedElementCollection<T> : List<T>
        where T : IObjectWithName //NamedConfigurationElement, new()
    {
        protected void BaseAdd(T element)
        {
            Add(element);
        }

        ///// <summary>
        ///// Performs the specified action on each element of the collection.
        ///// </summary>
        ///// <param name="action">The action to perform.</param>
        //public void ForEach(Action<T> action)
        //{
        //    for (int index = 0; index < Count; index++)
        //    {
        //        action(Get(index));
        //    }
        //}

        /// <summary>
        /// Gets the configuration element at the specified index location. 
        /// </summary>
        /// <param name="index">The index location of the <see name="T"/> to return. </param>
        /// <returns>The <see name="T"/> at the specified index. </returns>
        public T Get(int index)
        {
            return (T)this[index];
        }

        ///// <summary>
        ///// Add an instance of <typeparamref name="T"/> to the collection.
        ///// </summary>
        ///// <param name="element">An instance of <typeparamref name="T"/>.</param>
        //public void Add(T element)
        //{
        //    BaseAdd(element, true);
        //}

        /// <summary>
        /// Gets the named instance of <typeparamref name="T"/> from the collection.
        /// </summary>
        /// <param name="name">The name of the <typeparamref name="T"/> instance to retrieve.</param>
        /// <returns>The instance of <typeparamref name="T"/> with the specified key; otherwise, <see langword="null"/>.</returns>
        public T Get(string name)
        {
            return this.FirstOrDefault(x => x.Name == name);
        }

        ///// <summary>
        ///// Determines if the name exists in the collection.
        ///// </summary>
        ///// <param name="name">The name to search.</param>
        ///// <returns><see langword="true"/> if the name is contained in the collection; otherwise, <see langword="false"/>.</returns>
        //public bool Contains(string name)
        //{
        //    return BaseGet(name) != null;
        //}

        ///// <summary>
        ///// Remove the named element from the collection.
        ///// </summary>
        ///// <param name="name">The name of the element to remove.</param>
        //public void Remove(string name)
        //{
        //    BaseRemove(name);
        //}
    }
}
