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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// Service class used to get the assemblies that are used as part of the configuration designtime.
    /// </summary>
    /// <remarks>
    /// In order to get an instance of this class, declare it as a constructor argument on the consuming component or use the <see cref="IUnityContainer"/> to obtain an instance from code.
    /// </remarks>
    public class AssemblyLocator
    {    
        /// <summary>
        /// This constructor supports the configuration design-time and is not intended to be used directly from your code.
        /// </summary>
        protected AssemblyLocator()
        {
        }

        /// <summary>
        /// This constructor supports the configuration design-time and is not intended to be used directly from your code.
        /// </summary>
        public AssemblyLocator(string basePath)
        {
            this.assemblies = new List<Assembly>();

            LoadAssembliesFromDirectory(basePath);
        }

        private List<Assembly> assemblies;

        /// <summary>
        /// Gets the list of assemblies that are used as part of the configuration designtime.
        /// </summary>
        /// <value>
        /// The list of assemblies that are used as part of the configuration designtime.
        /// </value>
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
