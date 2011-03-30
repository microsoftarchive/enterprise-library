//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Base class for <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Filters.ILogFilter"/> configuration objects.
    /// </summary>
    /// <remarks>
    /// This class should be made abstract, but in order to use it in a NameTypeConfigurationElementCollection
    /// it must be public and have a no-args constructor.
    /// </remarks>
    public abstract partial class LogFilterData : NamedConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of <see cref="LogFilterData"/>.
        /// </summary>
        protected LogFilterData()
        {
        }

        /// <summary>
        /// Returns a <see cref="TypeRegistration"/> for this data section.
        /// </summary>
        /// <remarks>
        /// This must be overridden by any subclasses, but is not abstract due to configuration section serialization constraints.
        /// </remarks>
        /// <returns></returns>
        public abstract IEnumerable<TypeRegistration> GetRegistrations();
    }
}
