//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.SchedulingScenarios.given_a_scavenging_scheduler
{
    [TestClass]
    public class when_scheduling_work_more_than_once_that_does_not_overlap : Context
    {
        private AutoResetEvent workCompleted;
        private bool firstJobCompleted;
        private bool secondJobCompleted;

        protected override void Act()
        {
            base.Act();

            workCompleted = new AutoResetEvent(false);

            Scheduler.SetAction(() => workCompleted.Set());

            Scheduler.ScheduleWork();
            firstJobCompleted = workCompleted.WaitOne(5000);

            Scheduler.ScheduleWork();
            secondJobCompleted = workCompleted.WaitOne(5000);
        }

        [TestMethod]
        public void then_the_first_job_executed()
        {
            Assert.IsNull(initializeException);
            Assert.IsTrue(firstJobCompleted);
        }

        [TestMethod]
        public void then_the_second_job_executed()
        {
            Assert.IsTrue(secondJobCompleted);
        }
    }
}
