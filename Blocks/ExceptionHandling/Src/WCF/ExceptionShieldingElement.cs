//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using System.ServiceModel.Configuration;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    /// <summary>
    /// Represents a configuration element that specifies exception shielding features 
    /// for a Windows Communication Foundation (WCF) service.
    /// </summary>
    public class ExceptionShieldingElement : BehaviorExtensionElement
    {
        /// <summary>
        /// The attribute name for the exceptionPolicyName.
        /// </summary>
        public const string ExceptionPolicyNameAttributeName = "exceptionPolicyName";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ExceptionShieldingElement"/> class.
        /// </summary>
        public ExceptionShieldingElement()
        {
            this.ExceptionPolicyName = ExceptionShielding.DefaultExceptionPolicy;
        }

        /// <summary>
        /// Gets or sets the name of the exception policy.
        /// </summary>
        /// <value>The name of the exception policy.</value>
        [ConfigurationProperty(
            ExceptionShieldingElement.ExceptionPolicyNameAttributeName, 
            DefaultValue = ExceptionShielding.DefaultExceptionPolicy, 
            IsRequired=false)]
        public string ExceptionPolicyName
        {
            get { return this[ExceptionPolicyNameAttributeName] as string; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = ExceptionShielding.DefaultExceptionPolicy;
                }
                this[ExceptionPolicyNameAttributeName] = value; 
            }
        }

        /// <summary>
        /// Copies the content of the specified configuration element to this configuration element.
        /// </summary>
        /// <param name="from">The configuration element to be copied.</param>
        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);
            ExceptionShieldingElement element = (ExceptionShieldingElement)from;
            this.ExceptionPolicyName = element.ExceptionPolicyName;
        }

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"></see>.</returns>
        public override Type BehaviorType
        {
            get { return typeof(ExceptionShieldingBehavior); }
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>The behavior extension.</returns>
        protected override object CreateBehavior()
        {
            return new ExceptionShieldingBehavior(this.ExceptionPolicyName);
        }
    }
}
