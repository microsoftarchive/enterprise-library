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
using System.Reflection;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design
{
    public class AssemblySuggestionTypeConverter : SuggestionTypeConverterBase
    {
        protected override StandardValuesCollection GetStandardValuesForAssemblies(IEnumerable<Assembly> assemblies)
        {
            return new StandardValuesCollection(assemblies
                .Select(x => x.GetName().Name)
                .OrderBy(x => x)
                .ToArray());
        }
    }
}
