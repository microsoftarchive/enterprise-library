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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Properties;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
	/// Represents a collection of <see cref="SecurityCacheProviderData"/> configuration elements.
    /// </summary>
    [Image(typeof(SecurityCacheProviderCollectionNode))]
    [SelectedImage(typeof(SecurityCacheProviderCollectionNode))]
    public sealed class SecurityCacheProviderCollectionNode : ConfigurationNode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="SecurityCacheProviderCollectionNode"/> class.
        /// </summary>
        public SecurityCacheProviderCollectionNode()
			: base(Resources.SecurityCacheCollectionNodeDefaultName)
        {
			
        }        

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }            
        }       
    }
}