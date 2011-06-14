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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    partial class LogSource
    {
        /// <summary>
        /// Provides an update context for changing the <see cref="LogSource"/> settings.
        /// </summary>
        protected internal class LogSourceUpdateContext : ILogSourceUpdateContext, ICommitable
        {
            private class TraceListenersReferenceCollection : ICollection<string>
            {
                private readonly List<string> list;
                private readonly ICollection<string> availableValues = new List<string>();

                public TraceListenersReferenceCollection(IEnumerable<string> currentValues, IEnumerable<string> availableValues)
                {
                    this.list = new List<string>(currentValues);
                    this.availableValues = new List<string>(availableValues);
                }

                public IEnumerator<string> GetEnumerator()
                {
                    return this.list.GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return this.GetEnumerator();
                }

                public void Add(string item)
                {
                    if (string.IsNullOrEmpty(item))
                        throw new ArgumentNullException("item");

                    this.EnsureCanAdd(item);
                    
                    if (this.list.Contains(item))
                        throw new ArgumentException("item");

                    this.list.Add(item);
                }

                private void EnsureCanAdd(string item)
                {
                    if (!this.availableValues.Contains(item))
                    {
                        throw new ArgumentException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Resources.TraceListenersReferenceCollection_InvalidName,
                                item),
                            "item");
                    }
                }

                public void Clear()
                {
                    this.list.Clear();
                }

                public bool Contains(string item)
                {
                    return this.list.Contains(item);
                }

                public void CopyTo(string[] array, int arrayIndex)
                {
                    this.list.CopyTo(array, arrayIndex);
                }

                public bool Remove(string item)
                {
                    return this.list.Remove(item);
                }

                public int Count
                {
                    get { return this.list.Count; }
                }

                public bool IsReadOnly
                {
                    get { return false; }
                }
            }

            private readonly LogSource logSource;
            private readonly IList<TraceListener> availableTraceListeners;

            /// <summary>
            /// Initializes a new instance of <see cref="LogSourceUpdateContext"/>/
            /// </summary>
            /// <param name="logSource">The <see cref="LogSource"/> being configured.</param>
            /// <param name="availableTraceListeners">A collection of all the available <see cref="TraceListener"/>s in the application.</param>
            public LogSourceUpdateContext(LogSource logSource, IEnumerable<TraceListener> availableTraceListeners)
            {
                this.logSource = logSource;
                this.availableTraceListeners = availableTraceListeners.ToList();
                this.Name = logSource.Name;
                this.Level = logSource.Level;
                this.AutoFlush = logSource.AutoFlush;
                this.Listeners = new TraceListenersReferenceCollection(logSource.Listeners.Select(x => x.Name), this.availableTraceListeners.Select(x => x.Name));
            }

            /// <summary>
            /// Gets the name for the <see cref="LogSource"/> instance being configured.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets or sets the <see cref="SourceLevels"/> values at which to trace for the <see cref="LogSource"/> instance being configured.
            /// </summary>
            public SourceLevels Level { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="AutoFlush"/> values for the <see cref="LogSource"/> instance being configured.
            /// </summary>
            public bool AutoFlush { get; set; }

            /// <summary>
            /// Gets a collection of configured <see cref="TraceListener"/>s for the <see cref="LogSource"/> instance. This collection can be updated.
            /// </summary>
            public ICollection<string> Listeners { get; private set; }

            /// <summary>
            /// Gets the <see cref="LogSource"/> being configured.
            /// </summary>
            protected LogSource LogSource
            {
                get { return this.logSource; }
            }

            /// <summary>
            /// Applies the changes.
            /// </summary>
            protected internal virtual void ApplyChanges()
            {
                this.LogSource.Level = this.Level;
                this.LogSource.AutoFlush = this.AutoFlush;

                var newlist = this.LogSource.Listeners.Where(x => this.Listeners.Contains(x.Name)).ToList();
                foreach (var listenerName in this.Listeners.Except(newlist.Select(x => x.Name)))
                {
                    newlist.Add(this.availableTraceListeners.First(x => x.Name == listenerName));
                }
                
                this.LogSource.Listeners = newlist;
            }

            void ICommitable.Commit()
            {
                this.ApplyChanges();
            }
        }

        /// <summary>
        /// Provides an update context to batch change requests to the <see cref="LogSource"/> configuration.
        /// </summary>
        /// <param name="availableTraceListeners">The available trace listeners to be used.</param>
        /// <returns>Returns an <see cref="ILogSourceUpdateContext"/> instance that can be used to apply the configuration changes.</returns>
        protected internal virtual ILogSourceUpdateContext GetUpdateContext(IEnumerable<TraceListener> availableTraceListeners)
        {
            return new LogSourceUpdateContext(this, availableTraceListeners);
        }
    }
}
