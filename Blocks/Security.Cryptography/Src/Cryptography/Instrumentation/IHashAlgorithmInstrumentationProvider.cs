//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHashAlgorithmInstrumentationProvider
    {

        /// <summary>
        /// </summary>
        /// <param name="message">The message that describes the failure.</param>
        /// <param name="exception">The exception thrown during the failure.</param>
        void FireCyptographicOperationFailed(string message, Exception exception);

        /// <summary>
        /// </summary>
        void FireHashComparisonPerformed();

        /// <summary>
        /// </summary>
        void FireHashMismatchDetected();
        
        /// <summary>
        /// </summary>
        void FireHashOperationPerformed();
    }
}
