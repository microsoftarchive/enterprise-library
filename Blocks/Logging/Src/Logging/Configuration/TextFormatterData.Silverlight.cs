//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    partial class TextFormatterData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatterData"/> class with default values.
        /// </summary>
        public TextFormatterData()
        {
            this.Template = DefaultTemplate;
        }

        /// <summary>
        /// Gets or sets the template containing tokens to replace.
        /// </summary>
        public string Template
        {
            get;
            set;
        }
    }
}
