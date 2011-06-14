//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Validation;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// <para>Configuration data for an <c>IsolatedStorageTraceListenerData</c>.</para>
    /// </summary>	
    [ResourceDescription(typeof(LoggingResources), "IsolatedStorageTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(LoggingResources), "IsolatedStorageTraceListenerDataDisplayName")]
    [Browsable(true)]
    public class IsolatedStorageTraceListenerData : TraceListenerData
    {
        private const string maxSizeInKilobytesProperty = "maxSizeInKilobytes";
        private const string repositoryNameProperty = "repositoryName";

        private const int maxSizeInKilobytesDefaultValue = 256;

        /// <summary>
        /// <para>Initializes a new instance of <see cref="IsolatedStorageTraceListenerData"/> class.</para>
        /// </summary>
        public IsolatedStorageTraceListenerData()
            : base(typeof(IsolatedStorageTraceListener))
        {
        }

        /// <summary>
        /// Gets or sets the maximum size in bytes to be used when storing entries.
        /// </summary>
        [ConfigurationProperty(maxSizeInKilobytesProperty, DefaultValue = maxSizeInKilobytesDefaultValue)]
        [ResourceDescription(typeof(LoggingResources), "MaxSizeInKilobytesDescription")]
        [ResourceDisplayName(typeof(LoggingResources), "MaxSizeInKilobytesDisplayName")]
        [Validation(typeof(SizeOverSilverlightDefaultQuotaValidator))]
        [Browsable(true)]
        public int MaxSizeInKilobytes
        {
            get { return (int)this[maxSizeInKilobytesProperty]; }
            set { this[maxSizeInKilobytesProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the time interval that will be used for submiting the log entries to the server.
        /// </summary>
        [ConfigurationProperty(repositoryNameProperty, IsRequired = true)]
        [ResourceDescription(typeof(LoggingResources), "RepositoryNameDescription")]
        [ResourceDisplayName(typeof(LoggingResources), "RepositoryNameDisplayName")]
        [Validation(typeof(ValidFilenameValidator))]
        [Browsable(true)]
        public string RepositoryName
        {
            get { return (string)this[repositoryNameProperty]; }
            set { this[repositoryNameProperty] = value; }
        }

        /// <summary>
        /// Overridden in order to hide from the configuration designtime.
        /// </summary>
        [Browsable(false)]
        public new TraceOptions TraceOutputOptions
        {
            get;
            set;
        }

        /// <summary>
        /// Overridden in order to hide from the configuration designtime.
        /// </summary>
        [Browsable(false)]
        public new SourceLevels Filter
        {
            get;
            set;
        }
    }
}
