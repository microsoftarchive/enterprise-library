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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    ///<summary>
    /// <see cref="Property"/> extensions to support validation.
    ///</summary>
    public static class ValidationPropertyExtensions
    {
        ///<summary>
        /// Creates a nnew <see cref="ValidationError"/> for the given <see cref="ElementProperty"/> with the specified message.
        ///</summary>
        ///<param name="property">The <see cref="ElementProperty"/> as context for the error message.</param>
        ///<param name="errorMessage">The actual message to display</param>
        ///<returns>A new <see cref="ValidationError"/> initialized from <paramref name="property"/></returns>
        public static ValidationError ValidationError(this ElementProperty property, string errorMessage)
        {            
            return new ValidationError(property.DisplayName, errorMessage, property.DeclaringElement.Path);
        }
    }
}
