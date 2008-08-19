//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Base class defining the information for a policy injector.
    /// </summary>
    public class InjectorData : NameTypeConfigurationElement
    {
        /// <summary>
        /// Creates a new empty <see cref="InjectorData"/>.
        /// </summary>
        public InjectorData()
        {
            
        }

        /// <summary>
        /// Create a new <see cref="InjectorData"/> that configures the
        /// given injector type.
        /// </summary>
        /// <param name="injectorName"></param>
        /// <param name="injectorType"></param>
        public InjectorData(string injectorName, Type injectorType):
            base(injectorName, injectorType)
        {
            
        }
    }
}
