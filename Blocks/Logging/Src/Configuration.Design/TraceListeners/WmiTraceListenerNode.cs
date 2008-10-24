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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using System;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    /// <summary>
    /// Represents a <see cref="WmiTraceListenerData"/> configuration element.
    /// </summary>
    public class WmiTraceListenerNode : TraceListenerNode
    {       

        /// <summary>
        /// Initialize a new instance of the <see cref="WmiTraceListenerNode"/> class.
        /// </summary>
        public WmiTraceListenerNode()
            : this(new WmiTraceListenerData(Resources.WmiTraceListenerNode))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="WmiTraceListenerNode"/> class with a <see cref="WmiTraceListenerData"/> instance.
		/// </summary>
		/// <param name="wmiTraceListenerData">A <see cref="WmiTraceListenerData"/> instance.</param>
        public WmiTraceListenerNode(WmiTraceListenerData wmiTraceListenerData)
        {
			if (null == wmiTraceListenerData) throw new ArgumentNullException("wmiTraceListenerData");

			Rename(wmiTraceListenerData.Name);
			TraceOutputOptions = wmiTraceListenerData.TraceOutputOptions;
            Filter = wmiTraceListenerData.Filter;
        }

		/// <summary>
		/// Gets the <see cref="WmiTraceListenerData"/> this node represents.
		/// </summary>
		/// <value>The <see cref="WmiTraceListenerData"/> this node represents.</value>
		public override TraceListenerData TraceListenerData
		{
			get 
			{ 
				WmiTraceListenerData data = new WmiTraceListenerData(Name);
				data.TraceOutputOptions = TraceOutputOptions;
                data.Filter = Filter;
				return data;
			}
		}
    }
}
