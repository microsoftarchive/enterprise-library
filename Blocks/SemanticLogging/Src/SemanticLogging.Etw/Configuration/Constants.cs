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

using System;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Configuration
{
    /// <summary>
    /// Configuration constants and default values.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The configuration namespace.
        /// </summary>
        public const string Namespace = "http://schemas.microsoft.com/practices/2013/entlib/semanticlogging/etw";

        /// <summary>
        /// The default session name prefix.
        /// </summary>
        public const string DefaultSessionNamePrefix = "Microsoft-SemanticLogging-Etw";

        /// <summary>
        /// The default max timeout for flushing all pending events in the buffer.
        /// </summary>
        public static readonly TimeSpan DefaultBufferingFlushAllTimeout = TimeSpan.FromSeconds(5);
    }
}
