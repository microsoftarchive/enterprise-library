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
using System.Linq.Expressions;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	/// <summary>
	/// Base class for configuration data defining CacheManagerDataBase. Defines the information needed to properly configure
	/// a ICacheManager instance.
	/// </summary>    	
	public class CacheManagerDataBase : NameTypeConfigurationElement
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="CacheManagerDataBase"/> class.
		/// </summary>
		public CacheManagerDataBase()
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheManagerDataBase"/> class.
		/// </summary>
		/// <param name="name">
		/// The name of the <see cref="CacheManagerDataBase"/>.
		/// </param>
		/// <param name="type">The type of <see cref="ICacheManager"/>.</param>
		public CacheManagerDataBase(string name, Type type)
			: base(name, type)
		{
		}


        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> object needed to
        /// register the CacheManager represented by this config element.
        /// </summary>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public virtual IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            Expression<Func<ICacheManager>> newExpression = GetCacheManagerCreationExpression();

            TypeRegistration cacheManagerRegistration = new TypeRegistration<ICacheManager>(newExpression)
            {
                Name = this.Name
            };

            return new TypeRegistration[] { cacheManagerRegistration };

        }

        /// <summary>
        /// Gets the creation expression used to produce a <see cref="TypeRegistration"/> during
        /// <see cref="GetRegistrations"/>.
        /// </summary>
        /// <remarks>
        /// This must be overridden by a subclass, but is not marked as abstract due to configuration serialization needs.
        /// </remarks>
        /// <returns>A <see cref="Expression"/> that creates a <see cref="ICacheManager"/></returns>
        protected virtual Expression<Func<ICacheManager>> GetCacheManagerCreationExpression()
        {
            throw new NotImplementedException(Caching.Properties.Resources.ExceptionMethodMustBeImplementedBySubclasses);
        }
    }
}
