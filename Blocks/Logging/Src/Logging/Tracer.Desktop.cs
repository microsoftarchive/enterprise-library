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
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    partial class Tracer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        public Tracer(string operation)
            : this(operation, Logger.Writer, GetTracerInstrumentationProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// The activity id will override a previous activity id
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        public Tracer(string operation, Guid activityId)
            : this(operation, activityId, Logger.Writer, GetTracerInstrumentationProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="instrumentationConfiguration">The configuration source that is used to determine instrumentation should be enabled.</param>
        public Tracer(string operation, LogWriter writer, IConfigurationSource instrumentationConfiguration) :
            this(operation, writer, GetTracerInstrumentationProvider(instrumentationConfiguration))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical name.
        /// It retrieves require dependent objects from the given <paramref name="container"/>.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="container"><see cref="IServiceLocator"/> used to retrieve dependent objects.</param>
        public Tracer(string operation, IServiceLocator container)
            : this(operation, container.GetInstance<LogWriter>(), container.GetInstance<ITracerInstrumentationProvider>())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// The activity id will override a previous activity id
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="instrumentationConfiguration">configuration source that is used to determine instrumentation should be enabled</param>
        public Tracer(string operation, Guid activityId, LogWriter writer, IConfigurationSource instrumentationConfiguration) :
            this(operation, activityId, writer, GetTracerInstrumentationProvider(instrumentationConfiguration))
        {
        }

        internal static bool IsTracingAvailable()
        {
            bool tracingAvailable = false;

            try
            {
                tracingAvailable = SecurityManager.IsGranted(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
            }
            catch (SecurityException)
            { }

            return tracingAvailable;
        }

        private static ITracerInstrumentationProvider GetTracerInstrumentationProvider()
        {
            return EnterpriseLibraryContainer.Current.GetInstance<ITracerInstrumentationProvider>();
        }

        private static ITracerInstrumentationProvider GetTracerInstrumentationProvider(IServiceLocator serviceLocator)
        {
            if (serviceLocator == null) throw new ArgumentNullException("serviceLocator");
            return serviceLocator.GetInstance<ITracerInstrumentationProvider>();
        }

        private static ITracerInstrumentationProvider GetTracerInstrumentationProvider(IConfigurationSource configuration)
        {
            if (configuration == null) return new NullTracerInstrumentationProvider();

            var container = EnterpriseLibraryContainer.CreateDefaultContainer(configuration);
            return container.GetInstance<ITracerInstrumentationProvider>();
        }

        private LogWriter GetWriter()
        {
            return writer ?? Logger.Writer;
        }
    }
}
