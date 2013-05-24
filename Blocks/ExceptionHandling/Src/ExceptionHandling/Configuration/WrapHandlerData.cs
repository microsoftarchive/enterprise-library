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
    /// Represents the configuration data for a <see cref="WrapHandler"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "WrapHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "WrapHandlerDataDisplayName")]
    public class WrapHandlerData : ExceptionHandlerData
    {
        private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        private const string exceptionMessageProperty = "exceptionMessage";
        private const string wrapExceptionTypeProperty = "wrapExceptionType";
        private const string ExceptionMessageResourceTypeNameProperty = "exceptionMessageResourceType";
        private const string ExceptionMessageResourceNameProperty = "exceptionMessageResourceName";

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapHandlerData"/> class.
        /// </summary>
        public WrapHandlerData()
            : base(typeof(WrapHandler))
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
            ExceptionMessage = exceptionMessage;
            WrapExceptionTypeName = wrapExceptionTypeName;
        }

        /// <summary>
        /// Gets or sets the message for the replacement exception.
        /// </summary>
        [ConfigurationProperty(exceptionMessageProperty, IsRequired = false)]
        [ResourceDescription(typeof(DesignResources), "WrapHandlerDataExceptionMessageDescription")]
        [ResourceDisplayName(typeof(DesignResources), "WrapHandlerDataExceptionMessageDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.MultilineText, CommonDesignTime.EditorTypes.FrameworkElement)]
        public string ExceptionMessage
        {
            get { return (string)this[exceptionMessageProperty]; }
            set { this[exceptionMessageProperty] = value; }
        }


        /// <summary/>
        [ConfigurationProperty(ExceptionMessageResourceTypeNameProperty)]
        [ResourceDescription(typeof(DesignResources), "WrapHandlerDataExceptionMessageResourceTypeDescription")]
        [ResourceDisplayName(typeof(DesignResources), "WrapHandlerDataExceptionMessageResourceTypeDisplayName")]
        [ResourceCategory(typeof(ResourceCategoryAttribute), "CategoryLocalization")]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(Object), TypeSelectorIncludes.None)]
        public string ExceptionMessageResourceType
        {
            get { return (string)this[ExceptionMessageResourceTypeNameProperty]; }
            set { this[ExceptionMessageResourceTypeNameProperty] = value; }
        }

        /// <summary/>
        [ConfigurationProperty(ExceptionMessageResourceNameProperty)]
        [ResourceDescription(typeof(DesignResources), "WrapHandlerDataExceptionMessageResourceNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "WrapHandlerDataExceptionMessageResourceNameDisplayName")]
        [ResourceCategory(typeof(ResourceCategoryAttribute), "CategoryLocalization")]
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
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(Exception), TypeSelectorIncludes.BaseType)]
        [ResourceDescription(typeof(DesignResources), "WrapHandlerDataWrapExceptionTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "WrapHandlerDataWrapExceptionTypeNameDisplayName")]
        public string WrapExceptionTypeName
        {
            get { return (string)this[wrapExceptionTypeProperty]; }
            set { this[wrapExceptionTypeProperty] = value; }
        }

        /// <summary>
        /// Builds the exception handler represented by this configuration object.
        /// </summary>
        /// <returns>An <see cref="IExceptionHandler"/>.</returns>
        public override IExceptionHandler BuildExceptionHandler()
        {
            var exceptionMessageResolver =
                new ResourceStringResolver(ExceptionMessageResourceType, ExceptionMessageResourceName, ExceptionMessage);

            return new WrapHandler(exceptionMessageResolver, this.WrapExceptionType);
        }
    }
}
