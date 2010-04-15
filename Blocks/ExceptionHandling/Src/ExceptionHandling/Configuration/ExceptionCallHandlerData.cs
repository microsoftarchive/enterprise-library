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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
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

        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the call handler represented by this config element and its associated objects.
        /// </summary>
        /// <param name="nameSuffix">A suffix for the names in the generated type registration objects.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string nameSuffix)
        {
            yield return
                new TypeRegistration<ICallHandler>(() =>
                                                   new ExceptionCallHandler(Container.Resolved<ExceptionPolicyImpl>(this.ExceptionPolicyName))
                                                       {
                                                           Order = this.Order
                                                       })
                    {
                        Name = this.Name + nameSuffix,
                        Lifetime = TypeRegistrationLifetime.Transient
                    };
        }
    }
}
