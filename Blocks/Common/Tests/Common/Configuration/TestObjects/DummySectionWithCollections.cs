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
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects
{
    public class DummySectionWithCollections : DummySection
    {
        private const string leafElementProperty = "leaf";
        private const string elementCollectionProperty = "collection";
        private const string polymorphicCollectionProperty = "polymorphicCollection";
        private const string connectionStringsProperty = "connectionStrings";
        private const string settingsProperty = "settings";

        [ConfigurationProperty(leafElementProperty)]
        public TestLeafConfigurationElement LeafElement
        {
            get { return (TestLeafConfigurationElement)base[leafElementProperty]; }
            set { base[leafElementProperty] = value; }
        }

        [ConfigurationProperty(elementCollectionProperty)]
        public MergeableElementCollection LeafElementCollection
        {
            get { return (MergeableElementCollection)base[elementCollectionProperty]; }
            set { base[elementCollectionProperty] = value; }
        }

        [ConfigurationProperty(polymorphicCollectionProperty)]
        public PolymorphicElementCollection PolymorphicCollection
        {
            get { return (PolymorphicElementCollection)base[polymorphicCollectionProperty]; }
            set { base[polymorphicCollectionProperty] = value; }
        }

        [ConfigurationProperty(connectionStringsProperty)]
        public ConnectionStringSettingsCollection ConnectionStringSettingsCollection
        {
            get { return (ConnectionStringSettingsCollection)base[connectionStringsProperty]; }
            set { base[connectionStringsProperty] = value; }
        }

        [ConfigurationProperty(settingsProperty)]
        public KeyValueConfigurationCollection AppSettingsLikeCollection
        {
            get { return (KeyValueConfigurationCollection)base[settingsProperty]; }
            set { base[settingsProperty] = value; }
        }

    }
}
