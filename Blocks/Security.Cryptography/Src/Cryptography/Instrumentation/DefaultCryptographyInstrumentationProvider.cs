//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
    /// <summary>
    /// Defines the logical events that can be instrumented for the cryptography application block's <see cref="CryptographyManagerImpl"/>.
    /// </summary>
    /// <remarks>
    /// The concrete instrumentation is provided by an object bound to the events of the provider. 
    /// The default listener, automatically bound during construction, is <see cref="DefaultCryptographyEventLogger"/>.
    /// </remarks>
    [InstrumentationListener(typeof(DefaultCryptographyEventLogger))]
    public class DefaultCryptographyInstrumentationProvider
    {
        /// <summary>
        /// Occurs when an error is detected while trying to get a provider instance to use.
        /// </summary>
        [InstrumentationProvider("CryptographyErrorOccurred")]
        public event EventHandler<DefaultCryptographyErrorEventArgs> cryptographyErrorOccurred;

        /// <summary>
        /// Fires the <see cref="DefaultCryptographyInstrumentationProvider.cryptographyErrorOccurred"/> event.
        /// </summary>
        /// <param name="providerName">The name of the provider with the errror.</param>
        /// <param name="instanceName">The name of the instance with the errror.</param>
        /// <param name="message">The message that describes the failure.</param>
        public void FireCryptographyErrorOccurred(string providerName, string instanceName, string message)
        {
            if (cryptographyErrorOccurred != null)
                cryptographyErrorOccurred(this, new DefaultCryptographyErrorEventArgs(providerName, instanceName, message));
        }
    }
}
