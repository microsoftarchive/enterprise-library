using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    public class TraceEventCache
    {
        private DateTime dateTime = DateTime.MinValue;
        private string stackTrace;
        private long timeStamp = -1L;

        internal static int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

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

        public string ThreadId
        {
            get
            {
                return GetThreadId().ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
