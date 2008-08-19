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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Drawing.Design;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    /// <summary>
	/// Represents a <see cref="SystemDiagnosticsTraceListenerData"/> configuration element.
    /// </summary>
    public class SystemDiagnosticsTraceListenerNode : TraceListenerNode
    {
        private string typeName;
		private string initData;

        /// <summary>
		/// Initialize a new instance of the <see cref="SystemDiagnosticsTraceListenerNode"/> class.
        /// </summary>
        public SystemDiagnosticsTraceListenerNode()
            : this(new SystemDiagnosticsTraceListenerData(Resources.SystemDiagnosticsTraceListenerNode, string.Empty, string.Empty))
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="SystemDiagnosticsTraceListenerNode"/> class with a <see cref="SystemDiagnosticsTraceListenerData"/> listener.
        /// </summary>
		/// <param name="systemDiagnosticsTraceListenerData">A <see cref="SystemDiagnosticsTraceListenerData"/> listener.</param>
        public SystemDiagnosticsTraceListenerNode(SystemDiagnosticsTraceListenerData systemDiagnosticsTraceListenerData)
        {
			if (null == systemDiagnosticsTraceListenerData) throw new ArgumentNullException("systemDiagnosticsTraceListenerData");

			Rename(systemDiagnosticsTraceListenerData.Name);
			TraceOutputOptions = systemDiagnosticsTraceListenerData.TraceOutputOptions;
			this.typeName = systemDiagnosticsTraceListenerData.TypeName;
			this.initData = systemDiagnosticsTraceListenerData.InitData;

        }

		/// <summary>
		/// Gets or sets the initialization data.
		/// </summary>
		/// <value>
		/// The initialization data.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("InitDataDescription", typeof(Resources))]
        public string InitData
        {
            get { return initData; }
            set { initData = value; }
        }

        /// <summary>
        /// Gets or sets the type of the listener type.
        /// </summary>
		/// <value>
		/// The type of the listener type.
		/// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(TraceListener))]
        [SRDescription("SystemDiagnosticsTraceListenerTypeDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return typeName; }
            set { typeName = value; }
        }

		/// <summary>
		/// Gets the <see cref="SystemDiagnosticsTraceListenerData"/> that this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="SystemDiagnosticsTraceListenerData"/> that this node represents.
		/// </value>
		public override TraceListenerData TraceListenerData
		{
			get 
			{ 
				SystemDiagnosticsTraceListenerData data = new SystemDiagnosticsTraceListenerData(Name, typeName, initData);
				data.TraceOutputOptions = TraceOutputOptions;
				return data;
			}
		}
    }
}