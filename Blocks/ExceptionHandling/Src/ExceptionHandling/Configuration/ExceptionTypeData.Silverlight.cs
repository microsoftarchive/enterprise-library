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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an <see cref="System.Exception"/>
    /// that will be handled by an exception policy. 
    /// </summary>
    public partial class ExceptionTypeData : NamedConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTypeData"/> class.
        /// </summary>
        public ExceptionTypeData()
        {
            this.ExceptionHandlers = new NamedElementCollection<ExceptionHandlerData>();
            this.PostHandlingAction = PostHandlingAction.NotifyRethrow;
        }

        /// <summary>
        /// Gets or sets the fully qualified type name of the <see cref="Exception"/> type.
        /// </summary>
        /// <value>
        /// The fully qualified type name of the <see cref="Exception"/> type.
        /// </value>
        public string TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="PostHandlingAction"/> for the exception.
        /// </summary>
        /// <value>
        /// One of the <see cref="PostHandlingAction"/> values.
        /// </value>
        public PostHandlingAction PostHandlingAction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a collection of <see cref="ExceptionHandlerData"/> objects.
        /// </summary>
        /// <value>
        /// A collection of <see cref="ExceptionHandlerData"/> objects.
        /// </value>
        public NamedElementCollection<ExceptionHandlerData> ExceptionHandlers
        {
            get;
            private set;
        }
    }
}
