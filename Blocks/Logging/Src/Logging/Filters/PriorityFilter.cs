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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
    /// <summary>
    /// Represents a client-side log filter based on message priority. 
    /// Messages with priorities between the minimum and maximum values are allowed to be processed, all other messages are dropped.
    /// </summary>
	[ConfigurationElementType(typeof(PriorityFilterData))]    
	public class PriorityFilter : LogFilter
    {
        private int minimumPriority;
        private int maximumPriority;

        /// <summary>
		/// Initializes a new instance of the <see cref="PriorityFilter"/> class with a minimum priority and no maximum priority.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="minimumPriority">The minimum priority value.</param>
        public PriorityFilter(string name, int minimumPriority)
            : this(name, minimumPriority, int.MaxValue)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="PriorityFilter"/> class with a minimum and maximum priority.
		/// </summary>
		/// <param name="name">The name of the instance.</param>
		/// <param name="minimumPriority">The minimum priority value.</param>
		/// <param name="maximumPriority">The maximum priority value.</param>
		public PriorityFilter(string name, int minimumPriority, int maximumPriority)
			: base(name)
        {
            this.minimumPriority = minimumPriority;
            this.maximumPriority = maximumPriority;
		}

        /// <summary>
        /// Tests a log entry to see if its priority is within the allowed limits.
        /// </summary>
        /// <param name="log">Log entry to test.</param>
        /// <returns>Returns true if the log entry passes through the category filter.</returns>
        public override bool Filter(LogEntry log)
        {
			return ShouldLog(log.Priority);
        }

		/// <summary>
		/// Tests a log entry to see if its priority is within the allowed limits.
		/// </summary>
		/// <param name="priority">Priority to test.</param>
		/// <returns>Returns true if the priority passes through the category filter.</returns>
		public bool ShouldLog(int priority)
		{
			if (priority < 0)
			{
				priority = this.minimumPriority;
			}
			return (this.maximumPriority >= priority && priority >= this.minimumPriority);
		}

		/// <summary>
		/// The minimum value for messages to be processed.  Messages with a priority
		/// below the minimum are dropped immediately on the client.
		/// </summary>
		public int MinimumPriority
		{
            get { return minimumPriority; }
		}

		/// <summary>
		/// The maximum value for messages to be processed. If not specified
		/// this property defaults to Int32.MaxInt
		/// </summary>
		public int MaximumPriority
		{
			get { return maximumPriority; }
		}
    }
}