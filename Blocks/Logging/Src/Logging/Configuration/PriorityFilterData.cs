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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration for a priority filter.
    /// </summary>    
    [ResourceDescription(typeof(DesignResources), "PriorityFilterDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "PriorityFilterDataDisplayName")]
    [ElementValidation(LoggingDesignTime.ValidatorTypes.LogPriorityMinMaxValidatorType)]
    public class PriorityFilterData : LogFilterData
    {
        private const string minimumPriorityProperty = "minimumPriority";
        private const string maximumPriorityProperty = "maximumPriority";

        /// <summary>
        /// Initializes a new <see cref="PriorityFilterData"/>.
        /// </summary>
        public PriorityFilterData()
        {
            Type = typeof(PriorityFilter);
        }

        /// <summary>
        /// Initializes a new <see cref="PriorityFilterData"/> with a minimum priority.
        /// </summary>
        /// <param name="minimumPriority">The minimum priority.</param>
        public PriorityFilterData(int minimumPriority)
            : this("priority", minimumPriority)
        {
        }

        /// <summary>
        /// Initializes a new named <see cref="PriorityFilterData"/> with a minimum priority.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minimumPriority">The minimum priority.</param>
        public PriorityFilterData(string name, int minimumPriority)
            : base(name, typeof(PriorityFilter))
        {
            this.MinimumPriority = minimumPriority;
        }

        /// <summary>
        /// Gets or sets the minimum value for messages to be processed.  Messages with a priority
        /// below the minimum are dropped immediately on the client.
        /// </summary>
        [ConfigurationProperty(minimumPriorityProperty)]
        [ResourceDescription(typeof(DesignResources), "PriorityFilterDataMinimumPriorityDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PriorityFilterDataMinimumPriorityDisplayName")]
        public int MinimumPriority
        {
            get
            {
                return (int)this[minimumPriorityProperty];
            }
            set
            {
                this[minimumPriorityProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum priority value for messages to be processed.  Messages with a priority
        /// above the maximum are dropped immediately on the client.
        /// </summary>		
        [ConfigurationProperty(maximumPriorityProperty, DefaultValue = int.MaxValue)]
        [ResourceDescription(typeof(DesignResources), "PriorityFilterDataMaximumPriorityDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PriorityFilterDataMaximumPriorityDisplayName")]
        public int MaximumPriority
        {
            get
            {
                return (int)this[maximumPriorityProperty];
            }
            set
            {
                this[maximumPriorityProperty] = value;
            }
        }

        /// <summary>
        /// Builds the <see cref="ILogFilter" /> object represented by this configuration object.
        /// </summary>
        /// <returns>
        /// A <see cref="PriorityFilter"/>.
        /// </returns>
        public override ILogFilter BuildFilter()
        {
            return new PriorityFilter(this.Name, this.MinimumPriority, this.MaximumPriority);
        }
    }
}
