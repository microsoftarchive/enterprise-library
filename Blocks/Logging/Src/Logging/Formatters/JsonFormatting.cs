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


namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
    /// <summary>
    /// Specifies formatting options.
    /// </summary>
    public enum JsonFormatting
    {
        /// <summary>
        /// Specifies that no special formatting should be applied. This is the default.
        /// </summary>
        None,

        /// <summary>
        /// Specifies that child objects should be indented.
        /// </summary>
        Indented
    }
}
