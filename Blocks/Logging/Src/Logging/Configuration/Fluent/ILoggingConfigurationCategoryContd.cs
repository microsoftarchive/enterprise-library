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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface that allows tracelisteners to be configured for a Category Source.
    /// </summary>
    public interface ILoggingConfigurationCategoryContd : ILoggingConfigurationContd, IFluentInterface
    {
        /// <summary>
        /// Entry point for attaching Trace Listeners to a Category Source.
        /// </summary>
        ILoggingConfigurationSendTo SendTo { get; }
    }
}
