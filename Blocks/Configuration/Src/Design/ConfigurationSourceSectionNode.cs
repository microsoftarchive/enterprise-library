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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents the design time node for the <see cref="ConfigurationSourceSection"/>.
	/// </summary>
	[Image(typeof(ConfigurationSourceSectionNode))]
	[SelectedImage(typeof(ConfigurationSourceSectionNode))]
	public class ConfigurationSourceSectionNode : ConfigurationSectionNode
	{		
		private ConfigurationSourceElementNode configurationSourceElementNode;
		private EventHandler<ConfigurationNodeChangedEventArgs> onElementNodeRemoved;		

		/// <summary>
		/// 
		/// </summary>
		public ConfigurationSourceSectionNode()
			: base(Resources.ConfigurationSourceNodeName)
		{
			this.onElementNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnElementNodeRemoved);	
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="ConfigurationSourceSectionNode"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (null != configurationSourceElementNode) configurationSourceElementNode.Removed -= onElementNodeRemoved;
		}

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		/// <remarks>
		/// Overridden to make readonly in the design time.
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
		/// Gets or sets the selected (default) configuration source for the application.
		/// </summary>
		/// <value>
		/// The selected (default) configuration source for the application.
		/// </value>
		[Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
		[ReferenceType(typeof(ConfigurationSourceElementNode), true)]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("SelectedSourceDescription", typeof(Resources))]
		[Required]
		public ConfigurationSourceElementNode SelectedSource
		{
			get { return configurationSourceElementNode;  }
			set 
			{
				configurationSourceElementNode = LinkNodeHelper.CreateReference<ConfigurationSourceElementNode>(configurationSourceElementNode,
																								 value,
																								 onElementNodeRemoved,
																								 null);				
			}
		}

		/// <summary>
		/// Gets the selected configuration source.
		/// </summary>
		/// <value>
		/// The selected configuration source.
		/// </value>
		[Browsable(false)]
		public IConfigurationSource ConfigurationSource
		{
			get
			{
                if (configurationSourceElementNode != null)
                {
                    return configurationSourceElementNode.ConfigurationSource;
                }
                else
                {
                    return null;
                }
			}
		}

		/// <summary>
		/// Gets the configuration parameter for the selected configuration source.
		/// </summary>
		/// <value>
		/// The configuration parameter for the selected configuration source.
		/// </value>
		[Browsable(false)]
		public IConfigurationParameter ConfigurationParameter
		{
			get
			{
				return configurationSourceElementNode.ConfigurationParameter;
			}
		}		

		private void OnElementNodeRemoved(object sender, ConfigurationNodeChangedEventArgs args)
		{
			configurationSourceElementNode = null;
		}
	}
}
