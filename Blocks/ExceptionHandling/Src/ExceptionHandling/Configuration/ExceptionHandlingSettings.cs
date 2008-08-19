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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
	/// <summary>
	/// Represents the Exception Handling Application Block configuration section in a configuration file.
	/// </summary>
    public class ExceptionHandlingSettings : SerializableConfigurationSection
    {
		/// <summary>
		/// Gets the configuration section name for the library.
		/// </summary>
		public const string SectionName = "exceptionHandling";


        private const string policiesProperty = "exceptionPolicies";

        /// <summary>
        /// Gets the <see cref="ExceptionHandlingSettings"/> section in the configuration source.
        /// </summary>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> to get the section from.</param>
        /// <returns>The exception handling section.</returns>
        public static ExceptionHandlingSettings GetExceptionHandlingSettings(IConfigurationSource configurationSource)
		{
			return (ExceptionHandlingSettings)configurationSource.GetSection(SectionName);
		}

		/// <summary>
		/// Initializes a new instance of an <see cref="ExceptionHandlingSettings"/> class.
		/// </summary>
		public ExceptionHandlingSettings()
		{
			this[policiesProperty] = new NamedElementCollection<ExceptionPolicyData>();
		}

		/// <summary>
		/// Gets a collection of <see cref="ExceptionPolicyData"/> objects.
		/// </summary>
		/// <value>
		/// A collection of <see cref="ExceptionPolicyData"/> objects.
		/// </value>
		[ConfigurationProperty(policiesProperty)]
		public NamedElementCollection<ExceptionPolicyData> ExceptionPolicies
		{
			get { return (NamedElementCollection<ExceptionPolicyData>)this[policiesProperty]; }
		}        
    }
}