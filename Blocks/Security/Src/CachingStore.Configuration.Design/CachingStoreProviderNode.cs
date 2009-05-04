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

using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design
{
    /// <summary>
    /// Represents a design time representation of a <see cref="CachingStoreProviderData"/> configuration element.
    /// </summary>
    public sealed class CachingStoreProviderNode : SecurityCacheProviderNode
    {
        const int absoluteExpirationDefault = 60;
        const int slidingExpirationDefault = 10;

        int absoluteExpiration;
        internal string cacheManagerName;
        CacheManagerNode cacheManagerNode;
        EventHandler<ConfigurationNodeChangedEventArgs> onCacheManagerNodeRemoved;
        EventHandler<ConfigurationNodeChangedEventArgs> onCacheManagerNodeRenamed;
        int slidingExpiration;

        /// <summary>
        /// Initialize a new instance of the <see cref="CachingStoreProvider"/> class.
        /// </summary>
        public CachingStoreProviderNode()
            : this(new CachingStoreProviderData(Resources.SecurityInstance, slidingExpirationDefault, absoluteExpirationDefault, string.Empty)) {}

        /// <summary>
        /// Initialize a new instance of the <see cref="CachingStoreProvider"/> class with a <see cref="CachingStoreProviderData"/> instance.
        /// </summary>
        /// <param name="data">A <see cref="CachingStoreProviderData"/> instance</param>
        public CachingStoreProviderNode(CachingStoreProviderData data)
            : base()
        {
            if (null == data) throw new ArgumentNullException("data");

            onCacheManagerNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnCacheManagerNodeRemoved);
            onCacheManagerNodeRenamed = new EventHandler<ConfigurationNodeChangedEventArgs>(OnCacheManagerNodeRenamed);
            Rename(data.Name);
            absoluteExpiration = data.AbsoluteExpiration;
            slidingExpiration = data.SlidingExpiration;
            cacheManagerName = data.CacheManager;
        }

        /// <summary>
        /// Gets or sets the absolute expiration.
        /// </summary>
        /// <value>
        /// The absolute expiration.
        /// </value>
        [Required]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [SRDescription("AbsoluteExpirationDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int AbsoluteExpiration
        {
            get { return absoluteExpiration; }
            set { absoluteExpiration = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="CacheManagerNode"/> to use to cache the security credentials.
        /// </summary>
        /// <value>
        /// The <see cref="CacheManagerNode"/> to use to cache the security credentials.
        /// </value>
        [Required]
        [SRDescription("CacheInstanceDescription", typeof(Resources))]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(CacheManagerNode))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public CacheManagerNode CacheManager
        {
            get { return cacheManagerNode; }
            set
            {
                cacheManagerNode = LinkNodeHelper.CreateReference<CacheManagerNode>(cacheManagerNode,
                                                                                    value,
                                                                                    onCacheManagerNodeRemoved,
                                                                                    onCacheManagerNodeRenamed);

                cacheManagerName = (cacheManagerNode == null) ? String.Empty : cacheManagerNode.Name;
            }
        }

        /// <summary>
        /// Gets the <see cref="CachingStoreProviderData"/> this node represents.
        /// </summary>
        /// <value>
        /// The <see cref="CachingStoreProviderData"/> this node represents.
        /// </value>
        public override SecurityCacheProviderData SecurityCacheProviderData
        {
            get { return new CachingStoreProviderData(Name, slidingExpiration, absoluteExpiration, cacheManagerName); }
        }

        /// <summary>
        /// Gets or sets the sliding expiration.
        /// </summary>
        /// <value>
        /// The sliding expiration.
        /// </value>
        [Required]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [SRDescription("SlidingExpirationDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int SlidingExpiration
        {
            get { return slidingExpiration; }
            set { slidingExpiration = value; }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="CachingStoreProviderNode"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (cacheManagerNode != null)
                {
                    cacheManagerNode.Removed -= onCacheManagerNodeRemoved;
                    cacheManagerNode.Renamed -= onCacheManagerNodeRenamed;
                }
            }
            base.Dispose(disposing);
        }

        void OnCacheManagerNodeRemoved(object sender,
                                       ConfigurationNodeChangedEventArgs e)
        {
            cacheManagerNode = null;
        }

        void OnCacheManagerNodeRenamed(object sender,
                                       ConfigurationNodeChangedEventArgs e)
        {
            cacheManagerName = e.Node.Name;
        }
    }
}
