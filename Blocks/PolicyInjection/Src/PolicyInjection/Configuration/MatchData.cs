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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Linq;
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A <see cref="ConfigurationElement"/> that stores information about a single
    /// matchable item. Specifically, the string to match, and whether it is case
    /// sensitive or not.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "MatchDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "MatchDataDisplayName")]
    [NameProperty("Match")]
    public class MatchData : ConfigurationElement
    {
        private const string MatchPropertyName = "match";
        private const string IgnoreCasePropertyName = "ignoreCase";

        /// <summary>
        /// Constructs an empty <see cref="MatchData"/>.
        /// </summary>
        public MatchData()
        {
        }

        /// <summary>
        /// Constructs a <see cref="MatchData"/> with the given matching string.
        /// </summary>
        /// <param name="match">String to match.</param>
        public MatchData(string match)
        {
            Match = match;
        }

        /// <summary>
        /// Constructs a <see cref="MatchData"/> with the given matching string and case-sensitivity flag.
        /// </summary>
        /// <param name="match">String to match.</param>
        /// <param name="ignoreCase">true to do case insensitive comparison, false to do case sensitive.</param>
        public MatchData(string match, bool ignoreCase)
        {
            Match = match;
            IgnoreCase = ignoreCase;
        }

        /// <summary>
        /// Gets or sets the string to match against.
        /// </summary>
        /// <value>The "match" attribute value out of the configuration file.</value>
        [ConfigurationProperty(MatchPropertyName, IsRequired = true, IsKey=true)]
        [ResourceDescription(typeof(DesignResources), "MatchDataMatchDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MatchDataMatchDisplayName")]
        [ViewModel(CommonDesignTime.ViewModelTypeNames.CollectionEditorContainedElementProperty)]
        public string Match
        {
            get { return (string)base[MatchPropertyName]; }
            set { base[MatchPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the case sensitivity flag.
        /// </summary>
        /// <value>The "ignoreCase" attribute value out of the configuration file.</value>
        [ConfigurationProperty(IgnoreCasePropertyName, DefaultValue = false, IsRequired = false)]
        [ResourceDescription(typeof(DesignResources), "MatchDataIgnoreCaseDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MatchDataIgnoreCaseDisplayName")]
        [ViewModel(CommonDesignTime.ViewModelTypeNames.CollectionEditorContainedElementProperty)]
        public bool IgnoreCase
        {
            get { return (bool)base[IgnoreCasePropertyName]; }
            set { base[IgnoreCasePropertyName] = value; }
        }
    }

    /// <summary>
    /// A <see cref="ConfigurationElementCollection"/> storing <see cref="MatchData"/> elements,
    /// or elements derived from <see cref="MatchData"/>.
    /// </summary>
    /// <typeparam name="T">Type of element contained in the collection. Must be <see cref="MatchData"/> or derived from <see cref="MatchData"/>.</typeparam>
    public class MatchDataCollection<T> : ConfigurationElementCollection, IEnumerable<T>, IMergeableConfigurationElementCollection
        where T : MatchData, new()
    {
        /// <summary>
        /// Creates a new empty item to store into the collection.
        /// </summary>
        /// <returns>The created object.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        /// <summary>
        /// Gets the key value from the stored element.
        /// </summary>
        /// <param name="element">Element to retrieve key from.</param>
        /// <returns>The value of the "match" property.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            T match = (T)element;
            return match.Match;
        }

        /// <summary>
        /// Adds the given element to the collection.
        /// </summary>
        /// <param name="match">Element to add.</param>
        public void Add(T match)
        {
            BaseAdd(match);
        }

        /// <summary>
        /// Removes the element at the given index.
        /// </summary>
        /// <param name="index">Index to remove from.</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Removes the match from the element with the given name.
        /// </summary>
        /// <param name="match">Match string to remove.</param>
        public void Remove(string match)
        {
            BaseRemove(match);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// Gets or sets the item at the given index.
        /// </summary>
        /// <param name="index">Index to get/set item from.</param>
        /// <returns>Item at index.</returns>
        public T this[int index]
        {
            get { return (T)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Gets an <see cref="IEnumerator{T}"/> to do a foreach over
        /// the contents of the collection.
        /// </summary>
        /// <returns>The enumerator object.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return this[i];
            }
        }

        void IMergeableConfigurationElementCollection.ResetCollection(IEnumerable<ConfigurationElement> configurationElements)
        {
            while (Count > 0)
            {
                RemoveAt(0);
            }

            foreach (T element in configurationElements.Reverse())
            {
                base.BaseAdd(0, element);
            }
        }

        ConfigurationElement IMergeableConfigurationElementCollection.CreateNewElement(Type configurationType)
        {
            return CreateNewElement();
        }
    }
}
