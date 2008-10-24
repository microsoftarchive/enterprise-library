//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Specifies a property or event that is validated based on a regular expression.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=true, Inherited=true)]
    public sealed class RegexAttribute : ValidationAttribute
    {
		private readonly Type compiledRegexType;
		private readonly string pattern;
		private readonly RegexOptions options;
		private readonly bool optionsSpecified;

        /// <summary>
        /// Initialize a new instance of the <see cref="RegexAttribute"/> class with the regular expression pattern and options.
        /// </summary>
        /// <param name="pattern">
        /// The regular expression pattern to match.
        /// </param>     
        /// <param name="options">
        /// The regular expression options.
        /// </param>
        public RegexAttribute(string pattern, RegexOptions options) : this(pattern, options, true, null)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="RegexAttribute"/> class with the regular expression pattern.
        /// </summary>
        /// <param name="pattern">
        /// The regular expression pattern to match.
        /// </param>
        public RegexAttribute(string pattern) : this(pattern, RegexOptions.None, false, null)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="RegexAttribute"/> class with the regular expression type.
        /// </summary>
        /// <param name="compiledRegexType">
        /// The compiled <see cref="Type"/> for the regular expression.
        /// </param>
        public RegexAttribute(Type compiledRegexType) : this(null, RegexOptions.None, false, compiledRegexType)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="RegexAttribute"/> class with the regular expression type and options.
        /// </summary>
        /// <param name="compiledRegexType">
        /// The compiled <see cref="Type"/> for the regular expression.
        /// </param>
        /// <param name="options">
        /// The regular expression options.
        /// </param>
        public RegexAttribute(Type compiledRegexType, RegexOptions options) : this(null, options, true, compiledRegexType)
        {
        }

        private RegexAttribute(string pattern, RegexOptions options, bool optionsSpecified, Type compiledRegexType)
        {
            this.pattern = pattern;
            this.options = options;
            this.optionsSpecified = optionsSpecified;
            this.compiledRegexType = compiledRegexType;
        }

		/// <summary>
		/// Gets the options for the regular expression.
		/// </summary>
		/// <value>One of the <see cref="RegexOptions"/> values.</value>
		public RegexOptions Options
		{
			get { return options; }
		} 

		/// <summary>
		/// Determines if the <see cref="RegexOptions"/> were specified.
		/// </summary>
		/// <value>
		/// <c>true</c> if the options where specified; otherwise, <c>false</c>.
		/// </value>
		public bool OptionsSpecified
		{
			get { return optionsSpecified; }
		} 

		/// <summary>
		/// Gets the regular expression pattern to match.
		/// </summary>
		/// <value>
		/// The regular expression pattern to match.
		/// </value>
		public string Pattern
		{
			get { return pattern; }
		}

		/// <summary>
		/// Gets the compiled <see cref="Type"/> for the regular expression.
		/// </summary>
		/// <value>
		/// The compiled <see cref="Type"/> for the regular expression.
		/// </value>
		public Type CompiledRegexType
		{
			get { return compiledRegexType; }
		} 

        /// <summary>
        /// Validate the ranige data for the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.
        /// </summary>
        /// <param name="instance">
        /// The instance to validate.
        /// </param>
        /// <param name="propertyInfo">
        /// The property contaning the value to validate.
        /// </param>
        /// <param name="errors">
        /// The collection to add any errors that occur durring the validation.
        /// </param>		
		protected override void ValidateCore(object instance, PropertyInfo propertyInfo, IList<ValidationError> errors)
        {			
            object propertyValue = propertyInfo.GetValue(instance, null);
            Regex expression = null;
            if (this.compiledRegexType != null)
            {
                if (this.optionsSpecified)
                {
                    expression = (Regex)Activator.CreateInstance(this.compiledRegexType, new object[] {this.options});
                }
                else
                {
                    expression = (Regex)Activator.CreateInstance(this.compiledRegexType);
                }
            }
            else if (this.optionsSpecified)
            {
                expression = new Regex(this.pattern, this.options);
            }
            else
            {
                expression = new Regex(this.pattern);
            }

            if (!expression.IsMatch((string)propertyValue))
            {
                errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, string.Format(CultureInfo.CurrentUICulture, Resources.RegExErrorMessage, propertyValue.ToString())));
            }
        }
    }
}
