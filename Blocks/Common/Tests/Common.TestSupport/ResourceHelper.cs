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
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport
{
    /// <summary>
    /// Helps manage embedded resources in test assemblies
    /// to avoid DeploymentItem issues.
    /// </summary>
    public class ResourceHelper<TResourceLocator>
    {
        public string DumpResourceFileToDisk(string resourceName)
        {
            return DumpResourceFileToDisk(resourceName, string.Empty);
        }

        public string DumpResourceFileToDisk(string resourceName, string relativeDirectoryPath)
        {
            string configurationFilePath;

            using (Stream resourceStream = GetResourceStream(resourceName))
            using (Stream outputStream = GetOutputStream(resourceName, relativeDirectoryPath, out configurationFilePath))
            {
                CopyStream(resourceStream, outputStream);
            }
            return configurationFilePath;

        }

        private static Stream GetResourceStream(string resourceName)
        {
            string fullResourceName = GetResourceNamespace() + "." + resourceName;

            var currentAssembly = typeof(TResourceLocator).Assembly;
            return currentAssembly.GetManifestResourceStream(fullResourceName);
        }

        private static string GetResourceNamespace()
        {
            return typeof(TResourceLocator).Namespace;
        }

        private static Stream GetOutputStream(string resourceName, string relativeDirectoryPath, out string configFilePath)
        {
            string configFileDir = AppDomain.CurrentDomain.BaseDirectory;
            configFileDir = Path.Combine(configFileDir, relativeDirectoryPath);
            if (!Directory.Exists(configFileDir)) Directory.CreateDirectory(configFileDir);

            configFilePath = Path.Combine(configFileDir, resourceName);

            return new FileStream(configFilePath, FileMode.Create, FileAccess.Write);
        }

        private static void CopyStream(Stream inputStream, Stream outputStream)
        {
            var buffer = new byte[4096];
            int numRead = inputStream.Read(buffer, 0, buffer.Length);
            while (numRead > 0)
            {
                outputStream.Write(buffer, 0, numRead);
                numRead = inputStream.Read(buffer, 0, buffer.Length);
            }
        }
    }
}
