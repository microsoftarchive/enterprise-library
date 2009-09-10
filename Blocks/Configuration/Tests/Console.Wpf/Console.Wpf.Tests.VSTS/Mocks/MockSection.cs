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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class MockSection : ConfigurationSection
    {
        private const string childrenProperty = "children";

        public MockSection()
        {
            this[childrenProperty] = new NamedElementCollection<TestHandlerData>();
        }

        [ConfigurationProperty(childrenProperty)]
        public NamedElementCollection<TestHandlerData> Children
        {
            get
            {
                return (NamedElementCollection<TestHandlerData>)this[childrenProperty];
            }
        }
    }
}
