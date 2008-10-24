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

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design
{
    sealed class LoggingDatabaseCommandRegistrar : CommandRegistrar
    {
        public LoggingDatabaseCommandRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}

        void AddLoggingDatabaseCommand()
        {
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                                                                                        Resources.DatabaseTraceListenerUICommandText,
                                                                                        Resources.DatabaseTraceListenerUICommandLongText,
                                                                                        new AddLoggingDatabaseCommand(ServiceProvider),
                                                                                        typeof(LoggingDatabaseNode));
            AddUICommand(cmd, typeof(TraceListenerCollectionNode));
        }

        public override void Register()
        {
            AddLoggingDatabaseCommand();
            AddDefaultCommands(typeof(LoggingDatabaseNode));
        }
    }
}
