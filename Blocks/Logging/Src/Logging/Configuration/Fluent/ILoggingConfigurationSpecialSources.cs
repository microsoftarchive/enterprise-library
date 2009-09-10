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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{

    /// <summary>
    /// Fluent interface used to configure pre defined logging categories.
    /// </summary>
    public interface ILoggingConfigurationSpecialSources : IFluentInterface
    {
        /// <summary>
        /// Returns an interface to configure the category source used for internal errors and warnings.
        /// </summary>
        ILoggingConfigurationCategoryStart LoggingErrorsAndWarningsCategory { get; }

        /// <summary>
        /// Returns an interface to configure the category source used to log messages that could not be processed
        /// </summary>
        ILoggingConfigurationCategoryStart UnprocessedCategory { get; }

        /// <summary>
        /// Returns an interface to configure the category source used to log all events.
        /// </summary>
        ILoggingConfigurationCategoryStart AllEventsCategory { get; }
    }

}
