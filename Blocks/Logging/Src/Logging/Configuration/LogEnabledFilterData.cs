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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="LogEnabledFilter"/>.
    /// </summary>
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
        /// Creates an enumeration of <see cref="TypeRegistration"/> instances describing the filter represented by 
        /// this configuration object.
        /// </summary>
        /// <returns>A an enumeration of <see cref="TypeRegistration"/> instance describing a filter.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations()
        {
            yield return
                new TypeRegistration<ILogFilter>(() => new LogEnabledFilter(this.Name, this.Enabled))
                {
                    Name = this.Name,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
