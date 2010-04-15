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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Is a custom <see cref="Property"/> for a <see cref="SectionViewModel"/> that indicates if the section needs a protection provider and is, therefore, encrypted.
    /// </summary>
    public class ProtectionProviderProperty : CustomProperty<string>
    {
        ///<summary>
        /// Instantiates a new instance of <see cref="ProtectionProviderProperty"/>.
        ///</summary>
        ///<param name="serviceProvider">The provider to use to locate services.</param>
        public ProtectionProviderProperty(IServiceProvider serviceProvider)
            : base(serviceProvider, new ProtectionProviderTypeConverter(), "Protection Provider")
        {
            
        }

        ///<summary>
        /// Gets a value indicating that the sectio needs a protection provider.
        ///</summary>
        public bool NeedsProtectionProvider
        {
            get { return !String.IsNullOrEmpty((string) Value); }
        }
    }
}
