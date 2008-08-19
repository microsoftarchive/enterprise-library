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

using System;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// Defines the logical events that can be instrumented for <see cref="AuthorizationProvider"/> instances.
    /// </summary>
    /// <remarks>
    /// The concrete instrumentation is provided by an object bound to the events of the provider. 
    /// The default listener, automatically bound during construction, is <see cref="AuthorizationProviderInstrumentationListener"/>.
    /// </remarks>
	[InstrumentationListener(typeof(AuthorizationProviderInstrumentationListener))]
	public class AuthorizationProviderInstrumentationProvider
	{
		/// <summary>
        /// Occurs when authorization is verified by the <see cref="AuthorizationProvider"/>.
		/// </summary>
		[InstrumentationProvider("AuthorizationCheckPerformed")]
		public event EventHandler<AuthorizationOperationEventArgs> authorizationCheckPerformed;

        /// <summary>
        /// Occurs when authorization is denied by an instance of <see cref="AuthorizationProvider"/>.
        /// </summary>
		[InstrumentationProvider("AuthorizationCheckFailed")]
		public event EventHandler<AuthorizationOperationEventArgs> authorizationCheckFailed;

        /// <summary>
        /// Fires the <see cref="AuthorizationProviderInstrumentationProvider.authorizationCheckPerformed"/> event.
        /// </summary>
        /// <param name="identity">The name of the identify which authorization has been checked for.</param>
        /// <param name="ruleName">The name of the authorization rule that has been evaluated.</param>
		public void FireAuthorizationCheckPerformed(string identity, string ruleName)
		{
			if (authorizationCheckPerformed != null)
                authorizationCheckPerformed(this, new AuthorizationOperationEventArgs(identity, ruleName));
		}

        /// <summary>
        /// Fires the <see cref="AuthorizationProviderInstrumentationProvider.authorizationCheckFailed"/> event.
        /// </summary>
        /// <param name="identity">The name of the identify which authorization has been checked for.</param>
        /// <param name="ruleName">The name of the authorization rule that has been evaluated.</param>
        public void FireAuthorizationCheckFailed(string identity, string ruleName)
		{
			if (authorizationCheckFailed != null)
                authorizationCheckFailed(this, new AuthorizationOperationEventArgs(identity, ruleName));
		}
	}
}
