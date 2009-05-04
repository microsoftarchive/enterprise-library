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

using System.Linq.Expressions;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// Represents an injected parameter value that can be determined at the time of container configuration.
    /// </summary>
    public class ConstantParameterValue : ParameterValue
    {
        /// <summary>
        /// Initializes a value parameter with the specified expression to be evaluated when providing the value parameter.
        /// </summary>
        /// <param name="expression">The expression representing the value to provide to the parameter.</param>
        internal ConstantParameterValue(Expression expression)
            : base(expression)
        {
            Value = Expression.Lambda(Expression).Compile().DynamicInvoke();
        }

        /// <summary>
        /// The parameter value to inject.
        /// </summary>
        public object Value { get; private set;}
    }
}
