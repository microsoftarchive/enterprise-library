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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Utilities;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
    /// <summary>
    /// A collection of Policy objects. The policies within a PolicySet combine using
    /// an "or" operation.
    /// </summary>
    [CustomFactory(typeof(PolicySetCustomFactory))]
    public class PolicySet : CollectionEx<Policy>
    {
        private Dictionary<Type, PolicySet> policiesForTypeCache;
        private object policiesForTypeCacheLock = new object();
        private Dictionary<MethodBase, List<ICallHandler>> handlerListCache;
        private object handlerListCacheLock = new object();

        /// <summary>
        /// Creates a new <see cref="PolicySet"/> with no contained policies.
        /// </summary>
        public PolicySet() : this( new Policy[0] )
        {
        }

        /// <summary>
        /// Creates a new <see cref="PolicySet"/> containing the given policies.
        /// </summary>
        /// <remarks>The policy set always contains an <see cref="AttributeDrivenPolicy"/>
        /// as the first policy in the set.</remarks>
        /// <param name="policies">Policies to put into the policy set.</param>
        public PolicySet(params Policy[] policies )
        {
            ResetCaches();
            Add(new AttributeDrivenPolicy());
            AddRange(policies);
        }

        /// <summary>
        /// Checks to see if any policies in this policy set apply to any members of the
        /// given type.
        /// </summary>
        /// <param name="t"><see cref="System.Type"/> to check.</param>
        /// <returns>true if policies will be applied, false if not.</returns>
        public bool AppliesTo(Type t)
        {
            return GetPoliciesFor(t).Count != 0;
        }

        /// <summary>
        /// Returns a new <see cref="PolicySet"/> that contains only the policies
        /// that apply to the given type.
        /// </summary>
        /// <param name="t"><see cref="System.Type"/> to get policies for.</param>
        /// <returns>New policy set. May be empty.</returns>
        public PolicySet GetPoliciesFor(Type t)
        {
            if (!policiesForTypeCache.ContainsKey(t))
            {
                lock (policiesForTypeCacheLock)
                {
                    if (!policiesForTypeCache.ContainsKey(t))
                    {
                        Dictionary<Type, PolicySet> newCache = new Dictionary<Type, PolicySet>(policiesForTypeCache);
                        newCache[t] = CalculatePoliciesForType(t);
                        policiesForTypeCache = newCache;
                    }
                }
            }
            return policiesForTypeCache[t];
        }

        /// <summary>
        /// Gets the policies that apply to the given member.
        /// </summary>
        /// <param name="member">Member to get policies for.</param>
        /// <returns>Collection of policies that apply to this member.</returns>
        public IEnumerable<Policy> GetPoliciesFor(MethodBase member)
        {
            foreach(Policy policy in this)
            {
                if( policy.Matches(member))
                {
                    yield return policy;
                }
            }
        }

        /// <summary>
        /// Gets the policies in the <see cref="PolicySet"/> that do not
        /// apply to the given member.
        /// </summary>
        /// <param name="member">Member to check.</param>
        /// <returns>Collection of policies that do not apply to <paramref name="member"/>.</returns>
        public IEnumerable<Policy> GetPoliciesNotFor(MethodBase member)
        {
            foreach(Policy policy in this)
            {
                if( !policy.Matches(member))
                {
                    yield return policy;
                }
            }
        }

        /// <summary>
        /// Gets the handlers that apply to the given member based on all policies in the <see cref="PolicySet"/>.
        /// </summary>
        /// <param name="member">Member to get handlers for.</param>
        /// <returns>Collection of call handlers for <paramref name="member"/>.</returns>
        public IEnumerable<ICallHandler> GetHandlersFor(MethodBase member)
        {
            if (!handlerListCache.ContainsKey(member))
            {
                lock(handlerListCacheLock)
                {
                    if(!handlerListCache.ContainsKey(member))
                    {
                        Dictionary<MethodBase, List<ICallHandler>> newCache = new Dictionary<MethodBase, List<ICallHandler>>(handlerListCache);
                        newCache[member] = new List<ICallHandler>(CalculateHandlersFor(this, member));
                        handlerListCache = newCache;
                    }
                }
            }
            return handlerListCache[member];
        }

        private PolicySet CalculatePoliciesForType(Type t)
        {
            PolicySet result = new PolicySet();
            result.RemoveAt(0); // We don't want the extra attribute driven policy here.
            foreach (Policy policy in this)
            {
                if (policy.AppliesTo(t))
                {
                    result.Add(policy);
                }
            }
            return result;
        }

        internal static IEnumerable<ICallHandler> CalculateHandlersFor(IEnumerable<Policy> policies, MethodBase member)
        {
            List<ICallHandler> ordered = new List<ICallHandler>();
            List<ICallHandler> nonOrdered = new List<ICallHandler>();

            foreach (Policy p in policies)
            {
                foreach (ICallHandler handler in p.GetHandlersFor(member))
                {
                    if (handler.Order != 0)
                    {
                        bool inserted = false;
                        // add in order to ordered
                        for (int i = ordered.Count - 1; i >= 0; i--)
                        {
                            if (ordered[i].Order <= handler.Order)
                            {
                                ordered.Insert(i + 1, handler);
                                inserted = true;
                                break;
                            }
                        }
                        if (!inserted)
                        {
                            ordered.Insert(0, handler);
                        }
                    }
                    else
                    {
                        nonOrdered.Add(handler);
                    }
                }
            }
            ordered.AddRange(nonOrdered);
            return ordered;
        }

        #region Cache access and management

        private void ResetCaches()
        {
            lock(policiesForTypeCacheLock)
            {
                policiesForTypeCache = new Dictionary<Type, PolicySet>();
            }

            lock(handlerListCacheLock)
            {
                handlerListCache = new Dictionary<MethodBase, List<ICallHandler>>();
            }
        }

        private void OnPolicyChanged(object sender, EventArgs e)
        {
            ResetCaches();
        }

        #endregion

        #region Collection management

        /// <summary>
        /// Method called when any of the contained policies change.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">EventArgs.</param>
        protected override void OnCollectionChanged(object sender, EventArgs e)
        {
            base.OnCollectionChanged(sender, e);
            ResetCaches();
        }

        /// <summary>
        /// Called when all items are removed from the <see cref="PolicySet"/>.
        /// </summary>
        protected override void ClearItems()
        {
            ForEach(delegate(Policy p)
                    {
                        p.PolicyChanged -= OnPolicyChanged;
                    });
            base.ClearItems();
        }

        /// <summary>
        /// Called when an item is inserted or added into the <see cref="PolicySet"/>.
        /// </summary>
        /// <param name="index">Index where item was inserted.</param>
        /// <param name="item">Policy that was inserted.</param>
        protected override void InsertItem(int index, Policy item)
        {
            base.InsertItem(index, item);
            item.PolicyChanged += OnPolicyChanged;
        }

        /// <summary>
        /// Called when an item is removed from the <see cref="PolicySet"/>. This method
        /// is called just before the item is actually removed from the collection.
        /// </summary>
        /// <param name="index">Index of item to be removed.</param>
        protected override void RemoveItem(int index)
        {
            this[index].PolicyChanged -= OnPolicyChanged;
            base.RemoveItem(index);
        }

        /// <summary>
        /// Called when an item is being replaced with another one.
        /// </summary>
        /// <param name="index">Index of item that is being replaced.</param>
        /// <param name="item">New item.</param>
        protected override void SetItem(int index, Policy item)
        {
            this[index].PolicyChanged -= OnPolicyChanged;
            base.SetItem(index, item);
            item.PolicyChanged += OnPolicyChanged;
        }

        #endregion
    }
}
