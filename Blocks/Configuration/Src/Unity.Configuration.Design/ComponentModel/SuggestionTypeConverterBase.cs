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
using System.Reflection.Emit;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design
{
    public abstract class SuggestionTypeConverterBase : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var property = context as ElementProperty;
            if (property == null) return new StandardValuesCollection(new string[0]);

            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => !IsDynamicallyGeneratedAssembly(x));

            return GetStandardValuesForAssemblies(assemblies);
        }

        protected static bool IsDynamicallyGeneratedAssembly(Assembly assembly)
        {
            return assembly is AssemblyBuilder;
        }

        protected abstract StandardValuesCollection GetStandardValuesForAssemblies(IEnumerable<Assembly> assemblies);
    }
}
