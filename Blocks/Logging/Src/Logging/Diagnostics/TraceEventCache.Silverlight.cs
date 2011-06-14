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
using System.Globalization;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    /// <summary>
    /// Provides context information.
    /// </summary>
    public class TraceEventCache
    {
        private DateTime dateTime = DateTime.MinValue;
        private string stackTrace;

        internal static int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// Gets the call stack for the current thread.
        /// </summary>
        public string Callstack
        {
            get
            {
                if (this.stackTrace == null)
                {
                    this.stackTrace = new StackTrace().ToString();
                }
                return this.stackTrace;
            }
        }

        /// <summary>
        /// Gets the date and time at which the event trace occurred.
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                if (this.dateTime == DateTime.MinValue)
                {
                    this.dateTime = DateTime.UtcNow;
                }
                return this.dateTime;
            }
        }

        /// <summary>
        /// Gets a unique identifier for the current managed thread. 
        /// </summary>
        public string ThreadId
        {
            get
            {
                return GetThreadId().ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
