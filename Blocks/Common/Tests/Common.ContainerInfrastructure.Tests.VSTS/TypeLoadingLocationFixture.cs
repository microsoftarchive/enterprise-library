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
    public class GivenATypeLoadingLocatorWithAnUnloadableType
    {
        private TypeLoadingLocator loadStrategy;

        [TestInitialize]
        public void Given()
        {
            loadStrategy = new TypeLoadingLocator("bad type name");
        }

        [TestMethod]
        public void WhenCallingStrategy_ThenItReturnsNull()
        {
            var registrations = loadStrategy.GetRegistrations(new DictionaryConfigurationSource());

            Assert.AreEqual(0, registrations.Count());
        }
    }

    [TestClass]
    public class GivenATypeLoadingLocationStrategyWithAnUncreatableType
    {
        class BadProvider : ITypeRegistrationsProvider
        {
            public BadProvider(int someValue)
            {
            }

            public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Return the <see cref="TypeRegistration"/> objects needed to reconfigure
            /// the container after a configuration source has changed.
            /// </summary>
            /// <remarks>If there are no reregistrations, return an empty sequence.</remarks>
            /// <param name="configurationSource">The <see cref="IConfigurationSource"/> containing
            /// the configuration information.</param>
            /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
            public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
            {
                return Enumerable.Empty<TypeRegistration>();
            }
        }

        private TypeLoadingLocator loadStrategy;

        [TestInitialize]
        public void Given()
        {
            loadStrategy = new TypeLoadingLocator(typeof(BadProvider).AssemblyQualifiedName);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public void WhenCallingStrategy_ThenItThrows()
        {
            loadStrategy.GetRegistrations(new DictionaryConfigurationSource());
        }
	
    }

}
