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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class SecurityCacheFactoryFixture
    {
        [TestMethod]
        public void CanCreateDefaultSecurityCacheProviderFromConfiguration()
        {
            ISecurityCacheProvider provider = SecurityCacheFactory.GetSecurityCacheProvider();
        }

        [TestMethod]
        public void CanCreateSecurityCacheProviderFromConfiguration()
        {
            ISecurityCacheProvider provider = SecurityCacheFactory.GetSecurityCacheProvider("provider1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToCreateSecurityCacheProviderFromConfigurationWithNullName()
        {
            ISecurityCacheProvider provider = SecurityCacheFactory.GetSecurityCacheProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ActivationException))]
        public void TryToCreateSecurityCacheProviderFromConfigurationThatDoesNotExist()
        {
            ISecurityCacheProvider provider =  SecurityCacheFactory.GetSecurityCacheProvider("provider3");
        }
    }
}
