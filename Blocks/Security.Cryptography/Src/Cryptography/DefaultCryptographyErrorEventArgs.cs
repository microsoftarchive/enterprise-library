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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Provides data for the DefaultCryptographyInstrumentationProvider.cryptographyErrorOccurred event.
    /// </summary>
    public class DefaultCryptographyErrorEventArgs : EventArgs
    {
        private readonly string providerName;
        private readonly string instanceName;
		private readonly string message;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultCryptographyErrorEventArgs"/> class.
		/// </summary>
        /// <param name="providerName">The name of the provider.</param>
        /// <param name="instanceName">The name of the instance.</param>
		/// <param name="message">The message that describes the error.</param>
        public DefaultCryptographyErrorEventArgs(string providerName, string instanceName, string message)
		{
            this.providerName = providerName;
            this.instanceName = instanceName;
			this.message = message;
		}

        /// <summary>
        /// Gets the name of the provider associated to the error.
        /// </summary>
        public string ProviderName { get { return providerName; } }

		/// <summary>
		/// Gets the name of the instance associated to the error.
		/// </summary>
        public string InstanceName { get { return instanceName; } }

		/// <summary>
		/// Gets the message that describes the error.
		/// </summary>
		public string Message { get { return message; } }
    }
}
