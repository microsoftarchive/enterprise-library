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
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// Provides a utility method to match assemblies with assembly names;
    /// </summary>
    public static class AssemblyNameMatcher
    {
        /// <summary>
        /// Matches the name of <paramref name="assembly"/> with <paramref name="assemblyNameToMatch"/>.
        /// </summary>
        /// <param name="assembly">The assembly to match.</param>
        /// <param name="assemblyNameToMatch">The assembly name to match.</param>
        /// <returns><see langword="true"/> if the name matches; otherwise <see langword="false"/>.</returns>
        public static bool Matches(Assembly assembly, AssemblyName assemblyNameToMatch)
        {
            if (assembly == null) return false;
            if (assemblyNameToMatch == null) return false;

            var assemblyName = assembly.GetName();

            if (assemblyName.Name == assemblyNameToMatch.Name)
            {
                if (assemblyNameToMatch.Version != null &&
                    assemblyNameToMatch.Version.CompareTo(assemblyName.Version) != 0)
                {
                    return false;
                }
                byte[] requestedAsmPublicKeyToken = assemblyNameToMatch.GetPublicKeyToken();
                if (requestedAsmPublicKeyToken != null)
                {
                    byte[] cachedAssemblyPublicKeyToken = assemblyName.GetPublicKeyToken();

                    if (Convert.ToBase64String(requestedAsmPublicKeyToken) != Convert.ToBase64String(cachedAssemblyPublicKeyToken))
                    {
                        return false;
                    }
                }

                var requestedAssemblyCulture = assemblyNameToMatch.CultureInfo;
                if (requestedAssemblyCulture != null && requestedAssemblyCulture.LCID != CultureInfo.InvariantCulture.LCID)
                {
                    if (assemblyName.CultureInfo.LCID != requestedAssemblyCulture.LCID)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
