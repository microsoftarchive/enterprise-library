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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
    /// <summary>
    /// Represents a command that adds a <see cref="LoggingExceptionHandlerNode"/> as a child of the  <see cref="ConfigurationNode"/> that this command is executing upon.    
    /// </summary>
    public sealed class AddLoggingExceptionHandlerCommand : AddChildNodeCommand
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AddLoggingExceptionHandlerCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        public AddLoggingExceptionHandlerCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(LoggingExceptionHandlerNode)) {}

        /// <summary>
        /// Raises the <see cref="ConfigurationNodeCommand.Executed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);
            LoggingExceptionHandlerNode node = ChildNode as LoggingExceptionHandlerNode;
            if (null == node) return;

            if (null == CurrentHierarchy.FindNodeByType(typeof(LoggingSettingsNode)))
            {
                ConfigurationApplicationNode applicationNode = (ConfigurationApplicationNode)CurrentHierarchy.FindNodeByType(typeof(ConfigurationApplicationNode));
                new AddLoggingSettingsNodeCommand(ServiceProvider).Execute(applicationNode);
            }
        }
    }
}
