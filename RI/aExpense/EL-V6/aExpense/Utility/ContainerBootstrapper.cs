#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AExpense.DataAccessLayer;
using AExpense.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace AExpense
{
    public static class ContainerBootstrapper
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Microsoft.DisposeObjectsBeforeLosingScope",
            Justification = "The container is used from HttpApplicationState and cannot be disposed.")]
        public static void Configure(IUnityContainer container)
        {
            // Get Entlib config source (Current is in Web.EnterpriseLibrary.config)
            IConfigurationSource source = ConfigurationSourceFactory.Create();

            // Config container from Policy injection config settings 
            var policyInjectionSettings = (PolicyInjectionSettings)source.GetSection(PolicyInjectionSettings.SectionName);
            policyInjectionSettings.ConfigureContainer(container);

            // Config retry policy
            var retryPolicySettings = RetryPolicyConfigurationSettings.GetRetryPolicySettings(source);
            // turn off throwIfSet for unit testing
            RetryPolicyFactory.SetRetryManager(retryPolicySettings.BuildRetryManager(), throwIfSet: false);

            // get factories from config
            var policyFactory = new ExceptionPolicyFactory(source);
            var dbFactory = new DatabaseProviderFactory(source);
            var validationFactory = ConfigurationValidatorFactory.FromConfigurationSource(source);
            
            // Set default locator
            UnityServiceLocator locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);            
            
            container
                .AddNewExtension<Interception>()

                // register Entlib types with appropiate factory  
                .RegisterType<ExceptionManager>(new InjectionFactory(c => policyFactory.CreateManager()))
                .RegisterType<Database>(new InjectionFactory(c => dbFactory.CreateDefault()))
                .RegisterInstance<ValidatorFactory>(validationFactory)

                // use registration by convention extension for registering app types; IProfileStore, IUserRepository
                .RegisterTypes(AllClasses.FromAssemblies(Assembly.GetExecutingAssembly()), 
                               WithMappings.FromAllInterfacesInSameAssembly, 
                               WithName.Default, 
                               WithLifetime.ContainerControlled)

                // register types with interception 
                .RegisterType<AExpense.Model.User>(new Interceptor<VirtualMethodInterceptor>(),
                                                   new InterceptionBehavior<TracingBehavior>())
                .RegisterType<IExpenseRepository, ExpenseRepository>(new Interceptor<VirtualMethodInterceptor>(),
                                                                     new InterceptionBehavior<PolicyInjectionBehavior>());
        }
    }
}