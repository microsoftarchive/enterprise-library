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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="KeyedHashAlgorithmProviderData"/>.
    /// </summary>
    public class KeyedHashAlgorithmProviderNode : HashAlgorithmProviderNode, ICryptographicKeyConfigurationNode
    {
        private ProtectedKeySettings key;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public KeyedHashAlgorithmProviderNode() : this(new KeyedHashAlgorithmProviderData())
        {
            Name = Resources.HashAlgorithmProviderNodeName;
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="hashAlgorithmProviderData">The corresponding runtime configuration data.</param>
        public KeyedHashAlgorithmProviderNode(KeyedHashAlgorithmProviderData hashAlgorithmProviderData) : base(hashAlgorithmProviderData)
        {
            key = new ProtectedKeySettings(hashAlgorithmProviderData.ProtectedKeyFilename, hashAlgorithmProviderData.ProtectedKeyProtectionScope);
        }

        /// <summary>
        /// Gets or sets the hash key.
        /// </summary>
        [Required]
        [Editor(typeof(KeyManagerEditor), typeof(UITypeEditor))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("HashAlgorithmProviderKeyDescription", typeof(Resources))]
        [EnvironmentOverridable(false)]
        public ProtectedKeySettings Key
        {
            get{return key;}
            set{key = value;}
        }

        /// <summary>
        /// Gets a <see cref="HashProviderData"/> configuration object using the node data.
        /// </summary>
		/// <value>
        /// A <see cref="HashProviderData"/> configuration object using the node data.
		/// </value>
        public override HashProviderData HashProviderData
        {
            get
            {
                return new KeyedHashAlgorithmProviderData(Name, AlgorithmType, SaltEnabled, Key.Filename, Key.Scope);
            }
        }

        ProtectedKeySettings ICryptographicKeyConfigurationNode.KeySettings
        {
            get { return Key ; }
        }

        IKeyCreator ICryptographicKeyConfigurationNode.KeyCreator
        {
            get { return new KeyedHashAlgorithmKeyCreator(AlgorithmType); }
        }
    }
}
