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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;
using PropertyMatchingOption = Microsoft.Practices.Unity.InterceptionExtension.PropertyMatchingOption;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
    /// <summary>
    /// Represents a match on a property.
    /// </summary>
    public class PropertyMatch : Match
    {
        PropertyMatchingOption matchOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMatch"/> class with default values.
        /// </summary>
        public PropertyMatch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMatch"/> class with the supplied values.
        /// </summary>
        /// <param name="match">The string to match the property name.</param>
        /// <param name="ignoreCase">The indication of whether matching should be case-insensitive.</param>
        /// <param name="matchOption">The <see cref="PropertyMatchingOption"/> indicating how matching should be performed.</param>
        public PropertyMatch(string match, bool ignoreCase, PropertyMatchingOption matchOption)
            : base(match, ignoreCase)
        {
            this.matchOption = matchOption;
        }

        /// <summary>
        /// Gets or sets the <see cref="PropertyMatchingOption"/> indicating how matching should be performed.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public PropertyMatchingOption MatchOption
        {
            get { return matchOption; }
            set { matchOption = value; }
        }
    }
}
