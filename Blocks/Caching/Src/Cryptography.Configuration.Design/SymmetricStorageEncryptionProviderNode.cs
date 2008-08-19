//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design
{
    /// <summary>
    /// Represents a symmetric cache storage encryption provider.
    /// </summary>
    public class SymmetricStorageEncryptionProviderNode : CacheStorageEncryptionNode
    {
        
        private SymmetricCryptoProviderNode symmetricCryptoProviderNode;
        private EventHandler<ConfigurationNodeChangedEventArgs> onSymmetricCryptoProviderNodeRemoved;
        internal string symmetricCryptoProviderNodeName;
        
        /// <summary>
        /// Initialize a new instance of the <see cref="SymmetricStorageEncryptionProviderNode"/> class.
        /// </summary>
        public SymmetricStorageEncryptionProviderNode()
            :this(new SymmetricStorageEncryptionProviderData(Resources.SymmetricStorageEncryptionProvider, null))
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="SymmetricStorageEncryptionProviderNode"/> class with a <see cref="SymmetricStorageEncryptionProviderData"/> configuration object.
        /// </summary>
        /// <param name="symmetricStorageEncryptionProvider">A <see cref="SymmetricStorageEncryptionProviderData"/> configuration object</param>
        public SymmetricStorageEncryptionProviderNode(SymmetricStorageEncryptionProviderData symmetricStorageEncryptionProvider)
        {
            if (symmetricStorageEncryptionProvider == null)
            {
                throw new ArgumentNullException("symmetricStorageEncryptionProvider");
            }

            Rename(symmetricStorageEncryptionProvider.Name);

            this.symmetricCryptoProviderNodeName = symmetricStorageEncryptionProvider.SymmetricInstance;
            this.onSymmetricCryptoProviderNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnSymmetricCryptoProviderNodeRemoved);
		}

        /// <summary>
		/// <para>Releases the unmanaged resources used by the <see cref="SymmetricStorageEncryptionProviderNode "/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (symmetricCryptoProviderNode != null)
                    {
                        symmetricCryptoProviderNode.Removed -= onSymmetricCryptoProviderNodeRemoved;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets or sets the symmetric cryptography provider instance to use for the cache store encryption.
        /// </summary>
        /// <value>
        /// The symmetric cryptography provider instance to use for the cache store encryption.
        /// </value>
        [Required]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(SymmetricCryptoProviderNode))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("SymmetricInstanceDescription", typeof(Resources))]
        public SymmetricCryptoProviderNode SymmetricInstance
        {
            get { return symmetricCryptoProviderNode; }
            set
            {
                symmetricCryptoProviderNode = LinkNodeHelper.CreateReference<SymmetricCryptoProviderNode>(symmetricCryptoProviderNode,
                                                                                    value,
                                                                                    OnSymmetricCryptoProviderNodeRemoved,
                                                                                    null);
            }
        }

        /// <summary>
        /// Gets a <see cref="StorageEncryptionProviderData"/> configuration object using the node data.
        /// </summary>
        /// <value>
        /// A <see cref="StorageEncryptionProviderData"/> configuration object using the node data.
        /// </value>
        public override StorageEncryptionProviderData StorageEncryptionProviderData
        {
            get { return new SymmetricStorageEncryptionProviderData(Name, (SymmetricInstance == null) ? null : SymmetricInstance.Name); }
        }
        
        private void OnSymmetricCryptoProviderNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            symmetricCryptoProviderNode = null;
        }
    }
}
