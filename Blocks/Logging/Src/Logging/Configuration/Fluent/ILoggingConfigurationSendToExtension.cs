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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
#if SILVERLIGHT
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Allows access to the configuration classes used to configure <see cref="TraceListener"/> instances.
    /// </summary>
    public interface ILoggingConfigurationSendToExtension : ILoggingConfigurationExtension, IFluentInterface
    {
        /// <summary>
        /// Returns an interface that can be used to configure a logging category.
        /// </summary>
        ILoggingConfigurationCategoryContd LoggingCategoryContd { get; }

        /// <summary>
        /// Returns the logging category configuration currently being built up.
        /// </summary>
        TraceSourceData CurrentTraceSource { get; }
    }
}
