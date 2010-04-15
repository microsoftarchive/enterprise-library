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

            var body = GetEffectiveBody(expression);

            if (!serviceType.IsAssignableFrom(body.Type))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.ExceptionRegistrationServiceTypeIsNotCompatible,
                        serviceType.FullName,
                        expression.Body.Type.FullName),
                    "serviceType");
            }

            if (body.NodeType != ExpressionType.New && body.NodeType != ExpressionType.MemberInit)
            {
                throw new ArgumentException(Properties.Resources.ExceptionRegistrationTypeExpressionMustBeNewLambda);
            }

            LambdaExpression = expression;
            ServiceType = serviceType;
        }

        private static Expression GetEffectiveBody(LambdaExpression expression)
        {
            var body = expression.Body;

            if (body.NodeType == ExpressionType.Convert)
            {
                body = ((UnaryExpression)body).Operand;
            }
            return body;
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
                var body = GetEffectiveBody(LambdaExpression);

                if (body.NodeType == ExpressionType.New)
                {
                    return (NewExpression)body;
                }

                return ((MemberInitExpression)body).NewExpression;
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
                return name ?? DefaultName(ServiceType);
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Is this registration for a type that is part of a public API? If
        /// true, configurators should not transform the name in any way. If
        /// false, this is an internal implementation class that users will not
        /// be resolving directly, and as such the name can be manipulated safely
        /// without interfering with the public API.
        /// </summary>
        /// <remarks>Some containers have restrictions on the allowed names (for example,
        /// many require names to be globally unique). Some object names need to be
        /// left alone (for example, Database or Exception policies) becuase that is
        /// what the user will use to get those objects. Other names (like for instrumentation
        /// providers) are internal and can be freely changed by the configurator as
        /// needed to fit into the container.</remarks>
        public bool IsPublicName { get; set; }

        /// <summary>
        /// Returns the default name for a type that will be returned if no name
        /// is otherwise specified.
        /// </summary>
        /// <param name="serviceType">Type that was registered.</param>
        /// <returns>Default name that will be used.</returns>
        public static string DefaultName(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");

            return serviceType.Name + "." + "__default__";
        }

        /// <summary>
        /// Returns the default name for a type that will be returned if no name
        /// is otherwise specified.
        /// </summary>
        /// <typeparam name="TServiceType">Type that was registered.</typeparam>
        /// <returns>Default name that will be used.</returns>
        public static string DefaultName<TServiceType>()
        {
            return DefaultName(typeof(TServiceType));
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
                MemberInitExpression memberInitExpression = GetEffectiveBody(LambdaExpression) as MemberInitExpression;

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
