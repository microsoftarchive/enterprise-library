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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
	internal class MockLogFilterErrorHandler : ILogFilterErrorHandler
	{
		internal ICollection<ILogFilter> failingFilters = new List<ILogFilter>();
		private bool returnValue = false;

		internal MockLogFilterErrorHandler(bool returnValue)
		{
			this.returnValue = returnValue;
		}

		public bool FilterCheckingFailed(System.Exception ex, LogEntry logEntry, ILogFilter filter)
		{
			failingFilters.Add(filter);
			return returnValue;
		}
	}
}
