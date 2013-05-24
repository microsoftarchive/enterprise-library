#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System.IO;
using Diagnostics.Tracing.Parsers;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw
{
    internal class TraceEventManifestsCache
    {
        private const string ManifestExtension = ".manifest.xml";
        private const string ManifestSearchPattern = "*" + ManifestExtension;
        private static readonly object LockObject = new object(); 
        private static readonly string ManifestsPath = Path.Combine(Path.GetTempPath(), "7D2611AE-6432-4639-8B91-3E46EB56CADF");
        private readonly DynamicTraceEventParser parser;
        
        public TraceEventManifestsCache(DynamicTraceEventParser parser)
        {
            this.parser = parser;
        }

        public void Read()
        {
            lock (LockObject)
            {
                if (Directory.Exists(ManifestsPath))
                {
                    this.parser.ReadAllManifests(ManifestsPath);
                }
            }
        }

        public void Write()
        {       
            lock (LockObject)
            {
                this.parser.WriteAllManifests(ManifestsPath);
            }
        }
    }
}
