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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;


namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    /// <summary>
    /// Represents a <see cref="PriorityFilterData"/> configuration element.
    /// </summary>
    public sealed class PriorityFilterNode : LogFilterNode
    {
		private int maximumPriority;
		private int minimumPriority;

        /// <summary>
        /// Initialize a new instance of the <see cref="PriorityFilterNode"/> class.
        /// </summary>
        public PriorityFilterNode()
            : this(new PriorityFilterData(Resources.PriorityFilterNode, -1))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="PriorityFilterNode"/> class with a <see cref="PriorityFilterData"/> instance.
		/// </summary>
		/// <param name="priorityFilterData">A <see cref="PriorityFilterData"/> instance</param>
        public PriorityFilterNode(PriorityFilterData priorityFilterData)
            : base(null == priorityFilterData ? string.Empty : priorityFilterData.Name)
        {
			if (null == priorityFilterData) throw new ArgumentNullException("priorityFilterData");

			this.minimumPriority = priorityFilterData.MinimumPriority;
			this.maximumPriority = priorityFilterData.MaximumPriority;			
        }

        /// <summary>
        /// Gets the maximum priority.
        /// </summary>
		/// <value>
		/// The maximum priority.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("PriorityFilterNodeMaximumPriorityDescription", typeof(Resources))]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [PriorityFilterMaximumPriorityValidationAttribute]
		public int? MaximumPriority
		{
			get { return (maximumPriority == Int32.MaxValue) ? (int?)null : maximumPriority; }
			set { maximumPriority = value ?? Int32.MaxValue; }
		}


        /// <summary>
		/// Gets the minimum priority.
        /// </summary>
		/// <value>
		/// The minimum priority.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("PriorityFilterNodeMinimumPriorityDescription", typeof(Resources))]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
		public int? MinimumPriority
		{
			get { return (minimumPriority == -1) ? (int?)null : minimumPriority; }
			set { minimumPriority = value ?? -1; }
		}


		/// <summary>
		/// Gets the <see cref="PriorityFilterData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="PriorityFilterData"/> this node represents.
		/// </value>
		public override LogFilterData LogFilterData
		{
			get 
			{
				PriorityFilterData data = new PriorityFilterData(Name, minimumPriority);
				data.MaximumPriority = maximumPriority;
				return data;
			}
		}
    }
}