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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Contains settings specific to the registration of a <see cref="TypeRegistrationsProvider"/>.
    /// </summary>
    public class TypeRegistrationProviderElement : NamedConfigurationElement
    {
        /// <summary>
        /// The section name used to retrieve the <see cref="ITypeRegistrationsProvider"/> if available.
        /// </summary>
        public string SectionName
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the type that implements <see cref="ITypeRegistrationsProvider"/>. 
        /// </summary>
        public string ProviderTypeName
        {
            get;
            set;
        }
    }
}
