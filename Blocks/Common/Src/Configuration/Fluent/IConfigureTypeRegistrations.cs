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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{

    /// <summary>
    /// Fluent interface that allows to add <see cref="ITypeRegistrationsProvider"/> instances.
    /// </summary>
    /// <see cref="ITypeRegistrationsProvider"/>
    /// <see cref="TypeRegistrationProviderElement"/>
    public interface IConfigureTypeRegistrations : IFluentInterface
    {
        /// <summary>
        /// Adds a <see cref="ITypeRegistrationsProvider"/> instance to configuration source builder. <br/>
        /// </summary>
        /// <param name="typeRegistrationsProviderName">The name of the <see cref="ITypeRegistrationsProvider"/> instance.</param>
        IConfigureTypeRegistration AddTypeRegistrationsProviderNamed(string typeRegistrationsProviderName);
    }

    /// <summary>
    /// Fluent interface used to configure a <see cref="ITypeRegistrationsProvider"/> instance.
    /// </summary>
    /// <see cref="ITypeRegistrationsProvider"/>
    /// <see cref="TypeRegistrationProviderElement"/>
    public interface IConfigureTypeRegistration : IFluentInterface
    {
        /// <summary>
        /// Specifies the type of the <see cref="ITypeRegistrationsProvider"/> instance that should be used to retrieve type registrations.
        /// </summary>
        /// <typeparam name="TTypeRegistrationsProvider">The type of the <see cref="ITypeRegistrationsProvider"/> instance that should be used to retrieve type registrations.</typeparam>
        /// <returns>Fluent interface that allows to add more type registration providers.</returns>
        /// <see cref="ITypeRegistrationsProvider"/>
        /// <see cref="TypeRegistrationProviderElement"/>
        IConfigureTypeRegistrations ForType<TTypeRegistrationsProvider>() where TTypeRegistrationsProvider : ITypeRegistrationsProvider;

        /// <summary>
        /// Specifies the type of the <see cref="ITypeRegistrationsProvider"/> instance that should be used to retrieve type registrations.
        /// </summary>
        /// <param name="typeRegistrationProvider">The type of the <see cref="ITypeRegistrationsProvider"/> instance that should be used to retrieve type registrations.</param>
        /// <returns>Fluent interface that allows to add more type registration providers.</returns>
        /// <see cref="ITypeRegistrationsProvider"/>
        /// <see cref="TypeRegistrationProviderElement"/>
        IConfigureTypeRegistrations ForType(Type typeRegistrationProvider);

        /// <summary>
        /// Specifies the name of the configuration section that implements <see cref="ITypeRegistrationsProvider"/>. <br/>
        /// </summary>
        /// <param name="sectionName">The name of the configuration section that implements <see cref="ITypeRegistrationsProvider"/>. </param>
        /// <returns>Fluent interface that allows to add more type registration providers.</returns>
        /// <see cref="ITypeRegistrationsProvider"/>
        /// <see cref="TypeRegistrationProviderElement"/>
        IConfigureTypeRegistrations ForSection(string sectionName);
    }
}
