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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{

    /// <summary/>
    public static class WcfExceptionShieldingConfigurationSourceBuilderExtensions
    {
        /// <summary/>
        public static IExceptionConfigurationWcfShieldingProvider ShieldExceptionForWcf(this IExceptionConfigurationAddExceptionHandlers context, Type faultContractType, string faultContractMessage)
        {
            IExceptionHandlerExtension exceptionHandlerExtension = (IExceptionHandlerExtension)context;
            FaultContractExceptionHandlerData shieldingHandling = new FaultContractExceptionHandlerData
            {
                Name = faultContractType.FullName,
                FaultContractType = faultContractType.AssemblyQualifiedName,
                ExceptionMessage = faultContractMessage
            };
            exceptionHandlerExtension.CurrentExceptionTypeData.ExceptionHandlers.Add(shieldingHandling);

            return new ExceptionConfigurationLoggingProviderBuilder(
                (IExceptionConfigurationForExceptionTypeOrPostHandling)context,
                shieldingHandling);

        }

        private class ExceptionConfigurationLoggingProviderBuilder : ExceptionConfigurationAddExceptionHandlers, IExceptionConfigurationWcfShieldingProvider
        {
            FaultContractExceptionHandlerData schieldingHandler;

            public ExceptionConfigurationLoggingProviderBuilder(IExceptionConfigurationForExceptionTypeOrPostHandling context, FaultContractExceptionHandlerData schieldingHandler)
                :base(context)
            {
                this.schieldingHandler = schieldingHandler;
            }

            public IExceptionConfigurationWcfShieldingProvider MapProperty(string name, string source)
            {
                this.schieldingHandler.PropertyMappings.Add(
                    new FaultContractExceptionHandlerMappingData(name, source)
                );

                return this;
            }

        }
    }

    /// <summary/>
    public interface IExceptionConfigurationWcfShieldingProvider : IExceptionConfigurationForExceptionTypeOrPostHandling
    {
        /// <summary/>
        IExceptionConfigurationWcfShieldingProvider MapProperty(string name, string source);
    }
}
