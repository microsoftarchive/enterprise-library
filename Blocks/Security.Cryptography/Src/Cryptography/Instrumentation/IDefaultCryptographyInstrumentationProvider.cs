//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
    /// <summary>
    /// Interface defining instrumentation used to report errors in the Cryptography block.
    /// </summary>
    public interface IDefaultCryptographyInstrumentationProvider
    {
        /// <summary>
        /// Fires the CryptographyErrorOccurred event.
        /// </summary>
        /// <param name="providerName">The name of the provider with the errror.</param>
        /// <param name="instanceName">The name of the instance with the errror.</param>
        /// <param name="message">The message that describes the failure.</param>
        void FireCryptographyErrorOccurred(string providerName, string instanceName, string message);

        /// <summary>
        /// Logs the occurrence of a configuration error for the Enterprise Library Cryptography Application Block through the 
        /// available instrumentation mechanisms.
        /// </summary>
        /// <param name="instanceName">Name of the cryptographic provider instance in which the configuration error was detected.</param>
        /// <param name="messageTemplate">The template to build the event log entry.</param>
        /// <param name="exception">The exception raised for the configuration error.</param>
        void LogConfigurationError(string instanceName, string messageTemplate, Exception exception);

        /// <summary>
        /// Logs the occurrence of a configuration error for the Enterprise Library Cryptography Application Block through the 
        /// available instrumentation mechanisms.
        /// </summary>
        /// <param name="instanceName">Name of the cryptographic provider instance in which the configuration error was detected.</param>
        /// <param name="messageTemplate">The template to build the event log entry.</param>
        void LogConfigurationError(string instanceName, string messageTemplate);
    }
}
