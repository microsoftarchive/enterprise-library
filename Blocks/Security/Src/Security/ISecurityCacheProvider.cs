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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
	/// <summary>
	///	Allows end users to implement their own Security Caches.
	/// </summary>
	[ConfigurationNameMapper(typeof(SecurityCacheDataRetriever))]
	[CustomFactory(typeof(SecurityCacheProviderCustomFactory))]
	public interface ISecurityCacheProvider
	{
		/// <summary>
		/// Caches an authenticated IIdentity object.
		/// </summary>
		/// <param name="identity">
		/// IIdentity object representing an authenticated user.
		/// </param>
		/// <returns>Token to identify the cached Identity</returns>
		IToken SaveIdentity(IIdentity identity);

		/// <summary>
		/// Caches an authenticated IIdentity object using an existing token
		/// enabling the grouping of related items. 
		/// </summary>
		/// <param name="identity">
		/// IIdentity object representing an authenticated user.
		/// </param>
		/// <param name="token">
		/// Token identifying an existing cached entities.
		/// </param>
		void SaveIdentity(IIdentity identity, IToken token);

		/// <summary>
		/// Caches an authenticated IPrincipal object. 
		/// </summary>
		/// <param name="principal">
		/// IPrincipal object representing an authenticated user and roles.
		/// </param>
		/// <returns>Token to identify the cached IPrincipal object</returns>
		IToken SavePrincipal(IPrincipal principal);

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
		void SavePrincipal(IPrincipal principal, IToken token);

		/// <summary>
		/// Caches a profile for an authenticated user. 
		/// </summary>
		/// <param name="profile">
		/// Object representing the profile of authenticated user.
		/// </param>
		/// <returns>Token to identify the cached Profile object</returns>
		IToken SaveProfile(object profile);

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
		void SaveProfile(object profile, IToken token);

		/// <summary>
		/// Purges an existing IIdentity object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		void ExpireIdentity(IToken token);

		/// <summary>
		/// Purges an existing IPrincipal object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		void ExpirePrincipal(IToken token);

		/// <summary>
		/// Purges an existing Profile object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		void ExpireProfile(IToken token);

		/// <summary>
		/// Gets an existing IIdentity object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		/// <returns>A cached IIdentity object</returns>
		IIdentity GetIdentity(IToken token);

		/// <summary>
		/// Gets an existing IPrincipal object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		/// <returns>A cached IPrincipal object</returns>
		IPrincipal GetPrincipal(IToken token);

		/// <summary>
		/// Gets an existing Profile object from the cache.
		/// </summary>
		/// <param name="token">
		/// Token identifying an existing cached entity.
		/// </param>
		/// <returns>A cached Profile object</returns>
		Object GetProfile(IToken token);

	}
}