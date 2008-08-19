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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
    /// <summary>
    /// A <see cref="PolicyInjector"/> is a class that is responsible for
    /// determining if a particular type is interceptable based on the
    /// specific interception mechanism it implements, and will create
    /// the interceptor.
    /// </summary>
    [CustomFactory(typeof(PolicyInjectorCustomFactory))]
    public abstract class PolicyInjector
    {
        private PolicySet policies;

        /// <summary>
        /// Creates a new <see cref="RemotingPolicyInjector" /> with an 
        /// empty <see cref="PolicySet" />.
        /// </summary>
        public PolicyInjector()
        {
            policies = new PolicySet();
        }

        /// <summary>
        /// Creates a new <see cref="RemotingPolicyInjector" /> with the
        /// given <see cref="PolicySet"/>.
        /// </summary>
        /// <param name="policies"><see cref="PolicySet"/> to use when 
        /// creating object or wrapping existing ones.</param>
        public PolicyInjector(PolicySet policies)
        {
            this.policies = policies;
        }

        /// <summary>
        /// Gets or sets the policy set used when creating the proxies for objects.
        /// </summary>
        /// <value><see cref="PolicySet"/> defining rules and handler to apply.</value>
        public PolicySet Policies
        {
            get { return policies; }
            set { policies = value;  }
        }

        /// <summary>
        /// Checks to see if the given type can be intercepted.
        /// </summary>
        /// <param name="t">Type to check.</param>
        /// <returns>True if this type can be intercepted, false if it cannot.</returns>
        public abstract bool TypeSupportsInterception(Type t);

        /// <summary>
        /// Wraps the given instance in a proxy with interception hooked up if it
        /// is required by policy. If not required, returns the unwrapped instance.
        /// </summary>
        /// <param name="instance">object to wrap.</param>
        /// <param name="typeToReturn">Type of the reference to return.</param>
        /// <param name="policiesForThisType">Policy set specific to typeToReturn.</param>
        /// <returns>The object with policy added.</returns>
        protected abstract object DoWrap(object instance, Type typeToReturn, PolicySet policiesForThisType);

        /// <summary>
        /// Creates the target object and appropriate interception when one of the Injector's Create methods is called.
        /// </summary>
        /// <remarks>Implementors can override this method if they need to control creation of the target object. 
        /// The base class implementation calls 
        /// <see cref="System.Activator.CreateInstance(Type, object[])"/> and then defers the interception
        /// to <see cref="PolicyInjector.DoWrap(object, Type, PolicySet)"/>.</remarks>
        /// <param name="typeToCreate"><see cref="System.Type"/> of target object to create.</param>
        /// <param name="typeToReturn">Type of the reference to return.</param>
        /// <param name="policiesForThisType">Policy set specific to typeToReturn.</param>
        /// <param name="arguments">Constructor parameters.</param>
        /// <returns>The new target object.</returns>
        protected virtual object DoCreate(Type typeToCreate, Type typeToReturn, PolicySet policiesForThisType, object[] arguments)
        {
            object target = Activator.CreateInstance(typeToCreate, arguments);
            return DoWrap(target, typeToReturn, policiesForThisType);
        }

        /// <summary>
        /// Takes an existing object and returns a new reference that includes
        /// the policies specified in the current <see cref="PolicySet"/>.
        /// </summary>
        /// <param name="instance">The object to wrap.</param>
        /// <param name="typeToReturn">Type to return. This can be either an
        /// interface implemented by the object, or its concrete class.</param>
        /// <returns>A new reference to the object that includes the policies.</returns>
        public object Wrap(object instance, Type typeToReturn)
        {
            PolicySet policiesForThisType = policies.GetPoliciesFor(instance.GetType());
            EnsureTypeIsInterceptable(typeToReturn, policiesForThisType);
            return DoWrap(instance, typeToReturn, policiesForThisType);
        }

        /// <summary>
        /// Creates a new instance of typeToCreate and then applies policy.
        /// </summary>
        /// <param name="typeToCreate">The concrete type of the object to be created.</param>
        /// <param name="typeToReturn">The type of the reference to return. This can either
        /// be a concrete type, or the type of an interface that typeToCreate implements. If
        /// an interface type is specified, policy interception will only occur on calls
        /// to that interface.</param>
        /// <param name="args">Arguments to pass to the constructor.</param>
        /// <returns>The wrapped object instance of type typeToReturn.</returns>
        public object Create(Type typeToCreate, Type typeToReturn, params object[] args)
        {
            PolicySet policiesForThisType = policies.GetPoliciesFor(typeToCreate);
            EnsureTypeIsInterceptable(typeToReturn, policiesForThisType);
            return DoCreate(typeToCreate, typeToReturn, policiesForThisType, args);
        }

        /// <summary>
        /// Creates a new instance of typeToCreate and then applies policy.
        /// </summary>
        /// <param name="typeToCreate">The type of object to be created.</param>
        /// <param name="args">Arguments to be passed to the constructor.</param>
        /// <returns>The wrapped object instance of type typeToCreate.</returns>
        public object Create(Type typeToCreate, params object[] args)
        {
            return Create(typeToCreate, typeToCreate, args);
        }

        /// <summary>
        /// Takes an existing object and returns a new reference that includes
        /// the policies specified in the current <see cref="PolicySet"/>.
        /// </summary>
        /// <typeparam name="TInterface">The type of wrapper to return. Can be either
        /// an interface implemented by the target instance or its entire concrete type.</typeparam>
        /// <param name="instance">The object to wrap.</param>
        /// <returns>A new reference to the object that includes the policies.</returns>
        public TInterface Wrap<TInterface>(object instance)
        {
            return (TInterface)Wrap(instance, typeof(TInterface));
        }

        /// <summary>
        /// Creates new instance of type TObject and applies policy to it.
        /// </summary>
        /// <typeparam name="TObject">Type of object to create.</typeparam>
        /// <typeparam name="TInterface">Type of reference to return. If an interface type is
        /// specified here, policy is only applied to the methods of that interface.</typeparam>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>A reference to the created object.</returns>
        public TInterface Create<TObject, TInterface>(params object[] args)
        {
            return (TInterface)Create(typeof(TObject), typeof(TInterface), args);
        }

        /// <summary>
        /// Creates new instance of type TObject and applies policy to it.
        /// </summary>
        /// <typeparam name="TObject">Type of object to create.</typeparam>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>A reference to the created object.</returns>
        public TObject Create<TObject>(params object[] args)
        {
            return (TObject)Create(typeof(TObject), typeof(TObject), args);
        }

        /// <summary>
        /// Checks to see if the given type has any policies that apply to it.
        /// </summary>
        /// <param name="t">Type to check.</param>
        /// <returns>true if the current set of policies will require interception to be added,
        /// false if no policies apply to type t</returns>
        public bool TypeRequiresInterception(Type t)
        {
            return PolicyRequiresInterception(policies.GetPoliciesFor(t));
        }

        /// <summary>
        /// Checks to see if the given policy set requires interception on targets that it is applied to.
        /// </summary>
        /// <param name="policies">Policy set to check.</param>
        /// <returns>True if policy set contains anything, false if not.</returns>
        protected static bool PolicyRequiresInterception(PolicySet policies)
        {
            return policies.Count > 0;
        }

        /// <summary>
        /// Checks to see if the given type requires interception and if so if it
        /// is actually interceptable or not. If not, throws <see cref="System.ArgumentException"/>.
        /// </summary>
        /// <param name="typeToReturn">Type to check.</param>
        /// <param name="policiesForThisType">Policy set specific to typeToReturn.</param>
        private void EnsureTypeIsInterceptable(Type typeToReturn, PolicySet policiesForThisType)
        {
            if( PolicyRequiresInterception(policiesForThisType) )
            {
                if(!TypeSupportsInterception(typeToReturn))
                {
                    throw new ArgumentException(
                        string.Format(Resources.InterceptionNotSupported, typeToReturn.Name),
                        "typeToReturn");
                }
            }
        }
    }
}