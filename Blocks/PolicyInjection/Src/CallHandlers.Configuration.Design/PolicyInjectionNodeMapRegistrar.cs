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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// Register node maps for the call handler nodes.
    /// </summary>
    public class PolicyInjectionCallHandlerNodeMapRegistrar : NodeMapRegistrar
    {
        /// <summary>
        /// Create a new <see cref="PolicyInjectionCallHandlerNodeMapRegistrar"/>
        /// that registers using the given <see cref="System.IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider"><see cref="System.IServiceProvider"/> to use to register.</param>
        public PolicyInjectionCallHandlerNodeMapRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}

        /// <summary>
        /// Register the node maps used by the call handler nodes.
        /// </summary>
        /// <remarks>These node maps are used to map the sections in the
        /// configuration file to the underlying node type used by the
        /// config console.</remarks>
        public override void Register()
        {
            base.AddMultipleNodeMap(Resources.AuthorizationCallHandlerNodeName,
                                    typeof(AuthorizationCallHandlerNode),
                                    typeof(AuthorizationCallHandlerData));

            base.AddMultipleNodeMap(Resources.CachingCallHandlerNodeName,
                                    typeof(CachingCallHandlerNode),
                                    typeof(CachingCallHandlerData));

            base.AddMultipleNodeMap(Resources.CustomCallHandlerNodeName,
                                    typeof(CustomCallHandlerNode),
                                    typeof(CustomCallHandlerData));

            base.AddMultipleNodeMap(Resources.LogCallHandlerNodeName,
                                    typeof(LogCallHandlerNode),
                                    typeof(LogCallHandlerData));

            base.AddMultipleNodeMap(Resources.PerformanceCounterCallHandlerNodeName,
                                    typeof(PerformanceCounterCallHandlerNode),
                                    typeof(PerformanceCounterCallHandlerData));

            base.AddMultipleNodeMap(Resources.ValidationCallHandlerNodeName,
                                    typeof(ValidationCallHandlerNode),
                                    typeof(ValidationCallHandlerData));

            base.AddMultipleNodeMap(Resources.ExceptionCallHandlerNodeName,
                                    typeof(ExceptionCallHandlerNode),
                                    typeof(ExceptionCallHandlerData));
        }
    }
}