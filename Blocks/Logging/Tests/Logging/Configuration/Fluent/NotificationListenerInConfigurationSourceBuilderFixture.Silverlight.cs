//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    public abstract class Given_NotificationListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToNotificationTraceListener NotificationListenerBuilder;
        private string traceListenerName = "notification listener";

        protected override void Arrange()
        {
            base.Arrange();

            NotificationListenerBuilder = base.CategorySourceBuilder.SendTo.Notification(traceListenerName);
        }

        protected NotificationTraceListenerData GetNotificationTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<NotificationTraceListenerData>()
                .Where(x => x.Name == traceListenerName).First();
        }
    }

    [TestClass]
    public class When_CallingSendToNotificationListenerOnLogToCategoryConfigurationBuilder : Given_NotificationListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
        }

        [TestMethod]
        public void TheDataIsCreated()
        {
            Assert.IsNotNull(GetNotificationTraceListenerData());
        }
    }
}
