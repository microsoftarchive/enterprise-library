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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{
	[Assembler(typeof(MockTraceListenerAssembler))]
	public class MockTraceListenerData : TraceListenerData
	{
		public MockTraceListenerData()
		{
		}

		public MockTraceListenerData(string name)
			: base(name, typeof(MockTraceListener), TraceOptions.None, SourceLevels.All)
		{
		}
	}

	public class MockTraceListenerAssembler : IAssembler<TraceListener, TraceListenerData>
	{
		public TraceListener Assemble(IBuilderContext context,
									  TraceListenerData objectConfiguration,
									  IConfigurationSource configurationSource,
									  ConfigurationReflectionCache reflectionCache)
		{
			return new MockTraceListener();
		}
	}
}