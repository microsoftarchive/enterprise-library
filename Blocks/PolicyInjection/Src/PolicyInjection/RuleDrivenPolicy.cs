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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Utilities;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
    /// <summary>
    /// A policy is a combination of a matching rule set and a set of handlers.
    /// If the policy applies to a member, then the handlers will be enabled for
    /// that member.
    /// </summary>
    public class RuleDrivenPolicy : Policy
    {
        private MatchingRuleSet ruleSet = new MatchingRuleSet();
        private CollectionEx<ICallHandler> handlers = new CollectionEx<ICallHandler>();

        /// <summary>
        /// Creates a new <see cref="RuleDrivenPolicy"/> object.
        /// </summary>
        public RuleDrivenPolicy()
            : this("unnamed policy")
        {
        }

        /// <summary>
        /// Creates a new <see cref="RuleDrivenPolicy"/> object with the given name.
        /// </summary>
        /// <param name="name">Name of the policy.</param>
        public RuleDrivenPolicy(string name)
            : base(name)
        {
            ruleSet.CollectionChanged += OnCollectionChanged;
            handlers.CollectionChanged += OnCollectionChanged;
        }

        /// <summary>
        /// Gets the collection of MatchingRules that the policy uses to know when
        /// to apply handlers.
        /// </summary>
        /// <value>The rule set collection.</value>
        public MatchingRuleSet RuleSet
        {
            get { return ruleSet; }
        }

        /// <summary>
        /// Gets the collections of ICallHandlers that this policy applies when it matches.
        /// </summary>
        /// <value>The call handler collection.</value>
        public IList<ICallHandler> Handlers
        {
            get { return handlers; }
        }

        /// <summary>
        /// Checks to see if this policy contains rules that match members
        /// of the given type.
        /// </summary>
        /// <param name="t">Type to check.</param>
        /// <returns>true if any of the members of type match the ruleset for this policy, false if not.</returns>
        protected override bool DoesApplyTo(Type t)
        {
            if(AppliesToType(t))
            {
                return true;
            }
            foreach(Type implementedInterface in t.GetInterfaces())
            {
                if (AppliesToType(implementedInterface)) 
                {
                    return true;
                }
            }
            return false;
        }

        private bool AppliesToType(Type t)
        {
            return Array.Exists<MethodBase>(t.GetMethods(),
                delegate(MethodBase member) { return Matches(member); });
        }

        /// <summary>
        /// Checks if the rules in this policy match the given member info.
        /// </summary>
        /// <param name="member">MemberInfo to check against.</param>
        /// <returns>true if ruleset matches, false if it does not.</returns>
        protected override bool DoesMatch(MethodBase member)
        {
            return ruleSet.Matches(member);
        }

        /// <summary>
        /// Return ordered collection of handlers in order that apply to the given member.
        /// </summary>
        /// <param name="member">Member that may or may not be assigned handlers by this policy.</param>
        /// <returns>Collection of handlers (possibly empty) that apply to this member.</returns>
        protected override IEnumerable<ICallHandler> DoGetHandlersFor(MethodBase member)
        {
            if (Matches(member))
            {
                foreach (ICallHandler handler in handlers)
                {
                    yield return handler;
                }
            }
        }

        private void OnCollectionChanged(object sender, EventArgs e)
        {
            OnPolicyChanged(this, EventArgs.Empty);
        }
    }
}
