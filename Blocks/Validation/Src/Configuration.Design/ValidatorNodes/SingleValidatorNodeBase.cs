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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
	/// <summary>
	/// Represents the designtime configuration node for any noncomposite validator.
	/// </summary>
	public abstract class SingleValidatorNodeBase : ValidatorNodeBase
	{
		private string messageTemplate;
		private string messageTemplateResourceName;
		private string messageTemplateResourceTypeName;
		private string tag;

		/// <summary>
		/// Initializes a new instance of the <see cref="SingleValidatorNodeBase"/> representing <paramref name="validatorData"/>.
		/// </summary>
		/// <param name="validatorData">The represented <see cref="ValidatorData"/>.</param>
		protected SingleValidatorNodeBase(ValidatorData validatorData)
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
		/// Copies properties declared on this node to a runtime configuration data.
		/// </summary>
		/// <param name="validatorData">The runtime configuration data which should be updated with settings in this node.</param>
		protected void SetValidatorBaseProperties(ValidatorData validatorData)
		{
			validatorData.MessageTemplate = messageTemplate;
			validatorData.MessageTemplateResourceName = messageTemplateResourceName;
			validatorData.MessageTemplateResourceTypeName = messageTemplateResourceTypeName;
			validatorData.Tag = tag;
		}

		/// <summary>
		/// Performs validation for this node.
		/// </summary>
		/// <param name="errors">The list of errors to add any validation errors.</param>
		public override void Validate(IList<ValidationError> errors)
		{
			if (!String.IsNullOrEmpty(MessageTemplateResourceName) && !String.IsNullOrEmpty(MessageTemplate))
			{
				errors.Add(new ValidationError(this, "MessageTemplate", Resources.BothTemplateAndResourceNameAreSpecified));
			}
			base.Validate(errors);
		}
	}
}
