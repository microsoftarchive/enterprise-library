//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation
{
	/// <summary>
	/// Wrapper for validation <see cref="PerformanceCounter"/> that instantiates them
	/// on each increment request.
	/// </summary>
	public class ValidationPerformanceCounter : EnterpriseLibraryPerformanceCounter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationPerformanceCounter"/> class.
		/// </summary>
		/// <param name="counterCategoryName">The counter category name.</param>
		/// <param name="counterName">The counter name.</param>
		public ValidationPerformanceCounter(string counterCategoryName, string counterName)
			: base(counterCategoryName, counterName)
		{ }

		/// <summary>
		/// Increments the associated performance counters by one.
		/// </summary>
		/// <param name="instanceName">The instance to be incremented.</param>
		public void Increment(string instanceName)
		{
			PerformanceCounter counter = InstantiateCounter(instanceName);
			counter.Increment();
		}
	}
}
