//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
    sealed class ExceptionHandlingLoggingCommandRegistrar : CommandRegistrar
    {
        public ExceptionHandlingLoggingCommandRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}

        void AddLoggingExceptionHandlerCommand()
        {
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                                                                                        Resources.LoggingHandlerName,
                                                                                        string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.LoggingHandlerName),
                                                                                        new AddLoggingExceptionHandlerCommand(ServiceProvider),
                                                                                        typeof(ExceptionHandlerNode));
            AddUICommand(cmd, typeof(ExceptionTypeNode));
        }

        public override void Register()
        {
            AddLoggingExceptionHandlerCommand();
            AddDefaultCommands(typeof(LoggingExceptionHandlerNode));
            AddMoveUpDownCommands(typeof(LoggingExceptionHandlerNode));
        }
    }
}
