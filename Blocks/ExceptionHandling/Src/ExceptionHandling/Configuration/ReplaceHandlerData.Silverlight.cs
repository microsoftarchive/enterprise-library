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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    partial class ReplaceHandlerData
    {
        /// <summary>
        /// Gets or sets the message for the replacement exception.
        /// </summary>
        public string ExceptionMessage
        {
            get;
            set;
        }

        /// <summary/>
        public string ExceptionMessageResourceName
        {
            get;
            set;
        }

        /// <summary/>
        public string ExceptionMessageResourceType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fully qualified type name of the replacement exception.
        /// </summary>
        /// <value>
        /// The fully qualified type name of the replacement exception.
        /// </value>
        public string ReplaceExceptionTypeName
        {
            get;
            set;
        }
    }
}
