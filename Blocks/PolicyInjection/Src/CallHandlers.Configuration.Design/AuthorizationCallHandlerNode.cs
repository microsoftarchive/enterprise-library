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
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// A <see cref="ConfigurationNode"/> that represents the configuration
    /// of an <see cref="AuthorizationCallHandler" /> in the Configuration
    /// Console.
    /// </summary>
    public class AuthorizationCallHandlerNode : CallHandlerNode
    {
        string authorizationProviderName;
        AuthorizationProviderNode authorizationProviderNode;
        EventHandler<ConfigurationNodeChangedEventArgs> authorizationProviderNodeRemovedHandler;

        string operationName;

        /// <summary>
        /// Create a new <see cref="AuthorizationCallHandlerNode"/> with empty
        /// configuration information.
        /// </summary>
        public AuthorizationCallHandlerNode()
            : this(new AuthorizationCallHandlerData(Resources.AuthorizationCallHandlerNodeName)) {}

        /// <summary>
        /// Create a new <see cref="AuthorizationCallHandlerNode"/> with configuration
        /// data read from the supplied <see cref="AuthorizationCallHandlerData" />.
        /// </summary>
        /// <param name="callHandlerData"><see cref="AuthorizationCallHandlerData" /> that
        /// contains the configuration information for this handler.</param>
        public AuthorizationCallHandlerNode(AuthorizationCallHandlerData callHandlerData)
            : base(callHandlerData)
        {
            authorizationProviderName = callHandlerData.AuthorizationProvider;
            operationName = callHandlerData.OperationName;

            authorizationProviderNodeRemovedHandler = new EventHandler<ConfigurationNodeChangedEventArgs>(OnAuthorizationNodeRemoved);
        }

        /// <summary>
        /// The authorization provider to use when performing the authorization operation.
        /// </summary>
        /// <value>Gets or sets the authorization provider.</value>
        [SRDescription("AuthorizationProviderDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(AuthorizationProviderNode))]
        public AuthorizationProviderNode AuthorizationProvider
        {
            get { return authorizationProviderNode; }
            set
            {
                authorizationProviderNode =
                    LinkNodeHelper.CreateReference<AuthorizationProviderNode>(
                        authorizationProviderNode,
                        value,
                        authorizationProviderNodeRemovedHandler,
                        null);
            }
        }

        /// <summary>
        /// The operation name to use when performing the authorization operation.
        /// </summary>
        /// <value>Gets or sets the operation name to use as configured for the Security block.</value>
        [SRDescription("OperationNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Required]
        public string OperationName
        {
            get { return operationName; }
            set { operationName = value; }
        }

        /// <summary>
        /// Converts the information stored in the node and generate
        /// the corresponding configuration element to store in
        /// an <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource" />.
        /// </summary>
        /// <returns>Newly created <see cref="AuthorizationCallHandlerData"/> containing
        /// the configuration data from this node.</returns>
        public override CallHandlerData CreateCallHandlerData()
        {
            AuthorizationCallHandlerData callHandlerData = new AuthorizationCallHandlerData(Name, Order);
            callHandlerData.OperationName = operationName;
            if (authorizationProviderNode != null)
            {
                callHandlerData.AuthorizationProvider = authorizationProviderNode.Name;
            }

            return callHandlerData;
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="AuthorizationCallHandlerNode"/> and optionally releases the managed resources.</para>
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
                    if (null != authorizationProviderNode)
                    {
                        authorizationProviderNode.Removed -= authorizationProviderNodeRemovedHandler;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Clean up when authorization node is removed.
        /// </summary>
        /// <param name="sender">Source of event.</param>
        /// <param name="e">EventArgs</param>
        void OnAuthorizationNodeRemoved(object sender,
                                        ConfigurationNodeChangedEventArgs e)
        {
            authorizationProviderNode = null;
        }

        /// <summary>
        /// Updates the reference to the authorization node to point to the correct
        /// instance stored in the hierarchy.
        /// </summary>
        /// <remarks>This method supports the internal Enterprise Library configuration console
        /// and is not intended to be called by user code.</remarks>
        /// <param name="hierarchy"><see cref="IConfigurationUIHierarchy"/> to use to resolve
        /// references.</param>
        public override void ResolveNodeReferences(IConfigurationUIHierarchy hierarchy)
        {
            if (!String.IsNullOrEmpty(authorizationProviderName))
            {
                foreach (AuthorizationProviderNode authProviderNode in hierarchy.FindNodesByType(typeof(AuthorizationProviderNode)))
                {
                    if (string.Compare(authorizationProviderName, authProviderNode.Name) == 0)
                    {
                        AuthorizationProvider = authProviderNode;
                        break;
                    }
                }
            }
        }
    }
}