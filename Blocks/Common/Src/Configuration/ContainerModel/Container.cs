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
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// A static marker class to denote types constructed by the container when registering a <see cref="TypeRegistration"/>.
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// Indicates a type to be resolved from a container.
        /// </summary>
        /// <typeparam name="T">The type to resolve from the container.</typeparam>
        /// <returns>The type resolved</returns>
        public static T Resolved<T>()
        {
            return default(T);
        }

        /// <summary>
        /// Indicates a type to be resolved by name from a container.
        /// </summary>
        /// <typeparam name="T">The type to resolve from the container.</typeparam>
        /// <param name="name">The name to use when resolving the type.</param>
        /// <returns>The type resolved.</returns>
        public static T Resolved<T>(string name)
        {
            return default(T);
        }

        /// <summary>
        /// Indicates a type to be resolved by name from a container, if the name is not null.
        /// </summary>
        /// <typeparam name="T">The type to resolve from the container.</typeparam>
        /// <param name="name">The name to use when resolving the type.</param>
        /// <returns>The type resolved.</returns>
        public static T ResolvedIfNotNull<T>(string name)
        {
            return default(T);
        }

        /// <summary>
        /// Indicates an enumberable set to be resolved from a container using the names supplied
        /// in <paramref name="names"/>.
        /// </summary>
        /// <typeparam name="T">The type to resolve from the container.</typeparam>
        /// <param name="names">The set of names to use when resolving from the container.</param>
        /// <returns></returns>
        public static IEnumerable<T> ResolvedEnumerable<T>(IEnumerable<string> names)
        {
            return null;
        }

        internal static bool IsResolved(MethodInfo methodInfo)
        {
            return methodInfo.Name == "Resolved";
        }

        internal static bool IsResolvedEnumerable(MethodInfo methodInfo)
        {
            return methodInfo.Name == "ResolvedEnumerable";
        }

        internal static bool IsOptionalResolved(MethodInfo methodInfo)
        {
            return methodInfo.Name == "ResolvedIfNotNull";
        }

        internal static string CalculateNameForMethodCall(MethodCallExpression expression)
        {
            if (expression.Arguments.Any())
            {
                return Expression.Lambda(expression.Arguments[0]).Compile().DynamicInvoke() as string;
            }
            else
            {
                return null;
            }
        }
    }
}
