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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an <see cref="System.Exception"/>
    /// that will be handled by an exception policy. 
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "ExceptionTypeDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ExceptionTypeDataDisplayName")]
    [TypePickingCommand("TypeName", Replace = CommandReplacement.DefaultAddCommandReplacement, CommandModelTypeName = ExceptionHandlingDesignTime.CommandTypeNames.AddExceptionTypeCommand)]
    [ViewModel(ExceptionHandlingDesignTime.ViewModelTypeNames.ExceptionTypeDataViewModel)]
    public class ExceptionTypeData : NamedConfigurationElement
    {
        private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();
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
        /// <param name="postHandlingAction">One of the <see cref="PostHandlingAction"/> values that specifies the action that should occur after the exception is handled.</param>
        public ExceptionTypeData(string name, Type type, PostHandlingAction postHandlingAction)
            : this(name, typeConverter.ConvertToString(type), postHandlingAction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTypeData"/> class with a name, the fully qualified type name of the <see cref="Exception"/> and a <see cref="PostHandlingAction"/>.
        /// </summary>
        /// <param name="name">The name of the configured exception.</param>
        /// <param name="typeName">The fully qualified type name of the <see cref="Exception"/> type.</param>
        /// <param name="postHandlingAction">One of the <see cref="PostHandlingAction"/> values that specifies the action that should occur after the exception is handled.</param>
        public ExceptionTypeData(string name, string typeName, PostHandlingAction postHandlingAction)
            : base(name)
        {
            TypeName = typeName;
            PostHandlingAction = postHandlingAction;
        }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        /// <value>
        /// The name of the element.
        /// </value>
        [DesignTimeReadOnly(true)]
        [ResourceDescription(typeof(DesignResources), "ExceptionTypeDataNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ExceptionTypeDataNameDisplayName")]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
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
        [DesignTimeReadOnly(true)]
        [ConfigurationProperty(typeProperty, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "ExceptionTypeDataTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ExceptionTypeDataTypeNameDisplayName")]
        [BaseType(typeof(Exception), TypeSelectorIncludes.BaseType | TypeSelectorIncludes.AbstractTypes)]
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
        [ConfigurationProperty(postHandlingActionProperty, IsRequired = true, DefaultValue = PostHandlingAction.NotifyRethrow)]
        [ResourceDescription(typeof(DesignResources), "ExceptionTypeDataPostHandlingActionDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ExceptionTypeDataPostHandlingActionDisplayName")]
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
        [ResourceDescription(typeof(DesignResources), "ExceptionTypeDataExceptionHandlersDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ExceptionTypeDataExceptionHandlersDisplayName")]
        [ConfigurationCollection(typeof(ExceptionHandlerData))]
        [PromoteCommands]
        public NameTypeConfigurationElementCollection<ExceptionHandlerData, CustomHandlerData> ExceptionHandlers
        {
            get
            {
                return (NameTypeConfigurationElementCollection<ExceptionHandlerData, CustomHandlerData>)this[exceptionHandlersProperty];
            }
        }

        /// <summary>
        /// Builds an <see cref="ExceptionPolicyEntry"/> based on the configuration.
        /// </summary>
        /// <returns>The policy entry.</returns>
        public ExceptionPolicyEntry BuildExceptionType()
        {
            var handlers = this.ExceptionHandlers.Select(h => h.BuildExceptionHandler());

            return new ExceptionPolicyEntry(this.Type, this.PostHandlingAction, handlers);
        }
    }
}
