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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    internal class LoadedAssembliesDiscoveryService : IAssemblyDiscoveryService
    {
        IDictionary<string, IEnumerable<Assembly>> IAssemblyDiscoveryService.GetAvailableAssemblies()
        {
            var assemblies = new Dictionary<string, IEnumerable<Assembly>>();
            assemblies[Properties.Resources.LoadedAssembliesGroup] = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies;
        }

        bool IAssemblyDiscoveryService.SupportsAssemblyLoading
        {
            get { return true; }
        }
    }
}
