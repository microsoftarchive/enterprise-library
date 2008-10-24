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

using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using System.Diagnostics;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    /// <summary>
    /// Represents a <see cref="FormattedEventLogTraceListenerData"/> configuration element.
    /// </summary>
    public class FormattedEventLogTraceListenerNode : TraceListenerNode
    {
        private FormatterNode formatterNode;
		private string machineName;
		private string formatterName;
		private string log;
		private string source;

        /// <summary>
		/// Initialize a new instance of the <see cref="FormattedEventLogTraceListenerNode"/> class.
        /// </summary>
        public FormattedEventLogTraceListenerNode()
            : this(new FormattedEventLogTraceListenerData(Resources.FormattedEventLogTraceListenerNode, DefaultValues.EventLogListenerEventSource, DefaultValues.EventLogListenerLogName, string.Empty, string.Empty))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="FormattedEventLogTraceListenerNode"/> class with a <see cref="FormattedEventLogTraceListenerData"/> instance.
		/// </summary>
		/// <param name="traceListenerData">A <see cref="FormattedEventLogTraceListenerData"/> instance.</param>
        public FormattedEventLogTraceListenerNode(FormattedEventLogTraceListenerData traceListenerData)            
        {
			if (null == traceListenerData) throw new ArgumentNullException("traceListenerData");

			Rename(traceListenerData.Name);
			TraceOutputOptions = traceListenerData.TraceOutputOptions;
            this.machineName = traceListenerData.MachineName;
			this.formatterName = traceListenerData.Formatter;
			this.log = traceListenerData.Log;
			this.source = traceListenerData.Source;
        }

        /// <summary>
        /// Gets or sets the event log to use.
        /// </summary>
		/// <value>
		/// The event log to use.
		/// </value>
        [SRDescription("EventLogTraceListenerEventLogNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Log
        {
            get { return log; }
            set { log = value; }
        }

        /// <summary>
        /// Gets or sets the event source to use.
        /// </summary>
		/// <value>
		/// The event source to use.
		/// </value>
        [SRDescription("EventLogTraceListenerSourceNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Required]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        /// <summary>
        /// Gets or sets the machine name to use.
        /// </summary>
		/// <value>
		/// The machine name to use.
		/// </value>
        [SRDescription("MachineNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }

        /// <summary>
        /// Gets or sets the formatter to use.
        /// </summary>
		/// <value>
		/// The formatter to use.
		/// </value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(FormatterNode))]
        [SRDescription("FormatDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public FormatterNode Formatter
        {
            get { return formatterNode; }
            set
            {
                formatterNode = LinkNodeHelper.CreateReference<FormatterNode>(formatterNode,
                    value,
                    OnFormatterNodeRemoved,
                    OnFormatterNodeRenamed);

                formatterName = formatterNode == null ? string.Empty : formatterNode.Name;
            }
        }

		/// <summary>
		/// Gets the <see cref="FormattedEventLogTraceListenerData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="FormattedEventLogTraceListenerData"/> this node represents.
		/// </value>
		public override TraceListenerData TraceListenerData
		{
			get
			{
				FormattedEventLogTraceListenerData data = new FormattedEventLogTraceListenerData(Name, source, log, machineName, formatterName);
				data.TraceOutputOptions = TraceOutputOptions;
				return data;
			}
		}

		/// <summary>
		/// Sets the formatter to use for this listener.
		/// </summary>
		/// <param name="formatterNodeReference">
		/// A <see cref="FormatterNode"/> reference or <see langword="null"/> if no formatter is defined.
		/// </param>
		protected override void SetFormatterReference(ConfigurationNode formatterNodeReference)
		{
			if (formatterName == formatterNodeReference.Name) Formatter = (FormatterNode)formatterNodeReference;
		}

        private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
			formatterName = null;
        }

        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
			formatterName = e.Node.Name;
        }
    }
}
