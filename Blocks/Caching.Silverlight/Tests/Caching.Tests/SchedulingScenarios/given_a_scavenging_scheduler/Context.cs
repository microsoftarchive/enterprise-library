using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.EnterpriseLibrary.Caching.Scheduling;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.SchedulingScenarios.given_a_scavenging_scheduler
{
    public abstract class Context : ArrangeActAssert
    {
        protected IManuallyScheduledWork Scheduler;

        protected override void Arrange()
        {
            base.Arrange();

            Scheduler = new ScavengingScheduler();
        }
    }
}
