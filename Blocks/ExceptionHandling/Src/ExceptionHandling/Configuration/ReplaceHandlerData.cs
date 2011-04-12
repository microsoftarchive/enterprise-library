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
    /// Represents the configuration data for a <see cref="ReplaceHandler"/>.
    /// </summary>
    public partial class ReplaceHandlerData : ExceptionHandlerData
    {
        private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// Gets or sets the type of the replacement exception.
        /// </summary>
        public Type ReplaceExceptionType
        {
            get { return (Type)typeConverter.ConvertFrom(ReplaceExceptionTypeName); }
            set { ReplaceExceptionTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// A <see cref="TypeRegistration"/> container configuration model for <see cref="ReplaceHandler"/>.
        /// </summary>
        /// <param name="namePrefix">The prefix to use when determining references to child elements.</param>
        /// <returns>A <see cref="TypeRegistration"/> for registering a <see cref="ReplaceHandler"/> in the container.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string namePrefix)
        {
            IStringResolver resolver
                = new ResourceStringResolver(ExceptionMessageResourceType, ExceptionMessageResourceName, ExceptionMessage);

            yield return
                new TypeRegistration<IExceptionHandler>(
                    () => new ReplaceHandler(resolver, ReplaceExceptionType))
                {
                    Name = BuildName(namePrefix),
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
