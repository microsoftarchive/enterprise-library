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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
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
            var cacheManager = CachingViewModel.GetDescendentsOfType<CacheManagerData>().Single();
            try
            {
                cacheManager.Property("ExpirationPollFrequencyInSeconds").BindableProperty.BindableValue = "abc";
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
