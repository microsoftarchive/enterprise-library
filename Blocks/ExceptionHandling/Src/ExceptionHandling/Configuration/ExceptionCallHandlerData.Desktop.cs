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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    [ResourceDescription(typeof(DesignResources), "ExceptionCallHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ExceptionCallHandlerDataDisplayName")]
    [AddSateliteProviderCommand(ExceptionHandlingSettings.SectionName)]
    partial class ExceptionCallHandlerData
    {
        private const string ExceptionPolicyNamePropertyName = "exceptionPolicyName";

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        public ExceptionCallHandlerData()
        {
            Type = typeof(ExceptionCallHandler);
        }

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        public ExceptionCallHandlerData(string handlerName)
            : base(handlerName, typeof(ExceptionCallHandler))
        {
        }

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        /// <param name="exceptionPolicyName">Exception policy name to use in handler.</param>
        public ExceptionCallHandlerData(string handlerName, string exceptionPolicyName)
            : base(handlerName, typeof(ExceptionCallHandler))
        {
            ExceptionPolicyName = exceptionPolicyName;
        }

        /// <summary>
        /// Construct a new <see cref="ExceptionCallHandlerData"/>.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        /// <param name="handlerOrder">Order to use in handler.</param>
        public ExceptionCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(ExceptionCallHandler))
        {
            Order = handlerOrder;
        }

        /// <summary>
        /// The exception policy name as defined in configuration for the Exception Handling Application Block.
        /// </summary>
        /// <value>The "exceptionPolicyName" attribute in configuration</value>
        [ConfigurationProperty(ExceptionPolicyNamePropertyName, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "ExceptionCallHandlerDataExceptionPolicyNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ExceptionCallHandlerDataExceptionPolicyNameDisplayName")]
        [Reference(typeof(ExceptionHandlingSettings), typeof(ExceptionPolicyData))]
        public string ExceptionPolicyName
        {
            get { return (string)base[ExceptionPolicyNamePropertyName]; }
            set { base[ExceptionPolicyNamePropertyName] = value; }
        }
    }
}
