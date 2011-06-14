//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
#if !SILVERLIGHT
using System.Diagnostics;
#else
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
#endif


namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="LogSource"/>.
    /// </summary>
    public partial class TraceSourceData : NamedConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceData"/> class with default values.
        /// </summary>
        public TraceSourceData()
        {
            DefaultLevel = SourceLevels.All;
            AutoFlush = true;
        }

        /// <summary>
        /// Gets or sets the default <see cref="SourceLevels"/> for the trace source.
        /// </summary>
        public SourceLevels DefaultLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the AutoFlush indicating whether Flush should be called on the Listeners after every write.
        /// </summary>
        public bool AutoFlush
        {
            get;
            set;
        }

        private readonly NamedElementCollection<TraceListenerReferenceData> traceListeners = new NamedElementCollection<TraceListenerReferenceData>();

        /// <summary>
        /// Gets the collection of references to trace listeners for the trace source.
        /// </summary>
        public NamedElementCollection<TraceListenerReferenceData> TraceListeners
        {
            get { return this.traceListeners; }
        }
    }
}
