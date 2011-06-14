//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ContainerUtility;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS
{
    public abstract class ContainerContext : ArrangeActAssert
    {
        protected internal IUnityContainer Container { get; set; }

        protected Mock<IUIServiceWpf> UIServiceMock;

        protected override void Arrange()
        {
            base.Arrange();

            UIServiceMock = new Mock<IUIServiceWpf>(MockBehavior.Strict);

            Container = new UnityContainer();

            Container.RegisterType<AssemblyLocator, BinPathProbingAssemblyLocator>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ConfigurationSectionLocator, AssemblyAttributeSectionLocator>(new ContainerControlledLifetimeManager());
            Container.RegisterType<AnnotationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ElementLookup>(new ContainerControlledLifetimeManager());
            Container.RegisterType<DiscoverDerivedConfigurationTypesService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ConfigurationSourceModel>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ConfigurationSourceDependency>(new ContainerControlledLifetimeManager());
            Container.RegisterType<AnnotationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType(typeof(IResolver<>), typeof(GenericResolver<>));
            Container.RegisterInstance<IServiceProvider>(new ContainerProvider(Container));
            Container.RegisterInstance<IUIServiceWpf>(UIServiceMock.Object);
            Container.RegisterType<SaveOperation>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IApplicationModel, ApplicationViewModel>(new ContainerControlledLifetimeManager());

            this.Container.RegisterInstance<IAssemblyDiscoveryService>(new Mock<IAssemblyDiscoveryService>().Object);


            AnnotationService annotationService = Container.Resolve<AnnotationService>();
            annotationService.DiscoverSubstituteTypesFromAssemblies();
        }

        private class ContainerProvider : IServiceProvider
        {
            IUnityContainer container;
            public ContainerProvider(IUnityContainer container)
            {
                this.container = container;
            }

            public object GetService(Type serviceType)
            {
                return container.Resolve(serviceType);
            }
        }
    }
}
