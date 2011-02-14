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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    partial class ObjectCollectionValidatorData
    {
        private AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// Gets or sets the target element type.
        /// </summary>
        /// <value>
        /// The target element type.
        /// </value>
        public Type TargetType
        {
            get { return (Type)typeConverter.ConvertFrom(this.TargetTypeName); }
            set { this.TargetTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the name of the target element type for the represented validator.
        /// </summary>
        /// <seealso cref="ObjectCollectionValidatorData.TargetTypeName"/>
        public string TargetTypeName { get; set; }

        /// <summary>
        /// Gets or sets the name for the target ruleset for the represented validator.
        /// </summary>
        public string TargetRuleset { get; set; }
    }
}
