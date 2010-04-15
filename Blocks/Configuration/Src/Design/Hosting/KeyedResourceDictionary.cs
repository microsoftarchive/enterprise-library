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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting
{
    /// <summary>
    /// Maintains reference of <see cref="ResourceDictionary"/> items and auto-loads resources
    /// from well-known locations.
    /// </summary>
    /// <remarks>
    /// This dictionary is used in with hosted WPF applications to help maintain sets of dictionaries 
    /// can be added to and referenced by the <see cref="GlobalResources"/> static facade
    /// and used via Xaml by the <see cref="ConfigurationResources.MergedDictionariesProperty"/>.
    /// <br/>
    /// This dictionary generally maintains its reference of <see cref="ResourceDictionary"/> items as
    /// <see cref="WeakReference"/>s by default.  But can optionally maintain strongly-held references.
    /// <br />
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class KeyedResourceDictionary
    {
        private readonly Dictionary<string, WeakReference> resources = new Dictionary<string, WeakReference>();

        ///<summary>
        /// Returns the set of keys in the collection.
        ///</summary>
        public IEnumerable<string> Keys
        {
            get { return resources.Where(x => x.Value.IsAlive).Select(x => x.Key).ToArray(); }
        }

        ///<summary>
        /// Adds a new instance of <see cref="ResourceDictionary"/> to the collection and optionally keeps it alive.
        ///</summary>
        ///<param name="key">The key to the resource dictionary.</param>
        ///<param name="dictionary">The <see cref="ResourceDictionary"/> item to add.</param>
        ///<param name="keepAlive">True to maintain a strongly-held reference to the dictionary, false to maintain a weak-reference</param>
        public void Add(string key, ResourceDictionary dictionary, bool keepAlive)
        {
            PruneResources();
            resources.Add(key, keepAlive ? new FreezableWeakReference(dictionary) : new WeakReference(dictionary));
        }

        ///<summary>
        /// Retrieves the resource with the specified key.
        ///</summary>
        ///<param name="key"></param>
        ///<returns></returns>
        ///<exception cref="KeyNotFoundException">Thrown if the item cannot be found in the collecation and not auto-loaded from a well-known location.</exception>
        ///<remarks>
        ///If the item cannot be found in the collection by the key provided,
        ///it attempts to search for the resource from the /Resources path
        ///of this assembly for a Xaml file with the same name as key.
        ///</remarks>
        public ResourceDictionary Get(string key)
        {
            if (resources.ContainsKey(key))
            {
                if (resources[key].IsAlive) return (ResourceDictionary) resources[key].Target;
                resources.Remove(key);
            }

            try
            {
                TryAutoLoad(key);
            }
            catch (IOException ex)
            {
                // load fails throw expected exception
                throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.ResourceDictionaryKeyNotFound, key), ex);
            }
            return (ResourceDictionary) resources[key].Target;
        }

        private void TryAutoLoad(string key)
        {
            var fullAssemblyName = Assembly.GetExecutingAssembly().ManifestModule.Name;
            var shortAssemblyName = Path.GetFileNameWithoutExtension(fullAssemblyName);
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0};component/Resources/{1}.xaml", shortAssemblyName, key),
                              UriKind.Relative);
            var dictionary = Application.LoadComponent(uri) as ResourceDictionary;
            Add(key, dictionary, false);
        }

        ///<summary>
        /// Determines if there is an entry in the collection by the provided key name.
        ///</summary>
        ///<param name="key"></param>
        ///<returns></returns>
        public bool Contains(string key)
        {
            return resources.ContainsKey(key);
        }

        internal void Clear()
        {
            resources.Clear();
        }

        ///<summary>
        /// Removes the item by the name of the key specified. 
        ///</summary>
        ///<param name="key"></param>
        public void Remove(string key)
        {
            PruneResources();
            resources.Remove(key);
        }

        private void PruneResources()
        {
            var pruneList = resources.Where(x => x.Value.IsAlive == false).Select(x => x.Key).ToArray();
            foreach (var key in pruneList)
            {
                resources.Remove(key);
            }
        }

        #region Nested type: FreezableWeakReference

        private class FreezableWeakReference : WeakReference
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private readonly object strongReference;

            public FreezableWeakReference(object target) : this(target, false)
            {
            }

            private FreezableWeakReference(object target, bool trackResurrection) : base(target, trackResurrection)
            {
                strongReference = target;
            }
        }

        #endregion
    }
}
