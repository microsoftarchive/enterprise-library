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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration data for a <see cref="ReplaceHandler"/>.
    /// </summary>		
    public class ReplaceHandlerData : ExceptionHandlerData
    {
        private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();
        private const string exceptionMessageProperty = "exceptionMessage";
        private const string replaceExceptionTypeProperty = "replaceExceptionType";
        private const string ExceptionMessageResourceTypeNameProperty = "exceptionMessageResourceType";
        private const string ExceptionMessageResourceNameProperty = "exceptionMessageResourceName";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceHandlerData"/> class.
        /// </summary>
        public ReplaceHandlerData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceHandlerData"/> class with a name, exception message, and replace exception type name.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="ReplaceHandlerData"/>.
        /// </param>
        /// <param name="exceptionMessage">
        /// The exception message replacement.
        /// </param>
        /// <param name="replaceExceptionTypeName">
        /// The fully qualified assembly name the type of the replacement exception.
        /// </param>
        public ReplaceHandlerData(string name, string exceptionMessage, string replaceExceptionTypeName)
            : base(name, typeof(ReplaceHandler))
        {
            ExceptionMessage = exceptionMessage;
            ReplaceExceptionTypeName = replaceExceptionTypeName;
        }

        /// <summary>
        /// Gets or sets the message for the replacement exception.
        /// </summary>
        [ConfigurationProperty(exceptionMessageProperty, IsRequired = false)]
        public string ExceptionMessage
        {
            get { return (string)this[exceptionMessageProperty]; }
            set { this[exceptionMessageProperty] = value; }
        }

        /// <summary>
        /// !~!
        /// </summary>
        [ConfigurationProperty(ExceptionMessageResourceNameProperty)]
        public string ExceptionMessageResourceName
        {
            get { return (string)this[ExceptionMessageResourceNameProperty]; }
            set { this[ExceptionMessageResourceNameProperty] = value; }
        }

        /// <summary>
        /// !~!
        /// </summary>
        [ConfigurationProperty(ExceptionMessageResourceTypeNameProperty)]
        public string ExceptionMessageResourceType
        {
            get { return (string)this[ExceptionMessageResourceTypeNameProperty]; }
            set { this[ExceptionMessageResourceTypeNameProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the type of the replacement exception.
        /// </summary>
        public Type ReplaceExceptionType
        {
            get { return (Type)typeConverter.ConvertFrom(ReplaceExceptionTypeName); }
            set { ReplaceExceptionTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified type name of the replacement exception.
        /// </summary>
        /// <value>
        /// The fully qualified type name of the replacement exception.
        /// </value>
        [ConfigurationProperty(replaceExceptionTypeProperty, IsRequired = true)]
        public string ReplaceExceptionTypeName
        {
            get { return (string)this[replaceExceptionTypeProperty]; }
            set { this[replaceExceptionTypeProperty] = value; }
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
                        Name = BuildName(namePrefix)
                };
        }
    }
}
