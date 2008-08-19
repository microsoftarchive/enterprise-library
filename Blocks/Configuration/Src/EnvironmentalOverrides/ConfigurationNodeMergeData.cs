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
using System.Runtime.Serialization;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    /// <summary>
    /// Represents the overridden configuration settings for a specific <see cref="ConfigurationNode"/>.
    /// </summary>
    public class ConfigurationNodeMergeData
    {
        private bool overrideProperties = false;
        private Dictionary<string, object> overriddenProperties = new Dictionary<string,object>();

        /// <summary>
        /// Initialize an instance of <see cref="ConfigurationNodeMergeData"/>.
        /// </summary>
        public ConfigurationNodeMergeData()
        {
        }

        /// <summary>
        /// Initialize an instance of <see cref="ConfigurationNodeMergeData"/>, specifying whether properties should be overridden and
        /// another instance of <see cref="ConfigurationNodeMergeData"/> of which the overridden properties are copied.
        /// </summary>
        /// <param name="overrideProperties"><see langword="true"/> if the properties in the original configuration node should be overridden, otherwise <see langword="false"/>.</param>
        /// <param name="mergeData">An other instance of <see cref="ConfigurationNodeMergeData"/> of which the overridden properties are copied.</param>
        public ConfigurationNodeMergeData(bool overrideProperties, ConfigurationNodeMergeData mergeData)
        {
            this.overrideProperties = overrideProperties;
            this.overriddenProperties = mergeData.overriddenProperties;
        }

        /// <summary>
        /// Indicates wether properties are overridden with the information contained in this <see cref="ConfigurationNodeMergeData"/>.
        /// </summary>
        public bool OverrideProperties
        {
            get { return overrideProperties; }
        }

        /// <summary>
        /// Gets the overridden value for a specific property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="targetType">The type in which the overridden value should be returned.</param>
        /// <param name="defaultValue">The value for the overriden property that should be returned if no overridden value exists.</param>
        /// <param name="configurationHierarchy">The <see cref="ConfigurationUIHierarchy"/> that should be used for deserializing the property value.</param>
        /// <returns>
        /// The overridden value of the property specified, as an instance of <paramref name="targetType"/>.
        /// If no overridden value is configured, the defaultValue will be returned. 
        /// </returns>
        public object GetPropertyValue(string propertyName, Type targetType, object defaultValue, IConfigurationUIHierarchy configurationHierarchy)
        {
            object propertyValue;
            if (overriddenProperties.TryGetValue(propertyName, out propertyValue))
            {
                if (propertyValue != null)
                {
                    if (propertyValue is UnserializedPropertyValue)
                    {
                        propertyValue = ((UnserializedPropertyValue)propertyValue).DeserializePropertyValue(targetType, configurationHierarchy);
                    }
                    return propertyValue;
                }
                return null;
            }
            return defaultValue;
        }

        /// <summary>
        /// Stores an overridden value for a specific property.
        /// </summary>
        /// <param name="propertyName">The name of the property this overridden property applies to.</param>
        /// <param name="propertyValue">The value that should be stored for the overridden property.</param>
        public void SetPropertyValue(string propertyName, object propertyValue)
        {
            overriddenProperties[propertyName] = propertyValue;
        }

        /// <summary>
        /// Resets the overridden value for a specific property.
        /// </summary>
        /// <param name="propertyName">The name of the property that should be reset.</param>
        public void ResetPropertyValue(string propertyName)
        {
            if (overriddenProperties.ContainsKey(propertyName))
            {
                overriddenProperties.Remove(propertyName);
            }
        }

        /// <summary>
        /// Gets the list of names of the properties that have overridden values configurated.
        /// </summary>
        public List<String> AllPropertyNames
        {
            get { return new List<string>(overriddenProperties.Keys); }
        }
    }
}
