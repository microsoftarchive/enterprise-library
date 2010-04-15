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
using System.Windows;
using System.Windows.Markup;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    internal class ResourceDictionaryComparer : IEqualityComparer<ResourceDictionary>
    {
        public bool Equals(ResourceDictionary x, ResourceDictionary y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            IUriContext a = x;
            IUriContext b = y;

            return Uri.Compare(a.BaseUri, b.BaseUri, UriComponents.Path, UriFormat.Unescaped,
                               StringComparison.OrdinalIgnoreCase) == 0;
        }

        public int GetHashCode(ResourceDictionary obj)
        {
            return obj.GetHashCode();
        }
    }
}
