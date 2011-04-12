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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// A <see cref="IConfigurationSource"/> that uses a <see cref="ResourceDictionary"/> as its backing store.
    /// </summary>
    public class ResourceDictionaryConfigurationSource : IConfigurationSource
    {
        private const string DefaultConfigurationSourceResource = "/{0};component/Configuration.xaml";

        private ResourceDictionary dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceDictionaryConfigurationSource"/> class with a
        /// resource dictionary.
        /// </summary>
        /// <param name="dictionary">The resource dictionary.</param>
        public ResourceDictionaryConfigurationSource(ResourceDictionary dictionary)
        {
            Guard.ArgumentNotNull(dictionary, "dictionary");

            this.dictionary = dictionary;
        }

        /// <summary>
        /// Creates a new instance for the <see cref="ResourceDictionary"/> expressed in the XAML file located at 
        /// <paramref name="sourceUri"/>.
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        /// <returns>A <see cref="ResourceDictionaryConfigurationSource"/>.</returns>
        public static ResourceDictionaryConfigurationSource FromXaml(Uri sourceUri)
        {
            Guard.ArgumentNotNull(sourceUri, "sourceUri");

            var dictionary = LoadResource(sourceUri, true) as ResourceDictionary;

            if (dictionary == null)
            {
                throw new ConfigurationErrorsException(Resources.ExceptionXamlConfigurationInvalidFormat);
            }

            return new ResourceDictionaryConfigurationSource(dictionary);
        }

        /// <summary>
        /// Creates a new instance for the <see cref="ResourceDictionary"/> expressed in the XAML file located at 
        /// the default location.
        /// </summary>
        /// <remarks>The default location is a file named 'Configuration.xaml' in the XAP file for application.</remarks>
        /// <returns>A <see cref="ResourceDictionaryConfigurationSource"/>.</returns>
        public static ResourceDictionaryConfigurationSource CreateDefault()
        {
            string assemblyName = GetAssemblyName(Application.Current.GetType().Assembly);
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, DefaultConfigurationSourceResource, assemblyName), UriKind.Relative);
            var dictionary = LoadResource(uri, false) as ResourceDictionary;

            if (dictionary == null)
            {
                throw new ConfigurationErrorsException(Resources.ExceptionXamlConfigurationInvalidFormat);
            }

            return new ResourceDictionaryConfigurationSource(dictionary);
        }

        private static string GetAssemblyName(Assembly assembly)
        {
            // TODO find out if there is a direct way of getting the assembly short name without parsing the qualified name.
            string assemblyName = assembly.FullName;
            assemblyName = assemblyName.Substring(0, assemblyName.IndexOf(","));
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

                return new ResourceDictionary();
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

        /// <summary>
        /// Retrieves the specified <see cref="ConfigurationSection"/>.
        /// </summary>
        /// <param name="sectionName">The name of the section to be retrieved.</param>
        /// <returns>
        /// The specified <see cref="ConfigurationSection"/>, or <see langword="null"/> (<b>Nothing</b> in Visual Basic)
        /// if a section by that name is not found.
        /// </returns>
        public ConfigurationSection GetSection(string sectionName)
        {
            return this.dictionary[sectionName] as ConfigurationSection;
        }

        void IDisposable.Dispose()
        {
        }
    }
}
