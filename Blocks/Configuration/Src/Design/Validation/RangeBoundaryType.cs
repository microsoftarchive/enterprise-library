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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// A value describing the boundary conditions for a range.
    /// </summary>
    public enum RangeBoundaryType
    {
        /// <summary>
        /// The range should include the boundary.
        /// </summary>
        Inclusive = 0,
        /// <summary>
        /// The range should exclude the boundary.
        /// </summary>
        Exclusive
    }
}
