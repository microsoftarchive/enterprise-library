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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    /// <summary>
	/// Represents a <see cref="TraceListenerData"/> configuration element. This class is abstract.
    /// </summary>
    [Image(typeof (TraceListenerNode))]
    [SelectedImage(typeof (TraceListenerNode))]
    public abstract class TraceListenerNode : ConfigurationNode
    {
		private TraceOptions traceOptions;
        private SourceLevels filter;

        /// <summary>
        /// Initialize a new instance of the <see cref="TraceListenerNode"/> class.
        /// </summary>        
        protected TraceListenerNode() : this(TraceOptions.None, SourceLevels.All)    
        {			
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TraceListenerNode"/> class with a TraceOptions and Filter
        /// </summary>
        /// <param name="traceOptions">Trace data options to be written to the trace output.</param>
        /// <param name="filter">The trace filter for the trace listener.</param>
        protected TraceListenerNode(TraceOptions traceOptions, SourceLevels filter)
        {
            this.filter = filter;
            this.traceOptions = traceOptions;
        }

		/// <summary>
		/// Gets or sets the trace options.
		/// </summary>
		/// <value>
		/// One of the <see cref="TraceOptions"/> value.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("TraceOutputOptionsDescription", typeof(Resources))]
		public TraceOptions TraceOutputOptions
		{
			get { return traceOptions; }
			set { traceOptions = value; }
		}

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// One of the <see cref="SourceLevels"/> value.
        /// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("FilterDescription", typeof(Resources))]
        [DefaultValue(SourceLevels.All)]
        public SourceLevels Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        /// <summary>
        /// Gets the <see cref="TraceListenerData"/> this node represents.
        /// </summary>
		/// <returns>The <see cref="TraceListenerData"/> this node represents..</returns>
		[Browsable(false)]
		public abstract TraceListenerData TraceListenerData { get; }

		/// <summary>
		/// Enumerates the formatters and allows a node to set it's formatter reference.
		/// </summary>
		/// <param name="formatters">A collection of formatter nodes.</param>
		public void SetFormatter(ConfigurationNode formatters)
		{
			if (formatters == null) return;

			formatters.Nodes.ForEach(new Action<ConfigurationNode>(SetFormatterReference));
		}

		/// <summary>
		/// Sets the formatter to use for this listener.
		/// </summary>
		/// <param name="formatterNodeReference">
		/// A <see cref="FormatterNode"/> reference or <see langword="null"/> if no formatter is defined.
		/// </param>
		protected virtual void SetFormatterReference(ConfigurationNode formatterNodeReference)
		{
		}
    }
}
