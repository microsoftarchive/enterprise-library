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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Configuration element storing configuration information for the
    /// <see cref="ExceptionCallHandler"/> class.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "ExceptionCallHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ExceptionCallHandlerDataDisplayName")]
    [AddSateliteProviderCommand(ExceptionHandlingSettings.SectionName)]
    public class ExceptionCallHandlerData : CallHandlerData
    {
        private const string ExceptionPolicyNamePropertyName = "exceptionPolicyName";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCallHandlerData"/> class.
        /// </summary>
        public ExceptionCallHandlerData()
        {
            Type = typeof(ExceptionCallHandler);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCallHandlerData"/> class by using the specified handler name.
        /// </summary>
        /// <param name="handlerName">The name of the handler.</param>
        public ExceptionCallHandlerData(string handlerName)
            : base(handlerName, typeof(ExceptionCallHandler))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCallHandlerData"/> class by using the specified handler and exception policy name.
        /// </summary>
        /// <param name="handlerName">The name of the handler.</param>
        /// <param name="exceptionPolicyName">Exception policy name to use in handler.</param>
        public ExceptionCallHandlerData(string handlerName, string exceptionPolicyName)
            : base(handlerName, typeof(ExceptionCallHandler))
        {
            ExceptionPolicyName = exceptionPolicyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionCallHandlerData"/> class by using the specified handler name and order.
        /// </summary>
        /// <param name="handlerName">The name of the handler.</param>
        /// <param name="handlerOrder">The order to use in handler.</param>
        public ExceptionCallHandlerData(string handlerName, int handlerOrder)
            : base(handlerName, typeof(ExceptionCallHandler))
        {
            Order = handlerOrder;
        }

        /// <summary>
        /// Gets the exception policy name as defined in the configuration for the Exception Handling Application Block.
        /// </summary>
        /// <value>The "exceptionPolicyName" attribute in the configuration.</value>
        [ConfigurationProperty(ExceptionPolicyNamePropertyName, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "ExceptionCallHandlerDataExceptionPolicyNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ExceptionCallHandlerDataExceptionPolicyNameDisplayName")]
        [Reference(typeof(ExceptionHandlingSettings), typeof(ExceptionPolicyData))]
        public string ExceptionPolicyName
        {
            get { return (string)base[ExceptionPolicyNamePropertyName]; }
            set { base[ExceptionPolicyNamePropertyName] = value; }
        }

        /// <summary>
        /// Configures an <see cref="IUnityContainer"/> to resolve the represented call handler by using the specified name.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="registrationName">The name of the registration.</param>
        protected override void DoConfigureContainer(IUnityContainer container, string registrationName)
        {
            container.RegisterType<ICallHandler, ExceptionCallHandler>(
                registrationName,
                new InjectionConstructor(this.ExceptionPolicyName),
                new InjectionProperty("Order", this.Order));
        }
    }
}
