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
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    /// <summary>
    /// Exception handler that gets the configured fault contract type and 
    /// wraps it inside a <see cref="FaultContractWrapperException"/>. exception.
    /// </summary>
    [ConfigurationElementType(typeof(FaultContractExceptionHandlerData))]
    public class FaultContractExceptionHandler : IExceptionHandler
    {
        private NameValueCollection attributes;
        private Type faultContractType;
        private string exceptionMessage;
        private readonly IStringResolver exceptionMessageResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultContractExceptionHandler"/> class.
        /// </summary>
        /// <param name="faultContractType">Type of the fault contract.</param>
        /// <param name="attributes">The attributes.</param>
        public FaultContractExceptionHandler(Type faultContractType, NameValueCollection attributes)
            : this(faultContractType, null, attributes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultContractExceptionHandler"/> class.
        /// </summary>
        /// <param name="faultContractType">Type of the fault contract.</param>
        /// <param name="exceptionMessage">A fixed exception message that will replace the fault contract message.</param>
        /// <param name="attributes">A collection of nam value paries that specify the mapping
        /// between the properties of the FaultContract class and the values in the <see cref="Exception"/>
        /// instance. You can specify something like: FaultPropertyName = "{Message}" where {Message}
        /// points to the Message property value of the current exception. You can also specify a
        /// {Guid} value that will load the current exception handlingInstanceId value.
        /// See <see cref="HandleException"/>. NOTICE that names and values are case sensitive.</param>
        /// <exception cref="ArgumentNullException"/>
        public FaultContractExceptionHandler(Type faultContractType, string exceptionMessage, NameValueCollection attributes) :
            this(new ConstantStringResolver(exceptionMessage), faultContractType, attributes)
        {
            if (faultContractType == null)
            {
                throw new ArgumentNullException("faultContractType");
            }
            this.faultContractType = faultContractType;
            this.attributes = attributes;
            this.exceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultContractExceptionHandler"/> class.
        /// </summary>
        /// <param name="exceptionMessageResolver">The exception message resolver.</param>
        /// <param name="faultContractType">Type of the fault contract.</param>
        /// <param name="attributes">A collection of nam value paries that specify the mapping
        /// between the properties of the FaultContract class and the values in the <see cref="Exception"/>
        /// instance. You can specify something like: FaultPropertyName = "{Message}" where {Message}
        /// points to the Message property value of the current exception. You can also specify a
        /// {Guid} value that will load the current exception handlingInstanceId value.
        /// See <see cref="HandleException"/>. NOTICE that names and values are case sensitive.</param>
        /// <exception cref="ArgumentNullException"/>
        public FaultContractExceptionHandler(IStringResolver exceptionMessageResolver, Type faultContractType, NameValueCollection attributes)
        {
            if (faultContractType == null)
            {
                throw new ArgumentNullException("faultContractType");
            }

            this.faultContractType = faultContractType;
            this.attributes = attributes;
            this.exceptionMessageResolver = exceptionMessageResolver;
            this.exceptionMessage = this.exceptionMessageResolver.GetString();
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="handlingInstanceId">The handling instance id. This value may be injected into the
        /// current FaultContract by spefifying the attribute value "{Guid}".</param>
        /// <returns>An instance of <see cref="FaultContractWrapperException"/> class.</returns>
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            EnsureDefaultConstructor();
            object fault = Activator.CreateInstance(faultContractType);
            PopulateFaultContractFromException(fault, exception, handlingInstanceId);
            return new FaultContractWrapperException(
                fault, handlingInstanceId, ExceptionUtility.GetMessage(exception, exceptionMessage, handlingInstanceId));
        }

        private void EnsureDefaultConstructor()
        {
            if (faultContractType.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new MissingMethodException(
                    string.Format(CultureInfo.CurrentCulture, Properties.Resources.NoDefaultParameterInFaultContract, faultContractType.FullName));
            }
        }

        private void PopulateFaultContractFromException(object fault, Exception exception, Guid handlingInstanceId)
        {
            if (this.attributes != null)
            {
                foreach (PropertyInfo property in this.faultContractType.GetProperties())
                {
                    if (PropertyIsMappedInAttributes(property))
                    {
                        // Try to map with  a configured property first
                        string configProperty = this.attributes[property.Name];
                        if (!string.IsNullOrEmpty(configProperty))
                        {
                            // strip out the {}
                            configProperty = configProperty.Replace("{", "").Replace("}", "");

                            if (PropertyIsGuid(property, configProperty))
                            {
                                property.SetValue(fault, handlingInstanceId, null);
                            }
                            else
                            {
                                PropertyInfo mappedExceptionProperty = GetMappedProperty(exception, configProperty);
                                if (PropertiesMatch(property, mappedExceptionProperty))
                                {
                                    // by now it's certain that properties have matching types
                                    // and values can be both read and written as required
                                    property.SetValue(fault,
                                        mappedExceptionProperty.GetValue(exception, null),
                                        null);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (PropertyNamesMatch(property, exception))
                        {
                            // by now it's certain that properties have matching types
                            // and values can be both read and written as required
                            property.SetValue(fault,
                                GetExceptionProperty(property, exception).GetValue(exception, null),
                                null);
                        }
                    }
                }
            }
        }

        private bool PropertyIsGuid(PropertyInfo property, string configPropertyName)
        {
            return
                configPropertyName.Equals(
                    ExceptionShielding.HandlingInstanceIdPropertyMappingName,
                    StringComparison.OrdinalIgnoreCase)
                && property.PropertyType == typeof(Guid);
        }

        private bool PropertiesMatch(PropertyInfo faultProperty, PropertyInfo exceptionProperty)
        {
            // matching only occurs if properties have the same type, are not indexers,
            // and the fault contract property can be written while the exception property can be read
            return exceptionProperty != null
                && exceptionProperty.PropertyType == faultProperty.PropertyType
                && faultProperty.CanWrite
                && faultProperty.GetIndexParameters().Length == 0
                && exceptionProperty.CanRead
                && exceptionProperty.GetIndexParameters().Length == 0;
        }

        private bool PropertyNamesMatch(PropertyInfo faultProperty, Exception exception)
        {
            PropertyInfo exceptionProperty = GetExceptionProperty(faultProperty, exception);

            return PropertiesMatch(faultProperty, exceptionProperty);
        }

        private bool PropertyIsMappedInAttributes(PropertyInfo property)
        {
            return Array.Exists(this.attributes.AllKeys,
                delegate(string s) { return s == property.Name; });
        }

        private PropertyInfo GetMappedProperty(Exception exception, string configProperty)
        {
            return exception.GetType().GetProperty(configProperty);
        }

        private PropertyInfo GetExceptionProperty(PropertyInfo property, Exception exception)
        {
            return exception.GetType().GetProperty(property.Name);
        }
    }
}
