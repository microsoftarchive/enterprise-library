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
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an <see cref="System.Exception"/>
    /// that will be handled by an exception policy.
    /// </summary>		
	public class ExceptionTypeData : NamedConfigurationElement
    {
        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();
		private const string typeProperty = "type";
		private const string postHandlingActionProperty = "postHandlingAction";
		private const string exceptionHandlersProperty = "exceptionHandlers";

		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionTypeData"/> class.
		/// </summary>
		public ExceptionTypeData()
		{			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionTypeData"/> class with a name, the <see cref="Exception"/> type and a <see cref="PostHandlingAction"/>.
		/// </summary>
		/// <param name="name">The name of the configured exception.</param>
		/// <param name="type">The <see cref="Exception"/> type.</param>
		/// <param name="postHandlingAction">One of the <see cref="PostHandlingAction"/> values.</param>
		public ExceptionTypeData(string name, Type type, PostHandlingAction postHandlingAction)
            : this(name, typeConverter.ConvertToString(type), postHandlingAction)
		{					
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTypeData"/> class with a name, the fully qualified type name of the <see cref="Exception"/> and a <see cref="PostHandlingAction"/>.
        /// </summary>
        /// <param name="name">The name of the configured exception.</param>
        /// <param name="typeName">The fully qualified type name of the <see cref="Exception"/> type.</param>
        /// <param name="postHandlingAction">One of the <see cref="PostHandlingAction"/> values.</param>
        public ExceptionTypeData(string name, string typeName, PostHandlingAction postHandlingAction)
            : base(name)
        {
            this.TypeName = typeName;
            this.PostHandlingAction = postHandlingAction;	
        }

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
        /// Gets or sets the fully qualified type name of the <see cref="Exception"/> type.
        /// </summary>
        /// <value>
        /// The fully qualified type name of the <see cref="Exception"/> type.
        /// </value>
        [ConfigurationProperty(typeProperty, IsRequired = true)]
        public string TypeName
        {
            get { return (string)this[typeProperty]; }
            set { this[typeProperty] = value; }
        }

		/// <summary>
		/// Gets or sets the <see cref="PostHandlingAction"/> for the exception.
		/// </summary>
		/// <value>
		/// One of the <see cref="PostHandlingAction"/> values.
		/// </value>
		[ConfigurationProperty(postHandlingActionProperty, IsRequired= true)]		
		public PostHandlingAction PostHandlingAction
		{
			get { return (PostHandlingAction)this[postHandlingActionProperty]; }
			set { this[postHandlingActionProperty] = value; }
		}

		/// <summary>
		/// Gets a collection of <see cref="ExceptionHandlerData"/> objects.
		/// </summary>
		/// <value>
		/// A collection of <see cref="ExceptionHandlerData"/> objects.
		/// </value>
		[ConfigurationProperty(exceptionHandlersProperty)]
        public NameTypeConfigurationElementCollection<ExceptionHandlerData, CustomHandlerData> ExceptionHandlers
		{
			get
			{
                return (NameTypeConfigurationElementCollection<ExceptionHandlerData, CustomHandlerData>)this[exceptionHandlersProperty];
			}
		}
	}
}