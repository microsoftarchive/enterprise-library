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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that handles the entries for the &lt;categories&gt; element
    /// for the log call handler.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "LogCallHandlerCategoryEntryDescription")]
    [ResourceDisplayName(typeof(DesignResources), "LogCallHandlerCategoryEntryDisplayName")]
    [AddSateliteProviderCommand(LoggingSettings.SectionName, typeof(LoggingSettings), "DefaultCategory", "Name")]
    public class LogCallHandlerCategoryEntry : NamedConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogCallHandlerCategoryEntry"/> class.
        /// </summary>
        public LogCallHandlerCategoryEntry()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogCallHandlerCategoryEntry"/> class with the given
        /// category string.
        /// </summary>
        /// <param name="name">The category string.</param>
        public LogCallHandlerCategoryEntry(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        /// <value>
        /// The name of the element.
        /// </value>
        /// <remarks>
        /// Overridden in order to annotate with design-time attribute.
        /// </remarks>
        [ResourceDescription(typeof(DesignResources), "LogCallHandlerCategoryEntryNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "LogCallHandlerCategoryEntryNameDisplayName")]
        [Reference(typeof(NamedElementCollection<TraceSourceData>), typeof(TraceSourceData))]
        [ViewModel(CommonDesignTime.ViewModelTypeNames.CollectionEditorContainedElementReferenceProperty)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }
}
