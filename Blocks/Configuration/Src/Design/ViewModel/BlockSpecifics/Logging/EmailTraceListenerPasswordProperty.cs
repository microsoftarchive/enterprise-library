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
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Logging
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class EmailTraceListenerPasswordProperty : ElementProperty
    {
        public EmailTraceListenerPasswordProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty, new Attribute[] { new PasswordPropertyTextAttribute(true) })
        {
        }

        public override BindableProperty CreateBindableProperty(Property propertyToBind)
        {
            return new MaskedPasswordBindable(propertyToBind);
        }
    }

    public class MaskedPasswordBindable : BindableProperty
    {
        public MaskedPasswordBindable(Property property)
            : base(property)
        {
        }
    }

#pragma warning restore 1591
}
