#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace AExpense.FunctionalTests.Util
{
    public class InMemoryEventListener : EventListener
    {
        public InMemoryEventListener()
        {
            this.EventsWritten = new List<EventWrittenEventArgs>();
        }

        public IList<EventWrittenEventArgs> EventsWritten { get; private set; }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            this.EventsWritten.Add(eventData);
        }
    }
}
