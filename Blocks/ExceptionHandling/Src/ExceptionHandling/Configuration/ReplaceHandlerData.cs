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
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration data for a <see cref="ReplaceHandler"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "ReplaceHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ReplaceHandlerDataDisplayName")]
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
            : base(typeof(ReplaceHandler))
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
        [ResourceDescription(typeof(DesignResources), "ReplaceHandlerDataExceptionMessageDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ReplaceHandlerDataExceptionMessageDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.MultilineText, CommonDesignTime.EditorTypes.FrameworkElement)]
        public string ExceptionMessage
        {
            get { return (string)this[exceptionMessageProperty]; }
            set { this[exceptionMessageProperty] = value; }
        }

        /// <summary/>
        [ConfigurationProperty(ExceptionMessageResourceNameProperty)]
        [ResourceDescription(typeof(DesignResources), "ReplaceHandlerDataExceptionMessageResourceNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ReplaceHandlerDataExceptionMessageResourceNameDisplayName")]
        [ResourceCategory(typeof(ResourceCategoryAttribute), "CategoryLocalization")]
        public string ExceptionMessageResourceName
        {
            get { return (string)this[ExceptionMessageResourceNameProperty]; }
            set { this[ExceptionMessageResourceNameProperty] = value; }
        }

        /// <summary/>
        [ConfigurationProperty(ExceptionMessageResourceTypeNameProperty)]
        [ResourceDescription(typeof(DesignResources), "ReplaceHandlerDataExceptionMessageResourceTypeDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ReplaceHandlerDataExceptionMessageResourceTypeDisplayName")]
        [ResourceCategory(typeof(ResourceCategoryAttribute), "CategoryLocalization")]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
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
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(Exception), TypeSelectorIncludes.BaseType)]
        [ResourceDescription(typeof(DesignResources), "ReplaceHandlerDataReplaceExceptionTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ReplaceHandlerDataReplaceExceptionTypeNameDisplayName")]
        public string ReplaceExceptionTypeName
        {
            get { return (string)this[replaceExceptionTypeProperty]; }
            set { this[replaceExceptionTypeProperty] = value; }
        }

        /// <summary>
        /// Builds the exception handler represented by this configuration object.
        /// </summary>
        /// <returns>An <see cref="IExceptionHandler"/>.</returns>
        public override IExceptionHandler BuildExceptionHandler()
        {
            IStringResolver resolver
                = new ResourceStringResolver(ExceptionMessageResourceType, ExceptionMessageResourceName, ExceptionMessage);

            return new ReplaceHandler(resolver, ReplaceExceptionType);
        }
    }
}
