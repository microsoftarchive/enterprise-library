//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Logging.given_logging_configuraton
{
    public abstract class Context : ContainerContext
    {
        protected LoggingSettings TestLoggingSettings;
        protected SectionViewModel LoggingViewModel;
        protected ElementCollectionViewModel TestTraceListenersViewModel;

        protected DefaultElementCollectionAddCommand AddNewObjectCache;
        protected DefaultCollectionElementAddCommand AddNewRemoteServiceTraceListener;
        protected DefaultCollectionElementAddCommand AddNewIsolatedStorageTraceListener;
        protected DefaultCollectionElementAddCommand AddNewNotificationTraceListener;

        protected override void Arrange()
        {
            base.Arrange();

            TestLoggingSettings = new LoggingSettings();

            LoggingViewModel = SectionViewModel.CreateSection(Container, LoggingSettings.SectionName,
                                                                          TestLoggingSettings);

            TestTraceListenersViewModel = (ElementCollectionViewModel)LoggingViewModel.GetDescendentsOfType
                                      <TraceListenerDataCollection>().First();

            // Create the child
            AddNewObjectCache = TestTraceListenersViewModel.Commands.OfType<DefaultElementCollectionAddCommand>().First();

            // Create the trace listeners from the command
            AddNewIsolatedStorageTraceListener =
                TestTraceListenersViewModel.Commands
                    .SelectMany(x => x.ChildCommands)
                    .Cast<DefaultCollectionElementAddCommand>()
                    .First(x => x.ConfigurationElementType == typeof(IsolatedStorageTraceListenerData));

            AddNewRemoteServiceTraceListener =
                TestTraceListenersViewModel.Commands
                    .SelectMany(x => x.ChildCommands)
                    .Cast<DefaultCollectionElementAddCommand>()
                    .First(x => x.ConfigurationElementType == typeof(RemoteServiceTraceListenerData));

            AddNewNotificationTraceListener =
                TestTraceListenersViewModel.Commands
                    .SelectMany(x => x.ChildCommands)
                    .Cast<DefaultCollectionElementAddCommand>()
                    .First(x => x.ConfigurationElementType == typeof(NotificationTraceListenerData));
        }
    }
}
