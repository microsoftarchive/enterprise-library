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
    public class MetadataCollection
    {
        Dictionary<Type, List<Attribute>> attributesByType;

        public MetadataCollection(IEnumerable<Attribute> attributes)
        {
            attributesByType = attributes.GroupBy(x => GetAttributeType(x.GetType())).ToDictionary(k => k.Key, v => v.ToList());
        }

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

        public IEnumerable<Attribute> Attributes
        {
            get
            {
                return attributesByType.SelectMany(x => x.Value);
            }
        }

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

        public static IEnumerable<Attribute> CombineAttributes(IEnumerable<Attribute> baseSource, IEnumerable<Attribute> overriddenSource)
        {
            MetadataCollection collection = new MetadataCollection(baseSource);
            collection.Override(overriddenSource);
            return collection.Attributes;
        }
    }
}
