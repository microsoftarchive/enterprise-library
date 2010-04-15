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
using System.Windows;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting
{
    /// <summary>
    /// Provides access to the global <see cref="ResourceDictionary"/> items in the configuration tool.
    /// This is used in combination with the <see cref="ConfigurationResources.MergedDictionariesProperty"/> to
    /// manage merging of resource dictionaries, particarly in a hosted environment like Visual Studio,
    /// where you cannot rely on a global <see cref="Application"/> object to be available.
    /// <br/>
    /// This global class also maintains an ExtendedDictionary for extension components to place their <see cref="ResourceDictionary"/>s.
    /// New wizards, for example, may need to add their extended items.
    /// </summary>
    public static class GlobalResources
    {
        private static object lockObject = new object();
        private static KeyedResourceDictionary resources = new KeyedResourceDictionary();
        private const string ExtendedDictionaryKeyName = "ExtendedDictionary";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static GlobalResources()
        {
            lock (lockObject)
            {
                resources.Add(ExtendedDictionaryKeyName, new ResourceDictionary(), true);
            }
        }

        ///<summary>
        /// Retrieves a <see cref="ResourceDictionary"/> by the dictionary key specified.
        ///</summary>
        ///<param name="dictionaryKey"></param>
        ///<returns></returns>
        ///<exception cref="KeyNotFoundException">Thrown if the item cannot be found.</exception>
        ///<remarks>
        /// If the item cannot be located by the key, it will attempt to
        /// locate a xaml file from the Resources of this assembly with the same
        /// name as the key.
        /// </remarks>
        public static ResourceDictionary Get(string dictionaryKey)
        {
            lock (lockObject)
            {
                return resources.Get(dictionaryKey);
            }
        }

        ///<summary>
        /// Adds a resource dictionary based on a relative Uri.
        ///</summary>
        ///<param name="resourceDictionaryKey">The key entry for the dictionary.</param>
        ///<param name="relativeUri">The <see cref="UriKind.Relative"/> <see cref="Uri"/> specifying the resource dictionary.</param>
        /// <exception cref="System.ArgumentException">If an item with the key name already exists.</exception>
        ///<remarks>
        ///The Uri will attempt to be loaded via <see cref="Application.LoadComponent(System.Uri)"/>
        /// <br/>
        /// You cannot add a resource with the name of ExtendedDictionary as there is always one maintained by this class that is
        /// intended to support extensibility scenarios.
        ///</remarks>
        public static ResourceDictionary Add(string resourceDictionaryKey, Uri relativeUri)
        {
            lock (lockObject)
            {
                return Add(resourceDictionaryKey, relativeUri, true);
            }
        }

        ///<summary>
        /// Adds a resource dictionary based on a relative Uri.
        ///</summary>
        ///<param name="resourceDictionaryKey">The key entry for the dictionary.</param>
        ///<param name="relativeResourceUri">A string Uri referencing the resource to add. This string will be converted to a <see cref="UriKind.Relative"/> <see cref="Uri"/>.</param>
        ///<remarks>
        ///The Uri will attempt to be loaded via <see cref="Application.LoadComponent(System.Uri)"/>
        /// <br/>
        /// You cannot add a resource with the name of ExtendedDictionary as there is always one maintained by this class that is
        /// intended to support extensibility scenarios.
        ///</remarks>
        public static ResourceDictionary Add(string resourceDictionaryKey, string relativeResourceUri)
        {
            lock (lockObject)
            {
                return Add(resourceDictionaryKey, new Uri(relativeResourceUri, UriKind.Relative), true);
            }
        }

        ///<summary>
        /// Adds a resource dictionary based on a relative Uri.
        ///</summary>
        ///<param name="resourceDictionaryKey">The key entry for the dictionary.</param>
        ///<param name="relativeResourceUri">A string Uri referencing the resource to add. This string will be converted to a <see cref="UriKind.Relative"/> <see cref="Uri"/>.</param>
        ///<param name="keepAlive">True to maintain the created dictionary with a strong reference instead of a weak reference.</param>
        ///<remarks>
        ///The Uri will attempt to be loaded via <see cref="Application.LoadComponent(System.Uri)"/>
        /// <br/>
        /// You cannot add a resource with the name of ExtendedDictionary as there is always one maintained by this class that is
        /// intended to support extensibility scenarios.
        ///</remarks>
        public static ResourceDictionary Add(string resourceDictionaryKey, string relativeResourceUri, bool keepAlive)
        {
            lock (lockObject)
            {
                return Add(resourceDictionaryKey, new Uri(relativeResourceUri, UriKind.Relative), keepAlive);
            }
        }

        ///<summary>
        /// Adds a resource dictionary based on a relative Uri.
        ///</summary>
        ///<param name="resourceDictionaryKey">The key entry for the dictionary.</param>
        ///<param name="resourceUri">A Uri referencing the resource to add. This must be a <see cref="UriKind.Relative"/> <see cref="Uri"/>.</param>
        ///<param name="keepAlive">True to maintain the created dictionary with a strong reference instead of a weak reference.</param>
        ///<remarks>
        ///The Uri will attempt to be loaded via <see cref="Application.LoadComponent(System.Uri)"/>
        /// <br/>
        /// You cannot add a resource with the name of ExtendedDictionary as there is always one maintained by this class that is
        /// intended to support extensibility scenarios.
        ///</remarks>
        public static ResourceDictionary Add(string resourceDictionaryKey, Uri resourceUri, bool keepAlive)
        {
            var dictionary = (ResourceDictionary)Application.LoadComponent(resourceUri);

            lock (lockObject)
            {
                Add(resourceDictionaryKey, dictionary, keepAlive);
            }

            return dictionary;
        }

        ///<summary>
        /// Adds a resource dictionary with the specified key.
        ///</summary>
        ///<param name="resourceDictionaryKey">Key for the resource dictionary</param>
        ///<param name="dictionary">The <see cref="ResourceDictionary"/> to add.</param>
        ///<param name="keepAlive">True to maintain a strong-reference to the dictionary, false to maintain a weak-reference.</param>
        public static void Add(string resourceDictionaryKey, ResourceDictionary dictionary, bool keepAlive)
        {
            lock (lockObject)
            {
                resources.Add(resourceDictionaryKey, dictionary, keepAlive);
            }
        }

        /// <summary>
        /// Removes a resource dictionary of the specified key.
        /// </summary>
        /// <param name="resourceDictionaryKey">The key of the <see cref="ResourceDictionary"/> to remove.</param>
        public static void Remove(string resourceDictionaryKey)
        {
            lock (lockObject)
            {
                resources.Remove(resourceDictionaryKey);
            }
        }


        ///<summary>
        /// This will clear the resource dictionary except for ExtendedDictionary item.
        /// This method is largely used to support testability
        ///</summary>
        public static void Clear()
        {

            lock (lockObject)
            {
                foreach (var key in resources.Keys)
                {
                    if (key != ExtendedDictionaryKeyName)
                    {
                        resources.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// This is for test-support only and is not intended to be used by
        /// your code.
        /// </summary>
        /// <param name="resourceDictionary"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static void SetDictionary(KeyedResourceDictionary resourceDictionary)
        {
            Guard.ArgumentNotNull(resourceDictionary, "resourceDictionary");

            lock (lockObject)
            {
                resources = resourceDictionary;
                resources.Add(ExtendedDictionaryKeyName, new ResourceDictionary(), true);
            }
        }

        ///<summary>
        /// Adds a <see cref="ResourceDictionary"/> to the extended dictionaries.
        ///</summary>
        ///<param name="dictionary"></param>
        /// <remarks>
        /// This should be used by extensions to the configuration tool for their
        /// resources to get loaded into the visual tree.
        /// </remarks>
        public static void AddExtendedDictionary(ResourceDictionary dictionary)
        {
            lock (lockObject)
            {
                var extendedDictionary = resources.Get(ExtendedDictionaryKeyName);
                if (!extendedDictionary.MergedDictionaries.Contains(dictionary, new ResourceDictionaryComparer()))
                {
                    extendedDictionary.MergedDictionaries.Add(dictionary);
                }
            }
        }

        ///<summary>
        /// Removes a <see cref="ResourceDictionary"/> from the extended dictionaries.
        ///</summary>
        ///<param name="dictionary"></param>
        public static void RemoveExtendedDictionary(ResourceDictionary dictionary)
        {
            lock (lockObject)
            {
                var extendedDictionary = resources.Get(ExtendedDictionaryKeyName);
                extendedDictionary.MergedDictionaries.Remove(dictionary);
            }
        }

    }
}
