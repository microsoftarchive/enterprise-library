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

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="LogSource"/> described by a <see cref="TraceSourceData"/> configuration object.
	/// </summary>
	public class LogSourceCustomFactory
	{
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
        public static LogSourceCustomFactory Instance = new LogSourceCustomFactory();

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="LogSource"/> based on an instance of <see cref="TraceSourceData"/>.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <param name="traceListenersCache">The cache of already built trace listeners, used to share trace listeners across <see cref="LogSource"/> instances.</param>
		/// <returns>A fully initialized instance of <see cref="LogSource"/>.</returns>
		public LogSource Create(IBuilderContext context, TraceSourceData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache, TraceListenerCustomFactory.TraceListenerCache traceListenersCache)
		{
            List<TraceListener> traceListeners = new List<TraceListener>(objectConfiguration.TraceListeners.Count);

			foreach (TraceListenerReferenceData traceListenerReference in objectConfiguration.TraceListeners)
			{
				TraceListener traceListener
					= TraceListenerCustomFactory.Instance.Create(context, traceListenerReference.Name, configurationSource, reflectionCache, traceListenersCache);

                traceListeners.Add(traceListener);
			}

            LogSource createdObject
                = new LogSource(objectConfiguration.Name, traceListeners, objectConfiguration.DefaultLevel, objectConfiguration.AutoFlush);

            InstrumentationAttachmentStrategy instrumentationAttacher = new InstrumentationAttachmentStrategy();
            instrumentationAttacher.AttachInstrumentation(createdObject, configurationSource, reflectionCache);            

			return createdObject;
		}
	}
}
