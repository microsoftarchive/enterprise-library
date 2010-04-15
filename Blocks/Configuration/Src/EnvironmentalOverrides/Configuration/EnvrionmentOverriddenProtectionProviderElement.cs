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
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration
{

    /// <summary>
    /// Represents a collection of <see cref="EnvironmentOverriddenProtectionProviderElement"/>'s in configuration.
    /// </summary>
    [ConfigurationCollection(typeof(EnvironmentOverriddenProtectionProviderElement))]
    public class EnvironmentOverriddenProtectionProviderElementCollection : ConfigurationElementCollection, IMergeableConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="EnvironmentOverriddenProtectionProviderElement"/>.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="EnvironmentOverriddenProtectionProviderElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentOverriddenProtectionProviderElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="EnvironmentOverriddenProtectionProviderElement"/>.
        /// </returns>
        /// <param name="element">The <see cref="EnvironmentOverriddenProtectionProviderElement"/> to return the key for. </param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            EnvironmentOverriddenProtectionProviderElement protectionProviderElement = element as EnvironmentOverriddenProtectionProviderElement;
            if (protectionProviderElement != null)
            {
                return protectionProviderElement.ContainingSectionXPath;
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Adds a new <see cref="EnvironmentOverriddenProtectionProviderElement"/> instance to the collection.
        /// </summary>
        /// <param name="protectionProviderOverride">The <see cref="EnvironmentOverriddenProtectionProviderElement"/> instance that should be added to the collection.</param>
        public void Add(EnvironmentOverriddenProtectionProviderElement protectionProviderOverride)
        {
            base.BaseAdd(protectionProviderOverride);
        }

        /// <summary>
        /// Removes a <see cref="EnvironmentOverriddenProtectionProviderElement"/> instance from the collection.
        /// </summary>
        /// <param name="protectionProviderOverride">The <see cref="EnvironmentOverriddenProtectionProviderElement"/> instance that should be removed from the collection.</param>
        public void Remove(EnvironmentOverriddenProtectionProviderElement protectionProviderOverride)
        {
            base.BaseRemove(GetElementKey(protectionProviderOverride));
        }


        #region IMergeableConfigurationElementCollection Members
        void IMergeableConfigurationElementCollection.ResetCollection(IEnumerable<ConfigurationElement> configurationElements)
        {
            foreach (EnvironmentOverriddenProtectionProviderElement element in this.Cast<ConfigurationElement>().ToArray())
            {
                Remove(element);
            }

            foreach (EnvironmentOverriddenProtectionProviderElement element in configurationElements.Reverse())
            {
                base.BaseAdd(0, element);
            }
        }

        ConfigurationElement IMergeableConfigurationElementCollection.CreateNewElement(Type configurationType)
        {
            return (ConfigurationElement)Activator.CreateInstance(configurationType);
        }

        #endregion
    }


    /// <summary>
    /// A configuration element that allows a <see cref="ConfigurationSection"/>'s protected provider to be overwritten.
    /// </summary>
    public class EnvironmentOverriddenProtectionProviderElement : ConfigurationElement
    {
        private const string ProtectionProviderPropertyName = "overriddenValue";
        private const string ConfigurationSectionNameProperty = "containingConfigurationSectionName";
        private const string ContainingSectionXPathProperty = "containingSectionXPath";

        /// <summary>
        /// The protection provider that should be used to encrypt the associated section.
        /// </summary>
        [ConfigurationProperty(ProtectionProviderPropertyName, IsRequired = true)]
        public string ProtectionProvider
        {
            get { return (string)base[ProtectionProviderPropertyName]; }
            set { base[ProtectionProviderPropertyName] = value; }
        }

        /// <summary>
        /// Name of the containing configuration section.
        /// </summary>
        [ConfigurationProperty(ConfigurationSectionNameProperty, IsRequired = true)]
        public string ConfigurationSectionName
        {
            get { return (string)base[ConfigurationSectionNameProperty]; }
            set { base[ConfigurationSectionNameProperty] = value; }
        }

        /// <summary>
        /// XPath that allows to navigate to the XML element that corresponds to the containing configuration section.
        /// </summary>
        [ConfigurationProperty(ContainingSectionXPathProperty, IsRequired = true)]
        public string ContainingSectionXPath
        {
            get { return (string)base[ContainingSectionXPathProperty]; }
            set { base[ContainingSectionXPathProperty] = value; }
        }
    }
}
