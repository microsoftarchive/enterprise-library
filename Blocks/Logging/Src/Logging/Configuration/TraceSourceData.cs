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

using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings that describe a <see cref="LogSource"/>.
	/// </summary>
	public class TraceSourceData : NamedConfigurationElement
	{
		private const string defaultLevelProperty = "switchValue";
		private const string traceListenersProperty = "listeners";
        private const string autoFlushProperty = "autoFlush";

		/// <summary>
		/// Initializes a new instance of the <see cref="TraceSourceData"/> class with default values.
		/// </summary>
		public TraceSourceData()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TraceSourceData"/> class with name and default level.
		/// </summary>
		/// <param name="name">The name for the represented log source.</param>
		/// <param name="defaultLevel">The trace level for the represented log source.</param>
		public TraceSourceData(string name, SourceLevels defaultLevel)
			: base(name)
		{
			this.DefaultLevel = defaultLevel;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceSourceData"/> class with name, default level and auto flush;
        /// </summary>
        /// <param name="name">The name for the represented log source.</param>
        /// <param name="defaultLevel">The trace level for the represented log source.</param>
        /// <param name="autoFlush">If Flush should be called on the Listeners after every write.</param>
        public TraceSourceData(string name, SourceLevels defaultLevel, bool autoFlush)
            : this(name, defaultLevel)
        {
            this.AutoFlush = autoFlush;
        }

		/// <summary>
		/// Gets or sets the default <see cref="SourceLevels"/> for the trace source.
		/// </summary>
        [ConfigurationProperty(defaultLevelProperty, IsRequired = true, DefaultValue = SourceLevels.All)]
		public SourceLevels DefaultLevel
		{
			get { return (SourceLevels)base[defaultLevelProperty]; }
			set { base[defaultLevelProperty] = value; }
		}

        /// <summary>
        /// Gets or sets the AutoFlush indicating whether Flush should be called on the Listeners after every write.
        /// </summary>
        [ConfigurationProperty(autoFlushProperty, IsRequired = false, DefaultValue = LogSource.DefaultAutoFlushProperty)]
        public bool AutoFlush
        {
            get { return (bool)base[autoFlushProperty]; }
            set { base[autoFlushProperty] = value; }
        }

		/// <summary>
		/// Gets the collection of references to trace listeners for the trace source.
		/// </summary>
		[ConfigurationProperty(traceListenersProperty)]
		public NamedElementCollection<TraceListenerReferenceData> TraceListeners
		{
			get { return (NamedElementCollection<TraceListenerReferenceData>)base[traceListenersProperty]; }
		}
	}
}
