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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    /// <summary>
    /// Represents a serialization utility.
    /// </summary>
    public static class SerializationUtility
    {
        /// <summary>
        /// Serailize an object to a strings.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <param name="hierarchy">An <see cref="IConfigurationUIHierarchy"/> object.</param>
        /// <returns>The serailized object to string.</returns>
        public static string SerializeToString(object objectToSerialize, IConfigurationUIHierarchy hierarchy)
        {
            if (objectToSerialize == null)
            {
                return null;
            }
            if (objectToSerialize is IEnvironmentalOverridesSerializable)
            {
                IEnvironmentalOverridesSerializable serializableInstance = objectToSerialize as IEnvironmentalOverridesSerializable;
                return serializableInstance.SerializeToString();
            }
            else if (objectToSerialize is ConfigurationNode)
            {
                ConfigurationNode node = objectToSerialize as ConfigurationNode;
                return CreatePathRelativeToRootNode(node.Path, hierarchy);
            }
            else
            {
                Type targetType = objectToSerialize.GetType();
                TypeConverter converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    return converter.ConvertToInvariantString(objectToSerialize);
                }
            }
            return null;
        }

        /// <summary>
        /// Deserialize content from a string.
        /// </summary>
        /// <param name="serializedContents">The serialized content.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="hierarchy">An <see cref="IConfigurationUIHierarchy"/> object.</param>
        /// <returns>The deserialized content.</returns>
        public static object DeserializeFromString(string serializedContents, Type targetType, IConfigurationUIHierarchy hierarchy)
        {
            if (serializedContents == null)
            {
                return null;
            }
            else if (typeof(IEnvironmentalOverridesSerializable).IsAssignableFrom(targetType))
            {
                IEnvironmentalOverridesSerializable instance = (IEnvironmentalOverridesSerializable)Activator.CreateInstance(targetType);
                instance.DesializeFromString(serializedContents);

                return instance;
            }
            else if (typeof(ConfigurationNode).IsAssignableFrom(targetType))
            {
                string fullNodePath = SerializationUtility.CreateAbsolutePath(serializedContents, hierarchy);
                ConfigurationNode foundNode = hierarchy.FindNodeByPath(fullNodePath);
                if (foundNode != null)
                {
                    if (targetType.IsAssignableFrom(foundNode.GetType()))
                    {
                        return foundNode;
                    }
                }
                return null;
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    return converter.ConvertFromInvariantString(serializedContents);
                }
                return null;
            }
        }

        /// <summary>
        /// Create a path relative to the root node in the hierarchy.
        /// </summary>
        /// <param name="fullConfigurationNodePath">The full path of the node.</param>
        /// <param name="configurationHierarchy">An <see cref="IConfigurationUIHierarchy"/> object.</param>
        /// <returns>The path to the node.</returns>
        public static string CreatePathRelativeToRootNode(string fullConfigurationNodePath, IConfigurationUIHierarchy configurationHierarchy)
        {
            string rootNodeName = configurationHierarchy.RootNode.Name;
            string relativeNodeName = fullConfigurationNodePath.Substring(rootNodeName.Length);
            
            return relativeNodeName;
        }

        /// <summary>
        /// Creates an absolute path to node.
        /// </summary>
        /// <param name="configurationPathRelativeToRoot">The path relative to where the node is in configuration.</param>
        /// <param name="configurationHierarchy">An <see cref="IConfigurationUIHierarchy"/> object.</param>
        /// <returns>The absolute path.</returns>
        public static string CreateAbsolutePath(string configurationPathRelativeToRoot, IConfigurationUIHierarchy configurationHierarchy)
        {
            string rootNodeName = configurationHierarchy.RootNode.Name;
            string absoluteNodeName = string.Concat(rootNodeName, configurationPathRelativeToRoot);

            return absoluteNodeName;
        }
    }
}
