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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// A <see cref="ConfigurationNode"/> that represents the configuration
    /// of an <see cref="ValidationCallHandler" /> in the Configuration
    /// Console.
    /// </summary>
    public class ValidationCallHandlerNode : CallHandlerNode
    {
        string ruleSet;
        SpecificationSource specificationSource;

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerNode"/> with default settings.
        /// </summary>
        public ValidationCallHandlerNode()
            : this(new ValidationCallHandlerData(Resources.ValidationCallHandlerNodeName)) {}

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerNode"/> with the given settings.
        /// </summary>
        /// <param name="callHandlerData">Configuration information to initialize the node with.</param>
        public ValidationCallHandlerNode(ValidationCallHandlerData callHandlerData)
            : base(callHandlerData)
        {
            specificationSource = callHandlerData.SpecificationSource;
            ruleSet = callHandlerData.RuleSet;
        }

        /// <summary>
        /// Validation ruleset to use in the validation operation.
        /// </summary>
        /// <value>Get or set ruleset name.</value>
        [SRDescription("RuleSetDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string RuleSet
        {
            get { return ruleSet; }
            set { ruleSet = value; }
        }

        /// <summary>
        /// Validation specification source.
        /// </summary>
        /// <value>Get or set source of validation (config, attributes, or both).</value>
        [SRDescription("SpecificationSourceDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public SpecificationSource SpecificationSource
        {
            get { return specificationSource; }
            set { specificationSource = value; }
        }

        /// <summary>
        /// Converts the information stored in the node and generate
        /// the corresponding configuration element to store in
        /// an <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource" />.
        /// </summary>
        /// <returns>Newly created <see cref="ValidationCallHandlerData"/> containing
        /// the configuration data from this node.</returns>
        public override CallHandlerData CreateCallHandlerData()
        {
            ValidationCallHandlerData callHandlerData = new ValidationCallHandlerData(Name, Order);
            callHandlerData.SpecificationSource = specificationSource;
            callHandlerData.RuleSet = ruleSet;

            return callHandlerData;
        }
    }
}