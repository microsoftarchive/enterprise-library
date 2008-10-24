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
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Represents the root configuration for the Caching Application Block.
    /// </summary>
    [Image(typeof(CacheManagerSettingsNode))]
    [SelectedImage(typeof(CacheManagerSettingsNode))]
    public class CacheManagerSettingsNode : ConfigurationSectionNode
    {
        private CacheManagerBaseNode cacheManagerBaseNode;
		private EventHandler<ConfigurationNodeChangedEventArgs> cacheManagerNodeRemovedHandler;		
     
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheManagerSettingsNode"/> class.
		/// </summary>		
        public CacheManagerSettingsNode() : base(Resources.DefaultCacheManagerSettingsNodeName)
        {
			this.cacheManagerNodeRemovedHandler = new EventHandler<ConfigurationNodeChangedEventArgs>(OnCacheManagerNodeRemoved);			
			
        }

		/// <summary>
		/// <para>Releases the unmanaged resources used by the <see cref="CacheManagerSettingsNode "/> and optionally releases the managed resources.</para>
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
					if (null != cacheManagerBaseNode)
					{
						cacheManagerBaseNode.Removed -= cacheManagerNodeRemovedHandler;
					}

				}
			}
			finally
			{
				base.Dispose(disposing);
			}			
		}

		/// <summary>
		/// <para>Gets the name for the node.</para>
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
        /// Gets or sets the default cache manager
        /// </summary>
		/// <value>
		/// The default cache manager.
		/// </value>
        [Required]
        [SRDescription("DefaultCacheManagerDescription", typeof(Resources))]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(CacheManagerBaseNode))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public CacheManagerBaseNode DefaultCacheManager
        {
            get { return cacheManagerBaseNode; }
            set
            {
                cacheManagerBaseNode = LinkNodeHelper.CreateReference<CacheManagerBaseNode>(cacheManagerBaseNode, 
                                                                                    value, 
                                                                                    cacheManagerNodeRemovedHandler, 
                                                                                    null);
            }
        }

		/// <summary>
		/// Raises the <see cref="ConfigurationNode.ChildAdded"/> event and confirms that only one <see cref="CacheManagerCollectionNode"/> has been added.
		/// </summary>
		/// <param name="e">A <see cref="ConfigurationChangedEventArgs"/> that contains the event data.</param>
		protected override void OnChildAdded(ConfigurationNodeChangedEventArgs e)
        {
			base.OnChildAdded(e);

            if (Nodes.Count > 1 && e.Node.GetType() == typeof(CacheManagerCollectionNode))
            {
                throw new InvalidOperationException(Resources.ExceptionOnlyOneCacheManagerCollectionNode);
            }
        }

        private void OnCacheManagerNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.cacheManagerBaseNode = null;
        }
    }
}
