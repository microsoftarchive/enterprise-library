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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_host_adapter
{
    [TestClass]
    public class when_violating_validation_rule : given_host_adapter
    {
        bool TasksChangedFired;

        protected override void Arrange()
        {
            base.Arrange();

            TasksChangedFired = false;
            HostAdapter.TasksChanged += (sender, args) => TasksChangedFired = true;
        }

        protected override void Act()
        {
            var cacheManager = LoggingViewModel.GetDescendentsOfType<TraceListenerData>().Single();
            try
            {
                cacheManager.Property("TraceOutputOptions").BindableProperty.BindableValue = "abc";
            }
            catch { }
        }

        [TestMethod]
        public void then_task_changed_was_fired()
        {
            Assert.IsTrue(TasksChangedFired);
        }
    }
}
