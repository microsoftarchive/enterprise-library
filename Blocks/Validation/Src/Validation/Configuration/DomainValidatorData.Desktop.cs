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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    [ResourceDescription(typeof(DesignResources), "DomainValidatorDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "DomainValidatorDataDisplayName")]
    partial class DomainValidatorData
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DomainValidatorData"/> class.</para>
		/// </summary>
        public DomainValidatorData() { Type = typeof(DomainValidator<object>); }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DomainValidatorData"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public DomainValidatorData(string name)
			: base(name, typeof(DomainValidator<object>))
		{ }

		private const string DomainPropertyName = "domain";
		/// <summary>
		/// Gets the collection of elements for the domain for the represented <see cref="DomainValidator{T}"/>.
		/// </summary>
		[ConfigurationProperty(DomainPropertyName)]
        [ConfigurationCollection(typeof(DomainConfigurationElement))]
        [ResourceDescription(typeof(DesignResources), "DomainValidatorDataDomainDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DomainValidatorDataDomainDisplayName")]
        [System.ComponentModel.Editor(CommonDesignTime.EditorTypes.CollectionEditor, CommonDesignTime.EditorTypes.FrameworkElement)]
        [EnvironmentalOverrides(false)]
        [DesignTimeReadOnly(false)]
		public NamedElementCollection<DomainConfigurationElement> Domain
		{
			get { return (NamedElementCollection<DomainConfigurationElement>)this[DomainPropertyName]; }
		}
	}
}
