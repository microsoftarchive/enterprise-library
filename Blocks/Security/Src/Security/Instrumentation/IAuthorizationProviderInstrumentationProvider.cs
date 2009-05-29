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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// Defines the logical events that can be instrumented for <see cref="AuthorizationProvider"/> instances.
    /// </summary>
    public interface IAuthorizationProviderInstrumentationProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity">The name of the identify which authorization has been checked for.</param>
        /// <param name="ruleName">The name of the authorization rule that has been evaluated.</param>
        void FireAuthorizationCheckFailed(string identity, string ruleName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity">The name of the identify which authorization has been checked for.</param>
        /// <param name="ruleName">The name of the authorization rule that has been evaluated.</param>
        void FireAuthorizationCheckPerformed(string identity, string ruleName);
    }
}
