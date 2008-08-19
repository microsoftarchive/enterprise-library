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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of class <see cref="DomainValidatorData"/>.
	/// </summary>
	public class DomainValidatorData : ValueValidatorData
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DomainValidatorData"/> class.</para>
		/// </summary>
		public DomainValidatorData()
		{
		}

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
		public NamedElementCollection<DomainConfigurationElement> Domain
		{
			get { return (NamedElementCollection<DomainConfigurationElement>)this[DomainPropertyName]; }
		}

		/// <summary>
		/// Creates the <see cref="DomainValidator{T}"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="DomainValidator{T}"/>.</returns>	
		protected override Validator DoCreateValidator(Type targetType)
		{
			List<object> domainObjects = new List<object>();
			TypeConverter typeConverter = null;
			if (targetType != null)
			{
				typeConverter = TypeDescriptor.GetConverter(targetType);
			}

			foreach (DomainConfigurationElement domainConfigurationElement in Domain)
			{
				if (typeConverter != null)
				{
					domainObjects.Add(typeConverter.ConvertFromString(null, CultureInfo.CurrentCulture, domainConfigurationElement.Name));
				}
				else
				{	
					domainObjects.Add(domainConfigurationElement.Name);
				}
			}

			return new DomainValidator<object>(domainObjects, Negated);
		}
	}
}
