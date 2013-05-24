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
using System.Reflection;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile
{
    /// <summary>
    /// Represent a filter that match all types in the specified assembly.
    /// </summary>
    public class AssemblyMatchFilter : MatchFilter
    {
        private string name;
        private AssemblyName assemblyName;

        /// <summary>
        /// Gets or sets the assembly to match.
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.assemblyName = new AssemblyName(value);
            }
        }

        /// <summary>
        /// Returns true if the type match the current type filter.
        /// </summary>
        /// <param name="type">Type to match.</param>
        /// <returns>True if match.</returns>
        public override bool Match(Type type)
        {
            return AssemblyNameMatcher.Matches(type.Assembly, this.assemblyName);
        }
    }
}
