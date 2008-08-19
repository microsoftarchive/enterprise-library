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
	/// Defines the logical events that can be instrumented for hash providers.
	/// </summary>
	/// <remarks>
	/// The concrete instrumentation is provided by an object bound to the events of the provider. 
	/// The default listener, automatically bound during construction, is <see cref="HashAlgorithmInstrumentationListener"/>.
	/// </remarks>
	[InstrumentationListener(typeof(HashAlgorithmInstrumentationListener), typeof(HashAlgorithmInstrumentationBinder))]
	public class HashAlgorithmInstrumentationProvider
	{
		/// <summary>
		/// Occurs when a cryptographic operation fails for an <see cref="IHashProvider"/>.
		/// </summary>
		[InstrumentationProvider("CyptographicOperationFailed")]
		public event EventHandler<CrytographicOperationErrorEventArgs> cyptographicOperationFailed;

		/// <summary>
		/// Occurs when an hash operation is performed by an <see cref="IHashProvider"/>.
		/// </summary>
		[InstrumentationProvider("HashOperationPerformed")]
		public event EventHandler<EventArgs> hashOperationPerformed;

		/// <summary>
		/// Occurs when an hash comparison is performed by an <see cref="IHashProvider"/>.
		/// </summary>
		[InstrumentationProvider("HashComparisonPerformed")]
		public event EventHandler<EventArgs> hashComparisonPerformed;

		/// <summary>
		/// Occurs when an hash comparison is performed by an <see cref="IHashProvider"/>.
		/// </summary>	
		[InstrumentationProvider("HashMismatchDetected")]
		public event EventHandler<EventArgs> hashMismatchDetected;

		/// <summary>
		/// Fires the <see cref="HashAlgorithmInstrumentationProvider.cyptographicOperationFailed"/> event.
		/// </summary>
		/// <param name="message">The message that describes the failure.</param>
		/// <param name="exception">The exception thrown during the failure.</param>
		public void FireCyptographicOperationFailed(string message, Exception exception)
		{
			if (cyptographicOperationFailed != null)
				cyptographicOperationFailed(this, new CrytographicOperationErrorEventArgs(message, exception));
		}

		/// <summary>
		/// Fires the <see cref="HashAlgorithmInstrumentationProvider.hashOperationPerformed"/> event.
		/// </summary>
		public void FireHashOperationPerformed()
		{
			if (hashOperationPerformed != null)
				hashOperationPerformed(this, new EventArgs());
		}

		/// <summary>
		/// Fires the <see cref="HashAlgorithmInstrumentationProvider.hashComparisonPerformed"/> event.
		/// </summary>
		public void FireHashComparisonPerformed()
		{
			if (hashComparisonPerformed != null)
				hashComparisonPerformed(this, new EventArgs());
		}

		/// <summary>
		/// Fires the <see cref="HashAlgorithmInstrumentationProvider.hashMismatchDetected"/> event.
		/// </summary>
		public void FireHashMismatchDetected()
		{
			if (hashMismatchDetected != null)
				hashMismatchDetected(this, new EventArgs());
		}
	}
}
