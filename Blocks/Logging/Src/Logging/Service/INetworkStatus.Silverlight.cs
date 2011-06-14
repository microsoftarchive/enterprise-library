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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Provides notifications for when the network status changes.
    /// </summary>
    public interface INetworkStatus : IDisposable
    {
        /// <summary>
        /// Notifies that the network status might have changed.
        /// </summary>
        event EventHandler NetworkStatusUpdated;

        /// <summary>
        /// Indicates whether a network connection is available.
        /// </summary>
        /// <remarks>
        /// A network connection is considered to be available if any network interface is marked "up" and is not a loopback or tunnel interface.
        /// Many cases in which a computer is not connected to a useful network are still considered available and will return true. 
        /// For example, a computer connected to a hub or router where the hub or router has lost the upstream connection will return true.
        /// </remarks>
        /// <returns><see langword="true"/> if a network connection is available; otherwise, <see langword="false"/>.</returns>
        bool GetIsNetworkAvailable();
    }
}
