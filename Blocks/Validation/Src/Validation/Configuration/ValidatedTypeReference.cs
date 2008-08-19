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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Represents validation information for a type and its members.
	/// </summary>
	/// <seealso cref="ValidationRulesetData"/>
	public class ValidatedTypeReference : NamedConfigurationElement
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ValidatedTypeReference"/> class.</para>
		/// </summary>
		public ValidatedTypeReference()
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ValidatedTypeReference"/> class with a type.</para>
		/// </summary>
		/// <param name="type">The represented type.</param>
		public ValidatedTypeReference(Type type)
			: base(type.FullName)
		{ }

		private const string RulesetsPropertyName = "";
		/// <summary>
		/// Gets the collection with the validation rulesets configured the represented type.
		/// </summary>
		[ConfigurationProperty(RulesetsPropertyName, IsDefaultCollection = true)]
		public ValidationRulesetDataCollection Rulesets
		{
			get { return (ValidationRulesetDataCollection)this[RulesetsPropertyName]; }
		}

		private const string DefaultRulePropertyName = "defaultRuleset";
		/// <summary>
		/// Gets or sets the default ruleset for the represented type.
		/// </summary>
		[ConfigurationProperty(DefaultRulePropertyName)]
		public string DefaultRuleset
		{
			get { return (string)this[DefaultRulePropertyName]; }
			set { this[DefaultRulePropertyName] = value; }
		}

        private const string AssemblyNamePropertyName = "assemblyName";
		/// <summary>
		/// Used to resolve the reference type in designtime. This property is ignored at runtime.
		/// </summary>
		[ConfigurationProperty(AssemblyNamePropertyName)]
        public string AssemblyName
        {
            get { return (string)this[AssemblyNamePropertyName]; }
            set { this[AssemblyNamePropertyName] = value; }
        }
	}
}