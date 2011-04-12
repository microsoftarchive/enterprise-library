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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                NetworkChange.NetworkAddressChanged -= this.OnNetworkAddressChanged;
            }
        }

        ~NetworkStatus()
        {
            this.Dispose(false);
        }
    }
}
