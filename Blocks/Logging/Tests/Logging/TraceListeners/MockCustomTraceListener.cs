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
using System.Collections.Specialized;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
	public class MockCustomTraceListener : CustomTraceListener
	{
		internal const string AttributeKey = "attribute";
		internal readonly static string[] SupportedAttributes = new string[] { AttributeKey };

		internal string initData;

        public MockCustomTraceListener()
        {
        }

		// for sys.diags
		public MockCustomTraceListener(string initData)
		{
			this.initData = initData;
		}

		internal string Attribute
		{
			get { return Attributes[AttributeKey]; }
		}

		public override void Write(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void WriteLine(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override string[] GetSupportedAttributes()
		{
			return SupportedAttributes;
		}
	}
}
