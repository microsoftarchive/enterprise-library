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

using System.Linq;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Caching.given_caching_configuraton
{
    [TestClass]
    public class when_adding_cache_manager : DevTests.Contexts.ContainerContext
    {
        AddCacheManagerCommand addCacheManagerCommand;

        protected override void Arrange()
        {
            base.Arrange();
            this.Container.RegisterInstance<IAssemblyDiscoveryService>(new Mock<IAssemblyDiscoveryService>().Object);

            var cachingSettings = new CacheManagerSettings();
            var cachingSettingsModel = SectionViewModel.CreateSection(Container, CacheManagerSettings.SectionName, cachingSettings);

            var cacheManagerCollection = (ElementCollectionViewModel)cachingSettingsModel.GetDescendentsOfType<NameTypeConfigurationElementCollection<CacheManagerDataBase, CustomCacheManagerData>>().First();
            var cacheManagerCollectionCommands = cacheManagerCollection.Commands.SelectMany(x => x.ChildCommands);
            addCacheManagerCommand = cacheManagerCollectionCommands.OfType<AddCacheManagerCommand>().First();
        }

        protected override void Act()
        {
            addCacheManagerCommand.Execute(null);
        }

        [TestMethod]
        public void then_newly_added_cache_manager_has_backingstore_assigned()
        {
            Assert.IsFalse(string.IsNullOrEmpty((string)addCacheManagerCommand.AddedElementViewModel.Property("CacheStorage").Value));
        }

    }
}
