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
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration for a <see cref="TraceListener"/>.
	/// </summary>
	/// <remarks>
	/// Since trace listeners are not under our control, the building mechanism can't rely 
	/// on annotations to the trace listeners to determine the concrete <see cref="TraceListenerData"/> subtype 
	/// when deserializing. Because of this, the schema for <see cref="TraceListenerData"/> includes the actual 
	/// type of the instance to build.
	/// </remarks>
	public class TraceListenerData : NameTypeConfigurationElement
	{
        private AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

		/// <summary>
		/// Name of the property that holds the type for a <see cref="TraceListenerData"/>.
		/// </summary>
		/// <remarks>
		/// This property will hold the type of the object it holds it. However, it's used during the 
		/// deserialization process when the actual type of configuration element to create has to be determined.
		/// </remarks>
		protected internal const string listenerDataTypeProperty = "listenerDataType";
		
		/// <summary>
		/// Name of the property that holds the <see cref="TraceOptions"/> of a <see cref="TraceListenerData"/>.
		/// </summary>
		protected internal const string traceOutputOptionsProperty = "traceOutputOptions";

        /// <summary>
        /// Name of the property that holds the Filter of a <see cref="TraceListenerData"/>
        /// </summary>
        protected internal const string filterProperty = "filter";

		private static IDictionary<string, string> emptyAttributes = new Dictionary<string, string>(0);

		/// <summary>
		/// Initializes an instance of the <see cref="TraceListenerData"/> class.
		/// </summary>
		public TraceListenerData()
		{
		}

		/// <summary>
		/// Initializes an instance of <see cref="TraceListenerData"/> with a name and <see cref="TraceOptions"/> for 
		/// a TraceListenerType.
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		/// <param name="traceListenerType">The trace listener type.</param>
		/// <param name="traceOutputOptions">The trace options.</param>
		protected TraceListenerData(string name, Type traceListenerType, TraceOptions traceOutputOptions)
			: base(name, traceListenerType)
		{
			this.ListenerDataType = this.GetType();
			this.TraceOutputOptions = traceOutputOptions;
		}

        /// <summary>
        /// Initializes an instance of <see cref="TraceListenerData"/> with a name, a <see cref="TraceOptions"/> for 
        /// a TraceListenerType and a <see cref="SourceLevels"/> for a Filter.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        /// <param name="traceListenerType">The trace listener type.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter.</param>
        protected TraceListenerData(string name, Type traceListenerType, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, traceListenerType)
        {
            this.ListenerDataType = this.GetType();
            this.TraceOutputOptions = traceOutputOptions;
            this.Filter = filter;
        }

		/// <summary>
		/// Gets or sets the type of the actual <see cref="TraceListenerData"/> type.
		/// </summary>
		/// <remarks>
		/// Should match the this.GetType().
		/// </remarks>
        public Type ListenerDataType
        {
            get { return (Type)typeConverter.ConvertFrom(ListenerDataTypeName); }
            set { ListenerDataTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the actual <see cref="TraceListenerData"/> type.
        /// </summary>
        /// <value>
        /// the fully qualified name of the actual <see cref="TraceListenerData"/> type.
        /// </value>
        [ConfigurationProperty(listenerDataTypeProperty, IsRequired = true)]
        public string ListenerDataTypeName
        {
            get { return (string)this[listenerDataTypeProperty]; }
            set { this[listenerDataTypeProperty] = value; }
        }
		/// <summary>
		/// Gets or sets the <see cref="TraceOptions"/> for the represented <see cref="TraceListener"/>.
		/// </summary>
		[ConfigurationProperty(traceOutputOptionsProperty, IsRequired= false)]
		public TraceOptions TraceOutputOptions
		{
			get
			{
				return (TraceOptions)this[traceOutputOptionsProperty];
			}
			set
			{
				this[traceOutputOptionsProperty] = value;
			}
		}

        /// <summary>
        /// Gets or sets the <see cref="Filter"/> for the represented <see cref="TraceListener"/>
        /// </summary>
        [ConfigurationProperty(filterProperty, IsRequired = false, DefaultValue=SourceLevels.All)]
        public SourceLevels Filter
        {
            get { return (SourceLevels)this[filterProperty]; }
            set { this[filterProperty] = value; }
        }
	}
}
