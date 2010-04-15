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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="MetadataCollection"/> manages a base set of attributes and their overridden values.
    /// </summary>
    /// <remarks>
    /// This is used to manage <see cref="ElementViewModel"/> and <see cref="Property"/> <see cref="Attribute"/> values with overridden values 
    /// provided during the construction of <see cref="ElementViewModel"/> and <see cref="Property"/>.
    /// </remarks>
    public class MetadataCollection
    {
        Dictionary<Type, List<Attribute>> attributesByType;

        ///<summary>
        /// Initializes a new instance of <see cref="MetadataCollection"/>.
        ///</summary>
        ///<param name="attributes">The original, non-overridden, attribute set.</param>
        public MetadataCollection(IEnumerable<Attribute> attributes)
        {
            attributesByType = attributes.GroupBy(x => GetAttributeType(x.GetType())).ToDictionary(k => k.Key, v => v.ToList());
        }

        /// <summary>
        /// Applies a set of overridden <see cref="Attribute"/> values to the base set of attributes.
        /// </summary>
        /// <param name="attributes">The set of <see cref="Attribute"/>s to apply.</param>
        /// <remarks>
        /// The provided attributes are compared, by their type, to the internal set of attributes. And are updated:<br/>
        /// <list>
        /// <item>
        /// If there is no attribute of a particular type in the internal set, that attribute is added to the set.
        /// </item>
        /// <item>
        /// If there is an attribute of a particular type in the internal set already and the attribute does not allow
        /// multiple instances, then the internal attribute is replaced.
        /// </item>
        /// <item>
        /// If there is an attribute of a particular type in the internal set already and the attribute does allow
        /// multiple instances, then the attribute is added to the internal set of attributes.
        /// </item>
        /// </list>
        /// <br/>
        /// If multiple attributes of the same type are provided in <paramref name="attributes"/> and the attribute
        /// is does not allow multiple, only the first attribute will be used in replacing the existing attribute.
        /// </remarks>
        public void Override(IEnumerable<Attribute> attributes)
        {
            foreach (var attributesKeyedByType in attributes.GroupBy(x => GetAttributeType(x.GetType())))
            {
                Type attributeType = attributesKeyedByType.Key;
                List<Attribute> attributesOfType;

                if (!attributesByType.TryGetValue(attributeType, out attributesOfType))
                {
                    attributesOfType = new List<Attribute>();
                    attributesByType[attributeType] = attributesOfType;
                }

                AttributeUsageAttribute usage = AttributeUsageAttribute.GetCustomAttribute(attributesKeyedByType.Key, typeof(AttributeUsageAttribute)) as AttributeUsageAttribute;

                if (usage != null && usage.AllowMultiple)
                {
                    attributesOfType.AddRange(attributesKeyedByType);
                }
                else
                {
                    attributesOfType.Clear();
                    attributesOfType.Add(attributesKeyedByType.First());
                }
            }
        }

        ///<summary>
        /// Gets the set of attributes.
        ///</summary>
        public IEnumerable<Attribute> Attributes
        {
            get
            {
                return attributesByType.SelectMany(x => x.Value);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        private static Type GetAttributeType(Type type)
        {
            if (!typeof(Attribute).IsAssignableFrom(type)) throw new ArgumentException("type");
        
            var attributeType = type;
            while (attributeType.BaseType != typeof(Attribute) && attributeType.BaseType != null)
            {
                attributeType = attributeType.BaseType;
            }

            return attributeType;
        }

        /// <summary>
        /// Creates a new set of <see cref="Attribute"/> instances using <see cref="MetadataCollection"/> to
        /// combine a base set and <see cref="Attribute"/> instances and an override set of <see cref="Attribute"/> instances.
        /// </summary>
        /// <param name="baseSource">The base set of attributes that may be overridden.</param>
        /// <param name="overriddenSource">The overrideent set of attributes.</param>
        /// <returns>The resultant set of <see cref="Attribute"/> instances with overridden values applied.</returns>
        public static IEnumerable<Attribute> CombineAttributes(IEnumerable<Attribute> baseSource, IEnumerable<Attribute> overriddenSource)
        {
            MetadataCollection collection = new MetadataCollection(baseSource);
            collection.Override(overriddenSource);
            return collection.Attributes;
        }
    }
}
