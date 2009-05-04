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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="LogEnabledFilter"/>.
    /// </summary>
    [Assembler(typeof(LogEnabledFilterAssembler))]
    public class LogEnabledFilterData : LogFilterData
    {
        private const string enabledProperty = "enabled";

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="LogEnabledFilterData"/> class.</para>
        /// </summary>
        public LogEnabledFilterData()
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="LogEnabledFilterData"/> class.</para>
        /// </summary>
        /// <param name="enabled">True if logging should be enabled.</param>
        public LogEnabledFilterData(bool enabled)
            : this("enabled", enabled)
        {
        }

        /// <summary>
        /// <para>Initialize a new named instance of the <see cref="LogEnabledFilterData"/> class.</para>
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="enabled">True if logging should be enabled.</param>
        public LogEnabledFilterData(string name, bool enabled)
            : base(name, typeof(LogEnabledFilter))
        {
            this.Enabled = enabled;
        }


        /// <summary>
        /// Gets or sets the enabled value.
        /// </summary>
        [ConfigurationProperty(enabledProperty, IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)base[enabledProperty]; }
            set { base[enabledProperty] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TypeRegistration GetContainerConfigurationModel()
        {
            return
                new TypeRegistration<ILogFilter>(() => new LogEnabledFilter(this.Name, this.Enabled))
                {
                    Name = this.Name
                };
        }
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a <see cref="LogEnabledFilter"/> described by a <see cref="LogEnabledFilterData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="LogEnabledFilterData"/> type and it is used by the <see cref="LogFilterCustomFactory"/> 
    /// to build the specific <see cref="ILogFilter"/> object represented by the configuration object.
    /// </remarks>	
    public class LogEnabledFilterAssembler : IAssembler<ILogFilter, LogFilterData>
    {
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="LogEnabledFilter"/> based on an instance of <see cref="LogEnabledFilterData"/>.
        /// </summary>
        /// <seealso cref="LogFilterCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="LogEnabledFilterData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="LogEnabledFilter"/>.</returns>
        public ILogFilter Assemble(IBuilderContext context, LogFilterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            LogEnabledFilterData castedObjectConfiguration = (LogEnabledFilterData)objectConfiguration;

            ILogFilter createdObject
                = new LogEnabledFilter(
                    castedObjectConfiguration.Name,
                    castedObjectConfiguration.Enabled);

            return createdObject;
        }
    }
}
