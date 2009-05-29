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
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// Class that can be used as a base class for instance factories.
    /// </summary>
    /// <remarks>
    /// This class is used to create instances of types compatible with <typeparamref name="T"/> described 
    /// by a configuration source.
    /// </remarks>
    /// <typeparam name="T">Type of instance to create</typeparam>
    public class ContainerBasedInstanceFactory<T> : IDisposable
    {
        private IServiceLocator container;
        private readonly bool ownsContainer;

        /// <summary>
        /// Create an instance of <see cref="ContainerBasedInstanceFactory{T}"/> that resolves objects
        /// using the supplied <paramref name="container"/>.
        /// </summary>
        /// <param name="container"><see cref="IServiceLocator"/> to use to resolve objects.</param>
        public ContainerBasedInstanceFactory(IServiceLocator container)
        {
            this.container = container;
            ownsContainer = false;
        }

        /// <summary>
        /// Create an instance of <see cref="ContainerBasedInstanceFactory{T}"/>. A container will be
        /// constructed under the hood and be initialized with the information in <paramref name="configurationSource"/>.
        /// </summary>
        /// <param name="configurationSource">Configuration information.</param>
        public ContainerBasedInstanceFactory(IConfigurationSource configurationSource)
            : this(EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource))
        {
            ownsContainer = true;
        }

        /// <summary>
        /// Create an instance of <see cref="ContainerBasedInstanceFactory{T}"/> that resolves objects
        /// through the Entlib default container.
        /// </summary>
        public ContainerBasedInstanceFactory()
            : this(EnterpriseLibraryContainer.Current)
        {
            ownsContainer = false;
        }

        /// <summary>
        /// Returns a new instance of <typeparamref name="T"/> based on the default instance configuration.
        /// </summary>
        /// <returns>
        /// A new instance of <typeparamref name="T"/>.
        /// </returns>
        public T CreateDefault()
        {
            return container.GetInstance<T>();
        }

        /// <summary>
        /// Returns an new instance of <typeparamref name="T"/> based on the configuration for <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the required instance.</param>
        /// <returns>
        /// A new instance of <typeparamref name="T"/>.
        /// </returns>
        public T Create(string name)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "name");
            return container.GetInstance<T>(name);
        }

        /// <summary>
        /// Releases resources currently held by this object.
        /// </summary>
        public void Dispose()
        {
            if(container != null && ownsContainer)
            {
                container.Dispose();
                container = null;
            }
        }
    }
}
