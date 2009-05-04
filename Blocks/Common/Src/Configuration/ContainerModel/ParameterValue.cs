//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Linq.Expressions;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// Represents a strategy to retrieve a value to inject. 
    /// </summary>
    /// <remarks>
    /// These strategies can either represent values known at container configuration time or 
    /// values that need to be resolved during object construction.
    /// </remarks>
    /// <seealso cref="ContainerResolvedParameter"/>
    /// <seealso cref="ParameterValue"/>
    public abstract class ParameterValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterValue"/> class with a <see cref="Expression"/>.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> representing the value to inject.</param>
        protected ParameterValue(Expression expression)
        {
            this.Expression = expression;
        }

        /// <summary>
        /// Gets the <see cref="Expression"/> representing the value to inject.
        /// </summary>
        /// <remarks>
        /// Concrete strategies interpret the expression to provide relevant registration data.
        /// </remarks>
        public Expression Expression
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the value to inject.
        /// </summary>
        public Type Type
        {
            get { return Expression.Type; }
        }
    }
}
