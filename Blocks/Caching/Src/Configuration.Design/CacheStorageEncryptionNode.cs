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

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
	/// Represents the encryption for the cache storage. The class is abstract.
    /// </summary>
    [Image(typeof(CacheStorageEncryptionNode))]
    [SelectedImage(typeof(CacheStorageEncryptionNode))]
    public abstract class CacheStorageEncryptionNode : ConfigurationNode
    {       

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheStorageEncryptionNode"/> class.
		/// </summary>
        protected CacheStorageEncryptionNode() 
		{
		}

        /// <summary>
        /// Gets a <see cref="StorageEncryptionProviderData"/> configuration object based on the node data.
        /// </summary>
		/// <value>
		/// A <see cref="StorageEncryptionProviderData"/> configuration object based on the node data.
		/// </value>
		[Browsable(false)]
		public abstract StorageEncryptionProviderData StorageEncryptionProviderData { get; }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
        [UniqueName(typeof(CacheStorageEncryptionNode), typeof(CacheManagerSettingsNode))]
        public override string Name
        {
            get{return base.Name;} 
        }
    }
}