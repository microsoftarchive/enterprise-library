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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an <see cref="IExceptionHandler"/>.
    /// </summary>    
    public class ExceptionHandlerData : NameTypeConfigurationElement
    {
        /// <summary>
        /// Initializes an instance of a <see cref="ExceptionHandlerData"/> class.
        /// </summary>
        public ExceptionHandlerData()
        {
        }

        /// <summary>
        /// Initializes an instance of an <see cref="ExceptionHandlerData"/> class with a name and an <see cref="IExceptionHandler"/> type.
        /// </summary>
        /// <param name="type">
        /// The configured <see cref="IExceptionHandler"/> type.
        /// </param>
        public ExceptionHandlerData(Type type)
            : this(null, type)
        {
        }


        /// <summary>
        /// Initializes an instance of an <see cref="ExceptionHandlerData"/> class with a name and an <see cref="IExceptionHandler"/> type.
        /// </summary>
        /// <param name="name">
        /// The name of the configured <see cref="IExceptionHandler"/>.
        /// </param>
        /// <param name="type">
        /// The configured <see cref="IExceptionHandler"/> type.
        /// </param>
        public ExceptionHandlerData(string name, Type type)
            : base(name, type)
        {
        }

        /// <summary>
        /// Returns the <see cref="TypeRegistration" /> for the exception handler data provided.
        /// </summary>
        /// <remarks>
        /// The method at the <see cref="ExceptionHandlerData"/> should not be called directly and is expected to
        /// be implemented by inheritors.  Since the <see cref="ExceptionHandlerData"/> requires a default constructor for 
        /// serialization purposes, the class could not be made abstract.
        /// </remarks>
        /// <param name="namePrefix">The prefix to use when building references to child elements.</param>
        /// <returns>A <see cref="TypeRegistration"/> for the exception handler data</returns>
        public virtual IEnumerable<TypeRegistration> GetRegistrations(string namePrefix)
        {
            // Cannot make abstract for serialization reasons.
            throw new NotImplementedException("Must be implemented by subclasses.");
        }

        /// <summary>
        /// Builds a name with the supplied prefix. 
        /// </summary>
        /// <param name="prefix">Prefix to use when building a name</param>
        /// <returns>A name as string</returns>
        protected string BuildName(string prefix)
        {
            return prefix + "." + Name;
        }
    }
}
