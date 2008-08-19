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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	sealed class InstrumenationSectionCommandRegistrar : CommandRegistrar
	{
		public InstrumenationSectionCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{

		}
		
		public override void Register()
		{
			AddInstrumentationCommand();
			AddDefaultCommands(typeof(InstrumentationNode));
		}

		private void AddInstrumentationCommand()
		{
			AddSingleChildNodeCommand(Resources.InstrumentationUICommandText,
				Resources.InstrumentationUICommandLongText, typeof(InstrumentationNode),
				typeof(ConfigurationApplicationNode));
		}
	}
}
