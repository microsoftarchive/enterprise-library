//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    [ResourceDescription(typeof(DesignResources), "ValidationCallHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ValidationCallHandlerDataDisplayName")]
    [AddSateliteProviderCommand(ValidationSettings.SectionName)]
    partial class ValidationCallHandlerData
    {
        private const string RuleSetPropertyName = "ruleSet";
        private const string SpecificationSourcePropertyName = "specificationSource";

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerData"/> instance.
        /// </summary>
        public ValidationCallHandlerData()
        {
            Type = typeof (ValidationCallHandler);
        }

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerData"/> instance.
        /// </summary>
        /// <param name="handlerName">Name of handler in configuration.</param>
        public ValidationCallHandlerData(string handlerName)
            : base(handlerName, typeof(ValidationCallHandler))
        {
        }

        /// <summary>
        /// Create a new <see cref="ValidationCallHandlerData"/> instance.
        /// </summary>
        /// <param name="handlerName">Name of handler in configuration.</param>
        /// <param name="handlerOrder">Order of handler in configuration.</param>
        public ValidationCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(ValidationCallHandler))
        {
            this.Order = handlerOrder;
        }

        /// <summary>
        /// The ruleset name to use for all types. Empty string means default ruleset 
        /// </summary>
        /// <value>The "ruleSet" configuration property.</value>
        [ConfigurationProperty(RuleSetPropertyName)]
        [Reference(typeof(ValidationSettings), typeof(ValidationRulesetData))]
        public string RuleSet
        {
            get { return (string)base[RuleSetPropertyName]; }
            set { base[RuleSetPropertyName] = value; }
        }

        /// <summary>
        /// SpecificationSource (Both | Attributes | Configuration) : Where to look for validation rules. Default is Both.
        /// </summary>
        /// <value>The "specificationSource" configuration attribute.</value>
        [ConfigurationProperty(SpecificationSourcePropertyName, IsRequired = true, DefaultValue = SpecificationSource.Both)]
        [ResourceDescription(typeof(DesignResources), "ValidationCallHandlerDataSpecificationSourceDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ValidationCallHandlerDataSpecificationSourceDisplayName")]
        public SpecificationSource SpecificationSource
        {
            get { return (SpecificationSource)base[SpecificationSourcePropertyName]; }
            set { base[SpecificationSourcePropertyName] = value; }
        }
    }
}
