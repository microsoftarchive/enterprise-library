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
using System.Diagnostics;
using System.Drawing.Design;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    /// <summary>
    /// Represents a <see cref="CustomFormatterData"/> configuration element.
    /// </summary>
    public class CustomFormatterNode : FormatterNode
    {
        private string customFormatterTypeName;
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();

		/// <summary>
		/// Initialize a new instance of the <see cref="CustomFormatterNode"/> class.
		/// </summary>
        public CustomFormatterNode()
            : this(new CustomFormatterData(Resources.CustomFormatter, string.Empty))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="CustomFormatterNode"/> class with a <see cref="CustomFormatterData"/> instance.
		/// </summary>
		/// <param name="customFormatterData">A <see cref="CustomFormatterData"/> instance.</param>
        public CustomFormatterNode(CustomFormatterData customFormatterData)            
        {
			if (null == customFormatterData) throw new ArgumentNullException("customFormatterData");

            customFormatterTypeName = customFormatterData.TypeName;
            foreach (string key in customFormatterData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, customFormatterData.Attributes[key]));
            }
			Rename(customFormatterData.Name);
        }

		/// <summary>
		/// Gets the custom attributes for the formatter.
		/// </summary>
		/// <value>
		/// The custom attributes for the formatter.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("customFormatterAttributesDescription", typeof(Resources))]
        [CustomAttributesValidation]
        public List<EditableKeyValue> Attributes
        {
            get
            {
                return editableAttributes;
            }
        }

		/// <summary>
		/// Gets or sets the <see cref="Type"/> of the custom formatter.
		/// </summary>
		/// <value>
		/// The <see cref="Type"/> of the custom formatter.
		/// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(LogFormatter), typeof(CustomFormatterData))]
        [SRDescription("customFormatterDataTypeDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return customFormatterTypeName; }
            set { customFormatterTypeName = value; }
        }

        /// <summary>
		/// Gets the <see cref="CustomFormatterData"/> this node represents.
        /// </summary>
		/// <value>
		/// The <see cref="CustomFormatterData"/> this node represents.
		/// </value>
        [Browsable(false)]
		public override FormatterData FormatterData
        {
            get
            {
                CustomFormatterData customFormatterData = new CustomFormatterData(Name, customFormatterTypeName);                
                foreach (EditableKeyValue kv in editableAttributes)
                {
                    customFormatterData.Attributes.Add(kv.Key, kv.Value);
                }
				return customFormatterData;
            }
        }
    }
}
