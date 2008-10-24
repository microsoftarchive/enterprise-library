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

using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Properties;
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// Represents a <see cref="AuthorizationProviderData"/> configuraiton element.
    /// </summary>
    public sealed class AuthorizationRuleProviderNode : AuthorizationProviderNode
    {       

		/// <summary>
		/// Initialize a new instance of the <see cref="AuthorizationRuleProviderNode"/> class.
		/// </summary>
        public AuthorizationRuleProviderNode() : this(new AuthorizationRuleProviderData(Resources.AuthorizationRuleProviderName))
        {
            
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="AuthorizationRuleProviderNode"/> class with an <see cref="AuthorizationRuleProviderData"/> instance.
		/// </summary>
		/// <param name="data">An <see cref="AuthorizationRuleProviderData"/> instance</param>
		public AuthorizationRuleProviderNode(AuthorizationRuleProviderData data)
			: base()
        {
			if (null == data) throw new ArgumentNullException("data");
			Rename(data.Name);
        }
		
		/// <summary>
		/// Gets the <see cref="AuthorizationRuleProviderData"/> this ndoe represents.
		/// </summary>
		public override AuthorizationProviderData AuthorizationProviderData
		{
			get { return new AuthorizationRuleProviderData(Name); }
		}
    }
}
