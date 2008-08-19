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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// Base class for custom trace listeners that support formatters.
	/// </summary>
	public abstract class CustomTraceListener : TraceListener
	{
		private ILogFormatter formatter;

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomTraceListener"/> class.
		/// </summary>
		protected CustomTraceListener()
		{
		}

		/// <summary>
		/// Gets or sets the log entry formatter.
		/// </summary>
		public ILogFormatter Formatter
		{
			get { return this.formatter; }
			set { this.formatter = value; }
		}
	}
}
