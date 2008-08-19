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

using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// Represents a <see cref="AuthorizationRuleData"/> configuration element.
    /// </summary>
    [Image(typeof(AuthorizationRuleNode))]
    [SelectedImage(typeof(AuthorizationRuleNode))]
    public sealed class AuthorizationRuleNode : ConfigurationNode
    {
		private string expression;

        /// <summary>
        /// Initialize a new instance of the <see cref="AuthorizationRuleNode"/> class.
        /// </summary>
        public AuthorizationRuleNode() : this(new AuthorizationRuleData(Resources.AuthorizationRuleNodeName, string.Empty))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="AuthorizationRuleNode"/> class with an <see cref="AuthorizationRuleData"/> instance.
		/// </summary>
		/// <param name="authorizationRuleData">An <see cref="AuthorizationRuleData"/> instance</param>
        public AuthorizationRuleNode(AuthorizationRuleData authorizationRuleData)
            : base((authorizationRuleData == null) ? Resources.AuthorizationRuleNodeName : authorizationRuleData.Name)
        {
            if (authorizationRuleData == null)
            {
                throw new ArgumentNullException("authorizationRuleData");
            }

            this.expression = authorizationRuleData.Expression;
        }

        /// <summary>
        /// Gets or sets the expression for the current rule.
        /// </summary>
		/// <value>
		/// The expression for the current rule
		/// </value>
        [Required]
        [Editor(typeof(ExpressionEditor), typeof(UITypeEditor))]
        [ValidExpression]
        [SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("ExpressionDescription", typeof(Resources))]
        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }
        
		
		/// <summary>
		/// Gets the <see cref="AuthorizationRuleData"/> object the node represents.
		/// </summary>
		/// <value>
		/// The <see cref="AuthorizationRuleData"/> object the node represents.
		/// </value>
        [Browsable(false)]
        public AuthorizationRuleData AuthorizationRuleData
        {
            get { return new AuthorizationRuleData(Name, expression); }
        }
    }
}