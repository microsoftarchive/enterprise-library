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
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class MockSectionWithMultipleChildCollections : ConfigurationSection
    {
        private const string polymorphicChildrenProperty = "polymorphicChildren";
        private const string childrenProperty = "children";
        private const string moreChildrenProperty = "moreChildren";

        public MockSectionWithMultipleChildCollections()
        {
            this[childrenProperty] = new NamedElementCollection<TestHandlerData>();
            this[moreChildrenProperty] = new NamedElementCollection<TestHandlerDataWithChildren>();
            this[polymorphicChildrenProperty] =
                new NameTypeConfigurationElementCollection<TestHandlerData, CustomTestHandlerData>();
        }

        [ConfigurationProperty(childrenProperty)]
        [ConfigurationCollection(typeof(TestHandlerData))]
        public NamedElementCollection<TestHandlerData> Children
        {
            get
            {
                return (NamedElementCollection<TestHandlerData>)this[childrenProperty];
            }
        }

        [ConfigurationProperty(moreChildrenProperty)]
        [DisplayName("More Children Items")]
        [ConfigurationCollection(typeof(TestHandlerDataWithChildren))]
        public NamedElementCollection<TestHandlerDataWithChildren> MoreChildren
        {
            get
            {
                return (NamedElementCollection<TestHandlerDataWithChildren>)this[moreChildrenProperty];
            }
        }

        [ConfigurationProperty(polymorphicChildrenProperty)]
        [DisplayName("Polymorphic Children")]
        [ConfigurationCollection(typeof(TestHandlerData))]
        public NameTypeConfigurationElementCollection<TestHandlerData, CustomTestHandlerData> PolymorphicChildren
        {
            get
            {
                return
                    (NameTypeConfigurationElementCollection<TestHandlerData, CustomTestHandlerData>)
                    this[polymorphicChildrenProperty];
            }
        }
    }
}
