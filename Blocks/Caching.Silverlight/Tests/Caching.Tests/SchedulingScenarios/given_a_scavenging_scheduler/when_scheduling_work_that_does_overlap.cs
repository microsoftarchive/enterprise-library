using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.SchedulingScenarios.given_a_scavenging_scheduler
{
    [TestClass]
    public class when_scheduling_work_that_does_overlap : Context
    {
        private int jobsExecuted;
        private ManualResetEvent workCompleted;
        private bool firstJobStarted;

        protected override void Act()
        {
            base.Act();

            jobsExecuted = 0;
            var workStartedEvent = new ManualResetEvent(false);
            var continueWorkEvent = new ManualResetEvent(false);
            workCompleted = new ManualResetEvent(false);

            Scheduler.SetAction(() =>
            {
                workStartedEvent.Set();
                continueWorkEvent.WaitOne();
                ++jobsExecuted;
                workCompleted.Set();
            });

            Scheduler.ScheduleWork();
            firstJobStarted = workStartedEvent.WaitOne(5000);
            Scheduler.ScheduleWork();
            Scheduler.ScheduleWork();
            Scheduler.ScheduleWork();

            continueWorkEvent.Set();
        }

        [TestMethod]
        public void then_first_job_started_successfully()
        {
            Assert.IsTrue(firstJobStarted);
        }

        [TestMethod]
        public void then_at_least_one_job_ran_to_completion()
        {
            Assert.IsTrue(workCompleted.WaitOne(5000));
        }

        [TestMethod]
        public void then_two_jobs_total_ran()
        {
            // The scheduler will end up running two jobs. The first one, of course,
            // and the second one will get scheduled because the first job is still
            // running. All the others will not get scheduled.
            Assert.AreEqual(2, jobsExecuted);
        }
        
    }
}
