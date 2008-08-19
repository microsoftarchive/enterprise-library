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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// A <see cref="ConfigurationNode"/> that represents the configuration
    /// of an <see cref="ExceptionCallHandler" /> in the Configuration
    /// Console.
    /// </summary>
    public class ExceptionCallHandlerNode : CallHandlerNode
    {
        string exceptionPolicyName;

        ExceptionPolicyNode exceptionPolicyNode;
        EventHandler<ConfigurationNodeChangedEventArgs> exceptionPolicyNodeRemovedHandler;

        /// <summary>
        /// Create a new <see cref="ExceptionCallHandlerNode"/> with default configuration settings.
        /// </summary>
        public ExceptionCallHandlerNode()
            : this(new ExceptionCallHandlerData(Resources.ExceptionCallHandlerNodeName)) {}

        /// <summary>
        /// Create a new <see cref="ExceptionCallHandlerNode"/> with the supplied settings.
        /// </summary>
        /// <param name="callHandlerData">Configuration settings for this call handler.</param>
        public ExceptionCallHandlerNode(ExceptionCallHandlerData callHandlerData)
            : base(callHandlerData)
        {
            exceptionPolicyName = callHandlerData.ExceptionPolicyName;
            exceptionPolicyNodeRemovedHandler = new EventHandler<ConfigurationNodeChangedEventArgs>(OnExceptionPolicyNodeRemoved);
        }

        /// <summary>
        /// Name of exception policy to use.
        /// </summary>
        /// <value>Get or set exception policy Node.</value>
        [SRDescription("ExceptionPolicyNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Required]
        public ExceptionPolicyNode ExceptionPolicy
        {
            get { return exceptionPolicyNode; }
            set
            {
                exceptionPolicyNode = LinkNodeHelper.CreateReference<ExceptionPolicyNode>(exceptionPolicyNode,
                                                                                          value,
                                                                                          exceptionPolicyNodeRemovedHandler,
                                                                                          null);
            }
        }

        /// <summary>
        /// Converts the information stored in the node and generate
        /// the corresponding configuration element to store in
        /// an <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource" />.
        /// </summary>
        /// <returns>Newly created <see cref="ExceptionCallHandlerData"/> containing
        /// the configuration data from this node.</returns>
        public override CallHandlerData CreateCallHandlerData()
        {
            ExceptionCallHandlerData callHandlerData = new ExceptionCallHandlerData(Name, Order);
            if (exceptionPolicyNode != null)
            {
                callHandlerData.ExceptionPolicyName = exceptionPolicyNode.Name;
            }
            return callHandlerData;
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ExceptionCallHandlerNode "/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (null != exceptionPolicyNode)
                    {
                        exceptionPolicyNode.Removed -= exceptionPolicyNodeRemovedHandler;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Clean up when referenced exception policy node is removed.
        /// </summary>
        /// <param name="sender">Source of event.</param>
        /// <param name="e">EventArgs</param>
        void OnExceptionPolicyNodeRemoved(object sender,
                                          ConfigurationNodeChangedEventArgs e)
        {
            exceptionPolicyNode = null;
        }

        /// <summary>
        /// Updates the exception policy node reference to point to ones defined under
        /// the exception handling block.
        /// </summary>
        /// <param name="hierarchy">Hierarchy to use to locate other exception policy nodes.</param>
        public override void ResolveNodeReferences(IConfigurationUIHierarchy hierarchy)
        {
            if (!String.IsNullOrEmpty(exceptionPolicyName))
            {
                foreach (ExceptionPolicyNode exceptionPolicyNode in hierarchy.FindNodesByType(typeof(ExceptionPolicyNode)))
                {
                    if (string.Compare(exceptionPolicyName, exceptionPolicyNode.Name) == 0)
                    {
                        ExceptionPolicy = exceptionPolicyNode;
                        break;
                    }
                }
            }
        }
    }
}