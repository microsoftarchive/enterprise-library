//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
	/// <summary>
	/// Respresents the designtime configuration node for an <see cref="OrCompositeValidatorData"/>.
	/// </summary>
	public class OrCompositeValidatorNode : CompositeValidatorNodeBase
	{
		private string messageTemplate;
		private string messageTemplateResourceName;
		private string messageTemplateResourceTypeName;
		private string tag;

		/// <summary>
		/// Creates an instance of <see cref="OrCompositeValidatorNode"/> based on default values.
		/// </summary>
		public OrCompositeValidatorNode()
			: this(new OrCompositeValidatorData(Resources.OrCompositeValidatorNodeName))
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="OrCompositeValidatorNode"/> based on runtime configuration data.
		/// </summary>
		/// <param name="validatorData">The corresponding runtime configuration data.</param>
		public OrCompositeValidatorNode(OrCompositeValidatorData validatorData)
			: base(validatorData.Name)
		{
			messageTemplate = validatorData.MessageTemplate;
			messageTemplateResourceName = validatorData.MessageTemplateResourceName;
			messageTemplateResourceTypeName = validatorData.MessageTemplateResourceTypeName;
			tag = validatorData.Tag;
		}

		/// <summary>
		/// Gets or sets the MessageTemplate for this validator.
		/// </summary>
		[SRCategory("CategoryValidatorMessage", typeof(Resources))]
		[SRDescription("MessageTemplateDescription", typeof(Resources))]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		public string MessageTemplate
		{
			get { return messageTemplate; }
			set { messageTemplate = value; }
		}

		/// <summary>
		/// Gets or sets the MessageTemplateResourceName for this validator.
		/// When using localized message templates, this specifies the name of the resource that should be used to retrieve a localized template.
		/// </summary>
		[SRCategory("CategoryValidatorMessage", typeof(Resources))]
		[SRDescription("MessageTemplateResourceNameDescription", typeof(Resources))]
		public string MessageTemplateResourceName
		{
			get { return messageTemplateResourceName; }
			set { messageTemplateResourceName = value; }
		}

		/// <summary>
		/// Gets or sets the MessageTemplateResourceTypeName for this validator.
		/// When using localized message templates, this specifies the type that contains the localized resources.
		/// </summary>
		[SRCategory("CategoryValidatorMessage", typeof(Resources))]
		[SRDescription("MessageTemplateResourceTypeNameDescription", typeof(Resources))]
		[Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
		[BaseType(typeof(Object), TypeSelectorIncludes.None)]
		public string MessageTemplateResourceTypeName
		{
			get { return messageTemplateResourceTypeName; }
			set { messageTemplateResourceTypeName = value; }
		}

		/// <summary>
		/// Gets or sets the tag for this validator.
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("TagDescription", typeof(Resources))]
		public string Tag
		{
			get { return tag; }
			set { tag = value; }
		}

		/// <summary>
		/// Returns the runtime configuration data that is represented by this node.
		/// </summary>
		/// <returns>An instance of <see cref="OrCompositeValidatorData"/> that can be persisted to a configuration file.</returns>
		public override ValidatorData CreateValidatorData()
		{
			OrCompositeValidatorData validatorData = new OrCompositeValidatorData(Name);
			validatorData.MessageTemplate = messageTemplate;
			validatorData.MessageTemplateResourceName = messageTemplateResourceName;
			validatorData.MessageTemplateResourceTypeName = messageTemplateResourceTypeName;
			validatorData.Tag = tag;

			foreach (ConfigurationNode childNode in Nodes)
			{
				ValidatorNodeBase validatorNode = childNode as ValidatorNodeBase;
				if (validatorNode != null)
				{
					validatorData.Validators.Add(validatorNode.CreateValidatorData());
				}
			}
			return validatorData;
		}
	}
}