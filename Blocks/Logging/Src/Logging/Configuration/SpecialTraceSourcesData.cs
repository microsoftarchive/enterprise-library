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
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings that describe the "all events", "not processed" and "errors" <see cref="LogSource"/>s
	/// for a <see cref="LogWriter"/>.
	/// </summary>
	public class SpecialTraceSourcesData : ConfigurationElement
	{
		private const string mandatoryTraceSourceProperty = "allEvents";
		private const string notProcessedTraceSourceProperty = "notProcessed";
		private const string errorsTraceSourceProperty = "errors";

		/// <summary>
		/// Initializes a new instance of <see cref="SpecialTraceSourcesData"/> with default values.
		/// </summary>
		public SpecialTraceSourcesData()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="SpecialTraceSourcesData"/>.
		/// </summary>
		/// <param name="mandatory">The configuration for the optional trace source to send all messages received.</param>
		/// <param name="notProcessed">The configuration for the optional to send messages with unknown categories.</param>
		/// <param name="errors">The configuration for the mandatory trace source to log processing errors.</param>
		public SpecialTraceSourcesData(TraceSourceData mandatory, TraceSourceData notProcessed, TraceSourceData errors)
		{
			this.AllEventsTraceSource = mandatory;
			this.NotProcessedTraceSource = notProcessed;
			this.ErrorsTraceSource = errors;
		}

		/// <summary>
		/// Gets or sets the configuration for the optional trace source to send all messages received.
		/// </summary>
		[ConfigurationProperty(mandatoryTraceSourceProperty, IsRequired=false)]
		public TraceSourceData AllEventsTraceSource
		{
			get { return (TraceSourceData)base[mandatoryTraceSourceProperty]; }
			set { base[mandatoryTraceSourceProperty] = value; }
		}

		/// <summary>
		/// Gets or sets the configuration for the optional to send messages with unknown categories.
		/// </summary>
		[ConfigurationProperty(notProcessedTraceSourceProperty, IsRequired = false)]
		public TraceSourceData NotProcessedTraceSource
		{
			get { return (TraceSourceData)base[notProcessedTraceSourceProperty]; }
			set { base[notProcessedTraceSourceProperty] = value; }
		}

		/// <summary>
		/// Gets or sets the configuration for the mandatory trace source to log processing errors.
		/// </summary>
		[ConfigurationProperty(errorsTraceSourceProperty, IsRequired = true)]
		public TraceSourceData ErrorsTraceSource
		{
			get { return (TraceSourceData)base[errorsTraceSourceProperty]; }
			set { base[errorsTraceSourceProperty] = value; }
		}
	}
}
