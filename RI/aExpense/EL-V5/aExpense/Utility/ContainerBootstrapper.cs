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
using AExpense.DataAccessLayer;
using AExpense.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace AExpense
{
    public static class ContainerBootstrapper
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Microsoft.DisposeObjectsBeforeLosingScope", 
            Justification = "This container is used in the controller factory and cannot be disposed.")]
        public static void Configure(IUnityContainer container)
        {
            container
                .AddNewExtension<EnterpriseLibraryCoreExtension>()
                .RegisterType<IProfileStore, SimulatedLdapProfileStore>(new ContainerControlledLifetimeManager())
                .RegisterType<IUserRepository, UserRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<AExpense.Model.User>(new Interceptor<VirtualMethodInterceptor>(),
                                                   new InterceptionBehavior<TracingBehavior>())
                .RegisterType<IExpenseRepository, ExpenseRepository>(new ContainerControlledLifetimeManager(),
                                                                     new Interceptor<VirtualMethodInterceptor>(),
                                                                     new InterceptionBehavior<PolicyInjectionBehavior>());

            // Set default locator so we can use it from 
            UnityServiceLocator locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }
    }
}