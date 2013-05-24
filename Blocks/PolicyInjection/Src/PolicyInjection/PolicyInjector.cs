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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
    /// <summary>
    /// A non-static facade class that provides the main entry point into the
    /// Policy Injection Application Block. Methods on this class
    /// create intercepted objects, or wrap existing instances with
    /// interceptors.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This facade can be initialized with either an <see cref="IServiceLocator"/> or an 
    /// <see cref="IConfigurationSource"/>. In the latter case, a new container will be created and it will be disposed
    /// when the policy injector is disposed.
    /// </para>
    /// </remarks>
    public class PolicyInjector : IDisposable
    {
        private IUnityContainer container;
        private readonly bool ownsContainer;
        private InstanceInterceptionPolicySettingInjectionMember instanceInterceptionPolicySettingInjectionMember;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyInjector"/> class with the specified configuration source.
        /// </summary>
        /// <param name="configurationSource">The configuration source from which to retrieve configuration information.</param>
        /// <exception cref="ArgumentNullException"><paramref name="configurationSource"/> is <see langword="null"/>.</exception>
        public PolicyInjector(IConfigurationSource configurationSource)
        {
            if (configurationSource == null)
            {
                throw new ArgumentNullException("configurationSource");
            }

            this.ownsContainer = true;
            Initialize(CreateContainer(configurationSource));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyInjector"/> class with the specified service locator.
        /// </summary>
        /// <param name="serviceLocator">The service locator from which an <see cref="IUnityContainer"/> can be resolved
        /// to perform interception.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serviceLocator"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">An <see cref="IUnityContainer"/> cannot be resolved from the 
        /// <paramref name="serviceLocator"/>, or the resolved container does not have the <see cref="Interception"/>
        /// extension.</exception>
        public PolicyInjector(IServiceLocator serviceLocator)
        {
            if (serviceLocator == null)
            {
                throw new ArgumentNullException("serviceLocator");
            }

            Initialize(GetUnityContainer(serviceLocator));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyInjector"/> class with the specified container.
        /// </summary>
        /// <param name="container">The container to perform interception.</param>
        /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The container does not have the <see cref="Interception"/>
        /// extension.</exception>
        public PolicyInjector(IUnityContainer container)
        {
            Guard.ArgumentNotNull(container, "container");

            Initialize(container);
        }

        private void Initialize(IUnityContainer container)
        {
            if (container.Configure<Interception>() == null)
            {
                throw new ArgumentException("Container does not have the interception extension", "container");
            }

            if (container.Configure<TransientPolicyBuildUpExtension>() == null)
            {
                throw new ArgumentException("Container does not have the transient buildup extension", "container");
            }

            this.container = container;

            this.instanceInterceptionPolicySettingInjectionMember =
                new InstanceInterceptionPolicySettingInjectionMember(new TransparentProxyInterceptor());
        }

        /// <summary>
        /// Creates a proxy for the given object that adds interception policies.
        /// </summary>
        /// <remarks>
        /// Despite the name of the <typeparamref name="TInterface"/> parameter, this
        /// may be any type that the instance is assignable to, including both interfaces
        /// that it implements and the concrete type of the object.
        /// </remarks>
        /// <typeparam name="TInterface">Type of the proxy to return.</typeparam>
        /// <param name="instance">Instance object to wrap.</param>
        /// <returns>The proxy for the instance, or the raw object if no policies apply.</returns>
        public TInterface Wrap<TInterface>(object instance)
        {
            return (TInterface)Wrap(typeof(TInterface), instance);
        }

        /// <summary>
        /// Creates a proxy for the given object that adds interception policies.
        /// </summary>
        /// <param name="typeToReturn">Type of the proxy to return.</param>
        /// <param name="instance">Instance object to wrap.</param>
        /// <returns>The proxy for the instance, or the raw object if no policies apply.</returns>
        public object Wrap(Type typeToReturn, object instance)
        {
            Guard.ArgumentNotNull(typeToReturn, "typeToReturn");
            Guard.ArgumentNotNull(instance, "instance");
            Microsoft.Practices.Unity.Utility.Guard.InstanceIsAssignable(typeToReturn, instance, "instance");

            if (this.container == null)
            {
                throw new ObjectDisposedException("policyInjector");
            }

            return this.container.Configure<TransientPolicyBuildUpExtension>()
                .BuildUp(
                    typeToReturn,
                    instance,
                    null,
                    this.instanceInterceptionPolicySettingInjectionMember);
        }

        /// <summary>
        /// Creates a new object of type <typeparamref name="TObject"/> and
        /// adds interception as needed to match the policies specified for the injector.
        /// </summary>
        /// <typeparam name="TObject">Type of object to create.</typeparam>
        /// <param name="args">Arguments to pass to the <typeparamref name="TObject"/> constructor.</param>
        /// <returns>The intercepted object (or possibly a raw instance if no policies apply).</returns>
        public TObject Create<TObject>(params object[] args)
        {
            return (TObject)Create(typeof(TObject), args);
        }

        /// <summary>
        /// Creates a new object of type <typeparamref name="TObject"/> and
        /// adds interception as needed to match the policies specified for the injector.
        /// </summary>
        /// <typeparam name="TObject">Concrete object type to create.</typeparam>
        /// <typeparam name="TInterface">Type of reference to return. Must be an interface the object implements.</typeparam>
        /// <param name="args">Arguments to pass to the <typeparamref name="TObject"/> constructor.</param>
        /// <returns>The intercepted object (or possibly a raw instance if no policies apply).</returns>
        public TInterface Create<TObject, TInterface>(params object[] args)
            where TObject : TInterface
        {
            return (TInterface)Create(typeof(TObject), typeof(TInterface), args);
        }

        /// <summary>
        /// Creates a new object of type <paramref name="typeToCreate"/> and
        /// adds interception as needed to match the policies specified for the injector.
        /// </summary>
        /// <param name="typeToCreate">Type of object to create.</param>
        /// <param name="args">Arguments to pass to the <paramref name="typeToCreate"/> constructor.</param>
        /// <returns>The intercepted object (or possibly a raw instance if no policies apply).</returns>
        public object Create(Type typeToCreate, params object[] args)
        {
            return Create(typeToCreate, typeToCreate, args);
        }

        /// <summary>
        /// Creates a new object of type <paramref name="typeToCreate"/> and
        /// adds interception as needed to match the policies specified for the injector.
        /// </summary>
        /// <param name="typeToCreate">Concrete object type to create.</param>
        /// <param name="typeToReturn">Type of reference to return. Must be an interface the object implements.</param>
        /// <param name="args">Arguments to pass to the <paramref name="typeToCreate"/> constructor.</param>
        /// <returns>The intercepted object (or possibly a raw instance if no policies apply).</returns>
        public object Create(Type typeToCreate, Type typeToReturn, params object[] args)
        {
            Guard.ArgumentNotNull(typeToCreate, "typeToCreate");

            object instance = Activator.CreateInstance(typeToCreate, args);
            return Wrap(typeToReturn, instance);
        }

        /// <summary>
        /// Dispose this policy injector.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose this policy injector.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if being called from the IDisposable.Dispose method, 
        /// <see langword="false"/> if being called from a finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.container != null)
                {
                    var containerForDisposal = this.container;
                    this.container = null;
                    if (this.ownsContainer)
                    {
                        containerForDisposal.Dispose();
                    }
                }
            }
        }

        private static IUnityContainer CreateContainer(IConfigurationSource configurationSource)
        {
            var container = new UnityContainer();
            container
                .AddNewExtension<Interception>()
                .AddNewExtension<TransientPolicyBuildUpExtension>();

            var settings = configurationSource.GetSection(PolicyInjectionSettings.SectionName) as PolicyInjectionSettings;
            if (settings != null)
            {
                settings.ConfigureContainer(container);
            }

            return container;
        }

        private static IUnityContainer GetUnityContainer(IServiceLocator serviceLocator)
        {
            try
            {
                return serviceLocator.GetInstance<IUnityContainer>();
            }
            catch (ActivationException e)
            {
                throw new InvalidOperationException("Cannot resolve container", e);
            }
        }
    }
}
