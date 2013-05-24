#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Quick Start
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Diagnostics.Tracing;

namespace ApplicationDemo
{
    [EventSource(Name = "RxFlushQuickStart")]
    public class RxFlushQuickStartEventSource : EventSource
    {
        private static readonly Lazy<RxFlushQuickStartEventSource> log = new Lazy<RxFlushQuickStartEventSource>(() => new RxFlushQuickStartEventSource());

        public static RxFlushQuickStartEventSource Log
        {
            get { return log.Value; }
        }

        [Event(1, Level = EventLevel.Informational, Message = "Customer with customer id {0} was selected.")]
        public void NewCustomerSelected(int customerId)
        {
            if (IsEnabled()) WriteEvent(1, customerId);
        }

        [Event(2, Level = EventLevel.Warning, Message = "Transient error while accessing backend service. Number of transient errors in current session: {1}. Currently accessing customer {0}. The call will be retried later.")]
        public void TransientErrorWhileRefreshingCustomerData(int customerId, int retryCount)
        {
            if (IsEnabled()) WriteEvent(2, customerId, retryCount);
        }

        [Event(3, Level = EventLevel.Error, Message = "Oops, something happened here! Go figure out how we reached this invalid state! Check the informational messages.\r\n{0}")]
        public void UnknownError(string errorMessage)
        {
            if (IsEnabled()) WriteEvent(3, errorMessage);
        }
    }
}
