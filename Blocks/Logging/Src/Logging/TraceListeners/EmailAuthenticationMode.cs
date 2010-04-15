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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// This enumeration defines the options that the <see cref="EmailTraceListener"/>
    /// can use to authenticate to the STMP server.
    /// </summary>
    public enum EmailAuthenticationMode
    {
        /// <summary>
        /// No authentication
        /// </summary>
        None = 0,

        /// <summary>
        /// Use the Windows credentials for the current process
        /// </summary>
        WindowsCredentials,

        /// <summary>
        /// Pass a user name and password
        /// </summary>
        UserNameAndPassword
    }
}
