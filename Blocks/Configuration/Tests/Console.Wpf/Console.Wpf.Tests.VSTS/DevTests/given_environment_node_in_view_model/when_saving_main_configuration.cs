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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using System.IO;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Console.Wpf.Tests.VSTS.DevTests.given_environment_node_in_view_model
{
    [TestClass]
    public class when_saving_main_configuration :given_environmental_overrides_and_ehab
    {
        string targetDeltaFile;
        string targetMainFile;

        protected override void Arrange()
        {
            targetDeltaFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.dconfig", Guid.NewGuid()));
            targetMainFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.config", Guid.NewGuid()));
            base.Arrange();

            ApplicationViewModel applicationModel = (ApplicationViewModel)Container.Resolve<IApplicationModel>();
            applicationModel.ConfigurationFilePath = targetMainFile;

            ((EnvironmentSourceViewModel)base.EnvironmentViewModel).EnvironmentDeltaFile = targetDeltaFile;
        }

        protected override void Act()
        {
            ApplicationViewModel applicationModel = (ApplicationViewModel)Container.Resolve<IApplicationModel>();
            applicationModel.Save();
        }

        [TestMethod]
        public void then_environments_are_saved()
        {
            Assert.IsTrue(File.Exists(targetDeltaFile));   
        }

        protected override void Teardown()
        {
            File.Delete(targetDeltaFile);
            File.Delete(targetMainFile);
        }
    }
}
