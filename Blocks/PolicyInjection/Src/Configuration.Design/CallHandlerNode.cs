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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
	/// <summary>
	/// Base class for nodes representing concrete <see cref="CallHandlerData"/> instances.
	/// </summary>
	[Image(typeof(CallHandlerNode))]
	[SelectedImage(typeof(CallHandlerNode))]
	public abstract class CallHandlerNode : ConfigurationNode
	{
        private int order = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="CallHandlerNode"/> class representing a <see cref="CallHandlerData"/>.
		/// </summary>
		/// <param name="callHandlerData">The <see cref="CallHandlerData"/> to represent.</param>
		public CallHandlerNode(CallHandlerData callHandlerData)
			: base(callHandlerData.Name)
		{
            this.order = callHandlerData.Order;
        }

		/// <summary>
		/// Returns a <see cref="CallHandlerData"/> configuration object from the nodes data.
		/// </summary>
		/// <returns>
		/// A <see cref="CallHandlerData"/> configuration object from the nodes data.
		/// </returns>
		/// <remarks>
		/// Subclasses implement this method to return an instance of a concrete <see cref="CallHandlerData"/> subclass.
		/// </remarks>
		public abstract CallHandlerData CreateCallHandlerData();

		/// <summary>
		/// Resolves references to nodes in the hierarchy, if necessary.
		/// </summary>
		/// <param name="hierarchy">The <see cref="IConfigurationUIHierarchy"/> representing the configuration nodes.</param>
		/// <remarks>
		/// Subclasses with actual references to other nodes must implement this method to look for the
		/// nodes corresponding to the represented configuration and set the property holding the node reference.
		/// </remarks>
		public virtual void ResolveNodeReferences(IConfigurationUIHierarchy hierarchy)
		{
		}

        /// <summary>
        /// Order in which the handler will be executed.
        /// </summary>
        /// <value>Sets or get the expiration order.</value>
        [SRDescription("OrderDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }
	}
}
