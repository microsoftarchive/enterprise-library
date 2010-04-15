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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_model_for_disposal
{
    [TestClass]
    public class when_removing_configuration_loaded_from_file : ContainerContext
    {
        private string configFilePath;
        private ApplicationViewModel appModel;
        private WeakReference[] elementsWeakReference;

        protected override void Arrange()
        {
            base.Arrange();

            var resourceHelper = new ResourceHelper<ConfigFiles.ConfigFileLocator>();
            string resourceFile = "configuration_with_all_blocks.config";
            resourceHelper.DumpResourceFileToDisk(resourceFile);
            configFilePath = Path.Combine(Environment.CurrentDirectory, resourceFile);

            appModel = base.Container.Resolve<ApplicationViewModel>();
            appModel.Load(configFilePath);

            elementsWeakReference = appModel.CurrentConfigurationSource.Sections.SelectMany(s => s.DescendentElements().Union(new[] {s}))
                .Select(x => new WeakReference(x)).ToArray();

        }

        protected override void Act()
        {
            appModel.New(); 
            GC.Collect();
        }

        [TestMethod]
        public void then_elements_collected()
        {
            var livingConnections = elementsWeakReference.Where(r => r.IsAlive);

            Assert.IsFalse(livingConnections.Any());
        }
    }
}
