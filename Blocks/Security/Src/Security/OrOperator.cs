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
    /// Represents an operator that performs a logical-OR of its
    /// contained left and right expressions, but only evaluates
    /// its second expression if the first expression evaluates to true.
    /// </summary>
    public class OrOperator : AndOperator
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="OrOperator"/> class with the
        /// specified expressions.
        /// </summary>
        /// <param name="left">The first expression to evaluate.</param>
        /// <param name="right">The second expression to evaluate.</param>
        public OrOperator(
            BooleanExpression left,
            BooleanExpression right) : base(left, right)
        {
        }

        /// <summary>
        /// Evaluates the current expression against the specified 
        /// <see cref="System.Security.Principal.IPrincipal"/>.
        /// </summary>
        /// <param name="principal">The <see cref="System.Security.Principal.IPrincipal"/>
        /// that the current expression will be evaluated against.</param>
        /// <returns>True or false.</returns>
        public override bool Evaluate(IPrincipal principal)
        {
            return this.Left.Evaluate(principal) ||
                this.Right.Evaluate(principal);
        }        
    }
}