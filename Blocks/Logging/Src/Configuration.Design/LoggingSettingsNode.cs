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

using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    /// <summary>
    /// Represents the <see cref="LoggingSettings"/> configuration section.
    /// </summary>
    [Image(typeof (LoggingSettingsNode))]
    [SelectedImage(typeof(LoggingSettingsNode))]
    public sealed class LoggingSettingsNode : ConfigurationSectionNode
    {
		private bool loggingEnabled;
		private bool logWarning;
        private CategoryTraceSourceNode defaultCategoryTraceSourceNode;

		/// <summary>
		/// Initialize a new instance of the <see cref="LoggingSettingsNode"/> class.
		/// </summary>
        public LoggingSettingsNode() : base(Resources.LogSettingsNode)
        {			
        }

		/// <summary>
		/// Gets if children added to the node are sorted. Nodes are not sorted added to this node.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if nodes add are sorted; otherwise <see langword="false"/>. The default is <see langword="true"/>.
		/// </value>
        [Browsable(false)]
		public override bool SortChildren
        {
            get{return false;}
        }

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("LoggingSettingsNodeNameDescription", typeof(Resources))]
		[ReadOnly(true)]
		public override string Name
		{
			get { return base.Name; }			
		}

		/// <summary>
		/// Determines if tracing is enabled.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if tracing is enabled; otherwise, <see langword="false"/>.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("LoggingSettingsNodeTracingEnabledDescription", typeof(Resources))]
		public bool TracingEnabled
		{
			get { return loggingEnabled; }
			set { loggingEnabled = value; }
		}

		/// <summary>
		/// Gets or sets the default category.
		/// </summary>
		/// <value>
		/// The default category.
		/// </value>
		[Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
		[ReferenceType(typeof(CategoryTraceSourceNode))]
		[SRDescription("DefaultCategoryDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
        public CategoryTraceSourceNode DefaultCategory
		{
			get { return defaultCategoryTraceSourceNode; }
			set
			{
                defaultCategoryTraceSourceNode = LinkNodeHelper.CreateReference<CategoryTraceSourceNode>(defaultCategoryTraceSourceNode,
					value,
					OnDefaultCategoryNodeRemoved,
					null);
			}
		}

		/// <summary>
		/// Gets or sets if a warning is logged if no categories match.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if a warning is logged if no categories match; otherwise, <see langword="false"/>.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("LogWarningWhenNoCategoryMatchDescription", typeof(Resources))]
		public bool LogWarningWhenNoCategoriesMatch
		{
			get { return logWarning; }
			set { logWarning = value; }
		}

        private void OnDefaultCategoryNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.defaultCategoryTraceSourceNode = null;
        }
    }
}