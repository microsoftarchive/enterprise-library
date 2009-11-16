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

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class MockSectionWithUnnamedCollection : ConfigurationSection
    {
        private const string collectionProperty = "unamedCollection";

        public MockSectionWithUnnamedCollection()
        {
            this[collectionProperty] = new UnnamedChildCollection();
        }

        [ConfigurationProperty(collectionProperty, IsDefaultCollection = true)]
        public UnnamedChildCollection ChildCollection
        {
            get { return (UnnamedChildCollection) this[collectionProperty]; }
            set { this[collectionProperty] = value;}
        }
    }

    [ConfigurationCollection(typeof(UnnamedChild))]
    public class UnnamedChildCollection : ConfigurationElementCollection, IMergeableConfigurationElementCollection
    {
        public void ResetCollection(IEnumerable<ConfigurationElement> configurationElements)
        {
            this.BaseClear();
            foreach(var element in configurationElements)
            {
                this.BaseAdd(element);
            }
        }

        ConfigurationElement IMergeableConfigurationElementCollection.CreateNewElement(Type configurationType)
        {
            return CreateNewElement();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new UnnamedChild();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var childElement = element as UnnamedChild;
            if (childElement != null) return childElement.Id;
            return null;
        }
    }

    public class UnnamedChild : ConfigurationElement
    {
        private const string idProperty = "id";

        [ConfigurationProperty(idProperty, IsKey=true)]
        public int Id
        {
            get { return (int)this[idProperty]; }
            set { this[idProperty] = value; }
        }
    }
}
