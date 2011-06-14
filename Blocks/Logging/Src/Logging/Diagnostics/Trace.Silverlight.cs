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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    /// <summary>
    /// Entry point for the <see cref="CorrelationManager"/>.
    /// </summary>
    /// <remarks>
    /// This class supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// </remarks>
    public static class Trace
    {
        private static readonly CorrelationManager correlationManager = new CorrelationManager();

        /// <summary>
        /// Gets the <see cref="CorrelationManager"/>.
        /// </summary>
        public static CorrelationManager CorrelationManager
        {
            get
            {
                return correlationManager;
            }
        }
    }
}
