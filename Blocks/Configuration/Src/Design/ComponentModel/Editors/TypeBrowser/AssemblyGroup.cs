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

using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Represents a group of assemblies.
    /// </summary>
    public class AssemblyGroup
    {
        private string name;
        private IEnumerable<Assembly> assemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyGroup"/> class with a name and a set of assemblies.
        /// </summary>
        /// <param name="name">The name for the group.</param>
        /// <param name="assemblies">The assemblies in the group.</param>
        public AssemblyGroup(string name, IEnumerable<Assembly> assemblies)
        {
            this.name = name;
            this.assemblies = assemblies;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the set of assemblies.
        /// </summary>
        public IEnumerable<Assembly> Assemblies
        {
            get { return this.assemblies; }
        }
    }
}
