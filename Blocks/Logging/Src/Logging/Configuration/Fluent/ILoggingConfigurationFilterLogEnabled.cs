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

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="LogEnabledFilter"/> instance.
    /// </summary>
    /// <see cref="LogEnabledFilter"/>
    /// <see cref="LogEnabledFilterData"/>
    public interface ILoggingConfigurationFilterLogEnabled : ILoggingConfigurationOptions, IFluentInterface
    {
        /// <summary>
        /// Specifies that all logging should be enabled. <br/>
        /// The default for the <see cref="LogEnabledFilter"/> is that all logging is disabled.
        /// </summary>
        /// <returns>Fluent interface used to further configure the logging application block.</returns>
        /// <see cref="LogEnabledFilter"/>
        /// <see cref="LogEnabledFilterData"/>
        ILoggingConfigurationOptions Enable();
    }
}
