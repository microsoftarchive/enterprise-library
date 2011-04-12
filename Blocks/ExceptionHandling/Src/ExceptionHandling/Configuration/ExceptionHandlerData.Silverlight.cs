//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an <see cref="IExceptionHandler"/>.
    /// </summary>    
    public abstract partial class ExceptionHandlerData : NamedConfigurationElement
    {
        /// <summary>
        /// Initializes an instance of a <see cref="ExceptionHandlerData"/> class.
        /// </summary>
        protected ExceptionHandlerData()
        {
        }

        /// <summary>
        /// Returns the <see cref="TypeRegistration" /> for the exception handler data provided.
        /// </summary>
        /// <param name="namePrefix">The prefix to use when building references to child elements.</param>
        /// <returns>A <see cref="TypeRegistration"/> for the exception handler data</returns>
        public abstract IEnumerable<TypeRegistration> GetRegistrations(string namePrefix);
    }
}
