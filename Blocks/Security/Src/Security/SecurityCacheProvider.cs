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

using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
	/// <summary>
    /// Abstract implementation of the <see cref="ISecurityCacheProvider"/> interface.
	/// </summary>
	public abstract class SecurityCacheProvider : ISecurityCacheProvider, IInstrumentationEventProvider
	{
		private SecurityCacheProviderInstrumentationProvider instrumentationProvider;

		/// <summary>
        /// Initializes a new instance of the <see cref="SecurityCacheProvider"/> class. 
		/// </summary>
		protected SecurityCacheProvider()
		{
			this.instrumentationProvider = new SecurityCacheProviderInstrumentationProvider();
		}

		/// <summary>
		/// Caches an authenticated IIdentity object. There is no implicit
		/// expiration defined for this item.
		/// </summary>
		/// <param name="identity">
		/// IIdentity object representing an authenticated user.
		/// </param>
		/// <returns>Token to identify the cached Identity</returns>
		public abstract IToken SaveIdentity(IIdentity identity);

		/// <summary>
		/// Caches an authenticated IIdentity object using an existing token
		/// enabling the grouping of related items. There is no implicit
		/// expiration defined for this item.
		/// </summary>
		/// <param name="identity">
		/// IIdentity object representing an authenticated user.
		/// </param>
		/// <param name="token">
		/// Token identifying an existing cached entities.
		/// </param>
		public abstract void SaveIdentity(IIdentity identity, IToken token);

		/// <summary>
		/// Caches an authenticated IPrincipal object. There is no implicit
		/// expiration defined for this item.
		/// </summary>
		/// <param name="principal">
		/// IPrincipal object representing an authenticated user and roles.
		/// </param>
		/// <returns>Token to identify the cached IPrincipal object</returns>
		public abstract IToken SavePrincipal(IPrincipal principal);

		/// <summary>
		/// Caches an authenticated IPrincipal object using an existing token
		/// enabling the grouping of related items. There is no implicit
		/// expiration defined for this item.
		/// </summary>
		/// <param name="principal">
		/// IPrincipal object representing an authenticated user.
		/// </param>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public abstract void SavePrincipal(IPrincipal principal, IToken token);

		/// <summary>
		/// Caches a profile for an authenticated user. There is no implicit
		/// expiration defined for this item.
		/// </summary>
		/// <param name="profile">
		/// Object representing the profile of authenticated user.
		/// </param>
		/// <returns>Token to identify the cached Profile object</returns>
		public abstract IToken SaveProfile(object profile);

		/// <summary>
		/// Caches a profile for an authenticated user using an existing token
		/// (enabling the grouping of related items). There is no implicit
		/// expiration defined for this item.
		/// </summary>
		/// <param name="profile">
		/// Object representing the profile of authenticated user.
		/// </param>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public abstract void SaveProfile(object profile, IToken token);

		/// <summary>
		/// Purges an existing IIdentity object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public abstract void ExpireIdentity(IToken token);

		/// <summary>
		/// Purges an existing IPrincipal object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public abstract void ExpirePrincipal(IToken token);

		/// <summary>
		/// Purges an existing Profile object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		public abstract void ExpireProfile(IToken token);

		/// <summary>
		/// Gets an existing IIdentity object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		/// <returns>A cached IIdentity object</returns>
		public abstract IIdentity GetIdentity(IToken token);

		/// <summary>
		/// Gets an existing IPrincipal object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		/// <returns>A cached IPrincipal object</returns>
		public abstract IPrincipal GetPrincipal(IToken token);

		/// <summary>
		/// Gets an existing Profile object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		/// <returns>A cached Profile object</returns>
		public abstract object GetProfile(IToken token);

		/// <summary>
        /// Gets the <see cref="SecurityCacheProviderInstrumentationProvider"/> instance that defines the logical events used to instrument this Security Cache instance.
		/// </summary>
        /// <returns>The <see cref="SecurityCacheProviderInstrumentationProvider"/> instance that defines the logical events used to instrument this Security Cache instance.</returns>
		public object GetInstrumentationEventProvider()
		{
			return instrumentationProvider;
		}

		/// <summary>
        /// Gets the <see cref="SecurityCacheProviderInstrumentationProvider"/> instance that defines the logical events used to instrument this Security Cache instance.
		/// </summary>
		protected SecurityCacheProviderInstrumentationProvider InstrumentationProvider
		{
			get { return this.instrumentationProvider; }
		}
	}
}
