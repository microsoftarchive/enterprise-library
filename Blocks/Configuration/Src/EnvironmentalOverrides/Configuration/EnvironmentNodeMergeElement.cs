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
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="EnvironmentNodeMergeElement"/> objects.
    /// </summary>
    [ConfigurationCollection(typeof(EnvironmentNodeMergeElement), AddItemName="override")]
    public class EnvironmentNodeMergeElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="EnvironmentNodeMergeElement"/> instance.
        /// </summary>
        /// <returns>An instance of <see cref="EnvironmentNodeMergeElement"/></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentNodeMergeElement();
        }

        /// <summary>
        /// Returns an unique identifier which can be used to identify this <see cref="EnvironmentNodeMergeElement"/> instance within its collection.
        /// </summary>
        /// <param name="element">The <see cref="EnvironmentNodeMergeElement"/> to return an identier for.</param>
        /// <returns>An <see cref="Object"/> that acts as the key for the specified <see cref="EnvironmentNodeMergeElement"/>.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            EnvironmentNodeMergeElement environmentNodeMerge = element as EnvironmentNodeMergeElement;
            if (environmentNodeMerge != null)
            {
                return environmentNodeMerge.ConfigurationNodePath;
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Adds an instance of <see cref="EnvironmentNodeMergeElement"/> to the collection.
        /// </summary>
        /// <param name="mergeNode">An instance of <see cref="EnvironmentNodeMergeElement"/>.</param>
        public void Add(EnvironmentNodeMergeElement mergeNode)
        {
            base.BaseAdd(mergeNode);
        }

        /// <summary>
        /// Removed an instance of <see cref="EnvironmentNodeMergeElement"/> from the collection.
        /// </summary>
        /// <param name="mergeElement">An instance of <see cref="EnvironmentNodeMergeElement"/>.</param>
        public void Remove(EnvironmentNodeMergeElement mergeElement)
        {
            base.BaseRemove(mergeElement.ConfigurationNodePath);
        }
    }

    /// <summary>
    /// Represents the overridden configuration settings for one specific configuration node.
    /// <seealso cref="EnvironmentMergeSection"/>
    /// </summary>
    public class EnvironmentNodeMergeElement : ConfigurationElement
    {
        private const string ConfigurationNodePathPropertyName = "nodePath";
        private const string OverridePopertiesPropertyName = "overrideProperties";
        private const string OverriddenPropertiesPropertyName = "overridddenProperties";

        /// <summary>
        /// Gets or sets the path to the configuration node to which the overridden settings apply.
        /// </summary>
        /// <remarks>
        /// The path should not contain the application node part of the path.
        /// </remarks>
        [ConfigurationProperty(ConfigurationNodePathPropertyName, IsKey = true, IsRequired = true)]
        public string ConfigurationNodePath
        {
            get { return (string)base[ConfigurationNodePathPropertyName]; }
            set { base[ConfigurationNodePathPropertyName] = value; }
        }



        /// <summary>
        /// Gets or sets wether the settings in this configuration element should override the main configurations settings at designtime.
        /// </summary>
        [ConfigurationProperty(OverridePopertiesPropertyName)]
        public bool OverrideProperties
        {
            get { return (bool)base[OverridePopertiesPropertyName]; }
            set { base[OverridePopertiesPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets a collection of overridden settings, indexed by the name of the setting that should apply at designtime.
        /// </summary>
        [ConfigurationProperty(OverriddenPropertiesPropertyName, IsDefaultCollection = true)]
        public NameValueConfigurationCollection OverriddenProperties
        {
            get { return (NameValueConfigurationCollection)base[OverriddenPropertiesPropertyName]; }
            set { base[OverriddenPropertiesPropertyName] = value; }
        }
    }
}
