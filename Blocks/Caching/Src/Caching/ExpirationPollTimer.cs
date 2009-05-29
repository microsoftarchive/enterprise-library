//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// Represents an expiration poll timer.
	/// </summary>
    public sealed class ExpirationPollTimer : IDisposable
    {
        private Timer pollTimer;
        private int expirationPollFrequencyInMilliSeconds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expirationPollFrequencyInMilliSeconds"></param>
        public ExpirationPollTimer(int expirationPollFrequencyInMilliSeconds)
        {
            if (expirationPollFrequencyInMilliSeconds == 0)
            {
                throw new ArgumentException(); //todo: message
            }
            this.expirationPollFrequencyInMilliSeconds = expirationPollFrequencyInMilliSeconds;
        }

		/// <summary>
		/// Start the polling process.
		/// </summary>
		/// <param name="callbackMethod">The method to callback when a cycle has completed.</param>
        public void StartPolling(TimerCallback callbackMethod)
        {
            if (callbackMethod == null)
            {
                throw new ArgumentNullException("callbackMethod");
            }

            pollTimer = new Timer(callbackMethod, null, expirationPollFrequencyInMilliSeconds, expirationPollFrequencyInMilliSeconds);
        }

		/// <summary>
		/// Stop the polling process.
		/// </summary>
        public void StopPolling()
        {
            if (pollTimer == null)
            {
                throw new InvalidOperationException(Resources.InvalidPollingStopOperation);
            }

            pollTimer.Dispose();
            pollTimer = null;
        }

		void IDisposable.Dispose()
		{
			if (pollTimer != null) pollTimer.Dispose();
		}		
	}
}
