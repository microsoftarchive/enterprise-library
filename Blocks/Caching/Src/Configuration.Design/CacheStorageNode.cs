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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
	/// <summary>
	/// Represents a cache storage provider. The class is abstract.
	/// </summary>
	[Image(typeof(CacheStorageNode))]
	[SelectedImage(typeof(CacheStorageNode))]
	public abstract class CacheStorageNode : ConfigurationNode
	{
		private string storageEncryptionName;		

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheStorageNode"/> class.
		/// </summary>        
		protected CacheStorageNode()
		{			
		}		

		/// <summary>
		/// Gets a <see cref="CacheStorageData"/> configuration object from the nodes data.
		/// </summary>
		/// <value>
		/// A <see cref="CacheStorageData"/> configuration object from the nodes data.
		/// </value>
		[Browsable(false)]
		public abstract CacheStorageData CacheStorageData { get; }

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		[UniqueName(typeof(CacheStorageNode), typeof(CacheManagerSettingsNode))]
		public override string Name
		{
			get { return base.Name; }
		}

		/// <summary>
		/// Gets the name of the storage encryption provider to use for this storage.
		/// </summary>		
		/// <value>
		/// The name of the storage encryption provider to use for this storage.
		/// </value>
		protected string StorageEncryptionName
		{
			get { return storageEncryptionName; }
		}

		/// <summary>
		/// Raises the <see cref="ConfigurationNode.ChildAdded"/> event and sets the storage encryption provider name based on the child node added that must be a <see cref="CacheStorageEncryptionNode"/>.
		/// </summary>
		/// <param name="e">A <see cref="ConfigurationChangedEventArgs"/> that contains the event data.</param>
		protected override void OnChildAdded(ConfigurationNodeChangedEventArgs e)
		{
            base.OnChildAdded(e);
			CacheStorageEncryptionNode node = e.Node as CacheStorageEncryptionNode;
			Debug.Assert(null != node, "Only a CacheStorageEncryptionNode can be added to a CacheStorageNode");

			storageEncryptionName = node.Name;

		}
	}
}
