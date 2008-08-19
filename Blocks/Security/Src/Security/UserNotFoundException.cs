//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Exception thrown when Active Directory is unable to find the given user
    /// </summary>
    [Serializable]
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a newly created instance of <see cref="UserNotFoundException"></see>
        /// </summary>
        /// <param name="errorMessage">Error message from caller</param>
        public UserNotFoundException(string  errorMessage)
            : this(errorMessage, null)
        {
        }

        /// <summary>
        /// Initializes a newly created instance of <see cref="UserNotFoundException"></see>
        /// </summary>
        /// <param name="errorMessage">Error message from caller</param>
        /// <param name="innerException">Any nested exception</param>
        public UserNotFoundException(string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        /// <param name="info">The object that holds the serialized object data</param>
        /// <param name="context">The contextual information about the source or destination</param>
        public UserNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
