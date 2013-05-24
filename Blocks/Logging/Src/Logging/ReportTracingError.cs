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
    /// Error handling delegate.
    /// </summary>
    /// <param name="exception">The exception that was thrown while tracing.</param>
    /// <param name="data">The data.</param>
    /// <param name="source">The source.</param>
    public delegate void ReportTracingError(Exception exception, object data, string source);
}
