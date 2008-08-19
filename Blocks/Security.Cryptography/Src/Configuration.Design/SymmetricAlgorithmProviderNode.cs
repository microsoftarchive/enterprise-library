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

using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Security.Cryptography;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="SymmetricAlgorithmProviderData"/>.
    /// </summary>
    public class SymmetricAlgorithmProviderNode : SymmetricCryptoProviderNode, ICryptographicKeyConfigurationNode
    {
        private Type algorithmType;
        private ProtectedKeySettings key;

		/// <summary>
		/// Initializes with default configuration.
		/// </summary>
		public SymmetricAlgorithmProviderNode()
			: this(new SymmetricAlgorithmProviderData(Resources.SymmetricAlgorithmProviderNodeName, null, null, DataProtectionScope.CurrentUser))
		{
		}

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="symmetricAlgorithmProviderData">The corresponding runtime configuration data.</param>
        public SymmetricAlgorithmProviderNode(SymmetricAlgorithmProviderData symmetricAlgorithmProviderData) : base(symmetricAlgorithmProviderData)
        {
            key = new ProtectedKeySettings(symmetricAlgorithmProviderData.ProtectedKeyFilename, symmetricAlgorithmProviderData.ProtectedKeyProtectionScope);
            algorithmType = symmetricAlgorithmProviderData.AlgorithmType;
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Security.Cryptography.SymmetricAlgorithm"/> type.
        /// </summary>
        [ReadOnly(true)]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("SymmetricAlgorithmTypeDescription", typeof(Resources))]
        public Type AlgorithmType
        {
            get { return algorithmType ; }
            set { algorithmType  = value; }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [Required]
        [Editor(typeof(KeyManagerEditor), typeof(UITypeEditor))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("SymmetricAlgorithmKeyDescription", typeof(Resources))]
        [EnvironmentOverridable(false)]
        public ProtectedKeySettings Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// Gets a <see cref="SymmetricProviderData"/> configuration object using the node data.
        /// </summary>
        /// <value>
        /// A <see cref="SymmetricProviderData"/> configuration object using the node data.
        /// </value>
        public override SymmetricProviderData SymmetricCryptoProviderData
        {
            get
            {
                return new SymmetricAlgorithmProviderData(Name, AlgorithmType, Key.Filename, Key.Scope);
            }
        }

        ProtectedKeySettings ICryptographicKeyConfigurationNode.KeySettings
        {
            get { return Key; }
        }

        IKeyCreator ICryptographicKeyConfigurationNode.KeyCreator
        {
            get { return new SymmetricAlgorithmKeyCreator(AlgorithmType); }
        }

		///// <summary>
		///// Perform custom validation for this node.
		///// </summary>
		///// <param name="errors">The list of errors to add any validation errors.</param>
		//public override void Validate(IList<ValidationError> errors)
		//{
		//    if (key == null || key.ProtectedKey == null || key.ProtectedKey.DecryptedKey == null)
		//    {
		//        ValidationError error = new ValidationError(this, Resources.KeyPropertyName, Resources.InvalidKeyWhenSavingError);
		//        errors.Add(error);
		//    }
		//}
    }
}