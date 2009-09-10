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
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="PriorityFilter"/> instance.
    /// </summary>
    /// <see cref="PriorityFilter"/>
    /// <see cref="PriorityFilterData"/>
    public interface ILoggingConfigurationFilterOnPriority : ILoggingConfigurationOptions, IFluentInterface
    {
        /// <summary>
        /// Specifies that log messages with a priority below <paramref name="minimumPriority"/> should not be logged.
        /// </summary>
        /// <param name="minimumPriority">The minimum priority for log messages to pass this filter</param>
        /// <returns>Fluent interface to further configure this <see cref="PriorityFilter"/> instance.</returns>
        /// <see cref="PriorityFilter"/>
        /// <see cref="PriorityFilterData"/>
        ILoggingConfigurationFilterOnPriority StartingWithPriority(int minimumPriority);

        /// <summary>
        /// Specifies that log messages with a priority above <paramref name="maximumPriority"/> should not be logged.
        /// </summary>
        /// <param name="maximumPriority">The maximum priority for log messages to pass this filter</param>
        /// <returns>Fluent interface to further configure this <see cref="PriorityFilter"/> instance.</returns>
        /// <see cref="PriorityFilter"/>
        /// <see cref="PriorityFilterData"/>
        ILoggingConfigurationFilterOnPriority UpToPriority(int maximumPriority);
    }
}
