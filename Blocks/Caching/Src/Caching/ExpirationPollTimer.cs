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

		/// <summary>
		/// Start the polling process.
		/// </summary>
		/// <param name="callbackMethod">The method to callback when a cycle has completed.</param>
		/// <param name="pollCycleInMilliseconds">The time in milliseconds to poll.</param>
        public void StartPolling(TimerCallback callbackMethod, int pollCycleInMilliseconds)
        {
            if (callbackMethod == null)
            {
                throw new ArgumentNullException("callbackMethod");
            }
            if (pollCycleInMilliseconds <= 0)
            {
                throw new ArgumentException(Resources.InvalidExpirationPollCycleTime, "pollCycleInMilliseconds");
            }

            pollTimer = new Timer(callbackMethod, null, pollCycleInMilliseconds, pollCycleInMilliseconds);
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