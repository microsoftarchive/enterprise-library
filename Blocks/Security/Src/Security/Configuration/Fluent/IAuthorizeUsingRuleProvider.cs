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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Security;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="AuthorizationRuleProvider"/> instance.
    /// </summary>
    /// <seealso cref="AuthorizationRuleProvider"/>
    /// <seealso cref="AuthorizationRuleProviderData"/>
    public interface IAuthorizeUsingRuleProvider : IConfigureSecuritySettings
    {
        /// <summary>
        /// Specifies this <see cref="AuthorizationRuleProvider"/> instance as the default <see cref="IAuthorizationProvider"/>.
        /// </summary>
        /// <seealso cref="AuthorizationRuleProvider"/>
        /// <seealso cref="AuthorizationRuleProviderData"/>
        IAuthorizeUsingRuleProvider SetAsDefault();

        /// <summary>
        /// Adds a named rule to the <see cref="AuthorizationRuleProvider"/>'s configuration.
        /// </summary>
        /// <param name="ruleName">The name of the rule.</param>
        /// <param name="ruleExpression">The expression that makes up the authorization rule.</param>
        /// <seealso cref="AuthorizationRuleProvider"/>
        /// <seealso cref="AuthorizationRuleProviderData"/>
        IAuthorizeUsingRuleProvider SpecifyRule(string ruleName, string ruleExpression);
    }
}
