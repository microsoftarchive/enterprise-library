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


using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Stores information about a single
    /// matchable item. Specifically, the string to match, and whether it is case
    /// sensitive or not.
    /// </summary>
    public partial class MatchData : NamedConfigurationElement
    {
        /// <summary>
        /// Gets or sets the string to match against.
        /// </summary>
        /// <value>The "match" attribute value out of the configuration file.</value>
        public string Match
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the case sensitivity flag.
        /// </summary>
        /// <value>The "ignoreCase" attribute value out of the configuration file.</value>
        public bool IgnoreCase
        {
            get;
            set;
        }
    }
}
