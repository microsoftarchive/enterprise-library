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
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// The exception that is thrown when a syntax error
    /// is found in an identity role rule expression.
    /// </summary>
    [Serializable]
    public class SyntaxException : Exception
    {
        private const string IndexKey = "index";
        private int index;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SyntaxException"/> class.
        /// </summary>
        public SyntaxException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SyntaxException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SyntaxException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SyntaxException"/> class 
        /// with a specified error message and 
        /// a reference to the inner exception 
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the innerException parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception.
        /// </param>
        public SyntaxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SyntaxException"/> class with the
        /// specified message and index.
        /// </summary>
        /// <param name="message">The syntax error message.</param>
        /// <param name="index">The position in the expression where the syntax
        /// error was found.</param>
        public SyntaxException(string message, int index) : base(message)
        {
            this.index = index;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SyntaxException"/> class 
        /// with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds 
        /// the serialized object data about the 
        /// exception being thrown. 
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/>
        /// that contains contextual information 
        /// about the source or destination. 
        /// </param>
        protected SyntaxException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.index = info.GetInt32(IndexKey);
        }

        /// <summary>
        /// The position in the expression where the syntax
        /// error was found.
        /// </summary>
        /// <value>The zero-based starting position in the original 
        /// string where syntax error was found.</value>
        public int Index
        {
            get { return this.index; }
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo"/>
        /// with information about the exception.
        /// </summary>
        /// <param name="info">The SerializationInfo 
        /// that holds the serialized object data 
        /// about the exception being thrown. 
        /// </param>
        /// <param name="context">
        /// The StreamingContext that contains contextual 
        /// information about the source or destination. 
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(IndexKey, this.index);
        }
    }
}