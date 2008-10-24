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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design
{
	/// <summary>
	/// Represents a command for adding a database cache storage provider.
	/// </summary>
	public class AddDataCacheStorageCommand : AddChildNodeCommand
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="AddDataCacheStorageCommand"/> class with an <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		public AddDataCacheStorageCommand(IServiceProvider serviceProvider)
			: base(serviceProvider, typeof(DataCacheStorageNode))
		{

		}

		/// <summary>
		/// <para>Adds the <see cref="DataCacheStorageNode"/> to the current <see cref="CacheManagerNode"/>. If the Data Application Block configuration is not added to the current application, it is also added.</para>
		/// </summary>
		/// <param name="e"><para>An <see cref="EventArgs"/> containing the event data.</para></param>
		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
			DataCacheStorageNode node = ChildNode as DataCacheStorageNode;
			Debug.Assert(null != node, "Expected a DataCacheStorageNode");

			if (null == CurrentHierarchy.FindNodeByType(typeof(DatabaseSectionNode)))
			{				
				new AddDatabaseSectionNodeCommand(ServiceProvider).Execute(CurrentHierarchy.RootNode);
			}
		}
	}
}
