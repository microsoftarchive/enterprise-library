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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{

    /// <summary>
    /// Base Class for extension points that configure Trace Listeners within Category Sources.
    /// </summary>
    public abstract class SendToTraceListenerExtension : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd
    {
        readonly ILoggingConfigurationSendToExtension extensionContext;

        /// <summary>
        /// Creates a new instance of <see cref="SendToTraceListenerExtension"/>.
        /// </summary>
        /// <param name="context">The fluent interface extesion used to configure Trace Listeners. <br/>
        /// Must implement <see cref="ILoggingConfigurationSendToExtension"/>.
        /// </param>
        protected SendToTraceListenerExtension(ILoggingConfigurationSendTo context)
        {
            extensionContext = context as ILoggingConfigurationSendToExtension;

            if (extensionContext == null) throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, Resources.ParameterMustImplementType, typeof(ILoggingConfigurationSendToExtension)),
                "context");
        }

        /// <summary>
        /// Adds a <see cref="TraceListenerData"/> instance to the logging settings and adds a <see cref="TraceListenerReferenceData"/> to the current Category Source.
        /// </summary>
        /// <param name="traceListenerData">The <see cref="TraceListenerData"/> that sould be added to configuration.</param>
        protected void AddTraceListenerToSettingsAndCategory(TraceListenerData traceListenerData)
        {
            extensionContext.LoggingSettings.TraceListeners.Add(traceListenerData);
            extensionContext.CurrentTraceSource.TraceListeners.Add(new TraceListenerReferenceData(traceListenerData.Name));
        }

        /// <summary>
        /// The <see cref="LoggingSettings"/> Configuration Section that is currently being build.
        /// </summary>
        public LoggingSettings LoggingSettings
        {
            get { return extensionContext.LoggingSettings; }
        }

        /// <summary>
        /// The <see cref="TraceSourceData"/> Configuration Section that is currently being build.
        /// </summary>
        public TraceSourceData CurrentTraceSource
        {
            get { return extensionContext.CurrentTraceSource; }
        }

        ILoggingConfigurationSendTo ILoggingConfigurationCategoryContd.SendTo
        {
            get
            {
                return extensionContext.LoggingCategoryContd.SendTo;
            }
        }

        ILoggingConfigurationCustomCategoryStart ILoggingConfigurationContd.LogToCategoryNamed(string categoryName)
        {
            return extensionContext.LoggingCategoryContd.LogToCategoryNamed(categoryName);
        }

        ILoggingConfigurationSpecialSources ILoggingConfigurationContd.SpecialSources
        {
            get { return extensionContext.LoggingCategoryContd.SpecialSources; }
        }
    }
}
