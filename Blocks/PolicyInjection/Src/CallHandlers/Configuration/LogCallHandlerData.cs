//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration
{
    /// <summary>
    /// A configuration element for the data for the LogCallHandler.
    /// </summary>
    [Assembler(typeof(LogCallHandlerAssembler))]
    public class LogCallHandlerData : CallHandlerData
    {
        private const string LogBehaviorPropertyName = "logBehavior";
        private const string CategoriesPropertyName = "categories";
        private const string BeforeMessagePropertyName = "beforeMessage";
        private const string AfterMessagePropertyName = "afterMessage";
        private const string EventIdPropertyName = "eventId";
        private const string IncludeParameterValuesPropertyName = "includeParameterValues";
        private const string IncludeCallStackPropertyName = "includeCallStack";
        private const string IncludeCallTimePropertyName = "includeCallTime";
        private const string PriorityPropertyName = "priority";
        private const string SeverityPropertyName = "severity";

        /// <summary>
        /// Construct a new <see cref="LogCallHandlerData"/> instance.
        /// </summary>
        public LogCallHandlerData()
        {
        }

        /// <summary>
        /// Construct a new <see cref="LogCallHandlerData"/> instance.
        /// </summary>
        /// <param name="handlerName">Handler name</param>
        public LogCallHandlerData(string handlerName)
            :base(handlerName, typeof(LogCallHandler))
        {
        }

        /// <summary>
        /// Construct a new <see cref="LogCallHandlerData"/> instance.
        /// </summary>
        /// <param name="handlerName">Handler name</param>
        /// <param name="handlerOrder">Handler order</param>
        public LogCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(LogCallHandler))
        {
            this.Order = handlerOrder;
        }

        /// <summary>
        /// Should the handler log before the call, after the call, or both?
        /// </summary>
        /// <value>The "logBehavior" config attribute.</value>
        [ConfigurationProperty(LogBehaviorPropertyName, DefaultValue = HandlerLogBehavior.BeforeAndAfter)]
        public HandlerLogBehavior LogBehavior
        {
            get { return (HandlerLogBehavior)base[LogBehaviorPropertyName]; }
            set { base[LogBehaviorPropertyName] = value; }
        }

        /// <summary>
        /// Collection of log categories to use in the log message.
        /// </summary>
        /// <value>The "categories" nested element.</value>
        [ConfigurationProperty(CategoriesPropertyName)]
        [ConfigurationCollectionAttribute(typeof(NamedElementCollection<LogCallHandlerCategoryEntry>))]
        public NamedElementCollection<LogCallHandlerCategoryEntry> Categories
        {
            get { return (NamedElementCollection<LogCallHandlerCategoryEntry>)base[CategoriesPropertyName]; }
            set { base[CategoriesPropertyName] = value; }
        }

        /// <summary>
        /// Message for the log entry in before-call logging.
        /// </summary>
        /// <value>The "beforeMessage" config attribute.</value>
        [ConfigurationProperty(BeforeMessagePropertyName, DefaultValue = "")]
        public string BeforeMessage
        {
            get { return (string)base[BeforeMessagePropertyName]; }
            set { base[BeforeMessagePropertyName] = value; }
        }

        /// <summary>
        /// Message for the log entry in after-call logging.
        /// </summary>
        /// <value>The "afterMessage" config attribute.</value>
        [ConfigurationProperty(AfterMessagePropertyName, DefaultValue = "")]
        public string AfterMessage
        {
            get { return (string)base[AfterMessagePropertyName]; }
            set { base[AfterMessagePropertyName] = value; }
        }

        /// <summary>
        /// Event Id to put into log entries.
        /// </summary>
        /// <value>The "eventId" config attribute.</value>
        [ConfigurationProperty(EventIdPropertyName, DefaultValue = LogCallHandlerDefaults.EventId)]
        public int EventId
        {
            get { return (int)base[EventIdPropertyName]; }
            set { base[EventIdPropertyName] = value;  }
        }

        /// <summary>
        /// Include parameter values and return values in the log entry
        /// </summary>
        /// <value>The "includeParameterValues" config attribute.</value>
        [ConfigurationProperty(IncludeParameterValuesPropertyName, DefaultValue = LogCallHandlerDefaults.IncludeParameters)]
        public bool IncludeParameterValues
        {
            get { return (bool)base[IncludeParameterValuesPropertyName]; }
            set { base[IncludeParameterValuesPropertyName] = value; }
        }

        /// <summary>
        /// Include the call stack in the log entries.
        /// </summary>
        /// <remarks>Setting this to true requires UnmanagedCode permissions.</remarks>
        /// <value>The "includeCallStack" config attribute.</value>
        [ConfigurationProperty(IncludeCallStackPropertyName, DefaultValue = LogCallHandlerDefaults.IncludeCallStack)]
        public bool IncludeCallStack
        {
            get { return (bool)base[IncludeCallStackPropertyName]; }
            set { base[IncludeCallStackPropertyName] = value; }
        }

        /// <summary>
        /// Include the time to execute the call in the log entries.
        /// </summary>
        /// <value>The "includeCallTime" config attribute.</value>
        [ConfigurationProperty(IncludeCallTimePropertyName, DefaultValue = LogCallHandlerDefaults.IncludeCallTime)]
        public bool IncludeCallTime
        {
            get { return (bool)base[IncludeCallTimePropertyName]; }
            set { base[IncludeCallTimePropertyName] = value; }
        }

        /// <summary>
        /// The priority of the log entries.
        /// </summary>
        /// <value>the "priority" config attribute.</value>
        [ConfigurationProperty(PriorityPropertyName, DefaultValue = LogCallHandlerDefaults.Priority)]
        public int Priority
        {
            get { return (int)base[PriorityPropertyName]; }
            set { base[PriorityPropertyName] = value; }
        }

        /// <summary>
        /// The severity of the log entry.
        /// </summary>
        /// <value>the "severity" config attribute.</value>
        [ConfigurationProperty(SeverityPropertyName, DefaultValue = LogCallHandlerDefaults.Severity, IsRequired = false)]
        public TraceEventType Severity
        {
            get { return (TraceEventType)base[SeverityPropertyName]; }
            set { base[SeverityPropertyName] = value; }
        }
 
    }

    /// <summary>
    /// This enum control when the logging call handler will add log entries.
    /// </summary>
    public enum HandlerLogBehavior
    {
        /// <summary>
        /// Log both before and after the call.
        /// </summary>
        BeforeAndAfter =0,
        /// <summary>
        /// Log only before the call.
        /// </summary>
        Before =1,
        /// <summary>
        /// Log only after the call.
        /// </summary>
        After =2
    }

    /// <summary>
    /// A class used by ObjectBuilder to construct a <see cref="LogCallHandler"/> from
    /// a <see cref="LogCallHandlerData"/> instance.
    /// </summary>
    public class LogCallHandlerAssembler : IAssembler<ICallHandler, CallHandlerData>
    {
        /// <summary>
        /// Builds an instance of the subtype of <typeparamref name="TObject"/> type the receiver knows how to build,  based on 
        /// an a configuration object.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the <typeparamref name="TObject"/> subtype.</returns>
        public ICallHandler Assemble(IBuilderContext context, CallHandlerData objectConfiguration,
                                     IConfigurationSource configurationSource,
                                     ConfigurationReflectionCache reflectionCache)
        {
            LogCallHandlerData handlerData = (LogCallHandlerData) objectConfiguration;
            LogCallHandler callHandler = new LogCallHandler(configurationSource);
            switch (handlerData.LogBehavior)
            {
                case HandlerLogBehavior.Before:
                    callHandler.LogBeforeCall = true;
                    callHandler.LogAfterCall = false;
                    break;

                case HandlerLogBehavior.After:
                    callHandler.LogBeforeCall = false;
                    callHandler.LogAfterCall = true;
                    break;

                case HandlerLogBehavior.BeforeAndAfter:
                    callHandler.LogBeforeCall = true;
                    callHandler.LogAfterCall = true;
                    break;
            }

            callHandler.Order = handlerData.Order;
            callHandler.BeforeMessage = handlerData.BeforeMessage;
            callHandler.AfterMessage = handlerData.AfterMessage;
            callHandler.EventId = handlerData.EventId;
            callHandler.IncludeCallStack = handlerData.IncludeCallStack;
            callHandler.IncludeCallTime = handlerData.IncludeCallTime;
            callHandler.IncludeParameters = handlerData.IncludeParameterValues;
            callHandler.Priority = handlerData.Priority;
            callHandler.Severity = handlerData.Severity;
            callHandler.Categories.Clear();

            handlerData.Categories.ForEach(
                delegate(LogCallHandlerCategoryEntry entry)
                {
                    callHandler.Categories.Add(entry.Name);
                }
            );

            return callHandler;
        }
    }
}
