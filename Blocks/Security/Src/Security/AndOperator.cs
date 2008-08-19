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
    /// Represents an operator that performs a logical-AND of its
    /// contained left and right expressions, but only evaluates
    /// its second expression if the first expression evaluates to true.
    /// </summary>
    public class AndOperator : BooleanExpression
    {
        private BooleanExpression left;
        private BooleanExpression right;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndOperator"/>
        /// class.
        /// </summary>
        public AndOperator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndOperator"/>
        /// class with the specified
        /// </summary>
        /// <param name="left">The expression that will be evaluated first.</param>
        /// <param name="right">The expression that will be evaluated last.</param>
        public AndOperator(
            BooleanExpression left,
            BooleanExpression right)
        {
            this.left = left;
            this.right = right;
        }

        /// <summary>
        /// Gets or sets the first expression that will be evaluated -
        /// the expression to the left of the operator.
        /// </summary>
        /// <value>A <see cref="BooleanExpression"/>.</value>
        public BooleanExpression Left
        {
            get { return this.left; }
            set { this.left = value; }
        }

        /// <summary>
        /// Gets or sets the second expression that will be evaluated -
        /// the expression to the right of the operator.
        /// </summary>
        /// <value>A <see cref="BooleanExpression"/>.</value>
        public BooleanExpression Right
        {
            get { return this.right; }
            set { this.right = value; }
        }

        /// <summary>
        /// Performs the logical-AND of the left
        /// and right expressions.
        /// </summary>
        /// <param name="principal">The <see cref="System.Security.Principal.IPrincipal"/>
        /// that the current expression will be evaluated against.</param>
        /// <returns>True if both the left and right expressions evaluate to true,
        /// otherwise false.</returns>
        public override bool Evaluate(IPrincipal principal)
        {
            return this.left.Evaluate(principal) &&
                this.right.Evaluate(principal);
        }        
    }
}