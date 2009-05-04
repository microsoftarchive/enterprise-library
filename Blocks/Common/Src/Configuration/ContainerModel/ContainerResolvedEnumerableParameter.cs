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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// A parameter representing a set of named items to be resolved by the container.
    /// </summary>
    public class ContainerResolvedEnumerableParameter : ParameterValue
    {
        internal ContainerResolvedEnumerableParameter(MethodCallExpression expression)
            : base(expression)
        {
            IEnumerable<string> nameListArgument = Expression.Lambda(expression.Arguments[0]).Compile().DynamicInvoke() as IEnumerable<string>;
            if (nameListArgument == null) throw new ArgumentNullException(Container.CalculateNameForMethodCall(expression));
            Names = nameListArgument;

            ElementType = expression.Type.GetGenericArguments()[0];
        }

        /// <summary>
        /// The set of names to resolve in the container.
        /// </summary>
        public IEnumerable<string> Names { get; private set;}

        /// <summary>
        /// Enumeration type
        /// </summary>
        public Type ElementType { get; private set;}
    }
}
