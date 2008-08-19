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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

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
        private static volatile PolicyInjector defaultPolicyInjector;
        private static readonly object singletonLock = new object();

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
            return DefaultPolicyInjector.Create<TObject>(args);
        }

        /// <summary>
        /// Creates a new object of type <typeparamref name="TObject"/> and
        /// adds interception as needed to match the policies specified in
        /// the policy configuration supplied in <paramref name="configurationSource"/>.
        /// </summary>
        /// <typeparam name="TObject">Type of object to create.</typeparam>
        /// <param name="configurationSource"><see cref="IConfigurationSource"/> containing the policy configuration.</param>
        /// <param name="args">Arguments to pass to the <typeparamref name="TObject"/> constructor.</param>
        /// <returns>The intercepted object (or possibly a raw instance if no policies apply).</returns>
        public static TObject Create<TObject>(IConfigurationSource configurationSource, params object[] args)
        {
            PolicyInjector policyInjector = GetInjectorFromConfig(configurationSource);
            return policyInjector.Create<TObject>(args);
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
        {
            return DefaultPolicyInjector.Create<TObject, TInterface>(args);
        }

        /// <summary>
        /// Creates a new object of type <typeparamref name="TObject"/> and
        /// adds interception as needed to match the policies specified in
        /// the policy configuration supplied in <paramref name="configurationSource"/>.
        /// </summary>
        /// <typeparam name="TObject">Concrete object type to create.</typeparam>
        /// <typeparam name="TInterface">Type of reference to return. Must be an interface the object implements.</typeparam>
        /// <param name="configurationSource"><see cref="IConfigurationSource"/> containing the policy configuration.</param>
        /// <param name="args">Arguments to pass to the <typeparamref name="TObject"/> constructor.</param>
        /// <returns>The intercepted object (or possibly a raw instance if no policies apply).</returns>
        public static TInterface Create<TObject, TInterface>(IConfigurationSource configurationSource, params object[] args)
        {
            PolicyInjector policyInjector = GetInjectorFromConfig(configurationSource);
            return policyInjector.Create<TObject, TInterface>(args);
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
            return DefaultPolicyInjector.Wrap<TInterface>(instance);
        }

        /// <summary>
        /// Creates a proxy for the given object that adds interception policies as
        /// defined in <paramref name="configurationSource"/>.
        /// </summary>
        /// <remarks>
        /// Despite the name of the <typeparamref name="TInterface"/> parameter, this
        /// may be any type that the instance is assignable to, including both interfaces
        /// that it implements and the concrete type of the object.
        /// </remarks>
        /// <typeparam name="TInterface">Type of the proxy to return.</typeparam>
        /// <param name="configurationSource"><see cref="IConfigurationSource"/> containing the policy configuration.</param>
        /// <param name="instance">Instance object to wrap.</param>
        /// <returns>The proxy for the instance, or the raw object if no policies apply.</returns>
        public static TInterface Wrap<TInterface>(IConfigurationSource configurationSource, object instance)
        {
            PolicyInjector policyInjector = GetInjectorFromConfig(configurationSource);
            return policyInjector.Wrap<TInterface>(instance);
        }

        private static PolicyInjector DefaultPolicyInjector
        {
            get
            {
                if( defaultPolicyInjector == null)
                {
                    lock(singletonLock)
                    {
                        if(defaultPolicyInjector == null)
                        {
                            IConfigurationSource configurationSource =
                                ConfigurationSourceFactory.Create();
                            defaultPolicyInjector = GetInjectorFromConfig(configurationSource);
                        }
                    }
                }
                return defaultPolicyInjector;
            }
        }

        private static PolicyInjector GetInjectorFromConfig(IConfigurationSource configurationSource)
        {
            PolicyInjectorFactory injectorFactory = new PolicyInjectorFactory(configurationSource);
            return injectorFactory.Create();
        }

    }
}
