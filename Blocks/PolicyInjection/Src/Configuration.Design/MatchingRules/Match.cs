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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
    /// <summary>
    /// Represents an individual textual match.
    /// </summary>
    public class Match
    {
        private string match;
        private bool ignoreCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="Match"/> class with the provided settings.
        /// </summary>
        /// <param name="match">The text to match.</param>
        /// <param name="ignoreCase">The indication of whether matching should be case-insensitive.</param>
        public Match(string match, bool ignoreCase)
        {
            this.match = match;
            this.ignoreCase = ignoreCase;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="Match"/> class with default values.
        /// </summary>
        public Match()
        {
        }

        /// <summary>
        /// Gets or sets the value to match.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Value
        {
            get { return match; }
            set { match = value; }
        }

        /// <summary>
        /// Gets or sets the indication of whether matching should be case-insensitive.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool IgnoreCase
        {
            get { return ignoreCase; }
            set { ignoreCase = value; }
        }

        /// <summary>
        /// Returns a string representation of the object.
        /// </summary>
        /// <returns>The match value.</returns>
        public override string ToString()
        {
            return match;
        }
    }
}
