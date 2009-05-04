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
using System.Drawing;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
	/// <summary>
	/// Represents a cache manager implementation. The class is abstract.
	/// </summary>
	[Image(typeof(CacheManagerBaseNode))]
	[SelectedImage(typeof(CacheManagerBaseNode))]
	public abstract class CacheManagerBaseNode : ConfigurationNode
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="CacheManagerBaseNode"/> class.
		/// </summary>        
		protected CacheManagerBaseNode()
		{			
		}		

		/// <summary>
		/// Gets a <see cref="CacheManagerDataBase"/> configuration object from the nodes data.
		/// </summary>
		/// <value>
		/// A <see cref="CacheManagerDataBase"/> configuration object from the nodes data.
		/// </value>
		[Browsable(false)]
		public abstract CacheManagerDataBase CacheManagerData { get; }

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		[UniqueName(typeof(CacheManagerBaseNode), typeof(CacheManagerSettingsNode))]
		public override string Name
		{
			get { return base.Name; }
		}
	}
}
