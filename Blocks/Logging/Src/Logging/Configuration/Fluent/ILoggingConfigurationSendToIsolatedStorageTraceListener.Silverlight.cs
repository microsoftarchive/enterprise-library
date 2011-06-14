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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to specify settings on a <see cref="IsolatedStorageTraceListenerData"/>.
    /// </summary>
    /// <seealso cref="IsolatedStorageTraceListenerData"/>
    public interface ILoggingConfigurationSendToIsolatedStorageTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Specifies the name of the repository for entries.
        /// </summary>
        /// <param name="repositoryName">The name of the repository that should be used.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="IsolatedStorageTraceListenerData"/>.</returns>
        /// <seealso cref="IsolatedStorageTraceListener"/>
        /// <seealso cref="IsolatedStorageTraceListenerData"/>
        /// <seealso cref="IsolatedStorageTraceListenerData.RepositoryName"/>
        ILoggingConfigurationSendToIsolatedStorageTraceListener WithRepositoryName(string repositoryName);

        /// <summary>
        /// Specifies the maximum size in kilobytes to be used when storing entries.
        /// </summary>
        /// <param name="maxSizeInKilobytes">The maximum size in kilobytes.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="IsolatedStorageTraceListenerData"/>.</returns>
        /// <seealso cref="IsolatedStorageTraceListener"/>
        /// <seealso cref="IsolatedStorageTraceListenerData"/>
        /// <seealso cref="IsolatedStorageTraceListenerData.MaxSizeInKilobytes"/>
        ILoggingConfigurationSendToIsolatedStorageTraceListener SetMaxSizeInKilobytes(int maxSizeInKilobytes);
    }
}
