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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class CloseConfigurationApplicationNodeCommandFixture : ConfigurationDesignHost
    {
        CloseConfigurationApplicationCommand cmd;

        protected override void InitializeCore()
        {
            cmd = new CloseConfigurationApplicationCommand(ServiceProvider);
        }

        protected override void CleanupCore()
        {
            cmd.Dispose();
        }

        [TestMethod]
        public void CanCloseApplication()
        {
            ServiceHelper.GetUIHierarchyService(ServiceProvider).SelectedHierarchy = null;

            ConfigurationApplicationNode node = new ConfigurationApplicationNode();
            IConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(node, ServiceProvider);
            HiearchyService.AddHierarchy(hierarchy);
            UIService.SetUIDirty(hierarchy);
            cmd.Execute(node);

            Assert.AreEqual(HiearchyService.SelectedHierarchy, hierarchy);
            Assert.IsTrue(UIService.IsDirty(hierarchy));
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Assert.IsNull(HiearchyService.GetHierarchy(hierarchy.Id));
        }
    }
}
