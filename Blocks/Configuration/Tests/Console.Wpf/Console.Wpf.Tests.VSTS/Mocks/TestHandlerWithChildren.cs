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
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class TestHandlerDataWithChildren : NameTypeConfigurationElement
    {
        private const string childrenProperty = "children";

        public TestHandlerDataWithChildren()
        {
            this[childrenProperty] = new NamedElementCollection<TestHandlerData>();
        }

        [ConfigurationProperty(childrenProperty)]
        public NamedElementCollection<TestHandlerData> Children
        {
            get
            {
                return (NamedElementCollection<TestHandlerData>) this[childrenProperty];
            }
        }
    }
}
