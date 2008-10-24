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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Drawing.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    /// <summary>
    /// Represents the environmental overrides for a specific <see cref="ConfigurationNode"/> instance, which can be interfaces through the property grid.
    /// </summary>
    [TypeConverter(typeof(MergedConfigurationNodeConverter))]
    public class MergedConfigurationNode : ICustomTypeDescriptor
    {
        private ConfigurationNode masterConfigurationNode;
        private ConfigurationNodeMergeData mergeData;

        internal MergedConfigurationNode(ConfigurationNodeMergeData mergedConfigurationNodeData, MergedConfigurationNode mergedConfigurationNode)
            : this(mergedConfigurationNode.masterConfigurationNode, mergedConfigurationNodeData)
        {
        }

        /// <summary>
        /// Initialized a new instance of <see cref="MergedConfigurationNode"/>, given a <see cref="ConfigurationNode"/> instance and 
        /// an <see cref="ConfigurationNodeMergeData"/> instance.
        /// </summary>
        /// <param name="masterConfigurationNode">The <see cref="ConfigurationNode"/> instance this <see cref="MergedConfigurationNode"/> should override the properties from.</param>
        /// <param name="mergedConfigurationNodeData">The <see cref="ConfigurationNodeMergeData"/> instance that contains all the overridden properties for the <paramref name="masterConfigurationNode"/>.</param>
        public MergedConfigurationNode(ConfigurationNode masterConfigurationNode, ConfigurationNodeMergeData mergedConfigurationNodeData)
        {
            this.masterConfigurationNode = masterConfigurationNode;
            this.mergeData = mergedConfigurationNodeData;   
        }

        /// <summary>
        /// Gets all the overridden properties for this <see cref="MergedConfigurationNode"/>.
        /// </summary>
        public ConfigurationNodeMergeData MergeData
        {
            get { return mergeData; }
        }

        private static bool CanPropertyBeOverriden(PropertyDescriptor property)
        {
            if (property.IsReadOnly) return false;
            if (!property.IsBrowsable) return false;
            if (typeof(MergedConfigurationNode).IsAssignableFrom(property.PropertyType)) return false;
            if (property.Name == "Name") return false;
            if (!CanSerializeType(property.PropertyType)) return false;

            foreach(Attribute attributeOnProperty in property.Attributes)
            {
                EnvironmentOverridableAttribute overridableAttribute = attributeOnProperty as EnvironmentOverridableAttribute;
                if (overridableAttribute != null)
                {
                    if (!overridableAttribute.CanOverride)
                    {
                        return false;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            return true;
        }

        private static bool CanSerializeType(Type type)
        {
            if (typeof(ConfigurationNode).IsAssignableFrom(type)) return true;
            if (typeof(IEnvironmentalOverridesSerializable).IsAssignableFrom(type)) return true;
            if (TypeDescriptor.GetConverter(type) != null && TypeDescriptor.GetConverter(type).GetType() != typeof(TypeConverter)) return true;

            return false;
        }

        private PropertyDescriptorCollection SanitizeAndMergeProperties(PropertyDescriptorCollection propertiesOnMaster)
        {
            List<PropertyDescriptor> saneProperties = new List<PropertyDescriptor>();
            foreach (PropertyDescriptor propertyOnMaster in propertiesOnMaster)
            {
                if (CanPropertyBeOverriden(propertyOnMaster))
                {
                    MergedConfigurationProperty mergedProperty = new MergedConfigurationProperty(mergeData, propertyOnMaster, masterConfigurationNode, masterConfigurationNode.Hierarchy);

                    saneProperties.Add(mergedProperty);
                }
            }

            return new PropertyDescriptorCollection(saneProperties.ToArray());
        }

        #region ICustomTypeDescriptor Members

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(masterConfigurationNode);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(masterConfigurationNode);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(masterConfigurationNode);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return new MergedConfigurationNodeConverter();
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(masterConfigurationNode);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(masterConfigurationNode);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(masterConfigurationNode, editorBaseType);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(masterConfigurationNode, attributes);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return ((ICustomTypeDescriptor)this).GetEvents(new Attribute[0]);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection propertiesOnMaster = TypeDescriptor.GetProperties(masterConfigurationNode, attributes);

            return SanitizeAndMergeProperties(propertiesOnMaster);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return masterConfigurationNode;
        }

        #endregion
    }
}
