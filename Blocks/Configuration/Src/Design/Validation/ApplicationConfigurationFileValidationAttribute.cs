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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Represents the validation for the application configuration file.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public sealed class ApplicationConfigurationFileValidationAttribute: FileValidationAttribute
    {
        /// <summary>
        /// Returns the context path which should be used to validate relative paths.
        /// </summary>
		/// <value>The context path which should be used to validate relative paths.</value>
        protected override string ContextPath
        {
			get 
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
        }
    }
}
