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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Represents the designtime logic for a collection of <see cref="HashProviderNode"/>s.
    /// </summary>
    [Image(typeof(HashProviderCollectionNode))]
    public class HashProviderCollectionNode : ConfigurationNode
    {

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public HashProviderCollectionNode() : base(Resources.HashProviderCollectionNodeName)
        {
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
    }
}
