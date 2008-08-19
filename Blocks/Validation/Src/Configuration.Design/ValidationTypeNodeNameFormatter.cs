//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
    /// <summary>
    /// Used to create a friendly name for <see cref="TypeNode"/> instances.
    /// </summary>
    public class ValidationTypeNodeNameFormatter : TypeNodeNameFormatter
    {
        /// <summary>
        /// Retruns a friendly name based on a <see cref="ValidatedTypeReference"/> instance.
        /// </summary>
        /// <param name="typeConfiguration">The <see cref="ValidatedTypeReference"/> that should be used to create a name.</param>
        /// <returns>A friendly name that can be used as a display name.</returns>
        public string CreateName(ValidatedTypeReference typeConfiguration)
        {
            if (typeConfiguration == null) throw new ArgumentNullException("typeConfiguration");

            return base.CreateName(typeConfiguration.Name);
        }
    }
}
