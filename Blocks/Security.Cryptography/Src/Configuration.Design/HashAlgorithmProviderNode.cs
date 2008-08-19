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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="HashAlgorithmProviderData"/>.
    /// </summary>
    public class HashAlgorithmProviderNode : HashProviderNode
    {
        private Type algorithmType;
        private bool saltEnabled;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public HashAlgorithmProviderNode()
            : this(new HashAlgorithmProviderData(Resources.HashAlgorithmProviderNodeName, null, true))
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="hashAlgorithmProviderData">The corresponding runtime configuration data.</param>
        public HashAlgorithmProviderNode(HashAlgorithmProviderData hashAlgorithmProviderData) : base(hashAlgorithmProviderData)
        {
            algorithmType = hashAlgorithmProviderData.AlgorithmType;
            saltEnabled = hashAlgorithmProviderData.SaltEnabled;
        }

        /// <summary>
        /// Gets or sets the assembly qualified name of the <see cref="System.Security.Cryptography.HashAlgorithm"/>.
        /// </summary>
        [ReadOnly(true)]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("HashAlgorithmTypeDescription", typeof(Resources))]
        public Type AlgorithmType
        {
            get { return algorithmType; }
            set { algorithmType = value; }
        }

        /// <summary>
        /// Gets or sets the salt enabled flag.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("SaltEnabledDescription", typeof(Resources))]
        public bool SaltEnabled
        {
            get { return saltEnabled; }
            set { saltEnabled = value; }
        }

        /// <summary>
        /// Gets a <see cref="HashProviderData"/> configuration object using the node data.
        /// </summary>
        /// <value>
        /// A <see cref="HashProviderData"/> configuration object using the node data.
        /// </value>
        public override HashProviderData HashProviderData
        {
            get {return new HashAlgorithmProviderData(Name, AlgorithmType, SaltEnabled);}
        }
    }
}