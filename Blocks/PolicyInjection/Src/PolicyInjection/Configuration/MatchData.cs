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


namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Stores information about a single
    /// matchable item. Specifically, the string to match, and whether it is case
    /// sensitive or not.
    /// </summary>
    public partial class MatchData
    {
        /// <summary>
        /// Constructs an empty <see cref="MatchData"/>.
        /// </summary>
        public MatchData()
        {
        }

        /// <summary>
        /// Constructs a <see cref="MatchData"/> with the given matching string.
        /// </summary>
        /// <param name="match">String to match.</param>
        public MatchData(string match)
        {
            Match = match;
        }

        /// <summary>
        /// Constructs a <see cref="MatchData"/> with the given matching string and case-sensitivity flag.
        /// </summary>
        /// <param name="match">String to match.</param>
        /// <param name="ignoreCase">true to do case insensitive comparison, false to do case sensitive.</param>
        public MatchData(string match, bool ignoreCase)
        {
            Match = match;
            IgnoreCase = ignoreCase;
        }
    }
}
