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
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    /// <summary>
    /// common methods used by unittesting logic
    /// </summary>
    public static class CommonUtil
    {
        public static bool IsPropertyReadOnly(Type nodeType, string propertyName)
        {
            PropertyDescriptorCollection readOnlyProperties = TypeDescriptor.GetProperties(nodeType, new Attribute[] { new ReadOnlyAttribute(true) });
            return readOnlyProperties[propertyName] != null;
        }

        public static bool IsPropertyRequired(Type nodeType, string propertyName)
        {
            PropertyDescriptorCollection requiredProperties = TypeDescriptor.GetProperties(nodeType, new Attribute[] { new RequiredAttribute() });
            return requiredProperties[propertyName] != null;
        }
    }
}
