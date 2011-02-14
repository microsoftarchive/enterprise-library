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
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design
{
    public class NamespaceSuggestionTypeConverter : SuggestionTypeConverterBase
    {
        protected override StandardValuesCollection GetStandardValuesForAssemblies(IEnumerable<Assembly> assemblies)
        {
            return new StandardValuesCollection(assemblies
                .FilterSelectManySafe(x => x.GetExportedTypes().Select(t => t.Namespace))
                .Distinct()
                .OrderBy(x => x)
                .ToArray());
        }
    }
}
