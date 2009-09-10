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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects
{
    public class PolymorphicElementCollection : NameTypeConfigurationElementCollection<PolymorphicElement, CustomPolymorphicElement>
    {
    }

    public class PolymorphicElement : NameTypeConfigurationElement
    {

    }

    public class CustomPolymorphicElement : PolymorphicElement
    {
        public CustomPolymorphicElement()
        {
            Type = typeof(CustomPolymorphicElement);
        }
    }

    public class OtherDerivedPolymorphicElement : PolymorphicElement
    {
        public OtherDerivedPolymorphicElement()
        {
            Type = typeof(OtherDerivedPolymorphicElement);
        }
    }
}
