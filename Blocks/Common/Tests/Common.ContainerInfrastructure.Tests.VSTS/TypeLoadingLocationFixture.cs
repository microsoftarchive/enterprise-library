//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.ContainerInfrastructure.Tests.VSTS
{
    [TestClass]
    public class GivenATypeLoadingLocationStrategyWithAnUnloadableType
    {
        private TypeLoadingLocationStrategy loadStrategy;

        [TestInitialize]
        public void Given()
        {
            loadStrategy = new TypeLoadingLocationStrategy("bad type name");
        }

        [TestMethod]
        public void WhenCallingStrategy_ThenItReturnsNull()
        {
            var provider = loadStrategy.GetProvider(new DictionaryConfigurationSource());

            Assert.IsNull(provider);
        }
    }

    [TestClass]
    public class GivenATypeLoadingLocationStrategyWithAnUncreatableType
    {
        class BadProvider : ITypeRegistrationsProvider
        {
            public IEnumerable<TypeRegistration> CreateRegistrations()
            {
                throw new NotImplementedException();
            }
        }

        private TypeLoadingLocationStrategy loadStrategy;

        [TestInitialize]
        public void Given()
        {
            loadStrategy = new TypeLoadingLocationStrategy(typeof(BadProvider).AssemblyQualifiedName);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public void WhenCallingStrategy_ThenItThrows()
        {
            loadStrategy.GetProvider(new DictionaryConfigurationSource());
        }
	
    }

}
