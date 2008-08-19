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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
	/// <summary>
	/// Represents a command for adding the Caching Application Block to the current application.
	/// </summary>
	public class AddCacheManagerSettingsNodeCommand : AddChildNodeCommand
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="AddCacheManagerSettingsNodeCommand"/> class with an <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		public AddCacheManagerSettingsNodeCommand(IServiceProvider serviceProvider)
			: base(serviceProvider, typeof(CacheManagerSettingsNode))
		{

		}

		/// <summary>
		/// <para>Adds the <see cref="CacheManagerSettingsNode"/> to the current application with a default <see cref="CacheManagerNode"/>.</para>
		/// </summary>
		/// <param name="e"><para>An <see cref="EventArgs"/> containing the event data.</para></param>
		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
			CacheManagerSettingsNode node = ChildNode as CacheManagerSettingsNode;
			Debug.Assert(null != node, "The added node should be a CacheManagerSettingsNode");

			CacheManagerCollectionNode cacheManagerCollectionNode = new CacheManagerCollectionNode();
			node.AddNode(cacheManagerCollectionNode);
			int defaultNodeIdx = cacheManagerCollectionNode.AddNode(new CacheManagerNode());
			node.DefaultCacheManager = (CacheManagerNode)cacheManagerCollectionNode.Nodes[defaultNodeIdx];			
		}
	}
}
