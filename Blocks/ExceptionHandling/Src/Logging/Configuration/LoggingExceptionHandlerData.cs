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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration
{
    /// <summary>
    /// Represents configuration for a <see cref="LoggingExceptionHandler"/>.
    /// </summary>
    public partial class LoggingExceptionHandlerData : ExceptionHandlerData
    {
        private static readonly AssemblyQualifiedTypeNameConverter typeConverter
            = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// Gets or sets the formatter type.
        /// </summary>
        public Type FormatterType
        {
            get { return (Type)typeConverter.ConvertFrom(FormatterTypeName); }
            set { FormatterTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Returns the <see cref="TypeRegistration"/> for the exception handler data provided.
        /// </summary>
        /// <param name="namePrefix">The prefix to use when building references to child elements.</param>
        /// <returns>
        /// A <see cref="TypeRegistration"/> for the exception handler data
        /// </returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string namePrefix)
        {
            yield return new TypeRegistration<IExceptionHandler>(
                () =>
                new LoggingExceptionHandler(LogCategory, EventId, Severity, Title, Priority, FormatterType,
                                            Common.Configuration.ContainerModel.Container.Resolved<LogWriter>()))
                       {
                           Name = BuildName(namePrefix),
                           Lifetime = TypeRegistrationLifetime.Transient
                       };
        }
    }
}
