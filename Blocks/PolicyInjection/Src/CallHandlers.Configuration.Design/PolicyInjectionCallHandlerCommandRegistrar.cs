//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// Command registrar class that adds the commands for the various call handler nodes.
    /// </summary>
    class PolicyInjectionCallHandlerCommandRegistrar : CommandRegistrar
    {
        /// <summary>
        /// Create a new <see cref="PolicyInjectionCallHandlerCommandRegistrar"/> instance.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> to register with.</param>
        public PolicyInjectionCallHandlerCommandRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}

        /// <summary>
        /// Performs registration.
        /// </summary>
        public override void Register()
        {
            base.AddDefaultCommands(typeof(AuthorizationCallHandlerNode));
            base.AddMoveUpDownCommands(typeof(AuthorizationCallHandlerNode));
            base.AddMultipleChildNodeCommand(Resources.AddAuthorizationCallHandlerCommandText,
                                             Resources.AddAuthorizationCallHandlerCommandTextLong,
                                             typeof(AuthorizationCallHandlerNode),
                                             typeof(CallHandlersCollectionNode));

            base.AddDefaultCommands(typeof(ValidationCallHandlerNode));
            base.AddMoveUpDownCommands(typeof(ValidationCallHandlerNode));
            base.AddMultipleChildNodeCommand(Resources.AddValidationCallHandlerCommandText,
                                             Resources.AddValidationCallHandlerCommandTextLong,
                                             typeof(ValidationCallHandlerNode),
                                             typeof(CallHandlersCollectionNode));

            base.AddDefaultCommands(typeof(ExceptionCallHandlerNode));
            base.AddMoveUpDownCommands(typeof(ExceptionCallHandlerNode));
            base.AddMultipleChildNodeCommand(Resources.AddExceptionCallHandleCommandText,
                                             Resources.AddExceptionCallHandleCommandTextLong,
                                             typeof(ExceptionCallHandlerNode),
                                             typeof(CallHandlersCollectionNode));

            base.AddDefaultCommands(typeof(LogCallHandlerNode));
            base.AddMoveUpDownCommands(typeof(LogCallHandlerNode));
            base.AddMultipleChildNodeCommand(Resources.AddLogCallHandlerCommandText,
                                             Resources.AddLogCallHandlerCommandTextLong,
                                             typeof(LogCallHandlerNode),
                                             typeof(CallHandlersCollectionNode));

            base.AddDefaultCommands(typeof(CustomCallHandlerNode));
            base.AddMoveUpDownCommands(typeof(CustomCallHandlerNode));
            base.AddMultipleChildNodeCommand(Resources.AddCustomCallHandlerCommandText,
                                             Resources.AddCustomCallHandlerCommandTextLong,
                                             typeof(CustomCallHandlerNode),
                                             typeof(CallHandlersCollectionNode));

            base.AddDefaultCommands(typeof(CachingCallHandlerNode));
            base.AddMoveUpDownCommands(typeof(CachingCallHandlerNode));
            base.AddMultipleChildNodeCommand(Resources.AddCachingCallHandlerCommandText,
                                             Resources.AddCachingCallHandlerCommandTextLong,
                                             typeof(CachingCallHandlerNode),
                                             typeof(CallHandlersCollectionNode));

            base.AddDefaultCommands(typeof(PerformanceCounterCallHandlerNode));
            base.AddMoveUpDownCommands(typeof(PerformanceCounterCallHandlerNode));
            base.AddMultipleChildNodeCommand(Resources.AddPerformanceCounterCallHandlerCommandText,
                                             Resources.AddPerformanceCounterCallHandlerCommandTextLong,
                                             typeof(PerformanceCounterCallHandlerNode),
                                             typeof(CallHandlersCollectionNode));
        }
    }
}