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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability
{
	/// <summary>
	/// 
	/// </summary>
	public class ManageableConfigurationSourceConfigurationDesignManager : ConfigurationDesignManager
	{
		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		public override void Register(IServiceProvider serviceProvider)
		{
			ManageableConfigurationSourceCommandRegistrar cmdRegistrar = new ManageableConfigurationSourceCommandRegistrar(serviceProvider);
			cmdRegistrar.Register();
			ManageableConfigurationSourceNodeMapRegistrar nodeRegistrar = new ManageableConfigurationSourceNodeMapRegistrar(serviceProvider);
			nodeRegistrar.Register();
		}
	}
}
