//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Utilities
{
    /// <summary>
    /// An extended version of the generic Collection&lt;T&gt; class
    /// that adds some of the functional style methods from List, along with
    /// supporting an event that fires when the collection changes.
    /// </summary>
    /// <typeparam name="T">Type stored in the collection.</typeparam>
    public class CollectionEx<T> : Collection<T>
    {
        /// <summary>
        /// Event that fires when the contents of the collection changes.
        /// </summary>
        public event EventHandler CollectionChanged;

        private bool fireEvents = true;

        #region Constructors

        /// <summary>
        /// Creates a new empty <see cref="CollectionEx{T}" /> instance.
        /// </summary>
        public CollectionEx()
        {
            fireEvents = true;
        }
        
        /// <summary>
        /// Creates a new <see cref="CollectionEx{T}" /> instance which contains
        /// the items provided.
        /// </summary>
        /// <param name="list">List of items to seed the collection.</param>
        public CollectionEx(IList<T> list) : base(new List<T>(list))
        {
            fireEvents = true;
        }

        #endregion

        #region Collection manipulation overrides

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(this, EventArgs.Empty);

        }

        /// <summary>
        /// Inserts a new item into the collection.
        /// </summary>
        /// <param name="index">0-based index for the insertion point. If
        /// index is equal to Count, then insert at the end of the collection.
        /// </param>
        /// <param name="item">Item to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Removes an item at the given index.
        /// </summary>
        /// <param name="index">0-based index of item to remove.</param>
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            OnCollectionChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Replaces the item in the collection at the given index with a new
        /// item.
        /// </summary>
        /// <param name="index">0-based index of item to replace.</param>
        /// <param name="item">New item to store in the collection.</param>
        protected override void SetItem(int index, T item)
        {
            base.SetItem(index, item);
            OnCollectionChanged(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// This method is called when the contents of the collection changes.
        /// </summary>
        /// <remarks>If you override this method, be sure to call the base class
        /// version or the CollectionChanged event will not fire.</remarks>
        /// <param name="sender">set to this.</param>
        /// <param name="e">Extra information about the change.</param>
        protected virtual void OnCollectionChanged(object sender, EventArgs e)
        {
            if(fireEvents && CollectionChanged != null)
            {
                CollectionChanged(sender, e);
            }
        }

        #region Additional methods as per List<T>
        
        /// <summary>
        /// Add a collection of items to the list at the end.
        /// </summary>
        /// <param name="items">Items to add to the end of the list.</param>
        public void AddRange(IEnumerable<T> items)
        {
            fireEvents = false;
            try
            {
                foreach(T item in items)
                {
                    Add(item);
                }
            }
            finally
            {
                fireEvents = true;
            }
        }

        /// <summary>
        /// Determines whether the CollectionEx contains elements that match the conditions
        ///  defined by the specified predicate. 
        /// </summary>
        /// <param name="match">The Predicate delegate that defines the conditions
        ///  of the elements to search for.
        /// </param>
        /// <returns>true if the CollectionEx contains one or more elements that match the
        ///  conditions defined by the specified predicate; otherwise, false. </returns>
        /// <remarks>The Predicate is a delegate to a method that returns true if the object
        ///  passed to it matches the conditions defined in the delegate. The elements of the
        ///  current CollectionEx are individually passed to the Predicate delegate, and
        ///  processing is stopped when a match is found. 
        ///  This method performs a linear search; therefore, this method is an O(n) operation, 
        ///  where n is Count.
        /// </remarks>
        public bool Exists(Predicate<T> match)
        {
            foreach(T item in this)
            {
                if (match(item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the
        /// specified predicate, and returns the first occurrence within the
        /// entire CollectionEx.
        /// </summary>
        /// <param name="match">The Predicate delegate that defines the
        /// conditions of the element to search for.</param>
        /// <returns>The first element that matches the conditions defined by
        /// the specified predicate, if found; otherwise, the default value
        /// for type T. </returns>
        /// <remarks>
        /// <para>
        /// The Predicate is a delegate to a method that returns true if the
        /// object passed to it matches the conditions defined in the delegate.
        /// The elements of the current List are individually passed to the
        /// Predicate delegate, moving forward in the List, starting with the
        /// first element and ending with the last element. Processing is
        /// stopped when a match is found.
        /// </para>
        /// <para>
        /// Important: When searching a list containing value types, make sure
        /// the default value for the type does not satisfy the search predicate.
        /// Otherwise, there is no way to distinguish between a default value
        /// indicating that no match was found and a list element that happens
        /// to have the default value for the type. If the default value
        /// satisfies the search predicate, use the FindIndex method instead.
        /// </para>
        /// <para>
        /// This method performs a linear search; therefore, this method is an
        /// O(n) operation, where n is Count.
        /// </para>
        /// </remarks>
        public T Find(Predicate<T> match)
        {
            foreach (T item in this)
            {
                if (match(item))
                {
                    return item;
                }
            }
            return default(T);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the 
        /// specified predicate, and returns the zero-based index of the first 
        /// occurrence within the entire <see cref="CollectionEx{T}"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// The collection is searched forward starting at the first element and
        /// ending at the last element.
        /// </para>
        /// <para>
        /// The <see cref="Predicate{T}"/> is a delegate to a method that returns
        /// true if the object passed to it matches the conditions defined in 
        /// the delegate. The elements of the current collection are individually
        /// passed to the Predicate delegate.
        /// </para>
        /// <para>
        ///  This method performs a linear search; therefore, this method is 
        ///  an O(n) operation, where n is Count.
        /// </para>
        /// </remarks>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines 
        /// the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="ArgumentNullException">match is a null reference (Nothing in Visual Basic).</exception>
        public int FindIndex(Predicate<T> match)
        {
            return FindIndex(0, Count, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the 
        /// specified predicate, and returns the zero-based index of the first 
        /// occurrence within the entire <see cref="CollectionEx{T}"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// The collection is searched forward starting at the first element and
        /// ending at the last element.
        /// </para>
        /// <para>
        /// The <see cref="Predicate{T}"/> is a delegate to a method that returns
        /// true if the object passed to it matches the conditions defined in 
        /// the delegate. The elements of the current collection are individually
        /// passed to the Predicate delegate.
        /// </para>
        /// <para>
        ///  This method performs a linear search; therefore, this method is 
        ///  an O(n) operation, where n is Count.
        /// </para>
        /// </remarks>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines 
        /// the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="ArgumentNullException">match is a null reference (Nothing in Visual Basic).</exception>

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return FindIndex(startIndex, Count - startIndex, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the 
        /// specified predicate, and returns the zero-based index of the first 
        /// occurrence within the entire <see cref="CollectionEx{T}"/>. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// The collection is searched forward starting at the first element and
        /// ending at the last element.
        /// </para>
        /// <para>
        /// The <see cref="Predicate{T}"/> is a delegate to a method that returns
        /// true if the object passed to it matches the conditions defined in 
        /// the delegate. The elements of the current collection are individually
        /// passed to the Predicate delegate.
        /// </para>
        /// <para>
        ///  This method performs a linear search; therefore, this method is 
        ///  an O(n) operation, where n is Count.
        /// </para>
        /// </remarks>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines 
        /// the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="ArgumentNullException">match is a null reference (Nothing in Visual Basic).</exception>
        public int FindIndex( int startIndex, int count, Predicate<T> match)
        {
            for( int i = startIndex; i < startIndex + count; ++i )
            {
                if(match(this[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Performs the specified action on each element of the CollectionEx.
        /// </summary>
        /// <param name="action">The Action delegate to perform on each
        /// element of the List.
        /// </param>
        /// <remarks>
        /// The Action is a delegate to a method that performs an action on
        /// the object passed to it. The elements of the current List are
        /// individually passed to the Action delegate.
        /// This method is an O(n) operation, where n is Count.
        /// </remarks>
        public void ForEach(Action<T> action)
        {
            foreach(T item in this)
            {
                action(item);
            }
        }

        #endregion
    }
}
