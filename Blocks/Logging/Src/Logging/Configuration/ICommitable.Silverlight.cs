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
    /// <summary>
    /// Provides a way to apply batched changes made to an update context.
    /// </summary>
    /// <remarks>This service should not be used directly from client code, and it's meant to be used by
    /// the <see cref="LogWriter"/> implementations internally.</remarks>
    internal interface ICommitable
    {
        /// <summary>
        /// Commits the changes.
        /// </summary>
        void Commit();
    }
}
