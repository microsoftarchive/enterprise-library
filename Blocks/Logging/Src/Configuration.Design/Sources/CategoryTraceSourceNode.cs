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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
	/// <summary>
	/// Represents a trace source that uses categories.
	/// </summary>
	[Image(typeof(CategoryTraceSourceNode))]
	[SelectedImage(typeof(CategoryTraceSourceNode))]
	public sealed class CategoryTraceSourceNode : TraceSourceNode
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="CategoryTraceSourceNode"/>  
		/// </summary>
		public CategoryTraceSourceNode()
			: this(new TraceSourceData(Resources.CategoryTraceSourceNode, SourceLevels.All))
		{
		}

		/// <summary>
		/// Initialize a new instance of a <see cref="CategoryTraceSourceNode"/> class with a <see cref="TraceSourceData"/> instance.
		/// </summary>
		/// <param name="traceSourceData">A <see cref="TraceSourceData"/> instance.</param>
		public CategoryTraceSourceNode(TraceSourceData traceSourceData)
			
		{
			if (null == traceSourceData) throw new ArgumentNullException("traceSourceData");

			Rename(traceSourceData.Name);
			SourceLevels = traceSourceData.DefaultLevel;
		}

		/// <summary>
		/// Get the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		[ReadOnly(false)]
		public override string Name
		{
			get { return base.Name; }
		}
	}
}