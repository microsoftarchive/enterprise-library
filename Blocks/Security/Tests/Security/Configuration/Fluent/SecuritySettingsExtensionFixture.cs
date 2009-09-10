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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_AddingSecuritySettingsToConfigurationSourceBuilder : ArrangeActAssert
    {
        ConfigurationSourceBuilder configurationSourceBuilder;

        protected override void Act()
        {
            configurationSourceBuilder = new ConfigurationSourceBuilder();
            configurationSourceBuilder.ConfigureSecurity();
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsSecuritySettings()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            configurationSourceBuilder.UpdateConfigurationWithReplace(source);

            Assert.IsNotNull(source.GetSection(SecuritySettings.SectionName));
        }
    }

    public abstract class Given_SecuritySettingsInConfigurationSourceBuilder : ArrangeActAssert
    {
        IConfigurationSourceBuilder configurationSourceBuilder;
        protected IConfigureSecuritySettings ConfigureSecuritySettings;

        protected override void Arrange()
        {
            configurationSourceBuilder = new ConfigurationSourceBuilder();
            ConfigureSecuritySettings = configurationSourceBuilder.ConfigureSecurity();
        }

        protected SecuritySettings GetSecuritySettings()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            configurationSourceBuilder.UpdateConfigurationWithReplace(source);

            return (SecuritySettings)source.GetSection(SecuritySettings.SectionName);

        }

        protected IEnumerable<AuthorizationProviderData> AuthorizationProviders
        {
            get
            {
                return GetSecuritySettings().AuthorizationProviders;
            }
        }

        protected IEnumerable<SecurityCacheProviderData> SecurityCacheProviders
        {
            get
            {
                return GetSecuritySettings().SecurityCacheProviders;
            }
        }

        protected class CustomAuthorizationProvider : IAuthorizationProvider
        {
            public bool Authorize(IPrincipal principal, string context)
            {
                throw new NotImplementedException();
            }
        }

        protected class CustomSecurityCacheProvider : ISecurityCacheProvider
        {
            public IToken SaveIdentity(System.Security.Principal.IIdentity identity)
            {
                throw new NotImplementedException();
            }

            public void SaveIdentity(System.Security.Principal.IIdentity identity, IToken token)
            {
                throw new NotImplementedException();
            }

            public IToken SavePrincipal(System.Security.Principal.IPrincipal principal)
            {
                throw new NotImplementedException();
            }

            public void SavePrincipal(System.Security.Principal.IPrincipal principal, IToken token)
            {
                throw new NotImplementedException();
            }

            public IToken SaveProfile(object profile)
            {
                throw new NotImplementedException();
            }

            public void SaveProfile(object profile, IToken token)
            {
                throw new NotImplementedException();
            }

            public void ExpireIdentity(IToken token)
            {
                throw new NotImplementedException();
            }

            public void ExpirePrincipal(IToken token)
            {
                throw new NotImplementedException();
            }

            public void ExpireProfile(IToken token)
            {
                throw new NotImplementedException();
            }

            public System.Security.Principal.IIdentity GetIdentity(IToken token)
            {
                throw new NotImplementedException();
            }

            public System.Security.Principal.IPrincipal GetPrincipal(IToken token)
            {
                throw new NotImplementedException();
            }

            public object GetProfile(IToken token)
            {
                throw new NotImplementedException();
            }
        }
    }

}
