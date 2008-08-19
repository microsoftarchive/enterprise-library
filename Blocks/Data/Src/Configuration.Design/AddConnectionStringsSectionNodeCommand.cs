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
	sealed class AddConnectionStringsSectionNodeCommand : AddChildNodeCommand
	{
		public AddConnectionStringsSectionNodeCommand(IServiceProvider serviceProvider)
			: base(serviceProvider, typeof(ConnectionStringsSectionNode))
        {
        }

		protected override void OnExecuted(EventArgs e)
		{
			ConnectionStringsSectionNode node = ChildNode as ConnectionStringsSectionNode;
			Debug.Assert(null != node, "Expected ConnectionStringsSectionNode");

			new AddConnectionStringSettingsNodeCommand(ServiceProvider).Execute(node);
		}		
	}
}
