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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
    sealed class ExceptionHandlingLoggingNodeMapRegistrar : NodeMapRegistrar
    {
        public ExceptionHandlingLoggingNodeMapRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}

        public override void Register()
        {
            AddMultipleNodeMap(Resources.LoggingHandlerName,
                               typeof(LoggingExceptionHandlerNode),
                               typeof(LoggingExceptionHandlerData));
        }
    }
}
