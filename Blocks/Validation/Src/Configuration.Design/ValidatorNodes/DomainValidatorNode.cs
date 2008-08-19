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
using System.ComponentModel;
using System.Drawing.Design;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
	/// <summary>
	/// Respresents the designtime configuration node for an <see cref="DomainValidatorData"/>.
	/// </summary>
	public class DomainValidatorNode : ValueValidatorNode
	{
		private List<DomainValue> domain = new List<DomainValue>();
		
		/// <summary>
		/// Creates an instance of <see cref="DomainValidatorNode"/> based on default values.
		/// </summary>
		public DomainValidatorNode()
			: this(new DomainValidatorData(Resources.DomainValidatorNodeName))
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="DomainValidatorNode"/> based on runtime configuration data.
		/// </summary>
		/// <param name="validatorData">The corresponding runtime configuration data.</param>
		public DomainValidatorNode(DomainValidatorData validatorData)
			: base(validatorData)
		{
			foreach (DomainConfigurationElement domainConfigurationElement in validatorData.Domain)
			{
				domain.Add(new DomainValue(domainConfigurationElement.Name));
			}
		}

		/// <summary>
		/// Gets or sets the custom attributes for the provider.
		/// </summary>
		/// <value>
		/// The custom attributes for the provider.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("DomainDescription", typeof(Resources))]
		[CustomAttributesValidation]
		public List<DomainValue> Domain
		{
			get { return domain; }
		}

		/// <summary>
		/// Returns the runtime configuration data that is represented by this node.
		/// </summary>
		/// <returns>An instance of <see cref="DomainValidatorData"/> that can be persisted to a configuration file.</returns>
		public override ValidatorData CreateValidatorData()
		{
			DomainValidatorData validatorData = new DomainValidatorData(Name);
			SetValidatorBaseProperties(validatorData);
			SetValueValidatorBaseProperties(validatorData);

			foreach (DomainValue domainValue in Domain)
			{
				validatorData.Domain.Add(new DomainConfigurationElement(domainValue.Value));
			}

			return validatorData;
		}
	}
}
