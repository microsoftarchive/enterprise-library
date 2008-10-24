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

using System;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
	public class ExceptionThrowingMockTraceListener : TraceListener
	{
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			throw new Exception("exception while tracing");
		}

		public override void Write(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void WriteLine(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
