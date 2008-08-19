//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
	/// <summary>
    /// Abstract implementation of the <see cref="IAuthorizationProvider"/> interface.
	/// </summary>
	public abstract class AuthorizationProvider : IAuthorizationProvider, IInstrumentationEventProvider
	{
		AuthorizationProviderInstrumentationProvider instrumentationProvider;

		/// <summary>
        /// Initializes a new instance of <see cref="AuthorizationProvider"/>.
		/// </summary>
		protected AuthorizationProvider()
		{
			this.instrumentationProvider = new AuthorizationProviderInstrumentationProvider();
		}

        /// <summary>
        /// When implemented in a derived class, Evaluates the specified authority against the specified context.
        /// </summary>
        /// <param name="principal">Must be an <see cref="IPrincipal"/> object.</param>
        /// <param name="context">Must be a string that is the name of the rule to evaluate.</param>
        /// <returns><c>true</c> if the authority is authorized, otherwise <c>false</c>.</returns>
		public abstract bool Authorize(IPrincipal principal, string context);

        /// <summary>
        /// Gets the <see cref="AuthorizationProviderInstrumentationProvider"/> instance that defines the logical events used to instrument this Authorization Provider instance.
        /// </summary>
        /// <returns>The <see cref="AuthorizationProviderInstrumentationProvider"/> instance that defines the logical events used to instrument this Authorization Provider instance.</returns>
		public object GetInstrumentationEventProvider()
		{
			return this.instrumentationProvider;
		}

        /// <summary>
        /// Gets the <see cref="AuthorizationProviderInstrumentationProvider"/> instance that defines the logical events used to instrument this Authorization Provider instance.
        /// </summary>
		protected AuthorizationProviderInstrumentationProvider InstrumentationProvider
		{
			get { return this.instrumentationProvider; }
		}
	}
}
