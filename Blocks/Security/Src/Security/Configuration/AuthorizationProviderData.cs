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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents the common configuration data for all authorization providers.
    /// </summary>
    [ViewModel(SecurityDesignTime.ViewModelTypeNames.AuthorizationProviderDataViewModel)]
    public class AuthorizationProviderData : NameTypeConfigurationElement
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="AuthorizationProviderData"/> class.
        /// </summary>
        public AuthorizationProviderData()
        {
        }

		/// <summary>
		/// Initialize an instance of the <see cref="AuthorizationProviderData"/> class.
		/// </summary>
		/// <param name="name">The name of the element.</param>
		/// <param name="type">The <see cref="Type"/> that this element is the configuration for.</param>
		public AuthorizationProviderData(string name, Type type)
			: base(name, type)
		{
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            Expression<Func<IAuthorizationProvider>> newExpression =  GetCreationExpression();

            yield return new TypeRegistration<IAuthorizationProvider>(newExpression)
            {
                Name = this.Name
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationSource"></param>
        /// <returns></returns>
        protected TypeRegistration GetInstrumentationProviderRegistration(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);

            return new TypeRegistration<IAuthorizationProviderInstrumentationProvider>(
                () => new AuthorizationProviderInstrumentationProvider(
                    Name,
                    instrumentationSection.PerformanceCountersEnabled,
                    instrumentationSection.EventLoggingEnabled,
                    instrumentationSection.WmiEnabled,
                    instrumentationSection.ApplicationInstanceName))
            {
                Name = Name,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }


        /// <summary>
        /// Gets the creation expression used to produce a <see cref="TypeRegistration"/> during
        /// <see cref="GetRegistrations"/>.
        /// </summary>
        /// <remarks>
        /// This must be overridden by a subclass, but is not marked as abstract due to configuration serialization needs.
        /// </remarks>
        /// <returns>An Expression that creates a <see cref="IAuthorizationProvider"/></returns>
        /// <exception cref="NotImplementedException" />
        protected virtual Expression<Func<IAuthorizationProvider>> GetCreationExpression()
        {
            throw new NotImplementedException(Resources.ExceptionMustBeImplementedBySubclass);
        }
    }
}
