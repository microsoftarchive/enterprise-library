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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    public class AssemblyAttributeSectionLocator : ConfigurationSectionLocator
    {
        List<string> sectionNames;

        public AssemblyAttributeSectionLocator(AssemblyLocator assemblyLocator)
        {
            sectionNames = CollectSectionNames(assemblyLocator.Assemblies);
        }

        private static List<string> CollectSectionNames(IEnumerable<Assembly> assemblies)
        {
            List<String> sectionNames = new List<string>();
            foreach (var assembly in assemblies)
            {
                foreach (var attribute in assembly.GetCustomAttributes<HandlesSectionAttribute>())
                {
                    sectionNames.Add(attribute.SectionName);
                }
            }

            return sectionNames;
        }

        public override IEnumerable<string> ConfigurationSectionNames
        {
            get
            {
                return sectionNames;
            }
        }
    }

    static class AssemblyExtensions
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this Assembly assembly)
        {
            return assembly.GetCustomAttributes(typeof (T), false).OfType<T>();
        }
    }
}
