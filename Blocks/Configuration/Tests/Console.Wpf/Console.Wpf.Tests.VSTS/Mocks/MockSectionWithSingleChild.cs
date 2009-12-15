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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    [ViewModel(CommonDesignTime.ViewModelTypeNames.SectionViewModel)]
    public class MockSectionWithSingleChild : ConfigurationSection
    {
        private const string childrenProperty = "children";

        public MockSectionWithSingleChild()
        {
            this[childrenProperty] = new NamedElementCollection<TestHandlerDataWithChildren>();
        }

        [ConfigurationProperty(childrenProperty)]
        [ConfigurationCollectionAttribute(typeof(TestHandlerDataWithChildren))]
        public NamedElementCollection<TestHandlerDataWithChildren> Children
        {
            get
            {
                return (NamedElementCollection<TestHandlerDataWithChildren>)this[childrenProperty];
            }
        }
    }
}
