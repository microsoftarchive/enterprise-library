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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Buildup;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ContainerUtility;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// The dependency injection container used by the Enterprise Library configuration designtime.
    /// </summary>
    /// <remarks>
    /// The container provides registration and initialization of standard
    /// services needed for the design-time configuration</remarks>
    public class ConfigurationContainer : UnityContainer, IServiceProvider
    {
        private readonly IServiceProvider parentServiceProvider;

        /// <summary>
        /// Initializes an instance of <see cref="ConfigurationContainer"/>
        /// </summary>
        public ConfigurationContainer()
            : this(null, new Profile())
        { }

        /// <summary>
        /// Initializes an instance of <see cref="ConfigurationContainer"/>
        /// </summary>
        public ConfigurationContainer(IServiceProvider serviceProvider)
            : this(serviceProvider, new Profile())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationContainer"/> class.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public ConfigurationContainer(Profile profile)
            : this(null, profile)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationContainer"/> class.
        /// </summary>
        /// <param name="serviceProvider">A service provider to resolve services.</param>
        /// <param name="profile">The profile.</param>
        public ConfigurationContainer(IServiceProvider serviceProvider, Profile profile)
        {
            ConfigurationContainerRegistration.Registration(this, profile);
            this.parentServiceProvider = serviceProvider;
        }
        /// <summary>
        /// Discovers types via the <see cref="AnnotationService"/> that
        /// provide the design-time metadata for another class.
        /// </summary>
        public void DiscoverSubstituteTypesFromAssemblies()
        {
            AnnotationService annotationService = (AnnotationService)Resolve(typeof(AnnotationService), null);
            annotationService.DiscoverSubstituteTypesFromAssemblies();
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.
        ///                     -or- 
        ///                 null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        /// <param name="serviceType">An object that specifies the type of service object to get. 
        ///                 </param><filterpriority>2</filterpriority>
        public object GetService(Type serviceType)
        {
            try
            {
                return this.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return this.parentServiceProvider != null ? this.parentServiceProvider.GetService(serviceType) : null;
            }
        }

        private static class ConfigurationContainerRegistration
        {
            public static void Registration(ConfigurationContainer container, Profile profile)
            {
                container.RegisterType<AssemblyLocator, BinPathProbingAssemblyLocator>(new ContainerControlledLifetimeManager());
                container.RegisterType<ConfigurationSectionLocator, AssemblyAttributeSectionLocator>(new ContainerControlledLifetimeManager());
                container.RegisterType<AnnotationService>(new ContainerControlledLifetimeManager());
                container.RegisterType<ElementLookup>(new ContainerControlledLifetimeManager());
                container.RegisterType<ConfigurationSourceModel>(new ContainerControlledLifetimeManager());
                container.RegisterType<ViewModel.Services.MenuCommandService>(new ContainerControlledLifetimeManager());
                container.RegisterType<ConfigurationSourceDependency>(new ContainerControlledLifetimeManager());
                container.RegisterType<IApplicationModel, ApplicationViewModel>(new ContainerControlledLifetimeManager());
                container.RegisterType<ValidationModel>(new ContainerControlledLifetimeManager());
                container.RegisterType(typeof(IResolver<>), typeof(GenericResolver<>));
                container.RegisterType<SaveOperation>(new ContainerControlledLifetimeManager());
                container.RegisterInstance<IServiceProvider>(container);
                container.RegisterInstance(profile);
            }
        }
    }
}
