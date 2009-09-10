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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests.Configuration.Fluent
{

    [TestClass]
    public class When_ConfiguringCustomAuthorizationProvider : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        IAuthorizeUsingCustomProvider authUsingCustom;

        protected override void Act()
        {
            authUsingCustom = ConfigureSecuritySettings.AuthorizeUsingCustomProviderNamed("custom provider", typeof(CustomAuthorizationProvider));
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataIsAddedToSecurityConfiguration()
        {
            Assert.AreEqual(1, AuthorizationProviders.OfType<CustomAuthorizationProviderData>().Count());
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataHasAppropriateName()
        {
            var customAuthZProvider = AuthorizationProviders.OfType<CustomAuthorizationProviderData>().First();
            Assert.AreEqual("custom provider", customAuthZProvider.Name);
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataHasAppropriateType()
        {
            var customAuthZProvider = AuthorizationProviders.OfType<CustomAuthorizationProviderData>().First();
            Assert.AreEqual(typeof(CustomAuthorizationProvider), customAuthZProvider.Type);
        }

        [TestMethod]
        public void Then_CannAddAnotherCustomAuthorizationProvider()
        {
            authUsingCustom.AuthorizeUsingCustomProviderNamed<CustomAuthorizationProvider>("another");
            Assert.AreEqual(2, AuthorizationProviders.Count());
        }
    }


    [TestClass]
    public class When_ConfiguringCustomAuthorizationProviderPassingNullForName : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_AuthorizeUsingCustomProviderNamed_ThrowsArgumentException()
        {
            ConfigureSecuritySettings.AuthorizeUsingCustomProviderNamed(null, typeof(CustomAuthorizationProvider));
        }
    }

    [TestClass]
    public class When_ConfiguringCustomAuthorizationProviderPassingNullForType : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_AuthorizeUsingCustomProviderNamed_ThrowsArgumentNullException()
        {
            ConfigureSecuritySettings.AuthorizeUsingCustomProviderNamed("custom authz", null);
        }
    }

    [TestClass]
    public class When_ConfiguringCustomAuthorizationProviderPassingWrongType : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_AuthorizeUsingCustomProviderNamed_ThrowsArgumentException()
        {
            ConfigureSecuritySettings.AuthorizeUsingCustomProviderNamed("custom authz", typeof(object));
        }
    }

    [TestClass]
    public class When_ConfiguringCustomAuthorizationProviderPassingNullForAttributes : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_AuthorizeUsingCustomProviderNamed_ThrowsArgumentNullException()
        {
            ConfigureSecuritySettings.AuthorizeUsingCustomProviderNamed("custom authz", typeof(CustomAuthorizationProvider), null);
        }
    }

    [TestClass]
    public class When_ConfiguringCustomAuthorizationProviderGeneric : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        IAuthorizeUsingCustomProvider authUsingCustom;
        
        protected override void Act()
        {
            authUsingCustom = ConfigureSecuritySettings.AuthorizeUsingCustomProviderNamed<CustomAuthorizationProvider>("custom provider");
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataIsAddedToSecurityConfiguration()
        {
            Assert.AreEqual(1, AuthorizationProviders.OfType<CustomAuthorizationProviderData>().Count());
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataHasAppropriateName()
        {
            var customAuthZProvider = AuthorizationProviders.OfType<CustomAuthorizationProviderData>().First();
            Assert.AreEqual("custom provider", customAuthZProvider.Name);
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataHasAppropriateType()
        {
            var customAuthZProvider = AuthorizationProviders.OfType<CustomAuthorizationProviderData>().First();
            Assert.AreEqual(typeof(CustomAuthorizationProvider), customAuthZProvider.Type);
        }

        [TestMethod]
        public void Then_CannAddAnotherCustomAuthorizationProvider()
        {
            authUsingCustom.AuthorizeUsingCustomProviderNamed<CustomAuthorizationProvider>("another");
            Assert.AreEqual(2, AuthorizationProviders.Count());
        }
    }

    [TestClass]
    public class When_ConfiguringCustomAuthorizationProviderPassingAttributes : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        IAuthorizeUsingCustomProvider authUsingCustom;
        NameValueCollection attributes;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            authUsingCustom = ConfigureSecuritySettings.AuthorizeUsingCustomProviderNamed("custom provider", typeof(CustomAuthorizationProvider), attributes);
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataIsAddedToSecurityConfiguration()
        {
            Assert.AreEqual(1, AuthorizationProviders.OfType<CustomAuthorizationProviderData>().Count());
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataHasAppropriateName()
        {
            var customAuthZProvider = AuthorizationProviders.OfType<CustomAuthorizationProviderData>().First();
            Assert.AreEqual("custom provider", customAuthZProvider.Name);
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataHasAppropriateType()
        {
            var customAuthZProvider = AuthorizationProviders.OfType<CustomAuthorizationProviderData>().First();
            Assert.AreEqual(typeof(CustomAuthorizationProvider), customAuthZProvider.Type);
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataHasAppropriateAttributes()
        {
            var customAuthZProvider = AuthorizationProviders.OfType<CustomAuthorizationProviderData>().First();
            Assert.AreEqual(attributes.Count, customAuthZProvider.Attributes.Count);
            foreach (string key in attributes)
            {
                Assert.AreEqual(attributes[key], customAuthZProvider.Attributes[key]);
            }
        }

        [TestMethod]
        public void Then_CannAddAnotherCustomAuthorizationProvider()
        {
            authUsingCustom.AuthorizeUsingCustomProviderNamed<CustomAuthorizationProvider>("another");
            Assert.AreEqual(2, AuthorizationProviders.Count());
        }
    }


    [TestClass]
    public class When_ConfiguringCustomAuthorizationProviderPassingAttributesGeneric : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        IAuthorizeUsingCustomProvider authUsingCustom;
        NameValueCollection attributes;

        protected override void Act()
        {
            attributes = new NameValueCollection();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");

            authUsingCustom = ConfigureSecuritySettings.AuthorizeUsingCustomProviderNamed<CustomAuthorizationProvider>("custom provider", attributes);
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataIsAddedToSecurityConfiguration()
        {
            Assert.AreEqual(1, AuthorizationProviders.OfType<CustomAuthorizationProviderData>().Count());
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataHasAppropriateName()
        {
            var customAuthZProvider = AuthorizationProviders.OfType<CustomAuthorizationProviderData>().First();
            Assert.AreEqual("custom provider", customAuthZProvider.Name);
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataHasAppropriateType()
        {
            var customAuthZProvider = AuthorizationProviders.OfType<CustomAuthorizationProviderData>().First();
            Assert.AreEqual(typeof(CustomAuthorizationProvider), customAuthZProvider.Type);
        }

        [TestMethod]
        public void Then_CustomAuthorizationProviderDataHasAppropriateAttributes()
        {
            var customAuthZProvider = AuthorizationProviders.OfType<CustomAuthorizationProviderData>().First();
            Assert.AreEqual(attributes.Count, customAuthZProvider.Attributes.Count);
            foreach (string key in attributes)
            {
                Assert.AreEqual(attributes[key], customAuthZProvider.Attributes[key]);
            }
        }

        [TestMethod]
        public void Then_CannAddAnotherCustomAuthorizationProvider()
        {
            authUsingCustom.AuthorizeUsingCustomProviderNamed<CustomAuthorizationProvider>("another");
            Assert.AreEqual(2, AuthorizationProviders.Count());
        }
    }


    [TestClass]
    public class When_CallingSetAsDefaultOnCustomAuthZProvider : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            var authUsingCustom = ConfigureSecuritySettings.AuthorizeUsingCustomProviderNamed<CustomAuthorizationProvider>("custom provider");
            authUsingCustom.SetAsDefault();
        }

        [TestMethod]
        public void Then_SecurityConfigurationHasDefaultAuthZProvider()
        {
            Assert.AreEqual("custom provider", GetSecuritySettings().DefaultAuthorizationProviderName);
        }
    }
    
}
