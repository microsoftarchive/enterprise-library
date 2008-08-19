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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    /// <summary>
    /// Manages the performance counters for <see cref="Tracer"/> operations.
    /// </summary>
	/// <remarks>
    /// This class allows the <see cref="Tracer"/> class to interact with performace counter instances not know on forehand.
	/// </remarks>
    public class TracerPerformanceCounter: EnterpriseLibraryPerformanceCounter
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="TracerPerformanceCounter"/> class.
        /// </summary>
        /// <param name="counterCategoryName">The counter category name.</param>
        /// <param name="counterName">The counter name.</param>
        public TracerPerformanceCounter(string counterCategoryName, string counterName)
            : base(counterCategoryName, counterName, new string[0])
        {

        }

        /// <summary>
		/// Increments the associated performance counters by one.
        /// </summary>
        /// <param name="instanceName">The instance to be incremented.</param>
        public void Increment(string instanceName)
        {
            PerformanceCounter counter = InstantiateCounter(instanceName);
            counter.Increment();
        }

        /// <summary>
		/// Increments or decrements the value of the associated performance counter by a specified amount.
        /// </summary>
        /// <param name="instanceName">The instance to be incremented.</param>
        /// <param name="value">The value to increment by. A negative value decrements the counter.</param>
        public void IncrementBy(string instanceName, long value)
        {
            PerformanceCounter counter = InstantiateCounter(instanceName);
            counter.IncrementBy(value);
        }
    }
}
