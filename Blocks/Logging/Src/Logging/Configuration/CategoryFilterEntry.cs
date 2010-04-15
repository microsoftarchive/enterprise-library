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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents a single <see cref="CategoryFilterEntry"/> configuration settings.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "CategoryFilterEntryDescription")]
    [ResourceDisplayName(typeof(DesignResources), "CategoryFilterEntryDisplayName")]
    public class CategoryFilterEntry : NamedConfigurationElement
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CategoryFilterData"/> class.</para>
        /// </summary>
		public CategoryFilterEntry()
    	{
    	}

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CategoryFilterData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="CategoryFilterData"/>.</para>
        /// </param>
		public CategoryFilterEntry(string name)
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
        /// Overriden in order to annotate with designtime attribute.
        /// </remarks>
        [Reference(typeof(NamedElementCollection<TraceSourceData>), typeof(TraceSourceData))]
        [ViewModel(CommonDesignTime.ViewModelTypeNames.CollectionEditorContainedElementReferenceProperty)]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
	}
}
