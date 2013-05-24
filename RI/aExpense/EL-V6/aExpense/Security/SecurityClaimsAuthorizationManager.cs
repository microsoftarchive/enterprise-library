#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Xml;

namespace AExpense
{
    /// <summary>
    /// Custom <see cref="ClaimsAuthorizationManager"/> that make use of a configured claim rules (ex Authorization Rule Provider)
    /// </summary>
    public class SecurityClaimsAuthorizationManager : ClaimsAuthorizationManager
    {
        // Simple policy cache
        private Dictionary<string, IList<Claim>> policies = new Dictionary<string, IList<Claim>>();
        private IEqualityComparer<Claim> claimComparer = new ClaimEqualityComparer();

        public override bool CheckAccess(AuthorizationContext context)
        {
            string policyKey = context.Resource.First().Value;

            // Enforce access according to the stored policies
            if (this.policies.ContainsKey(policyKey))
            {
                var policy = this.policies[policyKey];
                return context.Principal.Claims.Intersect(policy, this.claimComparer).Any();
            }

            // Assume public access if no polices were configured for this context
            return true;
        }

        public override void LoadCustomConfiguration(XmlNodeList nodelist)
        {
            // Load policies from config
            // This may be loaded from an external resource in a production scenario.

            foreach (XmlNode node in nodelist.Cast<XmlNode>().Where(n => n.NodeType == XmlNodeType.Element))
            {
                string policyKey = node.Attributes["resource"].Value;

                // We accept GET/POST verbs so we ignore the value for simplicity
                var claims = node.ChildNodes.OfType<XmlNode>().
                    Select(n => new Claim(n.Attributes["claimType"].Value, n.Attributes["claimValue"].Value)).ToList();

                this.policies.Add(policyKey, claims);
            }
        }

        private class ClaimEqualityComparer : EqualityComparer<Claim>
        {
            public override bool Equals(Claim x, Claim y)
            {
                return x.Type == y.Type && x.Value == y.Value;
            }

            public override int GetHashCode(Claim obj)
            {
                return obj.Type.GetHashCode() ^ obj.Value.GetHashCode();
            }
        }
    }
}