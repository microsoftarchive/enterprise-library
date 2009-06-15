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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// Represents a container registration entry as a <see cref="LambdaExpression"/> and additional metadata.
    /// </summary>
    public class TypeRegistration
    {
        /// <summary>
        /// The name that will be returned for a <see cref="TypeRegistration"/>
        /// if no name is otherwise specified.
        /// </summary>
        public static readonly string DefaultName = "__default__";

        private string name;

        /// <summary>
        /// Initialize a new instance of the <see cref="TypeRegistration"/> class with a <see cref="LambdaExpression"/>
        /// as the model for injection.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> representing the injection.</param>
        public TypeRegistration(LambdaExpression expression)
            : this(expression, expression != null ? expression.Body.Type : null)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TypeRegistration"/> class with a <see cref="LambdaExpression"/>
        /// as the model for injection.
        /// </summary>
        /// <param name="expression">The <see cref="LambdaExpression"/> representing the injection.</param>
        /// <param name="serviceType">The service type to register the implementation against.</param>
        public TypeRegistration(LambdaExpression expression, Type serviceType)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            if (!serviceType.IsAssignableFrom(expression.Body.Type))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.ExceptionRegistrationServiceTypeIsNotCompatible,
                        serviceType.FullName,
                        expression.Body.Type.FullName),
                    "serviceType");
            }

            if (expression.Body.NodeType != ExpressionType.New && expression.Body.NodeType != ExpressionType.MemberInit)
            {
                throw new ArgumentException(Properties.Resources.ExceptionRegistrationTypeExpressionMustBeNewLambda);
            }

            LambdaExpression = expression;
            ServiceType = serviceType;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> for the registration entry.
        /// </summary>
        public Type ImplementationType
        {
            get
            {
                return NewExpressionBody.Type;
            }
        }

        /// <summary>
        /// Returns the expression body representing the creation constructor call.
        /// </summary>
        public NewExpression NewExpressionBody
        {
            get
            {
                if (LambdaExpression.Body.NodeType == ExpressionType.New)
                {
                    return (NewExpression)LambdaExpression.Body;
                }

                return ((MemberInitExpression)LambdaExpression.Body).NewExpression;
            }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> for which the <see cref="ImplementationType"/> provides an implementation.
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets the name under which the entry should be registered to the container.
        /// </summary>
        public string Name 
        {
            get
            {
                return name ?? DefaultName;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets <see cref="LambdaExpression"/> representing the injection.
        /// </summary>
        public LambdaExpression LambdaExpression { get; private set; }

        /// <summary>
        /// Gets <see langword="true"/> if the registration is to be considered the default for the service type, 
        /// <see langword="false"/> otherwise.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// The required lifetime for this service implementation.
        /// </summary>
        public TypeRegistrationLifetime Lifetime { get; set; }

        /// <summary>
        /// Gets the <see cref="ParameterValue"/> instances describing values injected through the constructor.
        /// </summary>
        public IEnumerable<ParameterValue> ConstructorParameters
        {
            get
            {
                return
                    from arg in NewExpressionBody.Arguments
                    select BuildDependencyParameter(arg);
            }
        }

        /// <summary>
        /// Gets the <see cref="InjectedProperty"/> instances describing values injected to properties.
        /// </summary>
        public IEnumerable<InjectedProperty> InjectedProperties
        {
            get
            {
                MemberInitExpression memberInitExpression = LambdaExpression.Body as MemberInitExpression;

                if (memberInitExpression != null)
                {
                    foreach (var binding in memberInitExpression.Bindings)
                    {
                        var memberAssignmentBinding = binding as MemberAssignment;

                        if (memberAssignmentBinding == null)
                        {
                            throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture,
                                                                          Properties.Resources.ExceptionUnsupportedBindingExpressionType,
                                                                          binding.Member.Name));
                        }

                        yield return new InjectedProperty(binding.Member.Name, BuildDependencyParameter(memberAssignmentBinding.Expression));
                    }
                }
            }
        }

        private static ParameterValue BuildDependencyParameter(Expression arg)
        {
            MethodCallExpression methodCallExpression = arg as MethodCallExpression;

            if (methodCallExpression != null
                && methodCallExpression.Method.DeclaringType == typeof(Container))
            {
                if (Container.IsResolved(methodCallExpression.Method))
                    return new ContainerResolvedParameter(methodCallExpression);

                if (Container.IsResolvedEnumerable(methodCallExpression.Method))
                    return new ContainerResolvedEnumerableParameter(methodCallExpression);

                if (Container.IsOptionalResolved(methodCallExpression.Method))
                {
                    if (string.IsNullOrEmpty(Container.CalculateNameForMethodCall(methodCallExpression)))
                    {
                        return new ConstantParameterValue(Expression.Constant(null, methodCallExpression.Type));
                    }
                    else
                    {
                        return new ContainerResolvedParameter(methodCallExpression);
                    }
                }

                throw new ArgumentException(Properties.Resources.ExceptionUnrecognizedContainerMarkerMethod);
            }
            return new ConstantParameterValue(arg);
        }
    }

    /// <summary>
    /// Represents a container registration entry as a <see cref="LambdaExpression"/> and additional metadata for constructing a specific type.
    /// </summary>
    /// <typeparam name="T">The service type registered with the container</typeparam>
    public class TypeRegistration<T> : TypeRegistration
    {
        /// <summary>
        /// Initializes the TypeRegistration with a <see cref="LambdaExpression"/> for T.
        /// </summary>
        /// <param name="expression"><see cref="LambdaExpression"/> that providing the construction model for T.</param>
        public TypeRegistration(Expression<Func<T>> expression)
            : base(expression, typeof(T))
        {
        }
    }

    /// <summary>
    /// A set of values indicating what the lifetime of service implementations
    /// in the container should be.
    /// </summary>
    public enum TypeRegistrationLifetime
    {
        /// <summary>
        /// This implementation should be stored by the container and it should return
        /// the same object for each request.
        /// </summary>
        Singleton = 0,

        /// <summary>
        /// A new instance should be returned for each request.
        /// </summary>
        Transient
    }
}
