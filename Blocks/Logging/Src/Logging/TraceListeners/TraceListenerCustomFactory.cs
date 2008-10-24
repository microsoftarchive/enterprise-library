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
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="TraceListener"/> described by a <see cref="TraceListenerData"/> configuration object.
	/// </summary>
    public class TraceListenerCustomFactory : AssemblerBasedCustomFactory<TraceListener, TraceListenerData>
    {
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public static TraceListenerCustomFactory Instance = new TraceListenerCustomFactory();

        /// <summary>
        /// Returns a new <see cref="TraceListenerCache"/> instance.
        /// </summary>
        /// <param name="size">The initial size for the new cache.</param>
        /// <returns>A new trace listener cache.</returns>
        public static TraceListenerCache CreateTraceListenerCache(int size)
        {
            return new TraceListenerCache(size);
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="TraceListener"/> based on an instance of a subclass of <see cref="TraceListenerData"/>.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="TraceListener"/>.</returns>
        public override TraceListener Create(IBuilderContext context, TraceListenerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            TraceListener createdObject = base.Create(context, objectConfiguration, configurationSource, reflectionCache);
            createdObject.Name = objectConfiguration.Name;
            createdObject.TraceOutputOptions = objectConfiguration.TraceOutputOptions;

            if (objectConfiguration.Filter != SourceLevels.All)
            {
                createdObject.Filter = new EventTypeFilter(objectConfiguration.Filter);
            }

            return createdObject;
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="TraceListener"/> based on the configuration found for the given name in the configuration source 
        /// if an instance for the name is not found in the cache, returns the existing instance from the cache otherwise.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="name">The name of the instance to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <param name="traceListenersCache">The cache for trace listener instances</param>
        /// <returns>A fully initialized instance of <see cref="TraceListener"/>.</returns>
        /// <exception cref="ConfigurationErrorsException"><paramref name="configurationSource"/> does not contain 
        /// logging settings, or the <paramref name="name"/> does not exist in the logging settings.</exception>
        public TraceListener Create(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache, TraceListenerCache traceListenersCache)
        {
            TraceListener createdObject;

            if (!traceListenersCache.cache.TryGetValue(name, out createdObject))
            {
                createdObject = Create(context, name, configurationSource, reflectionCache);
                traceListenersCache.cache.Add(name, createdObject);
            }

            return createdObject;
        }

        /// <summary>
        /// Returns the configuration object that represents the named <see cref="TraceListener"/> instance in the configuration source.
        /// </summary>
        /// <param name="name">The name of the required instance.</param>
        /// <param name="configurationSource">The configuration source where to look for the configuration object.</param>
        /// <returns>The configuration object that represents the instance with name <paramref name="name"/> in the logging 
        /// configuration section from <paramref name="configurationSource"/></returns>
        /// <exception cref="ConfigurationErrorsException"><paramref name="configurationSource"/> does not contain 
        /// logging settings, or the <paramref name="name"/> does not exist in the logging settings.</exception>
        protected override TraceListenerData GetConfiguration(string name, IConfigurationSource configurationSource)
        {
            LoggingSettings settings = LoggingSettings.GetLoggingSettings(configurationSource);
            ValidateSettings(settings);

            TraceListenerData objectConfiguration = settings.TraceListeners.Get(name);
            ValidateConfiguration(objectConfiguration, name);

            return objectConfiguration;
        }

        private void ValidateConfiguration(TraceListenerData objectConfiguration, string name)
        {
            if (objectConfiguration == null)
            {
                throw new ConfigurationErrorsException(
                    string.Format(
                        Resources.Culture,
                        Resources.ExceptionTraceListenerConfigurationNotFound,
                        name));
            }
        }

        private void ValidateSettings(LoggingSettings settings)
        {
            if (settings == null)
            {
                throw new ConfigurationErrorsException(Resources.ExceptionLoggingSectionNotFound);
            }
        }

        /// <summary>
        /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Holds a cache of already created <see cref="TraceListener"/>s.
        /// </summary>
        public struct TraceListenerCache
        {
            internal Dictionary<string, TraceListener> cache;

            internal TraceListenerCache(int size)
            {
                cache = new Dictionary<string, TraceListener>(size);
            }
        }
    }
}
