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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="IObjectWithName"/> objects with unique names.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IObjectWithName"/>.</typeparam>
    public class NamedElementCollection<T> : ConfigurationElementCollection<T>
        where T : IObjectWithName
    {
        /// <summary>
        /// Gets the configuration element at the specified index location. 
        /// </summary>
        /// <param name="index">The index location of the <see name="T"/> to return. </param>
        /// <returns>The <see name="T"/> at the specified index. </returns>
        public T Get(int index)
        {
            return this[index];
        }

        /// <summary>
        /// Gets the named instance of <typeparamref name="T"/> from the collection.
        /// </summary>
        /// <param name="name">The name of the <typeparamref name="T"/> instance to retrieve.</param>
        /// <returns>The instance of <typeparamref name="T"/> with the specified key; otherwise, <see langword="null"/>.</returns>
        public T Get(string name)
        {
            return this.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Determines if the item can be inserted into the collection.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns><see langword="true" /> if the item has a name that is unique in the collection.</returns>
        protected override bool CanInsert(T item)
        {
            string name = item.Name;
            return !string.IsNullOrEmpty(name) && !this.Any(x => x.Name == name);
        }
    }
}
