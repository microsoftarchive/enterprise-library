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
using System.Net.NetworkInformation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Provides notifications for when the network status changes, by wrapping the access to <see cref="NetworkChange"/> 
    /// and <see cref="NetworkInterface"/> classes.
    /// </summary>
    public class NetworkStatus : INetworkStatus
    {
        /// <summary>
        /// Notifies that the network status might have changed.
        /// </summary>
        public event EventHandler NetworkStatusUpdated;

        /// <summary>
        /// Initializes a new instance of <see cref="NetworkStatus"/>
        /// </summary>
        public NetworkStatus()
        {
            NetworkChange.NetworkAddressChanged += this.OnNetworkAddressChanged;
        }

        /// <summary>
        /// Indicates whether a network connection is available.
        /// </summary>
        /// <remarks>
        /// A network connection is considered to be available if any network interface is marked "up" and is not a loopback or tunnel interface.
        /// Many cases in which a computer is not connected to a useful network are still considered available and will return true. 
        /// For example, a computer connected to a hub or router where the hub or router has lost the upstream connection will return true.
        /// </remarks>
        /// <returns><see langword="true"/> if a network connection is available; otherwise, <see langword="false"/>.</returns>
        public bool GetIsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        private void OnNetworkAddressChanged(object sender, EventArgs e)
        {
            var handler = NetworkStatusUpdated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the method is being called from the <see cref="Dispose()"/> method. <see langword="false"/> if it is being called from within the object finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                NetworkChange.NetworkAddressChanged -= this.OnNetworkAddressChanged;
            }
        }

        /// <summary>
        /// Releases resources for a <see cref="NetworkStatus"/> before garbage collection.
        /// </summary>
        ~NetworkStatus()
        {
            this.Dispose(false);
        }
    }
}
