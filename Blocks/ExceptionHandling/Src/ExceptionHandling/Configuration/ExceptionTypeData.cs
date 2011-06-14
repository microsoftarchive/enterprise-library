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
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Container = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Container;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    partial class ExceptionTypeData
    {
        private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// Gets or sets the <see cref="Exception"/> type.
        /// </summary>
        /// <value>
        /// The <see cref="Exception"/> type
        /// </value>
        public Type Type
        {
            get { return (Type)typeConverter.ConvertFrom(TypeName); }
            set { TypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Returns the <see cref="TypeRegistration"/> container configuration model to register <see cref="ExceptionPolicyEntry"/> items with the container.
        /// </summary>
        /// <param name="namePrefix"></param>
        /// <returns>A <see cref="TypeRegistration"/></returns>
        public TypeRegistration GetRegistration(string namePrefix)
        {
            string registrationName = BuildChildName(namePrefix, Name);

            return new TypeRegistration<ExceptionPolicyEntry>(
                () =>
                    new ExceptionPolicyEntry(
                        Type,
                        PostHandlingAction,
                        Container.ResolvedEnumerable<IExceptionHandler>(from hd in ExceptionHandlers select BuildChildName(registrationName, hd.Name)),
                        Container.Resolved<IExceptionHandlingInstrumentationProvider>(namePrefix)))
               {
                   Name = registrationName,
                   Lifetime = TypeRegistrationLifetime.Transient
               };
        }

        private static string BuildChildName(string name, string childName)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", name, childName);
        }
    }
}
