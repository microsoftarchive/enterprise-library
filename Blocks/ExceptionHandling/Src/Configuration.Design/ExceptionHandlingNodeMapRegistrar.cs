//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	sealed class ExceptionHandlingNodeMapRegistrar : NodeMapRegistrar
	{
		public ExceptionHandlingNodeMapRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override void Register()
		{
			AddMultipleNodeMap(Resources.DefaultCustomHandlerNodeName,
				typeof(CustomHandlerNode),
				typeof(CustomHandlerData));

			AddMultipleNodeMap(Resources.DefaultWrapHandlerNodeName,
				typeof(WrapHandlerNode),
				typeof(WrapHandlerData));

			AddMultipleNodeMap(Resources.DefaultReplaceHandlerNodeName,
				typeof(ReplaceHandlerNode),
				typeof(ReplaceHandlerData));
		}        
	}
}
