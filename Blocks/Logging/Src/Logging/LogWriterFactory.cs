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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Factory to create <see cref="LogWriter"/> instances.
    /// </summary>
    public class LogWriterFactory
    {
        private readonly LogWriterConfigurationBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class with the default <see cref="IConfigurationSource"/> instance.
        /// </summary>
        public LogWriterFactory()
            : this(s => (ConfigurationSection)ConfigurationManager.GetSection(s))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class with a <see cref="IConfigurationSource"/> instance.
        /// </summary>
        /// <param name="configurationSource">The source for configuration information.</param>
        public LogWriterFactory(IConfigurationSource configurationSource)
        {
            Guard.ArgumentNotNull(configurationSource, "configurationSource");

            this.builder = new LogWriterConfigurationBuilder(configurationSource.GetSection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogWriter"/> class with a configuration accessor.
        /// </summary>
        /// <param name="configurationAccessor">The source for configuration information.</param>
        public LogWriterFactory(Func<string, ConfigurationSection> configurationAccessor)
        {
            Guard.ArgumentNotNull(configurationAccessor, "configurationAccessor");

            this.builder = new LogWriterConfigurationBuilder(configurationAccessor);
        }

        /// <summary>
        /// Creates a new instance of <see cref="LogWriter"/> based on the configuration in the <see cref="IConfigurationSource"/> 
        /// instance of the factory.
        /// </summary>
        /// <returns>The created <see cref="LogWriter"/> object.</returns>
        public LogWriter Create()
        {
            return this.builder.Create();
        }

        private class LogWriterConfigurationBuilder
        {
            private readonly LoggingSettings settings;

            public LogWriterConfigurationBuilder(Func<string, ConfigurationSection> configurationAccessor)
            {
                Guard.ArgumentNotNull(configurationAccessor, "configurationAccessor");

                this.settings = (LoggingSettings)configurationAccessor(LoggingSettings.SectionName);
            }

            public LogWriter Create()
            {
                if (this.settings == null)
                {
                    throw new InvalidOperationException(Resources.ExceptionLoggingSectionNotFound);
                }

                return this.settings.BuildLogWriter();
            }
        }
    }
}
