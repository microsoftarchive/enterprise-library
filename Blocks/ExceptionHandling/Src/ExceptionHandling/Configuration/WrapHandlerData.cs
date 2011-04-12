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
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration data for a <see cref="WrapHandler"/>.
    /// </summary>
    public partial class WrapHandlerData : ExceptionHandlerData
    {
        private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// Gets or sets the type of the replacement exception.
        /// </summary>
        public Type WrapExceptionType
        {
            get { return (Type)typeConverter.ConvertFrom(WrapExceptionTypeName); }
            set { WrapExceptionTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Retrieves a container configuration model for a <see cref="WrapHandler"/> based on the data in <see cref="WrapHandlerData"/>
        /// </summary>
        /// <param name="namePrefix">The name to use when building references to child items.</param>
        /// <returns>A <see cref="TypeRegistration"/> to register a <see cref="WrapHandler"/> in the container</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string namePrefix)
        {
            var exceptionMessageResolver =
                new ResourceStringResolver(ExceptionMessageResourceType, ExceptionMessageResourceName, ExceptionMessage);

            yield return
                new TypeRegistration<IExceptionHandler>(
                    () => new WrapHandler(exceptionMessageResolver, WrapExceptionType))
                    {
                        Name = BuildName(namePrefix),
                        Lifetime = TypeRegistrationLifetime.Transient
                    };
        }
    }
}
