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
    /// name of a role.
    /// </summary>
    public class RoleExpression : BooleanExpression
    {
        private WordExpression wordExpression;        

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleExpression"/>
        /// class with the specified role name.
        /// </summary>
        /// <param name="wordExpression">The name of a role.</param>
        public RoleExpression(WordExpression wordExpression)
        {
            this.wordExpression = wordExpression;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="RoleExpression"/> class.
        /// </summary>
        /// <param name="roleName">The name of the role
        /// to include in the expression.</param>
        public RoleExpression(string roleName)
        {
            if (roleName == "*")
            {
                this.wordExpression = new AnyExpression();
            }
            else
            {
                this.wordExpression = new WordExpression(roleName);
            }
        }

        /// <summary>
        /// Gets or sets the name of the role that the
        /// specified principal will be evaluated against.
        /// </summary>
        /// <value>A role name.</value>
        public WordExpression Word
        {
            get { return this.wordExpression; }            
        }

        /// <summary>
        /// Evaluates the current expression against the specified 
        /// <see cref="System.Security.Principal.IPrincipal"/>.
        /// </summary>
        /// <param name="principal">The <see cref="System.Security.Principal.IPrincipal"/>
        /// that the current expression will be evaluated against.</param>
        /// <returns>True if the princal is in this expression's 
        /// role, otherwise false.</returns>
        public override bool Evaluate(IPrincipal principal)
        {
            return this.Word.Evaluate(principal);
        }        
    }
}