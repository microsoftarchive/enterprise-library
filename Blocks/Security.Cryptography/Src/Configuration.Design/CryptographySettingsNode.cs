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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="CryptographySettings"/>.
    /// </summary>
    [Image(typeof(CryptographySettingsNode))]
    public class CryptographySettingsNode : ConfigurationSectionNode
    {
        private SymmetricCryptoProviderNode defaultSymmetricCryptoProviderNode;
        private HashProviderNode defaultHashProviderNode;

        private EventHandler<ConfigurationNodeChangedEventArgs> defaultHashProviderNodeRemoved;
        private EventHandler<ConfigurationNodeChangedEventArgs> defaultSymmetricCryptoProviderNodeRemoved;


        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public CryptographySettingsNode()
            : base(Resources.CryptographyNodeName)
        {
            defaultHashProviderNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnDefaultHashProviderNodeRemoved);
            defaultSymmetricCryptoProviderNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnDefaultSymmetricCryptoProviderNodeRemoved);
        }

        /// <summary>
        /// Gets if children added to the node are sorted. Nodes are not sorted added to this node.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if nodes add are sorted; otherwise <see langword="false"/>. The default is <see langword="true"/>.
        /// </value>
        public override bool SortChildren
        {
            get{return false;}
        }

        /// <summary>
        /// <para>Gets or sets the name for the node.</para>
        /// </summary>
        /// <value>
        /// <para>The display name for the node.</para>
        /// </value>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
        }


        /// <summary>
        /// Gets or sets the default hash provider.
        /// </summary>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(HashProviderNode))]
        [SRDescription("DefaultHashProviderDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public HashProviderNode DefaultHashProvider
        {
            get { return defaultHashProviderNode; }
            set
            {
                defaultHashProviderNode = LinkNodeHelper.CreateReference<HashProviderNode>(defaultHashProviderNode,
                                                                                    value,
                                                                                    defaultHashProviderNodeRemoved,
                                                                                    null);
            }
        }

        /// <summary>
        /// Gets or sets the default symmetric cryptography provider.
        /// </summary>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(SymmetricCryptoProviderNode))]
        [SRDescription("DefaultSymmetricCryptoProviderDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public SymmetricCryptoProviderNode DefaultSymmetricCryptoProvider
        {
            get { return defaultSymmetricCryptoProviderNode; }
            set
            {
                defaultSymmetricCryptoProviderNode = LinkNodeHelper.CreateReference<SymmetricCryptoProviderNode>(defaultSymmetricCryptoProviderNode,
                                                                                    value,
                                                                                    defaultSymmetricCryptoProviderNodeRemoved,
                                                                                    null);
            }
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="CryptographySettingsNode "/> and optionally releases the managed resources.</para>
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
                    if (null != defaultHashProviderNode)
                    {
                        defaultHashProviderNode.Removed -= defaultHashProviderNodeRemoved;
                    }

                    if (null != defaultSymmetricCryptoProviderNode)
                    {
                        defaultSymmetricCryptoProviderNode.Removed -= defaultSymmetricCryptoProviderNodeRemoved;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private void OnDefaultHashProviderNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.defaultHashProviderNode = null;
        }


        private void OnDefaultSymmetricCryptoProviderNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.defaultSymmetricCryptoProviderNode = null;
        }
    }
}
