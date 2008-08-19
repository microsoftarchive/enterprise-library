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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
	/// <summary>
	/// Represents a Cache Manager defined in the application's configuration.
	/// </summary>
    public class CacheManagerNode : CacheManagerBaseNode
    {
        internal const int defaultExpirationPollFreq = 60;
        internal const int defaultMaxElementsInCache = 1000;
        internal const int defaultNumberToRemoveWhenScavenging = 10;

		private int expirationPollFrequencyInSeconds;
		private int maximumElementsInCacheBeforeScavenging;
		private int numberToRemoveWhenScavenging;
		private string cacheStorageName;		

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheManagerNode"/> class.
        /// </summary>
        public CacheManagerNode()
			: this(new CacheManagerData(Resources.DefaultCacheManagerName, defaultExpirationPollFreq, defaultMaxElementsInCache, defaultNumberToRemoveWhenScavenging, Resources.NullStorageName))
        {
            
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheManagerNode"/> class with a <see cref="CacheManagerData"/> object.
		/// </summary>
		/// <param name="cacheManagerData">A <see cref="CacheManagerData"/> object.</param>
        public CacheManagerNode(CacheManagerData cacheManagerData)
        {
            if (cacheManagerData == null) { throw new ArgumentNullException("cacheManagerData"); }

			Rename(cacheManagerData.Name);

			this.expirationPollFrequencyInSeconds = cacheManagerData.ExpirationPollFrequencyInSeconds;
			this.maximumElementsInCacheBeforeScavenging = cacheManagerData.MaximumElementsInCacheBeforeScavenging;
			this.numberToRemoveWhenScavenging = cacheManagerData.NumberToRemoveWhenScavenging;
			this.cacheStorageName = cacheManagerData.CacheStorage;			
        }		

        /// <summary>
        /// Gets or sets the expiration poll frequency in seconds.
        /// </summary>
		/// <value>
		/// The expiration poll frequency in seconds.
		/// </value>
        [Required]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [SRDescription("ExpirationPollFrequencyInSecondsDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int ExpirationPollFrequencyInSeconds
        {
            get { return expirationPollFrequencyInSeconds; }
            set { expirationPollFrequencyInSeconds = value; }
        }

        /// <summary>
        /// Gets or sets the maximum elements in the cache before it is scavenged.
        /// </summary>
		/// <value>
		/// The maximum elements in the cache before it is scavenged.
		/// </value>
        [Required]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [SRDescription("MaximumElementsInCacheBeforeScavengingDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int MaximumElementsInCacheBeforeScavenging
        {
            get { return maximumElementsInCacheBeforeScavenging; }
            set { maximumElementsInCacheBeforeScavenging = value; }
        }

        /// <summary>
        /// Gets or set the number of items to remove from the cache when scavenging.
        /// </summary>
		/// <value>
		/// The number of items to remove from the cache when scavenging.
		/// </value>
        [Required]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [SRDescription("NumberToRemoveWhenScavengingDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int NumberToRemoveWhenScavenging
        {
            get { return numberToRemoveWhenScavenging; }
            set { numberToRemoveWhenScavenging = value; }
        }

		/// <summary>
		/// Gets the <see cref="CacheManagerDataBase"/> object to store in the application's configuration file.
		/// </summary>
		/// <value>
		/// The <see cref="CacheManagerDataBase"/> object to store in the application's configuration file.
		/// </value>
        [Browsable(false)]
        public override CacheManagerDataBase CacheManagerData
        {
            get
            {
				return new CacheManagerData(Name, expirationPollFrequencyInSeconds, maximumElementsInCacheBeforeScavenging, numberToRemoveWhenScavenging, cacheStorageName);
            }
        }		

		/// <summary>
		/// Raises the <see cref="ConfigurationNode.ChildAdded"/> event and sets the name of the cache storage based on the child <see cref="CacheStorageNode"/>.
		/// </summary>
		/// <param name="e">A <see cref="ConfigurationChangedEventArgs"/> that contains the event data.</param>
		protected override void OnChildAdded(ConfigurationNodeChangedEventArgs e)
		{
			base.OnChildAdded(e);
			CacheStorageNode node = e.Node as CacheStorageNode;
			Debug.Assert(null != node, "Only CacheStorageNode objects can be added to a CacheManagerNode");

			cacheStorageName = node.Name;
		}
	}
}