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

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Explicit binder for symmetric encryption instrumentation.
	/// </summary>
	public class SymmetricAlgorithmInstrumentationBinder : IExplicitInstrumentationBinder
	{
		/// <summary>
		/// Binds the events exposed by the source to the handlers in the listener.
		/// </summary>
		/// <param name="source">The source of instrumentation events. Must be an instance of <see cref="SymmetricAlgorithmInstrumentationProvider"/>.</param>
		/// <param name="listener">The listener for instrumentation events. Must be an instance of <see cref="SymmetricAlgorithmInstrumentationListener"/>.</param>
		public void Bind(object source, object listener)
		{
			SymmetricAlgorithmInstrumentationListener castedListener = (SymmetricAlgorithmInstrumentationListener)listener;
			SymmetricAlgorithmInstrumentationProvider castedProvider = (SymmetricAlgorithmInstrumentationProvider)source;

			castedProvider.cyptographicOperationFailed += castedListener.CyptographicOperationFailed;
			castedProvider.symmetricDecryptionPerformed+= castedListener.SymmetricDecryptionPerformed;
			castedProvider.symmetricEncryptionPerformed += castedListener.SymmetricEncryptionPerformed;
		}
	}
}
