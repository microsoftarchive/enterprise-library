//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents the design time node for the <see cref="InstrumentationConfigurationSection"/>.
	/// </summary>
	[Image(typeof(InstrumentationNode))]
	[SelectedImage(typeof(InstrumentationNode))]
	public class InstrumentationNode : ConfigurationSectionNode
	{
		private bool performanceCountersEnabled;
		private bool eventLoggingEnabled;
		private bool wmiEnabled;
        private string applicationInstanceName;

		/// <summary>
		/// Initialize a new instance of the <see cref="InstrumentationNode"/> class.
		/// </summary>
		public InstrumentationNode()
			: this(new InstrumentationConfigurationSection())
		{

		}

		/// <summary>
		/// Initialize a new instance of the <see cref="InstrumentationNode"/> class with a <see cref="InstrumentationConfigurationSection"/> instance.
		/// </summary>
		/// <param name="instrumentationSection">A <see cref="InstrumentationConfigurationSection"/> instance.</param>
		public InstrumentationNode(InstrumentationConfigurationSection instrumentationSection)
			: base(Resources.InstrumentationNodeName)
		{
			if (null == instrumentationSection) throw new ArgumentNullException("instrumentationSection");

			this.performanceCountersEnabled = instrumentationSection.PerformanceCountersEnabled;
			this.eventLoggingEnabled = instrumentationSection.EventLoggingEnabled;
			this.wmiEnabled = instrumentationSection.WmiEnabled;
			this.RequirePermission = instrumentationSection.SectionInformation.RequirePermission;
            this.applicationInstanceName = instrumentationSection.ApplicationInstanceName;
		}

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		/// <remarks>
		/// Overriden so it is readonly in the designer.
		/// </remarks>
		[ReadOnly(true)]
		public override string Name
		{
			get
			{
				return base.Name;
			}
		}

		/// <summary>
		/// Gets or sets if performance counters are enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if performace counters are enabled; otherwise <c>false</c>.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("PerformanceCountersEnabledDescription", typeof(Resources))]
		public bool PerformanceCountersEnabled
		{
			get { return performanceCountersEnabled; }
			set { performanceCountersEnabled = value; }
		}

		/// <summary>
		/// Gets or sets if event logging is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if event logging is eabled; otherwise, <c>false</c>.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("EventLoggingEnabledDescription", typeof(Resources))]
		public bool EventLoggingEnabled
		{
			get { return eventLoggingEnabled; }
			set { eventLoggingEnabled = value; }
		}

		/// <summary>
		/// Gets or sets if WMI is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if WMI is enabled; otherwise <c>false</c>.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("WMIEnabledDescription", typeof(Resources))]
		public bool WmiEnabled
		{
			get { return wmiEnabled; }
			set { wmiEnabled = value; }
		}

        /// <summary>
        /// Gets or sets the value of ApplicationInstanceName.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("ApplicationInstanceNameDescription", typeof(Resources))]
        public string ApplicationInstanceName
        {
            get { return applicationInstanceName; }
            set { applicationInstanceName = value; }
        }

		/// <summary>
		/// Gets the configured <see cref="InstrumentationConfigurationSection"/>.
		/// </summary>
		/// <value>
		/// The configured <see cref="InstrumentationConfigurationSection"/>.
		/// </value>
		[Browsable(false)]
		public InstrumentationConfigurationSection InstrumentationConfigurationSection
		{
			get
			{
				InstrumentationConfigurationSection section = new InstrumentationConfigurationSection(performanceCountersEnabled, eventLoggingEnabled, wmiEnabled);
				if (!this.RequirePermission)	// don't set if false
					section.SectionInformation.RequirePermission = this.RequirePermission;
				return section;
			}
		}
	}
}
