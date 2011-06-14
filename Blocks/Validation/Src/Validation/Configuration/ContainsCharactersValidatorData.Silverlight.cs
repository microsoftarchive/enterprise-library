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

using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    partial class ContainsCharactersValidatorData
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ContainsCharactersValidatorData"/> class.</para>
        /// </summary>
        public ContainsCharactersValidatorData()
        {
            ContainsCharacters = ContainsCharacters.Any;
        }

        /// <summary>
        /// Gets or sets the string containing the characters to use by the represented 
        /// <see cref="ContainsCharactersValidator"/>.
        /// </summary>
        public string CharacterSet { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ContainsCharacters"/> value indicating the behavior for 
        /// the represented <see cref="ContainsCharactersValidator"/>.
        /// </summary>
        public ContainsCharacters ContainsCharacters { get; set; }
    }
}
