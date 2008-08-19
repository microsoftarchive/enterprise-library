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

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Represents an expression that contains the 
    /// name of an <see cref="System.Security.Principal.IIdentity"/>.
    /// </summary>
    public class IdentityExpression : BooleanExpression
    {
        private WordExpression wordExpression;        

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IdentityExpression"/> class with
        /// the specified name.
        /// </summary>
        /// <param name="wordExpression">The name of an identity.</param>
        public IdentityExpression(WordExpression wordExpression)
        {
            this.wordExpression = wordExpression;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IdentityExpression"/> class with
        /// the specified identity name.
        /// </summary>
        /// <param name="identityName">The identity
        /// name that will be used to match the 
        /// specified identity during evaluation.</param>
        public IdentityExpression(string identityName)
        {
            if (identityName == "?")
            {
                this.wordExpression = new AnonymousExpression();
            }
            else if (identityName == "*")
            {
                this.wordExpression = new AnyExpression();
            }
            else
            {
                this.wordExpression = new WordExpression(identityName);
            }
        }

        /// <summary>
        /// Gets or sets the name of the identity that the
        /// specified principal will be evaluated against.
        /// </summary>
        /// <value>An identity name.</value>
        public WordExpression Word
        {
            get { return this.wordExpression; }            
        }

        /// <summary>
        /// Evaluates the specified principal against the
        /// current expression. 
        /// </summary>
        /// <param name="principal">The 
        /// <see cref="System.Security.Principal.IPrincipal"/>
        /// against which the current expression will be evaluated.</param>
        /// <returns><strong>True</strong> if the specified
        /// principal's identity matches this expressions identity,
        /// otherwise <strong>false</strong>.</returns>
        /// <remarks>The expression evaluates
        /// to true if the specified principal's identity has
        /// the same name as the current <see cref="Word"/>
        /// property. A case-insensitive string comparison
        /// is performed.</remarks>
        public override bool Evaluate(IPrincipal principal)
        {
            return this.wordExpression.Evaluate(principal.Identity);
        }
    }
}