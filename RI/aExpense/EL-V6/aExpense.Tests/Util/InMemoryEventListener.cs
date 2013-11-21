// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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
