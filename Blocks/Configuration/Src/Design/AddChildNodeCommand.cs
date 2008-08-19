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
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a command that adds a <see cref="ConfigurationNode"/> as a child of the  <see cref="ConfigurationNode"/> that this command is executing upon.    
    /// </summary>
    public class AddChildNodeCommand : ConfigurationNodeCommand
    {
        ConfigurationNode childNode;
        readonly Type childType;

        /// <summary>
        /// Initialize a new instance of the <see cref="AddChildNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="childType">
        /// The <see cref="Type"/> object for the configuration node to create and add to the node.        
        /// </param>
        public AddChildNodeCommand(IServiceProvider serviceProvider,
                                   Type childType)
            : this(serviceProvider, true, childType) {}

        /// <summary>
        /// Initialize a new instance of the <see cref="AddChildNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="clearErrorService">
        /// Determines if all the messages in the <see cref="IErrorLogService"/> should be cleared when the command has executed.
        /// </param>
        /// <param name="childType">
        /// The <see cref="Type"/> object for the configuration node to create and add to the node.
        /// </param>
        public AddChildNodeCommand(IServiceProvider serviceProvider,
                                   bool clearErrorService,
                                   Type childType)
            : base(serviceProvider, clearErrorService)
        {
            if (childType == null)
            {
                throw new ArgumentNullException("childType");
            }
            this.childType = childType;
        }

        /// <summary>
        /// Gets the <see cref="ConfigurationNode"/> that was added as a result of the command being executed.
        /// </summary>
        /// <value>
        /// The <see cref="ConfigurationNode"/> that was added as a result of the command being executed. 
        /// The default is a <see langword="null"/> reference(Nothing in Visual Basic).
        /// </value>
        public ConfigurationNode ChildNode
        {
            get { return childNode; }
        }

        ConfigurationNode CreateChild()
        {
            return (ConfigurationNode)Activator.CreateInstance(childType,
                                                               BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.CreateInstance,
                                                               null,
                                                               null,
                                                               null);
        }

        /// <summary>
        /// Creates an instance of the child node class and adds it as a child of the parent node.
        /// </summary>
        /// <param name="node">
        /// The parent node to add the newly created <see cref="ChildNode"/>.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                childNode = CreateChild();
                node.AddNode(childNode);
                UIService.SetUIDirty(node.Hierarchy);
                UIService.ActivateNode(childNode);
            }
            finally
            {
                UIService.EndUpdate();
            }
        }
    }
}