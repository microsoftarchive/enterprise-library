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
using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;

namespace Microsoft.Practices.Unity.Configuration.Design
{
    public class MergeableDeserializableConfigurationElementCollection<TElement> : IMergeableConfigurationElementCollection
        where TElement : DeserializableConfigurationElement
    {
        DeserializableConfigurationElementCollectionBase<TElement> actualCollection;

        public MergeableDeserializableConfigurationElementCollection(DeserializableConfigurationElementCollectionBase<TElement> actualCollection)
        {
            this.actualCollection = actualCollection;
        }

        public void ResetCollection(IEnumerable<ConfigurationElement> configurationElements)
        {
            actualCollection.Clear();

            foreach (TElement element in configurationElements)
            {
                actualCollection.Add(element);
            }
        }

        public ConfigurationElement CreateNewElement(Type configurationType)
        {
            return Activator.CreateInstance(configurationType) as ConfigurationElement;
        }
    }
}
