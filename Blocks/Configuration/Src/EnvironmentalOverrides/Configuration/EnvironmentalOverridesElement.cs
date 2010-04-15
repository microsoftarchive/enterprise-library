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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="EnvironmentalOverridesElement"/> objects.
    /// </summary>
    [ConfigurationCollection(typeof(EnvironmentalOverridesElement), AddItemName = "override")]
    public class EnvironmentalOverridesElementCollection : ConfigurationElementCollection, IMergeableConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="EnvironmentalOverridesElement"/> instance.
        /// </summary>
        /// <returns>A new instance of <see cref="EnvironmentalOverridesElement"/></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentalOverridesElement();
        }

        /// <summary>
        /// Returns an unique identifier which can be used to identify this <see cref="EnvironmentalOverridesElement"/> instance within its collection.
        /// </summary>
        /// <param name="element">The <see cref="EnvironmentalOverridesElement"/> to return an identier for.</param>
        /// <returns>An <see cref="Object"/> that acts as the key for the specified <see cref="EnvironmentalOverridesElement"/>.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            EnvironmentalOverridesElement environmentNodeMerge = element as EnvironmentalOverridesElement;
            if (environmentNodeMerge != null)
            {
                return environmentNodeMerge.LogicalParentElementPath;
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Adds an instance of <see cref="EnvironmentalOverridesElement"/> to the collection.
        /// </summary>
        /// <param name="mergeNode">An instance of <see cref="EnvironmentalOverridesElement"/>.</param>
        public void Add(EnvironmentalOverridesElement mergeNode)
        {
            base.BaseAdd(mergeNode);
        }

        /// <summary>
        /// Removed an instance of <see cref="EnvironmentalOverridesElement"/> from the collection.
        /// </summary>
        /// <param name="mergeElement">An instance of <see cref="EnvironmentalOverridesElement"/>.</param>
        public void Remove(EnvironmentalOverridesElement mergeElement)
        {
            if (mergeElement == null) throw new ArgumentNullException("mergeElement");

            base.BaseRemove(mergeElement.LogicalParentElementPath);
        }

        void IMergeableConfigurationElementCollection.ResetCollection(IEnumerable<ConfigurationElement> configurationElements)
        {
            foreach (EnvironmentalOverridesElement element in this.Cast<ConfigurationElement>().ToArray())
            {
                Remove(element);
            }

            foreach (EnvironmentalOverridesElement element in configurationElements.Reverse())
            {
                base.BaseAdd(0, element);
            }
        }

        ConfigurationElement IMergeableConfigurationElementCollection.CreateNewElement(Type configurationType)
        {
            return (ConfigurationElement)Activator.CreateInstance(configurationType);
        }
    }

    /// <summary>
    /// Represents the overridden configuration settings for one specific configuration node.
    /// </summary>
    /// <seealso cref="EnvironmentalOverridesSection"/>
    public class EnvironmentalOverridesElement : ConfigurationElement
    {
        private const string LogicalParentElementPathPropertyName = "logicalParentElementPath";
        private const string OverriddenPropertiesPropertyName = "overriddenProperties";

        /// <summary>
        /// Gets or sets the path to the configuration node to which the overridden settings apply.
        /// </summary>
        [ConfigurationProperty(LogicalParentElementPathPropertyName, IsKey = true, IsRequired = true)]
        public string LogicalParentElementPath
        {
            get { return (string)base[LogicalParentElementPathPropertyName]; }
            set { base[LogicalParentElementPathPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets a collection of overridden settings, indexed by the name of the setting that should apply at designtime.
        /// </summary>
        [ConfigurationProperty(OverriddenPropertiesPropertyName, IsDefaultCollection = true)]
        public EnvironmentOverriddenPropertyElementCollection OverriddenProperties
        {
            get { return (EnvironmentOverriddenPropertyElementCollection)base[OverriddenPropertiesPropertyName]; }
            set { base[OverriddenPropertiesPropertyName] = value; }
        }
    }
}
