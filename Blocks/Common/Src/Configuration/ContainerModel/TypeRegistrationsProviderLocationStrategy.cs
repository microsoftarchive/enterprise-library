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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// Base class for an object that can find an <see cref="ITypeRegistrationsProvider"/> instance
    /// given a particular config source.
    /// </summary>
    public abstract class TypeRegistrationsProviderLocationStrategy
    {
        /// <summary>
        /// Name that identifies this particular strategy object.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Find a single <see cref="ITypeRegistrationsProvider"/> object.
        /// </summary>
        /// <param name="configurationSource"></param>
        /// <returns></returns>
        public abstract ITypeRegistrationsProvider GetProvider(IConfigurationSource configurationSource);
    }
}
