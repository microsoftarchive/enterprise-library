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
using System.Linq;
using System.Text;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// This class implements the Visitor pattern over the hierarchy of
    /// <see cref="ParameterValue"/> types. This makes it easier for container
    /// authors to figure out which type of <see cref="ParameterValue"/> they're
    /// dealing with and centralize processing without manually having to switch
    /// on the runtime type.
    /// </summary>
    public abstract class ParameterValueVisitor
    {
        /// <summary>
        /// Main entry point. When this method is called, this class will figure out
        /// the current runtime type of the passed <paramref name="parameterValue"/>
        /// and then call the corresponding strongly-typed visit method based on that runtime
        /// type.
        /// </summary>
        /// <param name="parameterValue">The <see cref="ParameterValue"/> object to process.</param>
        /// <seealso cref="VisitConstantParameterValue"/>
        /// <seealso cref="VisitResolvedParameterValue"/>
        /// <seealso cref="VisitEnumerableParameterValue"/>
        /// <seealso cref="VisitParameterValue"/>
        public void Visit(ParameterValue parameterValue)
        {
            // This is a shorthand way of calling one method after the other without
            // having to write a ton of if statements. We need the result variable because
            // C# doesn't let us use a raw Boolean expression as a statement.
            bool result =
                Visit<ConstantParameterValue>(parameterValue, VisitConstantParameterValue) ||
                Visit<ContainerResolvedParameter>(parameterValue, VisitResolvedParameterValue) ||
                Visit<ContainerResolvedEnumerableParameter>(parameterValue, VisitEnumerableParameterValue) ||
                Visit<ParameterValue>(parameterValue, VisitParameterValue);
        }

        /// <summary>
        /// The method called when a <see cref="ConstantParameterValue"/> object is visited.
        /// </summary>
        /// <remarks>By default, this method throws an exception. Override it to provide your
        /// specific processing.</remarks>
        /// <param name="parameterValue">The <see cref="ConstantParameterValue"/> to process.</param>
        protected virtual void VisitConstantParameterValue(ConstantParameterValue parameterValue)
        {
            VisitParameterValue(parameterValue);
        }

        /// <summary>
        /// The method called when a <see cref="ContainerResolvedParameter"/> object is visited.
        /// </summary>
        /// <remarks>By default, this method throws an exception. Override it to provide your
        /// specific processing.</remarks>
        /// <param name="parameterValue">The <see cref="ContainerResolvedParameter"/> to process.</param>
        protected virtual void VisitResolvedParameterValue(ContainerResolvedParameter parameterValue)
        {
            VisitParameterValue(parameterValue);
        }

        /// <summary>
        /// The method called when a <see cref="ContainerResolvedEnumerableParameter"/> object is visited.
        /// </summary>
        /// <remarks>By default, this method throws an exception. Override it to provide your
        /// specific processing.</remarks>
        /// <param name="parameterValue">The <see cref="ContainerResolvedEnumerableParameter"/> to process.</param>
        protected virtual void VisitEnumerableParameterValue(ContainerResolvedEnumerableParameter parameterValue)
        {
            VisitParameterValue(parameterValue);
        }

        /// <summary>
        /// The method called when a <see cref="ParameterValue"/> object is visited and we haven't
        /// been able to otherwise identify the runtime type as a <see cref="ConstantParameterValue"/>,
        /// <see cref="ContainerResolvedParameter"/>, or <see cref="ContainerResolvedEnumerableParameter"/>.
        /// </summary>
        /// <remarks>By default, this method throws an exception. Override it to provide your
        /// specific processing or do further type checking if you have extended the type hierarchy.</remarks>
        /// <param name="parameterValue">The <see cref="ParameterValue"/> to process.</param>
        protected virtual void VisitParameterValue(ParameterValue parameterValue)
        {
            throw new ArgumentException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.ExceptionUnrecognizedDependencyParameterType,
                    parameterValue.GetType()),
                "parameterValue");
        }

        private static bool Visit<T>(ParameterValue parameterValue, Action<T> visitMethod) where T : ParameterValue
        {
            var casted = parameterValue as T;
            if (casted != null)
            {
                visitMethod(casted);
                return true;
            }
            return false;
        }
    }
}
