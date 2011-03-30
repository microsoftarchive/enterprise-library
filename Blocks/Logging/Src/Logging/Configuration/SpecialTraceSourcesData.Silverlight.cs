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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe the "all events", "not processed" and "errors" <see cref="LogSource"/>s
    /// for a <see cref="LogWriter"/>.
    /// </summary>
    public partial class SpecialTraceSourcesData
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SpecialTraceSourcesData"/> with default values.
        /// </summary>
        public SpecialTraceSourcesData()
        {
            this.AllEventsTraceSource = new TraceSourceData { Name = "Name" };
            this.NotProcessedTraceSource = new TraceSourceData { Name = "Name" };
            this.ErrorsTraceSource = new TraceSourceData { Name = "Name" };
        }

        /// <summary>
        /// Gets or sets the configuration for the optional trace source to send all messages received.
        /// </summary>
        public TraceSourceData AllEventsTraceSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the configuration for the optional to send messages with unknown categories.
        /// </summary>
        public TraceSourceData NotProcessedTraceSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the configuration for the mandatory trace source to log processing errors.
        /// </summary>
        public TraceSourceData ErrorsTraceSource
        {
            get;
            set;
        }
    }
}
