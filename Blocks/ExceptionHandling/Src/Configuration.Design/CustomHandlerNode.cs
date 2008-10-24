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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.ComponentModel;
using System.Drawing.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	/// <summary>
	/// Represents a design time representation of a <see cref="CustomHandlerData"/> configuration element.
	/// </summary>
	public sealed class CustomHandlerNode : ExceptionHandlerNode
	{
		private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
        private string typeName;

		/// <summary>
		/// Initialize a new instance of the <see cref="CustomHandlerNode"/> class.
		/// </summary>
		public CustomHandlerNode()
			: this(new CustomHandlerData(Resources.DefaultCustomHandlerNodeName, string.Empty))
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="CustomHandlerNode"/> class with a <see cref="CustomHandlerData"/> instance.
		/// </summary>
		/// <param name="customHandlerData">A <see cref="CustomHandlerData"/> instance.</param>
		public CustomHandlerNode(CustomHandlerData customHandlerData)
		{
			if (null == customHandlerData) throw new ArgumentNullException("customHandlerData");
			Rename(customHandlerData.Name);
			foreach (string key in customHandlerData.Attributes)
			{
				editableAttributes.Add(new EditableKeyValue(key, customHandlerData.Attributes[key]));
            }

            this.typeName = customHandlerData.TypeName;
		}

		/// <summary>
		/// Gets the attributes for the custom handler.
		/// </summary>
		/// <value>
		/// The attributes for the custom handler.
		/// </value>
		[SRDescription("ExceptionHandlerAdditionalPropertiesDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[CustomAttributesValidation]
		public List<EditableKeyValue> Attributes
		{
			get { return editableAttributes; }
		}


		/// <summary>
		/// Gets the fully qualified assembly name of the <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler"/>.
		/// </summary>
		/// <value>
		/// The fully qualified assembly name of the <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler"/>.
		/// </value>
		[Required]
		[Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
		[BaseType(typeof(IExceptionHandler), typeof(CustomHandlerData))]
		[SRDescription("ExceptionHandlerTypeDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public String Type
		{
            get { return typeName; }
            set { typeName = value; }
		}


		/// <summary>
		/// Gets the <see cref="CustomHandlerData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="CustomHandlerData"/> this node represents.
		/// </value>
		public override ExceptionHandlerData ExceptionHandlerData
		{
			get
			{
				CustomHandlerData customHandlerData = new CustomHandlerData(Name, typeName);
				customHandlerData.Attributes.Clear();

				foreach (EditableKeyValue kv in editableAttributes)
				{
					customHandlerData.Attributes.Add(kv.Key, kv.Value);
				}

				return customHandlerData;
			}
		}
	}
}
