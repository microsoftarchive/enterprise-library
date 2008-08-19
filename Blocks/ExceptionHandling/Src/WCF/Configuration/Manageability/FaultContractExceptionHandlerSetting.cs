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
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information from a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.FaultContractExceptionHandlerData"/> instance.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.FaultContractExceptionHandlerData"/>
    [ManagementEntity]
    public class FaultContractExceptionHandlerSetting : ExceptionHandlerSetting
    {
        string[] attributes;
        string exceptionMessage;
        string faultContractType;

        /// <summary>
        /// Initialize a new instance of the <see cref="FaultContractExceptionHandlerSetting"/> class with a source element,
        /// the name of the handler, the exception message, the fault contract type and the attributes for the contract type.
        /// </summary>
        /// <param name="sourceElement">The configuration source element.</param>
        /// <param name="name">The name of the handler.</param>
        /// <param name="exceptionMessage">The exception message for the handler.</param>
        /// <param name="faultContractType">The fault contract type.</param>
        /// <param name="attributes">The attributes for the fault contract.</param>
        public FaultContractExceptionHandlerSetting(ConfigurationElement sourceElement,
                                                    string name,
                                                    string exceptionMessage,
                                                    string faultContractType,
                                                    string[] attributes)
            : base(sourceElement, name)
        {
            this.exceptionMessage = exceptionMessage;
            this.faultContractType = faultContractType;
            this.attributes = attributes;
        }

        /// <summary>
        /// Gets the collection of attributes for the custom exception handler represented as a 
        /// <see cref="string"/> array of key/value pairs.
        /// </summary>
        [ManagementConfiguration]
        public string[] Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        /// <summary>
        /// Gets the name of the exception message for the represented configuration object.
        /// </summary>
        [ManagementConfiguration]
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }

        /// <summary>
        /// Gets the name of the fault contract type for the represented configuration object.
        /// </summary>
        [ManagementConfiguration]
        public string FaultContractType
        {
            get { return faultContractType; }
            set { faultContractType = value; }
        }

        /// <summary>
        /// Returns the <see cref="FaultContractExceptionHandlerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="applicationName">The value for the ApplicationName key property.</param>
        /// <param name="sectionName">The value for the SectionName key property.</param>
        /// <param name="policy"></param>
        /// <param name="exceptionType"></param>
        /// <param name="name">The value for the Name key property.</param>
        /// <returns>The published <see cref="FaultContractExceptionHandlerSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static FaultContractExceptionHandlerSetting BindInstance(string applicationName,
                                                                        string sectionName,
                                                                        string policy,
                                                                        string exceptionType,
                                                                        string name)
        {
            return BindInstance<FaultContractExceptionHandlerSetting>(applicationName, sectionName, policy, exceptionType, name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="FaultContractExceptionHandlerSetting"/> instances.
        /// </summary>
        /// <returns>Sequence of <see cref="FaultContractExceptionHandlerSetting"/></returns>
        [ManagementEnumerator]
        public static IEnumerable<FaultContractExceptionHandlerSetting> GetInstances()
        {
            return GetInstances<FaultContractExceptionHandlerSetting>();
        }

        ///<summary>
        /// Save changes from the Wmi objects back to configuration.
        ///</summary>
        ///<param name="sourceElement">The configuration element.</param>
        ///<returns>true if the changes were saved; otherwise, false.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return FaultContractExceptionHandlerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}