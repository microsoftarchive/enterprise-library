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

using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// Represents a construction parameter resolved through the container.
    /// </summary>
    public class ContainerResolvedParameter : ParameterValue
    {
        /// <summary>
        /// Initializes the construction parameter from the <see cref="MethodCallExpression"/>.  This method call expression 
        /// expected to be respresented through the <see cref="Container"/> static marker class.
        /// </summary>
        /// <remarks>
        /// 
        /// Given a class Example defined as:
        /// 
        /// public class Example
        /// {
        ///     public Example(Argument arg); 
        /// }
        /// 
        /// A <see cref="TypeRegistration{T}"/> and <see cref="LambdaExpression"/> for this configuration might appear as follows:
        ///   new TypeRegistration&lt;Example&gt;(() => new Example(Container.Resolved&lt;Argument&gt;("SomeName"));
        /// 
        /// During construction of the Example class, Argument will be resolved and injected by the container.
        /// The <see cref="Container.Resolved{T}()"/> marker interface is used to represent
        /// this requirement to a container configurator and is translated to a <see cref="ContainerResolvedParameter"/>.
        /// </remarks>
        /// <seealso cref="Container"/>
        /// <param name="expression">The method expression representing the type to resolve and named value.</param>
        internal ContainerResolvedParameter(MethodCallExpression expression): base(expression)
        {
            Name = Container.CalculateNameForMethodCall(expression);
        }

        /// <summary>
        /// The name to use when resolving the type represented by the method call expression.
        /// </summary>
        public string Name { get; private set;}
    }
}
