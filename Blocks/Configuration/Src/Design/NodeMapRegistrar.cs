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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// A helper base class for registering node creation.
	/// </summary>
	public abstract class NodeMapRegistrar
	{
		private readonly IServiceProvider serviceProvider;

		/// <summary>
		/// Initialize a new instance of the <see cref="NodeMapRegistrar"/> class with an <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		protected NodeMapRegistrar(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			this.serviceProvider = serviceProvider;
		}

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
		/// Registers the node maps.
		/// </summary>
		public abstract void Register();

		/// <summary>
		/// Adds a node map that can only be created once.
		/// </summary>		
		/// <param name="text">The text to display to the user.</param>
		/// <param name="nodeType">The type of node to create.</param>
		/// <param name="dataType">The configuration type for the node.</param>
		protected void AddSingleNodeMap(string text, Type nodeType, Type dataType)
		{
			INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);

			nodeCreationService.AddNodeCreationEntry(NodeCreationEntry.CreateNodeCreationEntryNoMultiples(
				new AddChildNodeCommand(serviceProvider, nodeType),
				nodeType,
				dataType,
				text));
		}

		/// <summary>
		/// Adds a node map that can only be created more than once.
		/// </summary>		
		/// <param name="text">The text to display to the user.</param>
		/// <param name="nodeType">The type of node to create.</param>
		/// <param name="dataType">The configuration type for the node.</param>
		protected void AddMultipleNodeMap(string text, Type nodeType, Type dataType)
		{
			INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(serviceProvider);

			nodeCreationService.AddNodeCreationEntry(NodeCreationEntry.CreateNodeCreationEntryWithMultiples(
				new AddChildNodeCommand(serviceProvider, nodeType),
				nodeType,
				dataType,
				text));
		}
	}
}
