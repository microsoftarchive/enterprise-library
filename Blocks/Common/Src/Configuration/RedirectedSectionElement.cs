//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary/>
    [ResourceDescription(typeof(DesignResources), "RedirectedSectionElementDescription")]
    [ResourceDisplayName(typeof(DesignResources), "RedirectedSectionElementDisplayName")]
    public class RedirectedSectionElement : NamedConfigurationElement
    {
        /// <summary/>
        public const string sourceNameProperty = "sourceName";

        /// <summary/>
        [ConfigurationProperty(sourceNameProperty, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "RedirectedSectionElementSourceNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RedirectedSectionElementSourceNameDisplayName")]
        [Reference(typeof(NameTypeConfigurationElementCollection<ConfigurationSourceElement, ConfigurationSourceElement>), typeof(ConfigurationSourceElement))]
        public string SourceName
        {
            get { return (string)this[sourceNameProperty]; }
            set { this[sourceNameProperty] = value; }
        }

    }
}
