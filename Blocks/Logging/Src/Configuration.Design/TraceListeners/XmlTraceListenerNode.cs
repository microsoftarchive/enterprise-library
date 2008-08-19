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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
	/// <summary>
	/// Represents a <see cref="XmlTraceListenerData"/> configuration element. 
	/// </summary>
	public class XmlTraceListenerNode : TraceListenerNode
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="XmlTraceListenerNode"/> class.
		/// </summary>
		public XmlTraceListenerNode()
			: this(new XmlTraceListenerData(Resources.XmlTraceListenerNodeUICommandText,DefaultValues.XmlTraceListenerFileName))
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="XmlTraceListenerNode"/> class with a <see cref="XmlTraceListenerData"/> instance.
		/// </summary>
		/// <param name="data">A <see cref="XmlTraceListenerData"/> instance</param>
		public XmlTraceListenerNode(XmlTraceListenerData data)
		{
			if (null == data) throw new ArgumentNullException("data");

			Rename(data.Name);
			this.fileName = data.FileName;
		}

		/// <summary>
		/// Gets the <see cref="XmlTraceListenerData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="XmlTraceListenerData"/> this node represents.
		/// </value>
		[Browsable(false)]
		public override TraceListenerData TraceListenerData
		{
			get
			{
				// TODO: Complete the LoggingWCF.Configuration.XmlTraceListenerData constructor parameters using the XmlTraceListenerNode properties and fields
				XmlTraceListenerData data = new XmlTraceListenerData(this.Name,this.fileName);
				return data;
			}
		}

		 

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="XmlTraceListenerNode"/> and optionally releases the managed resources.
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

		private System.String fileName;
		/// <summary>
		/// File name.
		/// </summary>
		[Required]
		[Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
		[FilteredFileNameEditor(typeof(Resources), "XmlTraceListenerFileDialogFilter")]
		[SRDescription("FileNameDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public System.String FileName
		{
			get { return this.fileName; }
			set { this.fileName = value; }
		}
	}
}
