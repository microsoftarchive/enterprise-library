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
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    partial class RegexValidatorData
    {
        private AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// Gets or sets the pattern for the represented validator.
        /// </summary>
        /// <seealso cref="RegexValidator.Pattern"/>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets or sets the regex options for the represented validator.
        /// </summary>
        /// <seealso cref="RegexOptions"/>
        /// <seealso cref="RegexValidator.Options"/>
        public RegexOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource holding the regex pattern.
        /// </summary>
        public string PatternResourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource type holding the regex pattern.
        /// </summary>
        public string PatternResourceTypeName { get; set; }

        /// <summary>
        /// Gets or sets the enum element type.
        /// </summary>
        public Type PatternResourceType
        {
            get { return (Type)typeConverter.ConvertFrom(this.PatternResourceTypeName); }
            set { this.PatternResourceTypeName = typeConverter.ConvertToString(value); }
        }
    }
}
