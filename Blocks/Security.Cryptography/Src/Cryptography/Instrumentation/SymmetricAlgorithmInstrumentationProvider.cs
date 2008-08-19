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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
	/// <summary>
	/// Defines the logical events that can be instrumented for symmetric crypto providers.
	/// </summary>
	/// <remarks>
	/// The concrete instrumentation is provided by an object bound to the events of the provider. 
	/// The default listener, automatically bound during construction, is <see cref="SymmetricAlgorithmInstrumentationListener"/>.
	/// </remarks>
	[InstrumentationListener(typeof(SymmetricAlgorithmInstrumentationListener), typeof(SymmetricAlgorithmInstrumentationBinder))]
	public class SymmetricAlgorithmInstrumentationProvider
	{
		/// <summary>
		/// Occurs when a cryptographic operation fails for an <see cref="ISymmetricCryptoProvider"/>.
		/// </summary>
		[InstrumentationProvider("CyptographicOperationFailed")]
		public event EventHandler<CrytographicOperationErrorEventArgs> cyptographicOperationFailed;

		/// <summary>
		/// Occurs when an encryption operation is performed by an <see cref="ISymmetricCryptoProvider"/>.
		/// </summary>
		[InstrumentationProvider("SymmetricEncryptionPerformed")]
		public event EventHandler<EventArgs> symmetricEncryptionPerformed;

		/// <summary>
		/// Occurs when an decryption operation is performed by an <see cref="ISymmetricCryptoProvider"/>.
		/// </summary>
		[InstrumentationProvider("SymmetricDecryptionPerformed")]
		public event EventHandler<EventArgs> symmetricDecryptionPerformed;

		/// <summary>
		/// Fires the <see cref="SymmetricAlgorithmInstrumentationProvider.cyptographicOperationFailed"/> event.
		/// </summary>
		/// <param name="message">The message that describes the failure.</param>
		/// <param name="exception">The exception thrown during the failure.</param>
		public void FireCyptographicOperationFailed(string message, Exception exception)
		{
			if (cyptographicOperationFailed != null)
				cyptographicOperationFailed(this, new CrytographicOperationErrorEventArgs(message, exception));
		}

		/// <summary>
		/// Fires the <see cref="SymmetricAlgorithmInstrumentationProvider.symmetricEncryptionPerformed"/> event.
		/// </summary>
		public void FireSymmetricEncryptionPerformed()
		{
			if (symmetricEncryptionPerformed != null)
				symmetricEncryptionPerformed(this, new EventArgs());
		}

		/// <summary>
		/// Fires the <see cref="SymmetricAlgorithmInstrumentationProvider.symmetricDecryptionPerformed"/> event.
		/// </summary>
		public void FireSymmetricDecryptionPerformed()
		{
			if (symmetricDecryptionPerformed != null)
				symmetricDecryptionPerformed(this, new EventArgs());
		}
	}
}
