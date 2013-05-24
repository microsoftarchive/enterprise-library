//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Factory for <see cref="ExceptionPolicyDefinition"/> objects. This class is responsible for creating all the internal
    /// classes needed to implement a <see cref="ExceptionPolicyDefinition" />.
    /// </summary>
    public class ExceptionPolicyFactory
    {
        private readonly ExceptionPolicyConfigurationBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionPolicyDefinition"/> class with the default <see cref="IConfigurationSource"/> instance.
        /// </summary>
        public ExceptionPolicyFactory()
            : this(s => (ConfigurationSection)ConfigurationManager.GetSection(s))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionPolicyDefinition"/> class with the specified <see cref="IConfigurationSource"/> instance.
        /// </summary>
        /// <param name="configurationSource">The source for configuration information.</param>
        public ExceptionPolicyFactory(IConfigurationSource configurationSource)
        {
            Guard.ArgumentNotNull(configurationSource, "configurationSource");

            this.builder = new ExceptionPolicyConfigurationBuilder(configurationSource.GetSection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionPolicyDefinition"/> class with a configuration accessor.
        /// </summary>
        /// <param name="configurationAccessor">The source for configuration information.</param>
        public ExceptionPolicyFactory(Func<string, ConfigurationSection> configurationAccessor)
        {
            Guard.ArgumentNotNull(configurationAccessor, "configurationAccessor");

            this.builder = new ExceptionPolicyConfigurationBuilder(configurationAccessor);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ExceptionPolicyDefinition"/> class based on the information in the configuration section.
        /// </summary>
        /// <param name="name">The name of the required instance.</param>
        /// <returns>The created <see cref="ExceptionPolicyDefinition"/> object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The configuration section does not exist or cannot be deserialized, or there are no settings for <paramref name="name"/>.</exception>
        public ExceptionPolicyDefinition CreatePolicy(string name)
        {
            return this.builder.CreatePolicy(name);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ExceptionManager"/> class based on the information in the configuration section.
        /// </summary>
        /// <returns>The created <see cref="ExceptionManager"/> object.</returns>
        /// <exception cref="InvalidOperationException">The configuration section does not exist or cannot be deserialized.</exception>
        public ExceptionManager CreateManager()
        {
            return this.builder.CreateManager();
        }

        /// <summary>
        /// Initializes the <see cref="ExceptionPolicy"/> class with a new <see cref="ExceptionManager"/> instance created based on the information in the configuration section.
        /// </summary>
        public void InitializeExceptionPolicy()
        {
            ExceptionPolicy.SetExceptionManager(this.CreateManager());
        }

        private class ExceptionPolicyConfigurationBuilder
        {
            private readonly Func<string, ConfigurationSection> configurationAccessor;

            public ExceptionPolicyConfigurationBuilder(Func<string, ConfigurationSection> configurationAccessor)
            {
                Guard.ArgumentNotNull(configurationAccessor, "configurationAccessor");

                this.configurationAccessor = configurationAccessor;
            }

            public ExceptionPolicyDefinition CreatePolicy(string name)
            {
                Guard.ArgumentNotNull(name, "name");

                try
                {
                    var settings = this.GetSettings();

                    if (settings == null)
                    {
                        throw new InvalidOperationException(Resources.ExceptionExceptionHandlingSectionNotFound);
                    }

                    return settings.BuildExceptionPolicy(name);
                }
                catch (ConfigurationErrorsException e)
                {
                    throw new InvalidOperationException(Resources.ExceptionCannotRetrieveConfiguration, e);
                }
            }

            public ExceptionManager CreateManager()
            {
                try
                {
                    var settings = this.GetSettings();

                    if (settings == null)
                    {
                        throw new InvalidOperationException(Resources.ExceptionExceptionHandlingSectionNotFound);
                    }

                    return settings.BuildExceptionManager();
                }
                catch (ConfigurationErrorsException e)
                {
                    throw new InvalidOperationException(Resources.ExceptionCannotRetrieveConfiguration, e);
                }
            }

            private ExceptionHandlingSettings GetSettings()
            {
                return (ExceptionHandlingSettings)this.configurationAccessor(ExceptionHandlingSettings.SectionName);
            }
        }
    }
}