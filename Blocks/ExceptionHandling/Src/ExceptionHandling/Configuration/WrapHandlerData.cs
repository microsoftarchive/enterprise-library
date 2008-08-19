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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration data for a <see cref="WrapHandler"/>.
    /// </summary>	
    [Assembler(typeof(WrapHandlerAssembler))]
    [ContainerPolicyCreator(typeof(WrapHandlerPolicyCreator))]
    public class WrapHandlerData : ExceptionHandlerData
    {
        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        private const string exceptionMessageProperty = "exceptionMessage";
        private const string wrapExceptionTypeProperty = "wrapExceptionType";
        private const string ExceptionMessageResourceTypeNameProperty = "exceptionMessageResourceType";
        private const string ExceptionMessageResourceNameProperty = "exceptionMessageResourceName";

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapHandlerData"/> class.
        /// </summary>
        public WrapHandlerData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapHandlerData"/> class with a name, an exception message, and the fully qualified assembly name of the type of the wrapping exception.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="WrapHandlerData"/>.
        /// </param>
        /// <param name="exceptionMessage">
        /// The exception message replacement.
        /// </param>
        /// <param name="wrapExceptionTypeName">
        /// The fully qualified assembly name of type of the wrapping exception
        /// </param>
        public WrapHandlerData(string name, string exceptionMessage, string wrapExceptionTypeName)
            : base(name, typeof(WrapHandler))
        {
            this.ExceptionMessage = exceptionMessage;
            this.WrapExceptionTypeName = wrapExceptionTypeName;
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
        [ConfigurationProperty(ExceptionMessageResourceTypeNameProperty)]
        public string ExceptionMessageResourceType
        {
            get { return (string)this[ExceptionMessageResourceTypeNameProperty]; }
            set { this[ExceptionMessageResourceTypeNameProperty] = value; }
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
        /// Gets or sets the type of the replacement exception.
        /// </summary>
        public Type WrapExceptionType
        {
            get { return (Type)typeConverter.ConvertFrom(WrapExceptionTypeName); }
            set { WrapExceptionTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified type name of the replacement exception.
        /// </summary>
        /// <value>
        /// The fully qualified type name of the replacement exception.
        /// </value>
        [ConfigurationProperty(wrapExceptionTypeProperty, IsRequired = true)]
        public string WrapExceptionTypeName
        {
            get { return (string)this[wrapExceptionTypeProperty]; }
            set { this[wrapExceptionTypeProperty] = value; }
        }
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a <see cref="WrapHandler"/> described by a <see cref="WrapHandlerData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="WrapHandlerData"/> type and it is used by the <see cref="ExceptionHandlerCustomFactory"/> 
    /// to build the specific <see cref="IExceptionHandler"/> object represented by the configuration object.
    /// </remarks>
    public class WrapHandlerAssembler : IAssembler<IExceptionHandler, ExceptionHandlerData>
    {
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="WrapHandler"/> based on an instance of <see cref="WrapHandlerData"/>.
        /// </summary>
        /// <seealso cref="ExceptionHandlerCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="WrapHandlerData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="WrapHandler"/>.</returns>
        public IExceptionHandler Assemble(IBuilderContext context, ExceptionHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            WrapHandlerData castedObjectConfiguration
                = (WrapHandlerData)objectConfiguration;

            IStringResolver exceptionMessageResolver
                = new ResourceStringResolver(
                    castedObjectConfiguration.ExceptionMessageResourceType,
                    castedObjectConfiguration.ExceptionMessageResourceName,
                    castedObjectConfiguration.ExceptionMessage);
            WrapHandler createdObject = new WrapHandler(exceptionMessageResolver, castedObjectConfiguration.WrapExceptionType);

            return createdObject;
        }
    }
}