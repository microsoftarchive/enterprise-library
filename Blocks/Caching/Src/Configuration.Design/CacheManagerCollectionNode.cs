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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Represents a node that contains a collection of <see cref="CacheManagerNode"/> instances.
    /// </summary>
    [Image(typeof(CacheManagerCollectionNode))]
    [SelectedImage(typeof(CacheManagerCollectionNode))]
    public class CacheManagerCollectionNode : ConfigurationNode
    {
		/// <summary>
		/// Initialize a new instance of the <see cref="CacheManagerCollectionNode"/> class.
		/// </summary>
        public CacheManagerCollectionNode()
            : base(Resources.DefaultCacheManagerCollectionNodeName)
        {
        }

		/// <summary>
		/// Gets or sets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		/// <remarks>
		/// This is overridden so it can be marked read-only in the designer.
		/// </remarks>
		[ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
        }
    }
}