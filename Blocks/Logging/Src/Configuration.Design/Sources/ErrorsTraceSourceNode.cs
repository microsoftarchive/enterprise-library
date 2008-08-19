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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
    /// <summary>
	/// Represents the trace source that receives log entries for errors and warnings that occur during the logging process.
    /// </summary>
    [Image(typeof(ErrorsTraceSourceNode))]
    [SelectedImage(typeof(ErrorsTraceSourceNode))]
    public sealed class ErrorsTraceSourceNode : TraceSourceNode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="ErrorsTraceSourceNode"/> with a <see cref="TraceSourceData"/> instance.
        /// </summary>
		/// <param name="traceSourceData">A <see cref="TraceSourceData"/> instance.</param>
        public ErrorsTraceSourceNode(TraceSourceData traceSourceData) : base(Resources.ErrorsTraceSourceNode)
        {
			if (null == traceSourceData) throw new ArgumentNullException("traceSourceData");
			
			SourceLevels = traceSourceData.DefaultLevel;
        }
    }
}