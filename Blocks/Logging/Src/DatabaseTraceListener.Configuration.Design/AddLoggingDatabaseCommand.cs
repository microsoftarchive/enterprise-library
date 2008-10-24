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
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design
{
    /// <summary>
    /// Represents a command that adds a <see cref="LoggingDatabaseNode"/> as a child of the  <see cref="ConfigurationNode"/> that this command is executing upon.    
    /// </summary>
    public sealed class AddLoggingDatabaseCommand : AddChildNodeCommand
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AddLoggingDatabaseCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        public AddLoggingDatabaseCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(LoggingDatabaseNode)) {}

        /// <summary>
        /// Raises the <see cref="ConfigurationNodeCommand.Executed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);
            LoggingDatabaseNode node = ChildNode as LoggingDatabaseNode;
            if (null == node) return;

            if (null == CurrentHierarchy.FindNodeByType(typeof(LoggingSettingsNode)))
            {
                new AddLoggingSettingsNodeCommand(ServiceProvider).Execute(CurrentHierarchy.RootNode);
            }

            if (null == CurrentHierarchy.FindNodeByType(typeof(DatabaseSectionNode)))
            {
                new AddDatabaseSectionNodeCommand(ServiceProvider).Execute(CurrentHierarchy.RootNode);
            }
        }
    }
}
