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
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Collections;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// A <see cref="IConfigurationSource"/> that uses an <see cref="IDictionary"/> as its backing store.
    /// </summary>
    partial class DictionaryConfigurationSource
    {
        private const string DefaultConfigurationSourceResource = "/{0};component/Configuration.xaml";

        /// <summary>
        /// Creates a new instance of <see cref="DictionaryConfigurationSource"/> from a dictionary expressed in the XAML file located at 
        /// <paramref name="sourceUri"/>.
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        /// <returns>A <see cref="DictionaryConfigurationSource"/> with all the keys that are <see cref="ConfigurationSection"/>s.</returns>
        /// <remarks>This method does not copy keys from the <see cref="ResourceDictionary.MergedDictionaries"/> property if the target dictionary is a <see cref="ResourceDictionary"/>.</remarks>
        public static DictionaryConfigurationSource FromXaml(Uri sourceUri)
        {
            Guard.ArgumentNotNull(sourceUri, "sourceUri");

            var dictionary = LoadResource(sourceUri, true) as IDictionary;

            if (dictionary == null)
            {
                throw new ConfigurationErrorsException(Resources.ExceptionXamlConfigurationInvalidFormat);
            }

            return CopyToDictionaryConfigurationSource(dictionary);
        }

        /// <summary>
        /// Creates a new instance of <see cref="DictionaryConfigurationSource"/> from a dictionary.
        /// </summary>
        /// <param name="dictionary">The source dictionary.</param>
        /// <returns>A <see cref="DictionaryConfigurationSource"/> with all the keys that are <see cref="ConfigurationSection"/>s.</returns>
        /// <remarks>This method does not copy keys from the <see cref="ResourceDictionary.MergedDictionaries"/> property if the target dictionary is a <see cref="ResourceDictionary"/>.</remarks>
        public static DictionaryConfigurationSource FromDictionary(IDictionary dictionary)
        {
            Guard.ArgumentNotNull(dictionary, "dictionary");
            return CopyToDictionaryConfigurationSource(dictionary);
        }

        /// <summary>
        /// Creates a new instance of <see cref="DictionaryConfigurationSource"/> from a dictionary expressed in the XAML file located at 
        /// the default location.
        /// </summary>
        /// <remarks>The default location is a file named 'Configuration.xaml' in the XAP file for application.
        /// This method does not copy keys from the <see cref="ResourceDictionary.MergedDictionaries"/> property if the target dictionary is a <see cref="ResourceDictionary"/>.</remarks>
        /// <returns>A <see cref="DictionaryConfigurationSource"/>.</returns>
        public static DictionaryConfigurationSource CreateDefault()
        {
            string assemblyName = GetAssemblyName(Application.Current.GetType().Assembly);
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, DefaultConfigurationSourceResource, assemblyName), UriKind.Relative);
            var dictionary = LoadResource(uri, false) as IDictionary;

            if (dictionary == null)
            {
                throw new ConfigurationErrorsException(Resources.ExceptionXamlConfigurationInvalidFormat);
            }

            return CopyToDictionaryConfigurationSource(dictionary);
        }

        private static DictionaryConfigurationSource CopyToDictionaryConfigurationSource(IDictionary dictionary)
        {
            DictionaryConfigurationSource source = null;
            try
            {
                source = new DictionaryConfigurationSource();
                foreach (string key in dictionary.Keys.OfType<string>())
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        var section = dictionary[key] as ConfigurationSection;
                        if (section != null)
                        {
                            source.Add(key, section);
                        }
                    }
                }
                return source;
            }
            catch
            {
                using (source) { }
                throw;
            }
        }

        private static string GetAssemblyName(Assembly assembly)
        {
            string assemblyName = assembly.FullName;
            assemblyName = assemblyName.Substring(0, assemblyName.IndexOf(",", StringComparison.Ordinal));
            return assemblyName;
        }

        private static object LoadResource(Uri uri, bool throwIfNotExists)
        {
            var sri = Application.GetResourceStream(uri);
            if (sri == null)
            {
                if (throwIfNotExists)
                {
                    throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionXamlConfigurationResourceNotFound, uri));
                }

                return new ConfigurationDictionary();
            }

            object content;
            try
            {
                string serializedConfiguration;
                using (var reader = new StreamReader(sri.Stream))
                {
                    serializedConfiguration = reader.ReadToEnd();
                }

                content = XamlReader.Load(serializedConfiguration);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(Resources.ExceptionXamlConfigurationInvalidFormat, ex);
            }

            if (content == null)
            {
                throw new ConfigurationErrorsException(Resources.ExceptionXamlConfigurationInvalidFormat);
            }

            return content;
        }
    }
}
