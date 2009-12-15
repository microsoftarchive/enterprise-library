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
using System.IO;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    public class AssemblyLocator
    {
        readonly string basePath;

        protected AssemblyLocator()
        {
        }

        public AssemblyLocator(string basePath)
        {
            this.basePath = basePath;
            this.assemblies = new List<Assembly>();

            LoadAssembliesFromDirectory(basePath);
        }

        private List<Assembly> assemblies;

        public virtual IEnumerable<Assembly> Assemblies
        {
            get { return assemblies; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        private void LoadAssembliesFromDirectory(string directory)
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                if (Path.GetExtension(file).ToUpperInvariant() == ".DLL" ||
                    Path.GetExtension(file).ToUpperInvariant() == ".EXE")
                {
                    string fullPath = Path.Combine(directory, file);

                    try
                    {
                        assemblies.Add(Assembly.LoadFrom(fullPath));
                    }
                    // Eat expected exceptions - if load fails just go on to the next file.
                    catch (BadImageFormatException) { }
                    catch (FileLoadException) { }
                }
            }
        }
    }
}
