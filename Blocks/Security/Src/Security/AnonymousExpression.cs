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

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Represents the value of an <see cref="System.Security.Principal.IIdentity"/> object
    /// whose <see cref="System.Security.Principal.IIdentity.IsAuthenticated"/> property is false.
    /// </summary>
    public class AnonymousExpression : WordExpression
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AnonymousExpression"/> class.
        /// </summary>
        public AnonymousExpression() : base("?")
        {
        }

        /// <summary>
        /// Evaluates the specified 
        /// <see cref="System.Security.Principal.IPrincipal"/>.
        /// </summary>
        /// <param name="principal">The <see cref="System.Security.Principal.IPrincipal"/>
        /// that the current expression will be evaluated against.</param>
        /// <returns><strong>True</strong> if the principal contains
        /// an anonymous identity, otherwise <strong>false</strong>.</returns>
        /// <exception cref="NotSupportedException">This expression
        /// can only be evaluated against an identity. It has no 
        /// meaning for a principal's roles because there
        /// is no common definition of an anonymous role.</exception>
        public override bool Evaluate(IPrincipal principal)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Evaluates the specified 
        /// <see cref="System.Security.Principal.IIdentity"/>.
        /// </summary>
        /// <param name="identity">The <see cref="System.Security.Principal.IIdentity"/>
        /// that the current expression will be evaluated against.</param>
        /// <returns><strong>True</strong> if the identity is
        /// an anonymous identity, otherwise <strong>false</strong>.</returns>
        public override bool Evaluate(IIdentity identity)
        {
            return !identity.IsAuthenticated;
        }        
    }
}