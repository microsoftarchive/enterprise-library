//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents the configuration data for a
    /// rule that is governed by an 
    /// <see cref="AuthorizationRuleProvider"/>.
    /// </summary>
    public class AuthorizationRuleData : NamedConfigurationElement, IAuthorizationRule
    {
        private const string expressionProperty = "expression";

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AuthorizationRuleData"/> class.
        /// </summary>
        public AuthorizationRuleData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationRuleData"/> class with the specified name and expression.
        /// </summary>
        /// <param name="name">The name of the rule</param>
        /// <param name="expression">The expression to evaluate.</param>
        public AuthorizationRuleData(string name, string expression) : base(name)
        {
            this.Expression = expression;
        }

        /// <summary>
        /// Gets or sets the expression associated with
        /// this rule.
        /// </summary>
		[ConfigurationProperty(expressionProperty, IsRequired= false)]
		public string Expression
		{
			get
			{
				return (string)this[expressionProperty];
			}
			set
			{
				this[expressionProperty] = value;
			}
		}

    }
}