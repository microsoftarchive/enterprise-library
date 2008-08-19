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
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design
{
    sealed class LoggingDatabaseNodeMapRegistrar : NodeMapRegistrar
    {
        public LoggingDatabaseNodeMapRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}

        public override void Register()
        {
            AddMultipleNodeMap(Resources.DatabaseTraceListenerUICommandText,
                               typeof(LoggingDatabaseNode),
                               typeof(FormattedDatabaseTraceListenerData));
        }
    }
}