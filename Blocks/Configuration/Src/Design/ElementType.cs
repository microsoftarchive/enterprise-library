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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// A wrapper class for <see cref="Type"/> to allow for dependency injection.
    /// </summary>
    public class ConfigurationElementType
    {
        readonly Type elementType;

        /// <summary>
        /// Initializes a new instance of <see cref="ConfigurationElementType"/>.
        /// </summary>
        /// <param name="elementType">The <see cref="Type"/> that should be injected.</param>
        public ConfigurationElementType(Type elementType)
        {
            this.elementType = elementType;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> instance that was injected.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> instance that was injected.
        /// </value>
        public Type ElementType
        {
            get { return elementType; }
        }
    }
}
