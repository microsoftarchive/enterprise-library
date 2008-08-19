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
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    /// <summary>
    /// Represents a <see cref="CustomLogFilterData"/> configuration element.
    /// </summary>
    public sealed class CustomLogFilterNode : LogFilterNode
    {
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
        private string customLogFilterTypeName;
      

		/// <summary>
		/// Initialize an instance of the <see cref="CustomLogFilterNode"/> class.
		/// </summary>
        public CustomLogFilterNode()
            :this(new CustomLogFilterData(Resources.CustomFilterNode, string.Empty))
        {
        }

		/// <summary>
		/// Initialize an instance of the <see cref="CustomLogFilterNode"/> class with a <see cref="CustomLogFilterData"/> instance.
		/// </summary>
		/// <param name="customLogFilterData">A <see cref="CustomLogFilterData"/> instance.</param>
        public CustomLogFilterNode(CustomLogFilterData customLogFilterData)
            : base(customLogFilterData == null ? string.Empty : customLogFilterData.Name)
        {
			if (null == customLogFilterData) throw new ArgumentNullException("customLogFilterData");

            customLogFilterTypeName = customLogFilterData.TypeName;
            foreach (string key in customLogFilterData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, customLogFilterData.Attributes[key]));
            }
        }

        /// <summary>
        /// Gets the custom attributes for the filter.
        /// </summary>
		/// <value>
		/// The custom attributes for the filter.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("CustomFilterExtensionsDescription", typeof(Resources))]
        [CustomAttributesValidation]
        public List<EditableKeyValue> Attributes
        {
            get
            {
                return editableAttributes;
            }
        }

		/// <summary>
		/// Gets or sets the <see cref="Type"/> of the custom filter.
		/// </summary>
		/// <value>
		/// The <see cref="Type"/> of the custom filter.
		/// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(LogFilter), typeof(CustomLogFilterData))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("CustomFilterTypeDescription", typeof(Resources))]
        public string Type
        {
            get { return customLogFilterTypeName; }
            set { customLogFilterTypeName = value; }
        }

        /// <summary>
        /// Gets the <see cref="CustomLogFilterData"/> this node represents.
        /// </summary>
		/// <value>
		/// The <see cref="CustomLogFilterData"/> this node represents.
		/// </value>
        public override LogFilterData LogFilterData
        {
            get
            {
                CustomLogFilterData customLogFilterData = new CustomLogFilterData(Name, customLogFilterTypeName);                                
                foreach (EditableKeyValue kv in editableAttributes)
                {
                    customLogFilterData.Attributes.Add(kv.Key, kv.Value);
                }
				return customLogFilterData;
            }
        }
        
    }
}