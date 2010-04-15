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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class RangeValidatorCultureProperty : ElementProperty
    {
        private static CultureInfoConverter cultureInfoConverter = new CultureInfoConverter();

        public RangeValidatorCultureProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty, 
                   new Attribute[]{
                                      new TypeConverterAttribute(typeof(CultureInfoConverter)), 
                                      new DesigntimeDefaultAttribute(cultureInfoConverter.ConvertToInvariantString(CultureInfo.CurrentCulture))
                                  })
        {
        }
    }

#pragma warning restore 1591
}
