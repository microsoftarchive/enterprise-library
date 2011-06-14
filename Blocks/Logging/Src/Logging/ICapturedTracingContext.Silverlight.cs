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

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents captured tracing context so tracing can be resumed in a different thread.
    /// </summary>
    public interface ICapturedTracingContext
    {
        /// <summary>
        /// Restores the tracing context captured by this object.
        /// </summary>
        /// <returns>An <see cref="IDisposable"/> implementation that will clear the restored tracing context on 
        /// <see cref="IDisposable.Dispose()"/>.</returns>
        /// <exception cref="InvalidOperationException">when the method is invoked a second time on the same instance.</exception>
        /// <remarks>
        /// If <see cref="IDisposable.Dispose()"/> is not invoked on the return value of this method, the restored
        /// tracing context will not be cleared.
        /// </remarks>
        IDisposable Restore();
    }
}
