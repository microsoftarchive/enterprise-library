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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents a way to describe a UI command.
	/// </summary>
	public class ConfigurationUICommand : IDisposable
	{
		private readonly string text;
		private readonly string longText;
		private readonly CommandState commandState;
		private readonly ConfigurationNodeCommand command;
		private readonly Type nodeType;
		private readonly InsertionPoint insertionPoint;
		private readonly Shortcut shortcut;
		private readonly Icon icon;
		private readonly IServiceProvider serviceProvider;
		private readonly NodeMultiplicity multiplicity;


		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationUICommand"/> class.
		/// </summary>		
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="text">The text for the command.</param>
		/// <param name="longText">The text that will be in the status bar.</param>
		/// <param name="commandState">One of the <see cref="CommandState"/> values.</param>		
		/// <param name="command">The command to execute.</param>		
		/// <param name="shortcut">A short cut for the command.</param>
		/// <param name="insertionPoint">One of the <see cref="InsertionPoint"/> values.</param>
		/// <param name="icon">The icon for the command.</param>
		public ConfigurationUICommand(IServiceProvider serviceProvider, string text,
			string longText, CommandState commandState,
			ConfigurationNodeCommand command, Shortcut shortcut,
			InsertionPoint insertionPoint, Icon icon) 
			: this(serviceProvider,
			text, longText, commandState, NodeMultiplicity.Allow, command, null,
			shortcut, insertionPoint, icon)
		{			
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationUICommand"/> class.
		/// </summary>		
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="text">The text for the command.</param>
		/// <param name="longText">The text that will be in the status bar.</param>
		/// <param name="commandState">One of the <see cref="CommandState"/> values.</param>		
		/// <param name="multiplicity">One of the <see cref="NodeMultiplicity"/> values.</param>
		/// <param name="command">The command to execute.</param>		
		/// <param name="nodeType">The node type that will be created when the command is executed.</param>
		/// <param name="shortcut">A short cut for the command.</param>
		/// <param name="insertionPoint">One of the <see cref="InsertionPoint"/> values.</param>
		/// <param name="icon">The icon for the command.</param>
		public ConfigurationUICommand(IServiceProvider serviceProvider, string text,
			string longText, CommandState commandState, NodeMultiplicity multiplicity,
			ConfigurationNodeCommand command, Type nodeType, Shortcut shortcut, 
			InsertionPoint insertionPoint, Icon icon)
		{
			this.text = text;
			this.longText = longText;
			this.commandState = commandState;
			this.command = command;
			this.nodeType = nodeType;
			this.insertionPoint = insertionPoint;
			this.shortcut = shortcut;
			this.icon = icon;
			this.serviceProvider = serviceProvider;
			this.multiplicity = multiplicity;
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="ConfigurationUICommand"/> and optionally releases the managed resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="ConfigurationUICommand"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (null != command)
				{
					command.Dispose();
				}			
			}			
		}

		/// <summary>
		/// Create an instance of a <see cref="ConfigurationUICommand"/> that only allows single creation.
		/// </summary>		
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="text">The text for the command.</param>
		/// <param name="longText">The text that will be in the status bar.</param>		
		/// <param name="command">The command to execute.</param>				
		/// <param name="nodeType">The type of node to be created.</param>
		public static ConfigurationUICommand CreateSingleUICommand(IServiceProvider serviceProvider, string text, string longText, ConfigurationNodeCommand command, Type nodeType)
		{
			return new ConfigurationUICommand(serviceProvider, 
				text,
				longText,
				CommandState.Enabled, 
                NodeMultiplicity.Disallow,
				command,
                nodeType,
				Shortcut.None, 
				InsertionPoint.New,
				null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="text"></param>
		/// <param name="longText"></param>
		/// <param name="command"></param>
		/// <param name="nodeType"></param>
		/// <returns></returns>
		public static ConfigurationUICommand CreateMultipleUICommand(IServiceProvider serviceProvider, string text, string longText, ConfigurationNodeCommand command, Type nodeType)
		{
			return new ConfigurationUICommand(serviceProvider,
				text,
				longText,
				CommandState.Enabled,
				NodeMultiplicity.Allow,
				command,
				nodeType,
				Shortcut.None,
				InsertionPoint.New,
				null);
		}

		/// <summary>
		/// Gets the text for the command.
		/// </summary>
		/// <value>
		/// The text for the command.
		/// </value>
		public string Text
		{
			get { return text; }
		}

		/// <summary>
		/// Gets the text to display in a status bar.
		/// </summary>
		/// <value>
		/// The text to display in a status bar.
		/// </value>
		public string LongText
		{
			get { return longText; }
		}		

		/// <summary>
		/// Gets the insertion point for the command.
		/// </summary>
		/// <value>
		/// One of the <see cref="InsertionPoint"/> values.
		/// </value>
		public InsertionPoint InsertionPoint
		{
			get { return insertionPoint;  }
		}

		/// <summary>
		/// Gets the short cut for the command.
		/// </summary>
		/// <value>
		/// One of the <see cref="Shortcut"/> values.
		/// </value>
		public Shortcut Shortcut
		{
			get { return shortcut; }
		}

		/// <summary>
		/// Gets the icon to use for the command.
		/// </summary>
		/// <value>
		/// The icon to use for the command.
		/// </value>
		public Icon Icon
		{
			get { return icon;  }
		}

		/// <summary>
		/// Gets the sate of the command based on the node.
		/// </summary>
		/// <value>
		/// One of the <see cref="CommandState"/> values.
		/// </value>
		public virtual CommandState GetCommandState(ConfigurationNode node)
		{
			if (multiplicity == NodeMultiplicity.Allow) return commandState;

			IConfigurationUIHierarchy currentHierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            return currentHierarchy.ContainsNodeType(node, nodeType) ? CommandState.Disabled : CommandState.Enabled;			
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="node">The node to execute the command.</param>
		public void Execute(ConfigurationNode node)
		{
			command.Execute(node);
		}
	}
}
