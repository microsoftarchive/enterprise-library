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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
	/// <summary>
	/// Represents a <see cref="RollingFlatFileTraceListenerData"/> configuration element. 
	/// </summary>
	// TODO: Include the images for your node.
	//[Image(typeof (RollingTraceListenerNode))]
	//[SelectedImage(typeof (RollingTraceListenerNode))]
	public class RollingTraceListenerNode : Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners.TraceListenerNode
	{
		private FormatterNode formatterNode;
		private string formatterName;
		private string header;
		private string footer;
		private RollInterval rollInterval;
		private RollFileExistsBehavior rollFileExistsBehavior;
		private string timeStampPattern;
		private int rollSizeKB;
		private string fileName;

		/// <summary>
		/// Initialize a new instance of the <see cref="RollingTraceListenerNode"/> class.
		/// </summary>
		public RollingTraceListenerNode()
			: this(new RollingFlatFileTraceListenerData(
							Resources.RollingTraceListenerNodeUICommandText,
							DefaultValues.RollingFlatFileTraceListenerFileName,
							DefaultValues.FlatFileListenerHeader,
							DefaultValues.FlatFileListenerFooter,
							DefaultValues.RollSizeKB,
							DefaultValues.TimeStampPattern,
							DefaultValues.RollFileExistsBehaviorValue,
							DefaultValues.RollIntervalValue,
							System.Diagnostics.TraceOptions.None,
							null,
                            System.Diagnostics.SourceLevels.All
			))
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="RollingTraceListenerNode"/> class with a <see cref="RollingFlatFileTraceListenerData"/> instance.
		/// </summary>
		/// <param name="data">A <see cref="RollingFlatFileTraceListenerData"/> instance</param>
		public RollingTraceListenerNode(Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.RollingFlatFileTraceListenerData data)
		{
			if (null == data) throw new ArgumentNullException("data");

			Rename(data.Name);
			this.fileName = data.FileName;
			this.rollSizeKB = data.RollSizeKB;
			this.timeStampPattern = data.TimeStampPattern;
			this.rollFileExistsBehavior = data.RollFileExistsBehavior;
			this.rollInterval = data.RollInterval;
			this.TraceOutputOptions = data.TraceOutputOptions;
            this.Filter = data.Filter;
			this.formatterName = data.Formatter;
			this.header = data.Header;
			this.footer = data.Footer;
		}

		/// <summary>
		/// Gets the <see cref="RollingFlatFileTraceListenerData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="RollingFlatFileTraceListenerData"/> this node represents.
		/// </value>
		[Browsable(false)]
		public override Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TraceListenerData TraceListenerData
		{
			get
			{
				RollingFlatFileTraceListenerData data = new RollingFlatFileTraceListenerData(this.Name,
																fileName,
																header,
																footer,
																rollSizeKB,
																timeStampPattern,
																rollFileExistsBehavior,
																rollInterval,
																this.TraceOutputOptions,
																formatterName,
                                                                this.Filter);
				return data;
			}
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="RollingTraceListenerNode"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[SRDescription("FileNameDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public System.String FileName
		{
			get { return this.fileName; }
			set { this.fileName = value; }
		}

		/// <summary>
		/// Gets or sets the header for the file.
		/// </summary>
		/// <value>
		/// The header for the file.
		/// </value>
		[SRDescription("FlatFileTraceListenerHeader", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string Header
		{
			get { return header; }
			set { header = value; }
		}

		/// <summary>
		/// Gets or sets the footer for the file.
		/// </summary>
		/// <value>
		/// The footer for the file.
		/// </value>
		[SRDescription("FlatFileTraceListenerFooter", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string Footer
		{
			get { return footer; }
			set { footer = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[SRDescription("RollSizeKBDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public int RollSizeKB
		{
			get { return rollSizeKB; }
			set { rollSizeKB = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[SRDescription("TimeStampPatternDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string TimeStampPattern
		{
			get { return timeStampPattern; }
			set { timeStampPattern = value; }
		}


		/// <summary>
		/// 
		/// </summary>
		[Required]
		[SRDescription("RollFileExistsBehaviorDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public RollFileExistsBehavior RollFileExistsBehavior
		{
			get { return rollFileExistsBehavior; }
			set { rollFileExistsBehavior = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[SRDescription("RollIntervalDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public RollInterval RollInterval
		{
			get { return rollInterval; }
			set { rollInterval = value; }
		}

		/// <summary>
		/// Gets or sets the formatter for the file.
		/// </summary>
		/// <value>
		/// The formatter for the file.
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
			formatterNode = null;
		}

		private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
		{
			formatterName = e.Node.Name;
		}
	}
}
