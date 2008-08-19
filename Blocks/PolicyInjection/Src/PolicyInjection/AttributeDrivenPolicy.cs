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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Utilities;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
    /// <summary>
    /// A <see cref="Policy"/> class that reads and constructs handlers
    /// based on <see cref="HandlerAttribute"/> on the target.
    /// </summary>
    public class AttributeDrivenPolicy : Policy
    {
        private AttributeDrivenPolicyMatchingRule attributeMatchRule;

        /// <summary>
        /// Constructs a new instance of the <see cref="AttributeDrivenPolicy"/>.
        /// </summary>
        public AttributeDrivenPolicy()
            : base("Attribute Driven Policy")
        {
            this.attributeMatchRule = new AttributeDrivenPolicyMatchingRule();
        }

        /// <summary>
        /// Returns ordered collection of handlers in order that apply to the given member.
        /// </summary>
        /// <param name="member">Member that may or may not be assigned handlers by this policy.</param>
        /// <returns>Collection of handlers (possibly empty) that apply to this member.</returns>
        public override IEnumerable<ICallHandler> GetHandlersFor(MethodBase member)
        {
            if (Matches(member))
            {
                foreach (MethodBase method in GetMethodSet(member))
                {
                    List<ICallHandler> handlers = new List<ICallHandler>(DoGetHandlersFor(method));
                    if (handlers.Count > 0)
                    {
                        foreach (ICallHandler handler in handlers)
                        {
                            yield return handler;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Derived classes implement this method to calculate if the policy
        /// provides any handlers for any methods on the given type.
        /// </summary>
        /// <param name="t">Type to check.</param>
        /// <returns>true if the policy applies to this type, false if it does not.</returns>
        protected override bool DoesApplyTo(Type t)
        {
            return Array.Exists(t.GetMethods(),
                delegate(MethodInfo method) { return Matches(method); });
        }

        /// <summary>
        /// Derived classes implement this method to calculate if the policy
        /// will provide any handler to the specified member.
        /// </summary>
        /// <param name="member">Member to check.</param>
        /// <returns>true if policy applies to this member, false if not.</returns>
        protected override bool DoesMatch(MethodBase member)
        {
            foreach (MethodBase method in GetMethodSet(member))
            {
                if(attributeMatchRule.Matches(method))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Derived classes implement this method to supply the list of handlers for
        /// this specific member.
        /// </summary>
        /// <param name="member">Member to get handlers for.</param>
        /// <returns>Enumerable collection of handlers for this method.</returns>
        protected override IEnumerable<ICallHandler> DoGetHandlersFor(MethodBase member)
        {
            foreach (HandlerAttribute attr in ReflectionHelper.GetAllAttributes<HandlerAttribute>(member, true))
            {
                yield return attr.CreateHandler();
            }
        }
    }
}
