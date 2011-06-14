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
using System.Windows.Markup;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Configuration object to describe an instance of custom <see cref="Validator"/> class.
    /// </summary>
    /// <remarks>
    /// Custom <see cref="Validator"/> classes must implement a constructor with name and value collection parameters.
    /// </remarks>
    [ContentProperty("Attributes")]
    partial class CustomValidatorData : IObjectWithNameAndType
    {
        private readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// Gets or sets the <see cref="Type"/> the element is the configuration for.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> the element is the configuration for.
        /// </value>
        public virtual Type Type
        {
            get { return (Type)typeConverter.ConvertFrom(TypeName); }
            set { this.TypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the <see cref="Type"/> the element is the configuration for.
        /// </summary>
        /// <value>
        /// the fully qualified name of the <see cref="Type"/> the element is the configuration for.
        /// </value>
        public virtual string TypeName { get; set; }

        //Use Dictionary as NameValueCollection is not available for Silverlight
        private readonly Dictionary<string, string> attributes = new Dictionary<string, string>();
        /// <summary>
        /// Gets the custom configuration attributes.
        /// </summary>
        public Dictionary<string, string> Attributes
        {
            get { return this.attributes; }
        }
    }
}
