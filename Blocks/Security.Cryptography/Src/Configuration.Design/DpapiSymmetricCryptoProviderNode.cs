//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using System.Security.Cryptography;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="DpapiSymmetricCryptoProviderData"/>.
    /// </summary>
    public class DpapiSymmetricCryptoProviderNode : SymmetricCryptoProviderNode
    {
        private DataProtectionScope scope;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public DpapiSymmetricCryptoProviderNode() : this(new DpapiSymmetricCryptoProviderData(Resources.DpapiSymmetricCryptoProviderNodeName, DataProtectionScope.CurrentUser))
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="data">The corresponding runtime configuration data.</param>
        public DpapiSymmetricCryptoProviderNode(DpapiSymmetricCryptoProviderData data) : base(data)
        {
            scope = data.Scope;
        }

        /// <summary>
        /// Gets a <see cref="SymmetricProviderData"/> configuration object using the node data.
        /// </summary>
		/// <value>
        /// A <see cref="SymmetricProviderData"/> configuration object using the node data.
		/// </value>
        public override SymmetricProviderData SymmetricCryptoProviderData
        {
            get { return new DpapiSymmetricCryptoProviderData(Name, scope); }
        }

        /// <summary>
        ///Gets or sets the <see cref="DataProtectionScope"/> for this <see cref="System.Security.Cryptography.DataProtectionScope"/>.
        /// </summary>
        [Required]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("ProtectionScopeDescription", typeof(Resources))]
        public DataProtectionScope ProtectionScope
        {
            get { return scope; }
            set { scope = value; }
        }
    }
}