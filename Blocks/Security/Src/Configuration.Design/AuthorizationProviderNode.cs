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
	/// Represents a <see cref="AuthorizationProviderData"/> configuration element. This class is abstract.
	/// </summary>
    [Image(typeof(AuthorizationProviderNode))]
    [SelectedImage(typeof(AuthorizationProviderNode))]
    public abstract class AuthorizationProviderNode : ConfigurationNode
    {
        /// <summary>
        /// Initailzie a new instance of the <see cref="AuthorizationProviderData"/> class.
        /// </summary>
        protected AuthorizationProviderNode() 
            : base()
        {            
        }

        /// <summary>
        /// Gets the <see cref="AuthorizationProviderData"/> this node represents.
        /// </summary>
		/// <value>
		/// The <see cref="AuthorizationProviderData"/> this node represents.
		/// </value>
        [Browsable(false)]
        public abstract AuthorizationProviderData AuthorizationProviderData { get; }        
    }
}
