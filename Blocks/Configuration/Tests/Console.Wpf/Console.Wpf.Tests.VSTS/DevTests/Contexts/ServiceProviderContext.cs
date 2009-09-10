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

using System.ComponentModel.Design;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Console.Wpf.Tests.VSTS.DevTests.Contexts
{
    public class ServiceProviderContext : ArrangeActAssert
    {
        protected internal ServiceContainer ServiceProvider { get; set; }

        protected override void Arrange()
        {
            base.Arrange();
            ServiceProvider = new ServiceContainer();

            ServiceProvider.AddService(typeof(DiscoverDerivedConfigurationTypesService),
                                       new DiscoverDerivedConfigurationTypesService(new BinPathProbingAssemblyLocator()));
            ServiceProvider.AddService(typeof(MergeableConfigurationCollectionService), new MergeableConfigurationCollectionService());
        }
    }
}
