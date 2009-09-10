//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Configuration settings for a DPAPI Symmetric Cryptography Provider.</para>
    /// </summary>		
    public class DpapiSymmetricCryptoProviderData : SymmetricProviderData
    {
        private const string scopeProperty = "scope";

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DpapiSymmetricCryptoProviderData"/> class.</para>
        /// </summary>
        public DpapiSymmetricCryptoProviderData() 
        {
            Type = typeof(DpapiSymmetricCryptoProvider);
        }

        /// <summary>
        /// <para>Initialize a new instnace of the <see cref="DpapiSymmetricCryptoProviderData"/> class with entropy and a <see cref="DataProtectionScope"/>.</para>
        /// </summary>
        /// <param name="name">The name of the configued instance.</param>		
        /// <param name="scope">
        /// <para>One of the <see cref="DataProtectionScope"/> values.</para>
        /// </param> 
        public DpapiSymmetricCryptoProviderData(string name, DataProtectionScope scope)
            : base(name, typeof (DpapiSymmetricCryptoProvider))
        {
            Scope = scope;
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="ProtectedData"/> storage scope.</para>
        /// </summary>
        /// <value>
        /// <para>One of the <see cref="DataProtectionScope"/> values.</para>
        /// </value>		
        [ConfigurationProperty(scopeProperty, IsRequired = false, DefaultValue = DataProtectionScope.CurrentUser)]
        //[TypeConverter(typeof(EnumConverter))]
            public DataProtectionScope Scope
        {
            get { return (DataProtectionScope) this[scopeProperty]; }
            set { this[scopeProperty] = value; }
        }

        /// <summary>
        /// Creates a <see cref="TypeRegistration"/> instance describing the provider represented by 
        /// this configuration object.
        /// </summary>
        /// <returns>A <see cref="TypeRegistration"/> instance describing a provider.</returns>
        /// <param name="configurationSource">TODO</param>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            yield return
                new TypeRegistration<ISymmetricCryptoProvider>(
                    () => new DpapiSymmetricCryptoProvider(Scope)
                    )
                    {
                        Lifetime = TypeRegistrationLifetime.Transient,
                        Name = Name
                    };
        }

    }
}
