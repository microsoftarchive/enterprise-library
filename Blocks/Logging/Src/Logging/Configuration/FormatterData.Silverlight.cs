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
    /// Represents the configuration settings for a log formatter.  This class is abstract.
    /// </summary>
    public abstract partial class FormatterData : NamedConfigurationElement
    {
        /// <summary>
        /// Create a new instance of a <see cref="FormatterData"/>.
        /// </summary>
        protected FormatterData()
        {
        }

        /// <summary>
        /// Returns the <see cref="TypeRegistration"/> entry for this data section.
        /// </summary>
        /// <returns>The type registration for this data section</returns>
        public abstract IEnumerable<TypeRegistration> GetRegistrations();
    }
}
