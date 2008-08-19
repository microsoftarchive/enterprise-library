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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design
{
    sealed class CommandRegistrar : EnterpriseLibrary.Configuration.Design.CommandRegistrar
    {
        public CommandRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}

        public override void Register()
        {
            AddMultipleChildNodeCommand(
                Resources.FaultContractExceptionHandlerNodeUICommandText,
                Resources.FaultContractExceptionHandlerNodeUICommandLongText,
                typeof(FaultContractExceptionHandlerNode),
                typeof(ExceptionTypeNode));
            AddDefaultCommands(typeof(FaultContractExceptionHandlerNode));
            AddMoveUpDownCommands(typeof(FaultContractExceptionHandlerNode));
        }
    }
}