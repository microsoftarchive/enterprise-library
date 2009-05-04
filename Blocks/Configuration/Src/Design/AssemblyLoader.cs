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
using System.Text;
using System.Reflection;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    class AssemblyLoader : IDisposable
    {
        private static bool resolving = false;
        string copiedAssemblyPath;
        string referenceDirectory;
        private ResolveEventHandler assemblyResolveHandler;

        public AssemblyLoader(string assemblyPath, string referenceDirectory)
        {
            this.copiedAssemblyPath = CreateCopyOfAsm(assemblyPath);
            this.referenceDirectory = referenceDirectory;
            this.assemblyResolveHandler = new ResolveEventHandler(AssemblyResolve);

            AppDomain.CurrentDomain.AssemblyResolve += assemblyResolveHandler;
            AppDomain.CurrentDomain.TypeResolve += assemblyResolveHandler;
        }

        private string CreateCopyOfAsm(string originalFilename)
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
                    assemblyFileName = CreateCopyOfAsm(assemblyFileName);
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
            AppDomain.CurrentDomain.AssemblyResolve -= assemblyResolveHandler;
            AppDomain.CurrentDomain.TypeResolve -= assemblyResolveHandler;
        }

        #endregion
    }
}
