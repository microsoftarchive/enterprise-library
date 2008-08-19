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
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Defines the basic functionality of an authorization provider.
    /// </summary>
    [ConfigurationNameMapper(typeof(AuthorizationProviderDataRetriever))]
	[CustomFactory(typeof(AuthorizationProviderCustomFactory))]
    public interface IAuthorizationProvider
    {
        /// <summary>
        /// Evaluates the specified authority against the specified context.
        /// </summary>
        /// <param name="principal">Must be an <see cref="IPrincipal"/> object.</param>
        /// <param name="context">Name of the rule to evaluate.</param>
        /// <returns><strong>True</strong> if the expression evaluates to true,
        /// otherwise <strong>false</strong>.</returns>
        bool Authorize(IPrincipal principal, string context);
    }
}