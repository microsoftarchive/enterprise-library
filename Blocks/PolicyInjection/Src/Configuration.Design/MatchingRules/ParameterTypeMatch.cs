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
using ParameterKind = Microsoft.Practices.Unity.InterceptionExtension.ParameterKind;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
    /// <summary>
    /// This class encapsulates the information about a single
    /// entry in the list for the ParameterTypeMatchingRule.
    /// </summary>
    public class ParameterTypeMatch : Match
    {
        private ParameterKind kind;

        /// <summary>
        /// Construct a new ParameterTypeMatch object with empty settings.
        /// </summary>
        public ParameterTypeMatch()
        {
        }

        /// <summary>
        /// Construct a new ParameterTypeMatch object from configuration data.
        /// </summary>
        /// <param name="matchData"></param>
        public ParameterTypeMatch(ParameterTypeMatchData matchData)
            : base(matchData.Match, matchData.IgnoreCase)
        {
            this.kind = matchData.ParameterKind;
        }

        /// <summary>
        /// Construct a new ParameterTypeMatch object.
        /// </summary>
        /// <param name="match">String the defines the type to match.</param>
        /// <param name="ignoreCase">If true, use case insensitive comparison for the type. If false, case sensitive.</param>
        /// <param name="kind">Wether this parameter is an input, output, input or output, or return type.</param>
        public ParameterTypeMatch(string match, bool ignoreCase, ParameterKind kind)
            : base(match, ignoreCase)
        {
            this.kind = kind;
        }

        /// <summary>
        /// Construct the configuration node from this design time node.
        /// </summary>
        /// <returns>Configuration data.</returns>
        public ParameterTypeMatchData ToMatchData()
        {
            return new ParameterTypeMatchData(Value, kind, IgnoreCase);
        }

        /// <summary>
        /// Gets or sets the kind of parameter to match: inputs, outputs, inputs or outputs, or return values.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public ParameterKind Kind
        {
            get { return kind; }
            set { kind = value; }
        }
    }
}
