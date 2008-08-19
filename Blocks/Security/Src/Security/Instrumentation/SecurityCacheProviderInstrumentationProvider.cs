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
    /// Defines the logical events that can be instrumented for <see cref="SecurityCacheProvider"/> instances.
    /// </summary>
    /// <remarks>
    /// The concrete instrumentation is provided by an object bound to the events of the provider. 
    /// The default listener, automatically bound during construction, is <see cref="SecurityCacheProviderInstrumentationListener"/>.
    /// </remarks>
	[InstrumentationListener(typeof(SecurityCacheProviderInstrumentationListener))]
	public class SecurityCacheProviderInstrumentationProvider
	{
        /// <summary>
        /// Occurs when an object is read from the <see cref="SecurityCacheProvider"/>.
        /// </summary>
		[InstrumentationProvider("SecurityCacheReadPerformed")]
        public event EventHandler<SecurityCacheOperationEventArgs> securityCacheReadPerformed;

        /// <summary>
        /// Fires the <see cref="SecurityCacheProviderInstrumentationProvider.securityCacheReadPerformed"/> event.
        /// </summary>
        /// <param name="itemType">The type of item that is read from the <see cref="SecurityCacheProvider"/>.</param>
        /// <param name="token">The token that was is used to read an item from the <see cref="SecurityCacheProvider"/>.</param>
        public void FireSecurityCacheReadPerformed(SecurityEntityType itemType, IToken token)
		{
			if (securityCacheReadPerformed != null)
                securityCacheReadPerformed(this, new SecurityCacheOperationEventArgs(itemType, token));
		}
	}
}
