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
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{

    /// <summary>
    /// Provides fluent configuration exception handling extensions to <see cref="IExceptionConfigurationAddExceptionHandlers"/>
    /// </summary>
    public static class WcfExceptionShieldingConfigurationSourceBuilderExtensions
    {
        /// <summary>
        /// Shields an exception in Wcf by wrapping it in with a fault contract type.
        /// </summary>
        /// <param name="context">Interface to extend to add ShieldExceptionForWcf options.</param>
        /// <param name="faultContractType">Fault contract type use when wrapping an exception.</param>
        /// <param name="faultContractMessage">Exeption message to use on new exception.</param>
        /// <returns></returns>
        public static IExceptionConfigurationWcfShieldingProvider ShieldExceptionForWcf(this IExceptionConfigurationAddExceptionHandlers context, Type faultContractType, string faultContractMessage)
        {
            if (faultContractType == null) throw new ArgumentNullException("faultContractType");

            return new ExceptionConfigurationLoggingProviderBuilder((IExceptionConfigurationForExceptionTypeOrPostHandling)context,
                                                                    faultContractType, 
                                                                    faultContractMessage);
        }

        private class ExceptionConfigurationLoggingProviderBuilder : ExceptionHandlerConfigurationExtension, IExceptionConfigurationWcfShieldingProvider
        {
            readonly FaultContractExceptionHandlerData shieldingHandling;

            public ExceptionConfigurationLoggingProviderBuilder(IExceptionConfigurationForExceptionTypeOrPostHandling context, 
                                                                Type faultContractType, 
                                                                string faultContractMessage)
                :base(context)
            {
                shieldingHandling = new FaultContractExceptionHandlerData
                {
                    Name = faultContractType.FullName,
                    FaultContractType = faultContractType.AssemblyQualifiedName,
                    ExceptionMessage = faultContractMessage
                };
                
                base.CurrentExceptionTypeData.ExceptionHandlers.Add(shieldingHandling);
            }

            public IExceptionConfigurationWcfShieldingProvider MapProperty(string name, string source)
            {
                if (string.IsNullOrEmpty(name)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "name");

                this.shieldingHandling.PropertyMappings.Add(
                    new FaultContractExceptionHandlerMappingData(name, source)
                );

                return this;
            }

        }
    }
}
