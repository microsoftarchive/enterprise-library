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
	/// Explicit binder for hash algorithm instrumentation.
	/// </summary>
	public class HashAlgorithmInstrumentationBinder : IExplicitInstrumentationBinder
	{
		/// <summary>
		/// Binds the events exposed by the source to the handlers in the listener.
		/// </summary>
		/// <param name="source">The source of instrumentation events. Must be an instance of HashAlgorithmInstrumentationProvider.</param>
		/// <param name="listener">The listener for instrumentation events. Must be an instance of HashAlgorithmInstrumentationListener.</param>
		public void Bind(object source, object listener)
		{
			HashAlgorithmInstrumentationListener castedListener = (HashAlgorithmInstrumentationListener)listener;
			HashAlgorithmInstrumentationProvider castedProvider = (HashAlgorithmInstrumentationProvider)source;

			castedProvider.cyptographicOperationFailed += castedListener.CyptographicOperationFailed;
			castedProvider.hashComparisonPerformed += castedListener.HashComparisonPerformed;
			castedProvider.hashMismatchDetected += castedListener.HashMismatchDetected;
			castedProvider.hashOperationPerformed += castedListener.HashOperationPerformed;
		}
	}
}
