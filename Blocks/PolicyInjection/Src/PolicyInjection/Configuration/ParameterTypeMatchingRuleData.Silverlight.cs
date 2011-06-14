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
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A class storing configuration information for an instance of
    /// <see cref="ParameterTypeMatchingRule"/>.
    /// </summary>
    public partial class ParameterTypeMatchingRuleData
    {
        private readonly MatchDataCollection<ParameterTypeMatchData> matches = new MatchDataCollection<ParameterTypeMatchData>();

        /// <summary>
        /// The collection of parameter types to match against.
        /// </summary>
        public MatchDataCollection<ParameterTypeMatchData> Matches
        {
            get { return this.matches; }
        }
    }

    /// <summary>
    /// An extended <see cref="MatchData"/> class that also includes the
    /// <see cref="ParameterKind"/> of the parameter to match.
    /// </summary>
    public partial class ParameterTypeMatchData
    {
        private ParameterKind parameterKind = ParameterKind.InputOrOutput;

        /// <summary>
        /// What kind of parameter is this? See <see cref="ParameterKind"/> for available values.
        /// </summary>
        public ParameterKind ParameterKind
        {
            get { return this.parameterKind; }
            set { this.parameterKind = value; }
        }
    }
}
