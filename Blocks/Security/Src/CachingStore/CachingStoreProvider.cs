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
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore
{
	/// <summary>
    /// <see cref="ISecurityCacheProvider"/> implementation that caches the authenticated session information and
	/// performs token handling.
	/// </summary>
	[ConfigurationElementType(typeof(CachingStoreProviderData))]
	public class CachingStoreProvider : SecurityCacheProvider
	{
		private int slidingExpiration;
		private int absoluteExpiration;
		private ICacheManager securityCacheManager;

		/// <summary>
		/// <para>Initialize a new instance of the <see cref="CachingStoreProvider"/> class.</para>
		/// </summary>
		public CachingStoreProvider(int slidingExpiration, int absoluteExpiration, ICacheManager securityCacheManager)
            :this(slidingExpiration, absoluteExpiration, securityCacheManager, new NullSecurityCacheProviderInstrumentationProvider())
		{
		}


        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CachingStoreProvider"/> class with a specific <see cref="ISecurityCacheProviderInstrumentationProvider"/>.</para>
        /// </summary>
        public CachingStoreProvider(int slidingExpiration, int absoluteExpiration, ICacheManager securityCacheManager, ISecurityCacheProviderInstrumentationProvider instrumentationProvider)
            :base(instrumentationProvider)
        {
            this.slidingExpiration = slidingExpiration;
            this.absoluteExpiration = absoluteExpiration;
            this.securityCacheManager = securityCacheManager;
        }

		/// <summary>
        /// Caches an authenticated <see cref="IIdentity"/> object.
		/// </summary>
		/// <param name="identity">
        /// <see cref="IIdentity"/> object representing an authenticated user.
		/// </param>
		/// <returns>Token to identify the cached Identity</returns>
		public override IToken SaveIdentity(IIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}

			IToken guidToken = new GuidToken();

			SaveIdentity(identity, guidToken);

			return guidToken;
		}

		/// <summary>
        /// Caches an authenticated <see cref="IIdentity"/> object using an existing token
		/// enabling the grouping of related items.
		/// </summary>
		/// <param name="identity">
		/// IIdentity object representing an authenticated user.
		/// </param>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public override void SaveIdentity(IIdentity identity, IToken token)
		{
			GetSecurityCacheItem(token, true).Identity = identity;
		}

		/// <summary>
		/// Caches an authenticated IPrincipal object.
		/// </summary>
		/// <param name="principal">
		/// IPrincipal object representing an authenticated user and roles.
		/// </param>
		/// <returns>Token to identify the cached IPrincipal object</returns>
		public override IToken SavePrincipal(IPrincipal principal)
		{
			if (principal == null)
			{
				throw new ArgumentNullException("principal");
			}

			IToken guidToken = new GuidToken();

			SavePrincipal(principal, guidToken);

			return guidToken;
		}

		/// <summary>
		/// Caches an authenticated IPrincipal object using an existing token
		/// enabling the grouping of related items.
		/// </summary>
		/// <param name="principal">
		/// IPrincipal object representing an authenticated user.
		/// </param>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public override void SavePrincipal(IPrincipal principal, IToken token)
		{
			GetSecurityCacheItem(token, true).Principal = principal;
		}

		/// <summary>
		/// Caches a profile for an authenticated user.
		/// </summary>
		/// <param name="profile">
		/// Object representing the profile of authenticated user.
		/// </param>
		/// <returns>Token to identify the cached Profile object</returns>
		public override IToken SaveProfile(object profile)
		{
			if (profile == null)
			{
				throw new ArgumentNullException("profile");
			}

			GuidToken guidToken = new GuidToken();

			SaveProfile(profile, guidToken);

			return guidToken;
		}

		/// <summary>
		/// Caches a profile for an authenticated user using an existing token
		/// (enabling the grouping of related items).
		/// </summary>
		/// <param name="profile">
		/// Object representing the profile of authenticated user.
		/// </param>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public override void SaveProfile(object profile, IToken token)
		{
			GetSecurityCacheItem(token, true).Profile = profile;
		}

		/// <summary>
		/// Purges an existing IIdentity object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public override void ExpireIdentity(IToken token)
		{
			ExpireSecurityCacheItem(token, SecurityEntityType.Identity);
		}

		/// <summary>
		/// Purges an existing IPrincipal object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public override void ExpirePrincipal(IToken token)
		{
			ExpireSecurityCacheItem(token, SecurityEntityType.Principal);
		}

		/// <summary>
		/// Purges an existing Profile object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public override void ExpireProfile(IToken token)
		{
			ExpireSecurityCacheItem(token, SecurityEntityType.Profile);
		}

		/// <summary>
		/// Gets an existing IIdentity object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		/// <returns>A cached IIdentity object</returns>
		public override IIdentity GetIdentity(IToken token)
		{
			SecurityCacheItem item = GetSecurityCacheItem(token);
			IIdentity identity = item == null ? null : item.Identity;

			InstrumentationProvider.FireSecurityCacheReadPerformed(SecurityEntityType.Identity, token);

			return identity;
		}

		/// <summary>
		/// Gets an existing IPrincipal object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		/// <returns>A cached IPrincipal object</returns>
		public override IPrincipal GetPrincipal(IToken token)
		{
			SecurityCacheItem item = GetSecurityCacheItem(token);
			IPrincipal principal = item == null ? null : item.Principal;

            InstrumentationProvider.FireSecurityCacheReadPerformed(SecurityEntityType.Principal, token);

			return principal;
		}

		/// <summary>
		/// Gets an existing Profile object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		/// <returns>A cached Profile object</returns>
		public override object GetProfile(IToken token)
		{
			SecurityCacheItem item = GetSecurityCacheItem(token);
			object profile = item == null ? null : item.Profile;

            InstrumentationProvider.FireSecurityCacheReadPerformed(SecurityEntityType.Profile, token);

			return profile;
		}

		private void ExpireSecurityCacheItem(IToken token, SecurityEntityType entityType)
		{
			SecurityCacheItem item = GetSecurityCacheItem(token);
			if (item != null)
			{
				ClearCachItemEntity(item, entityType);

				if (item.IsRemoveable)
				{
					securityCacheManager.Remove(token.Value);
				}
			}
		}

		private void ClearCachItemEntity(SecurityCacheItem item, SecurityEntityType entityType)
		{
			switch (entityType)
			{
				case SecurityEntityType.Identity:
					item.Identity = null;
					break;
				case SecurityEntityType.Principal:
					item.Principal = null;
					break;
				case SecurityEntityType.Profile:
					item.Profile = null;
					break;
			}
		}

		private SecurityCacheItem GetSecurityCacheItem(IToken token)
		{
			return GetSecurityCacheItem(token, false);
		}

		private SecurityCacheItem GetSecurityCacheItem(IToken token, bool createIfNull)
		{
			SecurityCacheItem item = null;
			item = securityCacheManager.GetData(token.Value) as SecurityCacheItem;

			if (item == null && createIfNull)
			{
				item = new SecurityCacheItem();
				securityCacheManager.Add(token.Value, item, CacheItemPriority.Normal, null, GetCacheExpirations());
			}

			return item;
		}

		private ICacheItemExpiration[] GetCacheExpirations()
		{
			ICacheItemExpiration[] cachingExpirations = new ICacheItemExpiration[2];
			cachingExpirations[0] = new AbsoluteTime(new TimeSpan(0, 0, ConvertExpirationTimeToSeconds(absoluteExpiration)));
			cachingExpirations[1] = new SlidingTime(new TimeSpan(0, 0, ConvertExpirationTimeToSeconds(slidingExpiration)));
			return cachingExpirations;
		}

		private int ConvertExpirationTimeToSeconds(int expirationInMinutes)
		{
			return expirationInMinutes * 60;
		}
	}
}
