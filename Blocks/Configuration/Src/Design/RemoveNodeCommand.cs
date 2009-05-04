//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a command that will remove a node from it's associated <see cref="IConfigurationUIHierarchy"/>.
    /// </summary>
    public class RemoveNodeCommand : ConfigurationNodeCommand
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="RemoveNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        public RemoveNodeCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="RemoveNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="clearErrorLog">
        /// Determines if all the messages in the <see cref="IErrorLogService"/> should be cleared when the command has executed.
        /// </param>
        public RemoveNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog) : base(serviceProvider, clearErrorLog)
        {
        }

        /// <summary>
        /// Removes the node from the <see cref="IConfigurationUIHierarchy"/>.
        /// </summary>
        /// <param name="node">
        /// The node to execute the command upon.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                UIService.SetUIDirty(node.Hierarchy);
                node.Remove();                
            }
            finally
            {
                UIService.EndUpdate();
            }
        }
    }
}
