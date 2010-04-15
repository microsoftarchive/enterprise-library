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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// This class supports the configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="ConfigurationSectionLocator"/> to obtain a list of configuration section names available in the configuration designer.
    /// </remarks>
    /// <seealso cref="ConfigurationSectionLocator"/>
    public class AssemblyAttributeSectionLocator : ConfigurationSectionLocator
    {
        readonly List<HandlesSectionAttribute> sectionHandleAttributes = new List<HandlesSectionAttribute>();

        /// <summary>
        /// This constructor supports the configuration design-time and is not
        /// intended to be used directly from your code.
        /// </summary>
        public AssemblyAttributeSectionLocator(AssemblyLocator assemblyLocator)
        {
            CollectSectionNames(assemblyLocator.Assemblies);
        }

        private void CollectSectionNames(IEnumerable<Assembly> assemblies)
        {
            var handlesSectionAttributes =
                assemblies.FilterSelectManySafe(
                    a => a.GetCustomAttributes(typeof(HandlesSectionAttribute), true).OfType<HandlesSectionAttribute>());

            foreach (var handlesSectionAttribute in handlesSectionAttributes)
            {
                sectionHandleAttributes.Add(handlesSectionAttribute);
            }
        }

        /// <summary>
        /// This property supports the configuration design-time and is not
        /// intended to be used directly from your code.
        /// </summary>
        /// <remarks>
        /// Use the <see cref="ConfigurationSectionLocator"/> class instead.
        /// </remarks>
        public override IEnumerable<string> ConfigurationSectionNames
        {
            get
            {
                return sectionHandleAttributes.Where(a => !a.ClearOnly).Select(a => a.SectionName);
            }
        }

        /// <summary>
        /// This property supports the configuration design-time and is not
        /// intended to be used directly from your code.
        /// </summary>
        /// <remarks>
        /// Use the <see cref="ConfigurationSectionLocator"/> class instead.
        /// </remarks>
        public override IEnumerable<string> ClearableConfigurationSectionNames
        {
            get { return sectionHandleAttributes.Select(a => a.SectionName); }
        }
    }
}
