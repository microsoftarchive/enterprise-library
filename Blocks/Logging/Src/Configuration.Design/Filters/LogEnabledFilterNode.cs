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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    /// <summary>
    /// Represents a <see cref="LogEnabledFilterData"/> configuration element.
    /// </summary>
    public sealed class LogEnabledFilterNode : LogFilterNode
    {
        private bool enabled;

		/// <summary>
		/// Initialize an instance of the <see cref="LogEnabledFilterNode"/> class.
		/// </summary>
        public LogEnabledFilterNode()
            : this(new LogEnabledFilterData(Resources.LogEnabledFilterNode, false))
        {
        }

		/// <summary>
		/// Initialize an instance of the <see cref="LogEnabledFilterNode"/> class with a <see cref="LogEnabledFilterData"/> instance.
		/// </summary>
		/// <param name="logEnabledFilterData">A <see cref="LogEnabledFilterData"/> instance</param>
        public LogEnabledFilterNode(LogEnabledFilterData logEnabledFilterData)
			: base(null == logEnabledFilterData ? string.Empty : logEnabledFilterData.Name)
        {
			if (null == logEnabledFilterData) throw new ArgumentNullException("logEnabledFilterData");

			this.enabled = logEnabledFilterData.Enabled;
        }

		/// <summary>
		/// Gets or sets if the filters are enabled.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if the filters are enabled; otherwise, <see langword="false"/>.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("EnabledDescription", typeof(Resources))]
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

		/// <summary>
		/// Gets the <see cref="LogEnabledFilterData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="LogEnabledFilterData"/> this node represents.
		/// </value>
		public override LogFilterData LogFilterData
		{
			get { return new LogEnabledFilterData(Name, enabled); }
		}

    }
}