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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using System.ComponentModel;
using System.Drawing.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration data for a <see cref="ReplaceHandler"/>.
    /// </summary>
    [ResourceDisplayName(typeof(Resources), "AddReplaceHandlerData")]
    [ResourceDescription(typeof(Resources), "AddReplaceHandlerDataDescription")]
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
        public ReplaceHandlerData() : base(typeof(ReplaceHandler))
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
        /// Gets or sets the <see cref="Type"/> the element is the configuration for.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> the element is the configuration for.
        /// </value>
        [Browsable(false)]
        public override Type Type
        {
            get
            {
                return base.Type;
            }
            set
            {
                base.Type = value;
            }
        }

        /// <summary>
        /// Gets or sets the message for the replacement exception.
        /// </summary>
        [ConfigurationProperty(exceptionMessageProperty, IsRequired = false)]
        [ResourceDescription(typeof(Resources), "ExceptionMessageDescription")]
        [ResourceDisplayName(typeof(Resources), "ExceptionMessageDisplayName")]
        public string ExceptionMessage
        {
            get { return (string)this[exceptionMessageProperty]; }
            set { this[exceptionMessageProperty] = value; }
        }

        /// <summary/>
        [ConfigurationProperty(ExceptionMessageResourceNameProperty)]
        [ResourceDescription(typeof(Resources), "ExceptionMessageResourceNameDescription")]
        [ResourceDisplayName(typeof(Resources), "ExceptionMessageResourceNameDisplayName")]
        [ResourceCategory(typeof(ResourceCategoryAttribute), "CategoryLocalization")]
        public string ExceptionMessageResourceName
        {
            get { return (string)this[ExceptionMessageResourceNameProperty]; }
            set { this[ExceptionMessageResourceNameProperty] = value; }
        }

        /// <summary/>
        [ConfigurationProperty(ExceptionMessageResourceTypeNameProperty)]
        [ResourceDescription(typeof(Resources), "ExceptionMessageResourceTypeDescription")]
        [ResourceDisplayName(typeof(Resources), "ExceptionMessageResourceTypeDisplayName")]
        [ResourceCategory(typeof(ResourceCategoryAttribute), "CategoryLocalization")]
        [Editor(EditorTypes.TypeSelector, typeof(UITypeEditor))]
        [BaseType(typeof(Object), TypeSelectorIncludes.None)]
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
        [Editor(EditorTypes.TypeSelector, typeof(UITypeEditor))]
        [BaseType(typeof(Exception), TypeSelectorIncludes.BaseType)]
        [ResourceDescription(typeof(Resources), "ReplaceExceptionTypeNameDescription")]
        [ResourceDisplayName(typeof(Resources), "ReplaceExceptionTypeNameDisplayName")]
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
                        Name = BuildName(namePrefix),
                        Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
