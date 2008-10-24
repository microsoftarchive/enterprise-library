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
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
	public class MockBlockingCustomTraceListener : CustomTraceListener
	{
		public static object traceRequestMonitor = new object();
		public static object disposeMonitor = new object();

		public static bool waitOnDispose = false;

		private static int processedTraceRequests = 0;
		private static int pendingTraceRequests = 0;
		private static List<LogEntry> entries = new List<LogEntry>();
		private static List<MockBlockingCustomTraceListener> instances = new List<MockBlockingCustomTraceListener>();

		public bool disposeCalled = false;
		private static int pendingDisposeRequests = 0;

		public MockBlockingCustomTraceListener()
			: this("")
		{
		}

		public MockBlockingCustomTraceListener(string name)
		{
			this.Name = name;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			disposeCalled = true;

			lock (disposeMonitor)
			{
				pendingDisposeRequests++;
				if (waitOnDispose)
					Monitor.Wait(disposeMonitor);
				pendingDisposeRequests--;
			}
		}

		public static void Reset()
		{
			entries.Clear();
			instances.Clear();
			processedTraceRequests = 0;
			pendingTraceRequests = 0;
			waitOnDispose = false;
			pendingDisposeRequests = 0;
		}

 		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			lock (traceRequestMonitor)
			{
				pendingTraceRequests++;

				Monitor.Wait(traceRequestMonitor);

				MockBlockingCustomTraceListener.Entries.Add(data as LogEntry);
				MockBlockingCustomTraceListener.Instances.Add(this);

				pendingTraceRequests--;
				processedTraceRequests++;
			}
		}

		public static List<LogEntry> Entries
		{
			get { return entries; }
			set { entries = value; }
		}

		public static List<MockBlockingCustomTraceListener> Instances
		{
			get { return instances; }
			set { instances = value; }
		}

		public static int ProcessedTraceRequests
		{
			get { lock (traceRequestMonitor) { return processedTraceRequests; } }
		}

		public static int PendingTraceRequests
		{
			get { lock (traceRequestMonitor) { return pendingTraceRequests; } }
		}

		public static int PendingDisposeRequests
		{
			get { lock (disposeMonitor) { return pendingDisposeRequests; } }
		}

		public override void Write(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void WriteLine(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool IsThreadSafe
		{
			get
			{
				return true;
			}
		}
	}
}
