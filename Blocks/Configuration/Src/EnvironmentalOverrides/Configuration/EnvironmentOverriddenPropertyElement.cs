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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="EnvironmentOverriddenPropertyElement"/> objects.
    /// </summary>
    [ConfigurationCollection(typeof(EnvironmentOverriddenPropertyElement), AddItemName = "property")]
    public class EnvironmentOverriddenPropertyElementCollection : ConfigurationElementCollection, IMergeableConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="EnvironmentOverriddenPropertyElement"/> instance.
        /// </summary>
        /// <returns>An instance of <see cref="EnvironmentOverriddenPropertyElement"/></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentOverriddenPropertyElement();
        }

        /// <summary>
        /// Returns an unique identifier which can be used to identify this <see cref="EnvironmentOverriddenPropertyElement"/> instance within its collection.
        /// </summary>
        /// <param name="element">The <see cref="EnvironmentOverriddenPropertyElement"/> to return an identier for.</param>
        /// <returns>An <see cref="Object"/> that acts as the key for the specified <see cref="EnvironmentOverriddenPropertyElement"/>.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            EnvironmentOverriddenPropertyElement environmentNodeMerge = element as EnvironmentOverriddenPropertyElement;
            if (environmentNodeMerge != null)
            {
                return string.Format(CultureInfo.InvariantCulture,"{0}.{1}", environmentNodeMerge.ContainingElementXPath, environmentNodeMerge.Attribute);
            }
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Adds an instance of <see cref="EnvironmentOverriddenPropertyElement"/> to the collection.
        /// </summary>
        /// <param name="property">An instance of <see cref="EnvironmentOverriddenPropertyElement"/>.</param>
        public void Add(EnvironmentOverriddenPropertyElement property)
        {
            base.BaseAdd(property);
        }

        /// <summary>
        /// Removed an instance of <see cref="EnvironmentOverriddenPropertyElement"/> from the collection.
        /// </summary>
        /// <param name="property">An instance of <see cref="EnvironmentOverriddenPropertyElement"/>.</param>
        public void Remove(EnvironmentOverriddenPropertyElement property)
        {
            base.BaseRemove(GetElementKey(property));
        }

        void IMergeableConfigurationElementCollection.ResetCollection(IEnumerable<ConfigurationElement> configurationElements)
        {
            foreach (EnvironmentOverriddenPropertyElement element in this.Cast<ConfigurationElement>().ToArray())
            {
                Remove(element);
            }

            foreach (EnvironmentOverriddenPropertyElement element in configurationElements.Reverse())
            {
                base.BaseAdd(0, element);
            }
        }

        ConfigurationElement IMergeableConfigurationElementCollection.CreateNewElement(Type configurationType)
        {
            return (ConfigurationElement)Activator.CreateInstance(configurationType);
        }


        ///<summary>
        /// Clears the collection.
        ///</summary>
        public void Clear()
        {
            base.BaseClear();
        }
    }

    /// <summary>
    /// Contains information about an overridden property.
    /// </summary>
    public class EnvironmentOverriddenPropertyElement : ConfigurationElement
    {
        private const string ContainingElementXPathPropertyName = "containingElementXPath";
        private const string AttributePropertyName = "attribute";
        private const string OverriddenValuePropertyName = "overridenValue";
        private const string ConfigurationSectionNameProperty = "containingConfigurationSectionName";
        private const string ContainingSectionXPathProperty = "containingSectionXPath";

        /// <summary>
        /// The name of the attribute in XML that contains the overridden value.
        /// </summary>
        [ConfigurationProperty(AttributePropertyName, IsRequired = true, IsKey = true)]
        public string Attribute
        {
            get { return (string)base[AttributePropertyName]; }
            set { base[AttributePropertyName] = value; }
        }

        /// <summary>
        /// The XPath to the element in XML that declares the attribute which contains the overridden value.
        /// </summary>
        [ConfigurationProperty(ContainingElementXPathPropertyName, IsRequired = true, IsKey = true)]
        public string ContainingElementXPath
        {
            get { return (string)base[ContainingElementXPathPropertyName]; }
            set { base[ContainingElementXPathPropertyName] = value; }
        }

        /// <summary>
        /// The overridden value for this property.
        /// </summary>
        [ConfigurationProperty(OverriddenValuePropertyName, IsRequired = true)]
        public string OverriddenValue
        {
            get { return (string)base[OverriddenValuePropertyName]; }
            set { base[OverriddenValuePropertyName] = value; }
        }

        /// <summary>
        /// Name of the containing configuration section.
        /// </summary>
        [ConfigurationProperty(ConfigurationSectionNameProperty, IsRequired = true)]
        public string ConfigurationSectionName
        {
            get { return (string)base[ConfigurationSectionNameProperty];}
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
