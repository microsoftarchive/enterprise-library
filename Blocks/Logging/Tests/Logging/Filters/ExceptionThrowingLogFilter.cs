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
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
	class ExceptionThrowingLogFilter : ILogFilter
	{
		private string name;

		internal ExceptionThrowingLogFilter(string name)
		{
			this.name = name;
		}

		public bool Filter(LogEntry log)
		{
			throw new Exception("exception during evaluation.");
		}

		public string Name
		{
			get { return name; }
		}
	}
}
