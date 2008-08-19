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
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
	/// <summary>
	/// Represents a <see cref="Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.ValidatorData"/> configuration element. 
	/// </summary>
	// TODO: Include the images for your node.
	//[Image(typeof (ValidationNode))]
	//[SelectedImage(typeof (ValidationNode))]
	public abstract class ValidationNode : Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ConfigurationNode
	{

		/// <summary>
		/// Initialize a new instance of the <see cref="ValidationNode"/> class.
		/// </summary>
		public ValidationNode()
		{
		}

		/// <summary>
		/// Gets the <see cref="Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.ValidatorData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.ValidatorData"/> this node represents.
		/// </value>
		[Browsable(false)]
		public abstract Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.ValidatorData ValidatorData { get; }

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="ValidationNode"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			base.Dispose(disposing);
		}

		private System.String messageTemplate;
		/// <summary>
		/// 
		/// </summary>
		/// <value>
		/// 
		/// </value>
		[SRDescription("MessageTemplateDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public System.String MessageTemplate
		{
			get { return this.messageTemplate; }
			set { this.messageTemplate = value; }
		}

		private System.String messageTemplateResourceName;
		/// <summary>
		/// 
		/// </summary>
		/// <value>
		/// 
		/// </value>
		[SRDescription("MessageTemplateResourceNameDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public System.String MessageTemplateResourceName
		{
			get { return this.messageTemplateResourceName; }
			set { this.messageTemplateResourceName = value; }
		}

		private System.String messageTemplateResourceTypeName;
		/// <summary>
		/// 
		/// </summary>
		/// <value>
		/// 
		/// </value>
		[SRDescription("MessageTemplateResourceTypeNameDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public System.String MessageTemplateResourceTypeName
		{
			get { return this.messageTemplateResourceTypeName; }
			set { this.messageTemplateResourceTypeName = value; }
		}

	}
}
