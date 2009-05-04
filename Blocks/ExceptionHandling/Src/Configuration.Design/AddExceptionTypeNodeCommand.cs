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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// Represents a command that adds a <see cref="ExceptionTypeNode"/> as a child of the  <see cref="ConfigurationNode"/> that this command is executing upon.    
    /// </summary>
    public class AddExceptionTypeNodeCommand : AddChildNodeCommand
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AddChildNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="childType">
        /// The <see cref="Type"/> object for the configuration node to create and add to the node.        
        /// </param>
        public AddExceptionTypeNodeCommand(IServiceProvider serviceProvider, Type childType) : base(serviceProvider, childType)
        {
        }

        /// <summary>
        /// Creates an instance of the child node class and adds it as a child of the parent node.
        /// </summary>
        /// <param name="node">
        /// The parent node to add the newly created <see cref="AddChildNodeCommand.ChildNode"/>.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
			Type selectedType = SelectedType;
			if (null == selectedType) return;
			base.ExecuteCore(node);
			ExceptionTypeNode typeNode = (ExceptionTypeNode)ChildNode;
			typeNode.PostHandlingAction = PostHandlingAction.NotifyRethrow;			
			try
			{
				typeNode.Type = selectedType.AssemblyQualifiedName;
			}
			catch (InvalidOperationException)
			{
				typeNode.Remove();
				UIService.ShowError(string.Format(Resources.Culture, Resources.DuplicateExceptionTypeErrorMessage, selectedType != null ? selectedType.Name : string.Empty));
			}		
        }

        /// <summary>
        /// Gets the selected type for the node.
        /// </summary>
        /// <value>
        /// The selected type for the node.
        /// </value>
		protected virtual Type SelectedType
		{
			get
			{
				TypeSelectorUI selector = new TypeSelectorUI(
					typeof(Exception),
					typeof(Exception),
					TypeSelectorIncludes.BaseType |
						TypeSelectorIncludes.AbstractTypes);
				DialogResult result = selector.ShowDialog();
				if (result == DialogResult.OK)
				{
					return selector.SelectedType;
				}
				return null;
			}
		}
    }
}
