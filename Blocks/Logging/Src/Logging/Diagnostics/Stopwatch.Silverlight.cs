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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics
{
    /// <summary>
    /// Provides a set of methods and properties that you can use to measure elapsed time. 
    /// </summary>
    /// <remarks>
    /// This implementation relies on the lower-resolution <see cref="DateTime"/> class.
    /// </remarks>
    public class Stopwatch
    {
        private bool isRunning;
        private long startTimeStamp;
        private long elapsed;

        /// <summary>
        /// Gets the milliseconds elapsed for the stopwatch.
        /// </summary>
        public long ElapsedMilliseconds
        {
            get
            {
                return (this.GetElapsedDateTimeTicks() / 10000L);
            }
        }

        /// <summary>
        /// Gets the elapsed time for the stopwatch.
        /// </summary>
        public TimeSpan? Elapsed
        {
            get
            {
                return new TimeSpan(this.GetElapsedDateTimeTicks());
            }
        }

        /// <summary>
        /// Initializes a new <see cref="Stopwatch"/> instance, sets the elapsed time property to zero, and starts 
        /// measuring elapsed time.
        /// </summary>
        /// <returns>The new <see cref="Stopwatch"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "Not a new version of a method")]
        public static Stopwatch StartNew()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }

        /// <summary>
        /// Gets the current number of ticks.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static long GetTimestamp()
        {
            return DateTime.Now.Ticks;
        }

        /// <summary>
        /// Starts, or resumes, measuring elapsed time for an interval.
        /// </summary>
        public void Start()
        {
            if (!this.isRunning)
            {
                this.startTimeStamp = GetTimestamp();
                this.isRunning = true;
            }
        }

        /// <summary>
        /// Stops measuring elapsed time for an interval.
        /// </summary>
        public void Stop()
        {
            if (this.isRunning)
            {
                var run = GetTimestamp() - this.startTimeStamp;
                this.elapsed += run;
                this.isRunning = false;
                if (this.elapsed < 0L)
                {
                    this.elapsed = 0L;
                }
            }
        }

        private long GetElapsedDateTimeTicks()
        {
            var elapsed = this.elapsed;
            if (this.isRunning)
            {
                long run = GetTimestamp() - this.startTimeStamp;
                elapsed += run;
            }
            return elapsed;
        }
    }
}
