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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// Implement this interface to create an object that can read a set
    /// of <see cref="TypeRegistration"/> objects representing the current
    /// Enterprise Library configuration and configure a dependency injection
    /// container with that information.
    /// 
    /// </summary>
    public interface IContainerConfigurator
    {
        /// <summary>
        /// Consume the set of <see cref="TypeRegistration"/> objects and
        /// configure the associated container.
        /// </summary>
        /// <param name="registrations">The <see cref="TypeRegistration"/> objects
        /// describing the current Enterprise Library configuration.</param>
        void RegisterAll(IEnumerable<TypeRegistration> registrations);
    }
}
