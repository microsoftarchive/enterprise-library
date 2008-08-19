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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability
{
	internal sealed class ManageableConfigurationSourceNodeMapRegistrar : NodeMapRegistrar
	{
		public ManageableConfigurationSourceNodeMapRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override void Register()
		{
			AddSingleNodeMap(Resources.ManageableConfigurationSourceUICommandText,
				typeof(ManageableConfigurationSourceElementNode),
				typeof(ManageableConfigurationSourceElement));
		}
	}
}
