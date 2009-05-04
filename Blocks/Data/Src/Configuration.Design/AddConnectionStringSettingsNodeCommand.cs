//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class AddConnectionStringSettingsNodeCommand : AddChildNodeCommand
	{
		public AddConnectionStringSettingsNodeCommand(IServiceProvider serviceProvider)
			: base(serviceProvider, typeof(ConnectionStringSettingsNode))
		{
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
			ConnectionStringSettingsNode node = ChildNode as ConnectionStringSettingsNode;
			Debug.Assert(null != node, "Expected ConnectionStringSettingsNode");
		}
	}
}
