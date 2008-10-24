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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents a design time node for the <see cref="SystemConfigurationSourceElement"/> configuration.
	/// </summary>
	public class SystemConfigurationSourceElementNode : ConfigurationSourceElementNode
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="SystemConfigurationSourceElement"/> class.
		/// </summary>
		public SystemConfigurationSourceElementNode() : this(new SystemConfigurationSourceElement())
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="SystemConfigurationSourceElement"/> class with a <see cref="SystemConfigurationSourceElement"/>.
		/// </summary>
		/// <param name="element">The <see cref="SystemConfigurationSourceElement"/> to initialize.</param>
		public SystemConfigurationSourceElementNode(SystemConfigurationSourceElement element) : base(null == element ? string.Empty : element.Name)
		{
		}


		/// <summary>
		/// Gets the <see cref="Type"/> of the <see cref="IConfigurationSource"/>.
		/// </summary>
		/// <value>
		/// The <see cref="Type"/> of the <see cref="IConfigurationSource"/>.
		/// </value>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("TypeNameDescription", typeof(Resources))]
		[ReadOnly(true)]
		public string Type
		{
			get { return ConfigurationSourceElement.Type.AssemblyQualifiedName;  }
		}

		/// <summary>
		/// Gets the <see cref="SystemConfigurationSourceElement"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="SystemConfigurationSourceElement"/> this node represents.
		/// </value>
		public override ConfigurationSourceElement ConfigurationSourceElement
		{
			get { return new SystemConfigurationSourceElement(Name); }
		}

		/// <summary>
		/// Gets the <see cref="IConfigurationSource"/> that this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="IConfigurationSource"/> that this node represents.
		/// </value>
		/// <remarks>
		/// Returns a <see cref="FileConfigurationSource"/> based on the application's configuration file.
		/// </remarks>
		public override IConfigurationSource ConfigurationSource
		{
			get 
			{
                string configurationFile = ServiceHelper.GetApplicationConfigurationFile(Site);
                FileConfigurationSource.ResetImplementation(configurationFile, false);

				return new FileConfigurationSource(configurationFile); 
			}
		}

		/// <summary>
		/// Gets the <see cref="IConfigurationParameter"/> that this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="IConfigurationParameter"/> that this node represents.
		/// </value>
		/// <remarks>
		/// Returns a <see cref="FileConfigurationParameter"/> based on the application's configuration file.
		/// </remarks>
		public override IConfigurationParameter ConfigurationParameter
		{
			get 
			{				
				return new FileConfigurationParameter(ServiceHelper.GetApplicationConfigurationFile(Site));
			}
		}
	}
}
