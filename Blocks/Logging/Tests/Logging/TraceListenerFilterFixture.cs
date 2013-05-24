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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class TraceListenerFilterFixture
    {
        [TestMethod]
        public void TraceListenerFilterOnEmptyCollectionReturnsHasNoElements()
        {
            TraceListenerFilter filter = new TraceListenerFilter();
            IEnumerable<TraceListener> traceListenersCollection = new TraceListener[0];

            int i = 0;
            Dictionary<TraceListener, int> listeners = new Dictionary<TraceListener, int>();
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }

            Assert.AreEqual(0, i);
        }

        [TestMethod]
        public void TraceListenerFilterOnSingleElementCollectionReturnsHasSingleElement()
        {
            TraceListenerFilter filter = new TraceListenerFilter();

            TraceListener listener1 = new MockTraceListener();
            IEnumerable<TraceListener> traceListenersCollection = new TraceListener[] { listener1 };

            int i = 0;
            Dictionary<TraceListener, int> listeners = new Dictionary<TraceListener, int>();
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }

            Assert.AreEqual(1, i);
            Assert.AreEqual(1, listeners[listener1]);
        }

        [TestMethod]
        public void TraceListenerFilterOnMultipleElementsCollectionReturnsHasSameElements()
        {
            TraceListenerFilter filter = new TraceListenerFilter();

            TraceListener listener1 = new MockTraceListener();
            TraceListener listener2 = new MockTraceListener();
            IEnumerable<TraceListener> traceListenersCollection = new TraceListener[] { listener1, listener2 };

            int i = 0;
            Dictionary<TraceListener, int> listeners = new Dictionary<TraceListener, int>();
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }

            Assert.AreEqual(2, i);
            Assert.AreEqual(1, listeners[listener1]);
            Assert.AreEqual(1, listeners[listener2]);
        }

        [TestMethod]
        public void TraceListenerFilterOnMultipleCollectionsWithDisjointElementsDoesNotRepeatElements()
        {
            TraceListenerFilter filter = new TraceListenerFilter();

            TraceListener listener1 = new MockTraceListener();
            TraceListener listener2 = new MockTraceListener();
            IEnumerable<TraceListener> traceListenersCollection1 = new TraceListener[] { listener1, listener2 };
            TraceListener listener3 = new MockTraceListener();
            TraceListener listener4 = new MockTraceListener();
            IEnumerable<TraceListener> traceListenersCollection2 = new TraceListener[] { listener3, listener4 };

            int i = 0;
            Dictionary<TraceListener, int> listeners = new Dictionary<TraceListener, int>();
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection1))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection2))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }

            Assert.AreEqual(4, i);
            Assert.AreEqual(1, listeners[listener1]);
            Assert.AreEqual(1, listeners[listener2]);
            Assert.AreEqual(1, listeners[listener3]);
            Assert.AreEqual(1, listeners[listener4]);
        }

        [TestMethod]
        public void TraceListenerFilterOnMultipleCollectionsWithCommonElementsDoesNotRepeatElements()
        {
            TraceListenerFilter filter = new TraceListenerFilter();

            TraceListener listener1 = new MockTraceListener();
            TraceListener listener2 = new MockTraceListener();
            IEnumerable<TraceListener> traceListenersCollection1 = new TraceListener[] { listener1, listener2 };
            TraceListener listener3 = new MockTraceListener();
            TraceListener listener4 = new MockTraceListener();
            IEnumerable<TraceListener> traceListenersCollection2 = new TraceListener[] { listener2, listener3, listener4 };

            int i = 0;
            Dictionary<TraceListener, int> listeners = new Dictionary<TraceListener, int>();
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection1))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }
            foreach (TraceListener listener in filter.GetAvailableTraceListeners(traceListenersCollection2))
            {
                i++;
                listeners[listener] = (listeners.ContainsKey(listener) ? listeners[listener] : 0) + 1;
            }

            Assert.AreEqual(4, i);
            Assert.AreEqual(1, listeners[listener1]);
            Assert.AreEqual(1, listeners[listener2]);
            Assert.AreEqual(1, listeners[listener3]);
            Assert.AreEqual(1, listeners[listener4]);
        }
    }
}
