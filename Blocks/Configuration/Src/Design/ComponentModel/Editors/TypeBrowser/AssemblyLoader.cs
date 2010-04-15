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
using System.Text;
using System.Reflection;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    class AssemblyLoader : IDisposable
    {
        private static bool resolving = false;
        private readonly string copiedAssemblyPath;
        private readonly string referenceDirectory;

        public AssemblyLoader(string assemblyPath, string referenceDirectory)
            : this(referenceDirectory)
        {
            this.copiedAssemblyPath = CreateCopyOfAssembly(assemblyPath);
        }

        public AssemblyLoader(string referenceDirectory)
        {
            this.referenceDirectory = referenceDirectory;

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
            AppDomain.CurrentDomain.TypeResolve += AssemblyResolve;
        }

        internal static string CreateCopyOfAssembly(string originalFilename)
        {
            string assemblyFilename = originalFilename;
            try
            {
                string tempFilename = Path.GetTempFileName();
                File.Copy(originalFilename, tempFilename, true);
                assemblyFilename = tempFilename;
            }
            catch (IOException)
            {
                // No big deal if we couldn't copy the file to temp directory.
                // We will use the original assembly file instead. The downside
                // of using the original assembly file is that you cannot 
                // update the assembly containing the type
                // until you close the Configuration Manager tool.
            }
            return assemblyFilename;
        }

        public string CopiedAssemblyPath
        {
            get { return copiedAssemblyPath; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (resolving) return null;
            try
            {
                resolving = true;
                AssemblyName asmName = null;

                try
                {
                    asmName = new AssemblyName(args.Name);
                }
                catch
                {
                    return null;
                }
                string assemblyFileName = asmName.Name;
                assemblyFileName = Path.Combine(referenceDirectory, assemblyFileName);
                assemblyFileName += ".dll";

                if (File.Exists(assemblyFileName))
                {
                    assemblyFileName = CreateCopyOfAssembly(assemblyFileName);
                }
                else
                {
                    return null;
                }

                try
                {
                    return Assembly.LoadFrom(assemblyFileName);
                }
                catch
                {
                    return null;
                }
            }
            finally
            {
                resolving = false;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
            AppDomain.CurrentDomain.TypeResolve -= AssemblyResolve;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
