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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.ObjectBuilder
{
    /// <summary>
    /// ObjectBuilder policy used by the <see cref="PolicyInjectionStrategy"/>.
    /// </summary>
    public interface IPolicyInjectionPolicy : IBuilderPolicy
    {
        /// <summary>
        /// Should policies be applied at all?
        /// </summary>
        /// <remarks>true if yes, false if no.</remarks>
        bool ApplyPolicies { get; }

        /// <summary>
        /// Stores the configuration source used to retrieve the <see cref="PolicySet"/>
        /// from configuration.
        /// </summary>
        /// <param name="configSource">Configuration source to read policies from.</param>
        void SetPolicyConfigurationSource(IConfigurationSource configSource);

        /// <summary>
        /// Creates interception for the given instance.
        /// </summary>
        /// <param name="instanceToProxy">Target object to create interception for.</param>
        /// <param name="typeToProxy">Interface or class type to return.</param>
        /// <returns>The proxy to the target, or the raw instance if no proxying is required.</returns>
        object ApplyProxy(object instanceToProxy, Type typeToProxy);
    }
}
