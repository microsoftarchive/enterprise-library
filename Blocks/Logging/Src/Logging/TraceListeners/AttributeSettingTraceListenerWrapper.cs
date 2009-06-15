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
using System.Collections.Specialized;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Wraps a <see cref="TraceListener"/> to allow the attribute properties to
    /// be injected.  This is primarily used with custom trace listeners that
    /// provide attributes in their configuration.
    /// </summary>
    public class AttributeSettingTraceListenerWrapper : TraceListenerWrapper
    {
        ///<summary>
        /// Initializes an instance of <see cref="AttributeSettingTraceListenerWrapper"/>.
        ///</summary>
        ///<param name="listener">The <see cref="TraceListener"/> to wrap.</param>
        ///<param name="attributes">The attributes to set on the trace listener.</param>
        public AttributeSettingTraceListenerWrapper(
            TraceListener listener,
            NameValueCollection attributes)
        {
            this.innerTraceListener = listener;

            foreach (string key in attributes)
            {
                this.InnerTraceListener.Attributes.Add(key, attributes[key]);
            }

            this.TraceOutputOptions = this.InnerTraceListener.TraceOutputOptions;
            this.IndentLevel = this.InnerTraceListener.IndentLevel;
            this.IndentSize = this.InnerTraceListener.IndentSize;
            this.Filter = this.InnerTraceListener.Filter;
        }

        private TraceListener innerTraceListener;

        /// <summary>
        /// Gets the wrapped <see cref="TraceListener"/>.
        /// </summary>
        public override TraceListener InnerTraceListener
        {
            get { return this.innerTraceListener; }
        }

        /// <summary>
        /// Gets or sets a name for this <see cref="T:System.Diagnostics.TraceListener"/>.
        /// </summary>
        /// <returns>
        /// A name for this <see cref="T:System.Diagnostics.TraceListener"/>. The default is an empty string ("").
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string Name
        {
            get
            {
                return this.InnerTraceListener.Name;
            }
            set
            {
                this.InnerTraceListener.Name = value;
            }
        }
    }
}
