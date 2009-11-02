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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration
{
    /// <summary>
    /// A configuration element that handles the entries for the &lt;categories&gt; element
    /// for the Log Call handler.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "LogCallHandlerCategoryEntryDescription")]
    [ResourceDisplayName(typeof(DesignResources), "LogCallHandlerCategoryEntryDisplayName")]
    [ViewModel(PolicyInjectionCallHandlersDesignTime.ViewModelTypes.LogCallHandlerCategoryEntryViewModel)]
    public class LogCallHandlerCategoryEntry : NamedConfigurationElement
    {
        /// <summary>
        /// Construct an empty <see cref="LogCallHandlerCategoryEntry"/>.
        /// </summary>
        public LogCallHandlerCategoryEntry()
        {
        }

        /// <summary>
        /// Construct a <see cref="LogCallHandlerCategoryEntry"/> with the given
        /// category string.
        /// </summary>
        /// <param name="name">Category string.</param>
        public LogCallHandlerCategoryEntry(string name) : base(name)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [ResourceDescription(typeof(DesignResources), "LogCallHandlerCategoryEntryNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LogCallHandlerCategoryEntryNameDisplayName")]
        [Reference(typeof(NamedElementCollection<TraceSourceData>), typeof(TraceSourceData))]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }
}
