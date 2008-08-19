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
    /// Represents a <see cref="FlatFileTraceListenerData"/> configuration element.
    /// </summary>
    public class FlatFileTraceListenerNode : TraceListenerNode
    {
        private FormatterNode formatterNode;
		private string formatterName;
		private string fileName;
		private string header;
		private string footer;

        /// <summary>
        /// Initialize a new instance of the <see cref="FlatFileTraceListenerNode"/> class.
        /// </summary>
        public FlatFileTraceListenerNode()
            : this(new FlatFileTraceListenerData(Resources.FlatFileTraceListenerNode, DefaultValues.FlatFileListenerFileName, DefaultValues.FlatFileListenerFooter, DefaultValues.FlatFileListenerHeader, string.Empty))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="FlatFileTraceListenerNode"/> class with a <see cref="FlatFileTraceListenerData"/> instance.
		/// </summary>
		/// <param name="traceListenerData">A <see cref="FlatFileTraceListenerData"/> instance.</param>
        public FlatFileTraceListenerNode(FlatFileTraceListenerData traceListenerData)
        {
			if (null == traceListenerData) throw new ArgumentNullException("traceListenerData");

			Rename(traceListenerData.Name);
			TraceOutputOptions = traceListenerData.TraceOutputOptions;
			this.formatterName = traceListenerData.Formatter;
			this.fileName = traceListenerData.FileName;
			this.header = traceListenerData.Header;
			this.footer = traceListenerData.Footer;
        }        

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
		/// <value>
		/// The file name.
		/// </value>
        [Required]
        [Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
        [FilteredFileNameEditor(typeof(Resources), "FlatFileTraceListenerFileDialogFilter")]
        [SRDescription("FlatFileTraceListenerFlatFileName", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Filename
        {
            get { return fileName; }
            set { fileName = value; }
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
		/// Gets the <see cref="FlatFileTraceListenerData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="FlatFileTraceListenerData"/> this node represents.
		/// </value>
		public override TraceListenerData TraceListenerData
		{
			get
			{
				FlatFileTraceListenerData data = new FlatFileTraceListenerData(Name, fileName, header, footer, formatterName);
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
            formatterNode = null;
        }

        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            formatterName = e.Node.Name;
        }
    }
}