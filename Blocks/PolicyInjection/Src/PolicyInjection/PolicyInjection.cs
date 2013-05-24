//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
    /// <summary>
    /// A static facade class that provides the main entry point into the
    /// Policy Injection Application Block. Methods on this class
    /// create intercepted objects, or wrap existing instances with
    /// interceptors.
    /// </summary>
    public static class PolicyInjection
    {
        private static PolicyInjector policyInjector;

        /// <summary>
        /// Creates a new object of type <typeparamref name="TObject"/> and
        /// adds interception as needed to match the policies specified in
        /// the default policy configuration.
        /// </summary>
        /// <typeparam name="TObject">Type of object to create.</typeparam>
        /// <param name="args">Arguments to pass to the <typeparamref name="TObject"/> constructor.</param>
        /// <returns>The intercepted object (or possibly a raw instance if no policies apply).</returns>
        public static TObject Create<TObject>(params object[] args)
        {
            return GetPolicyInjector().Create<TObject>(args);
        }

        /// <summary>
        /// Creates a new object of type <typeparamref name="TObject"/> and
        /// adds interception as needed to match the policies specified in
        /// the default policy configuration.
        /// </summary>
        /// <typeparam name="TObject">Concrete object type to create.</typeparam>
        /// <typeparam name="TInterface">Type of reference to return. Must be an interface the object implements.</typeparam>
        /// <param name="args">Arguments to pass to the <typeparamref name="TObject"/> constructor.</param>
        /// <returns>The intercepted object (or possibly a raw instance if no policies apply).</returns>
        public static TInterface Create<TObject, TInterface>(params object[] args)
            where TObject : TInterface
        {
            return GetPolicyInjector().Create<TObject, TInterface>(args);
        }

        /// <summary>
        /// Creates a new object of type <paramref name="typeToCreate"/> and
        /// adds interception as needed to match the policies specified in
        /// the default policy configuration.
        /// </summary>
        /// <param name="typeToCreate">Type of object to create.</param>
        /// <param name="args">Arguments to pass to the <paramref name="typeToCreate"/> constructor.</param>
        /// <returns>The intercepted object (or possibly a raw instance if no policies apply).</returns>
        public static object Create(Type typeToCreate, params object[] args)
        {
            return GetPolicyInjector().Create(typeToCreate, args);
        }

        /// <summary>
        /// Creates a new object of type <paramref name="typeToCreate"/> and
        /// adds interception as needed to match the policies specified in
        /// the default policy configuration.
        /// </summary>
        /// <param name="typeToCreate">Concrete object type to create.</param>
        /// <param name="typeToReturn">Type of reference to return. Must be an interface the object implements.</param>
        /// <param name="args">Arguments to pass to the <paramref name="typeToCreate"/> constructor.</param>
        /// <returns>The intercepted object (or possibly a raw instance if no policies apply).</returns>
        public static object Create(Type typeToCreate, Type typeToReturn, params object[] args)
        {
            return GetPolicyInjector().Create(typeToCreate, typeToReturn, args);
        }

        /// <summary>
        /// Creates a proxy for the given object that adds interception policies as
        /// defined in the default configuration source.
        /// </summary>
        /// <remarks>
        /// Despite the name of the <typeparamref name="TInterface"/> parameter, this
        /// may be any type that the instance is assignable to, including both interfaces
        /// that it implements and the concrete type of the object.
        /// </remarks>
        /// <typeparam name="TInterface">Type of the proxy to return.</typeparam>
        /// <param name="instance">Instance object to wrap.</param>
        /// <returns>The proxy for the instance, or the raw object if no policies apply.</returns>
        public static TInterface Wrap<TInterface>(object instance)
        {
            return GetPolicyInjector().Wrap<TInterface>(instance);
        }

        /// <summary>
        /// Creates a proxy for the given object that adds interception policies as
        /// defined in the default configuration source.
        /// </summary>
        /// <param name="typeToReturn">Type of the proxy to return.</param>
        /// <param name="instance">Instance object to wrap.</param>
        /// <returns>The proxy for the instance, or the raw object if no policies apply.</returns>
        public static object Wrap(Type typeToReturn, object instance)
        {
            return GetPolicyInjector().Wrap(typeToReturn, instance);
        }

        /// <summary>
        /// Sets the policy injector for the static facade.
        /// </summary>
        /// <param name="policyInjector">The policy injector.</param>
        /// <param name="throwIfSet"><see langword="true"/> to throw an exception if the policy injector is already set; otherwise, <see langword="false"/>. Defaults to <see langword="true"/>.</param>
        /// <exception cref="InvalidOperationException">The policy injector is already set and <paramref name="throwIfSet"/> is <see langword="true"/>.</exception>
        public static void SetPolicyInjector(PolicyInjector policyInjector, bool throwIfSet = true)
        {
            Guard.ArgumentNotNull(policyInjector, "policyInjector");

            var currentPolicyInjector = PolicyInjection.policyInjector;
            if (currentPolicyInjector != null && throwIfSet)
            {
                throw new InvalidOperationException(Resources.ExceptionPolicyInjectorAlreadySet);
            }

            PolicyInjection.policyInjector = policyInjector;

            if (currentPolicyInjector != null)
            {
                currentPolicyInjector.Dispose();
            }
        }

        /// <summary>
        /// Resets the policy injector for the static facade.
        /// </summary>
        /// <remarks>
        /// Used for tests.
        /// </remarks>
        public static void Reset()
        {
            var currentPolicyInjector = policyInjector;
            policyInjector = null;
            if (currentPolicyInjector != null)
            {
                currentPolicyInjector.Dispose();
            }
        }

        private static PolicyInjector GetPolicyInjector()
        {
            var currentPolicyInjector = policyInjector;
            if (currentPolicyInjector == null)
            {
                throw new InvalidOperationException(Resources.ExceptionPolicyInjectorNotSet);
            }

            return currentPolicyInjector;
        }
    }
}
