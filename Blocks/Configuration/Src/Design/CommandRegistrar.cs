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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents an encapsulation for registering commands for a configuration design. The class is abstract.
	/// </summary>
	public abstract class CommandRegistrar
	{
		private readonly IServiceProvider serviceProvider;

		/// <summary>
		/// Initialize a new instance of the <see cref="CommandRegistrar"/> class with an <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		protected CommandRegistrar(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			this.serviceProvider = serviceProvider;
		}

		/// <summary>
		/// Register the commands.
		/// </summary>
		public abstract void Register();

		/// <summary>
		/// Gets the a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </summary>
		/// <value>
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </value>
		protected IServiceProvider ServiceProvider
		{
			get { return serviceProvider;  }
		}

		/// <summary>
		/// Add move up and move down commands to all instances of a particular type.
		/// </summary>		
		/// <param name="registerType">
		/// The <see cref="Type"/> to apply the commands.
		/// </param>
		protected void AddMoveUpDownCommands(Type registerType)
		{
			AddMoveUpCommand(registerType);
			AddMoveDownCommand(registerType);
		}

		/// <summary>
		/// Add move up command to all instances of a particular type.
		/// </summary>		
		/// <param name="registerType">
		/// The <see cref="Type"/> to apply the commands.
		/// </param>
		protected void AddMoveUpCommand(Type registerType)
		{
			AddUICommand(new MoveUpConfigurationUICommand(serviceProvider), registerType);
		}

		/// <summary>
		/// Add move down command to all instances of a particular type.
		/// </summary>		
		/// <param name="registerType">
		/// The <see cref="Type"/> to apply the commands.
		/// </param>
		protected void AddMoveDownCommand(Type registerType)
		{
			AddUICommand(new MoveDownConfigurationUICommand(serviceProvider), registerType);
		}

		/// <summary>
		/// Adds the default remove and validate commands to all instances of a particular type.
		/// </summary>		
		/// <param name="registerType">
		/// The <see cref="Type"/> to apply the commands.
		/// </param>
		protected void AddDefaultCommands(Type registerType)
		{
			AddRemoveCommand(registerType);
			AddValidateCommand(registerType);
		}

		/// <summary>
		/// Add the remove command to all the instances of a particular type.
		/// </summary>
		/// <param name="registerType">
		/// The <see cref="Type"/> to apply the commands.
		/// </param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
		protected void AddRemoveCommand(Type registerType)
		{
			ConfigurationUICommand item = new ConfigurationUICommand(serviceProvider,
				Resources.RemoveUICommandText,
				Resources.RemoveUICommandLongText,
				CommandState.Enabled,
				new RemoveNodeCommand(serviceProvider, false),
				Shortcut.None,
				InsertionPoint.Action,
				null);
			AddUICommand(item, registerType);
		}

		/// <summary>
		/// Add the validate command to all the instances of a particular type.
		/// </summary>
		/// <param name="registerType">
		/// The <see cref="Type"/> to apply the commands.
		/// </param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
		protected void AddValidateCommand(Type registerType)
		{
			ConfigurationUICommand item = new ConfigurationUICommand(serviceProvider,
				Resources.ValidateUICommandText,
				Resources.ValidateUICommandLongText,
				CommandState.Enabled,
				new ValidateNodeCommand(serviceProvider, false),
				Shortcut.CtrlShiftV,
				InsertionPoint.Action,
				null);
			AddUICommand(item, registerType);
		}

		/// <summary>
		/// Add a <see cref="AddChildNodeCommand"/> and allows the node type to be created multiple times.
		/// </summary>		
		/// <param name="text">The text of the command.</param>
		/// <param name="longText">The long text of the command.</param>
		/// <param name="nodeType">
		/// The node type to display the command upon.
		/// </param>
		/// <param name="registerType">
		/// The <see cref="Type"/> the command creates.
		/// </param>
		protected void AddMultipleChildNodeCommand(string text, string longText, Type nodeType, Type registerType)
		{
			AddMultipleChildNodeCommand(text, longText, nodeType, nodeType, registerType);
		}

		/// <summary>
		/// Add a <see cref="AddChildNodeCommand"/> and allows the node type to be created multiple times.
		/// </summary>		
		/// <param name="text">The text of the command.</param>
		/// <param name="longText">The long text of the command.</param>
		/// <param name="nodeType">
		/// The node type to display the command upon.
		/// </param>
		/// <param name="childType">
		/// The child node type created.
		/// </param>
		/// <param name="registerType">
		/// The <see cref="Type"/> to apply the commands.
		/// </param>
		protected void AddMultipleChildNodeCommand(string text, string longText, Type childType, Type nodeType, Type registerType)
		{
			AddChildNodeCommand(text, longText, nodeType, childType, registerType, NodeMultiplicity.Allow);
		}

		/// <summary>
		/// Add a <see cref="AddChildNodeCommand"/> and allows the node type to be created only once.
		/// </summary>		
		/// <param name="text">The text of the command.</param>
		/// <param name="longText">The long text of the command.</param>
		/// <param name="nodeType">
		/// The node type to display the command upon.
		/// </param>
		/// <param name="registerType">
		/// The <see cref="Type"/> the command creates.
		/// </param>
		protected void AddSingleChildNodeCommand(string text, string longText, Type nodeType, Type registerType)
		{
			AddSingleChildNodeCommand(text, longText, nodeType, nodeType, registerType);
		}

		/// <summary>
		/// Add a <see cref="AddChildNodeCommand"/> and allows the node type to be created only once.
		/// </summary>		
		/// <param name="text">The text of the command.</param>
		/// <param name="longText">The long text of the command.</param>
		/// <param name="nodeType">
		/// The node type to display the command upon.
		/// </param>
		/// <param name="childType">
		/// The child node type created.
		/// </param>
		/// <param name="registerType">
		/// The <see cref="Type"/> to apply the command.
		/// </param>
		protected void AddSingleChildNodeCommand(string text, string longText, Type childType, Type nodeType, Type registerType)
		{
			AddChildNodeCommand(text, longText, childType, nodeType, registerType, NodeMultiplicity.Disallow);
		}

		/// <summary>
		/// Add a <see cref="AddChildNodeCommand"/> and allows the node type to be created only once.
		/// </summary>		
		/// <param name="text">The text of the command.</param>
		/// <param name="longText">The long text of the command.</param>
		/// <param name="childType">The child type that will be created.</param>
		/// <param name="nodeType">The node type for the command.</param>
		/// <param name="registerType">
		/// The <see cref="Type"/> to apply the command.
		/// </param>
		/// <param name="nodeMultiplicity">One of the <see cref="NodeMultiplicity"/> values.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
		protected void AddChildNodeCommand(string text, string longText, Type childType, Type nodeType, Type registerType, NodeMultiplicity nodeMultiplicity)
		{
			ConfigurationUICommand item = new ConfigurationUICommand(serviceProvider, text,
				longText,
                CommandState.Enabled, nodeMultiplicity,
				new AddChildNodeCommand(serviceProvider, childType),
				nodeType, Shortcut.None, InsertionPoint.New, null);

			AddUICommand(item, registerType);
		}

		/// <summary>
		/// Add a <see cref="ConfigurationUICommand"/> object to the <see cref="IUICommandService"/>.
		/// </summary>
		/// <param name="uiCommand">The <see cref="ConfigurationUICommand"/> to add.</param>
		/// <param name="registerType">The <see cref="Type"/> the command is registered.</param>
		protected void AddUICommand(ConfigurationUICommand uiCommand, Type registerType)
		{
			IUICommandService commandService = ServiceHelper.GetUICommandService(serviceProvider);
			commandService.AddCommand(registerType, uiCommand);
		}
	}
}
