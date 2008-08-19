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
    /// Represents the logical negation operator 
    /// is a unary operator that negates its operand. 
    /// It returns true if and only if its operand is false.
    /// </summary>
    public class NotOperator : BooleanExpression
    {
        private BooleanExpression expression;        

        /// <summary>
        /// Intializes a new instance of the 
        /// <see cref="NotOperator"/> class.
        /// </summary>
        /// <param name="expression">The operand that this
        /// operator will negate.</param>
        public NotOperator(BooleanExpression expression)
        {
            this.expression = expression;
        }

        /// <summary>
        /// Gets or sets the expression that will be negated by the
        /// current operator.
        /// </summary>
        /// <value>A <see cref="BooleanExpression"/> object.</value>
        public BooleanExpression Expression
        {
            get { return this.expression; }            
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
            return !this.expression.Evaluate(principal);
        }
    }
}