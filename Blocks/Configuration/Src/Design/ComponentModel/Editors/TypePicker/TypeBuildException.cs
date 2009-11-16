//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Represents an error while building a type from a <see cref="TypeBuildNode"/>.
    /// </summary>
    [Serializable]
    public class TypeBuildException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuildException"/> class.
        /// </summary>
        public TypeBuildException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuildException"/> class with the given message.
        /// </summary>
        /// <param name="message">The message describing the error.</param>
        public TypeBuildException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuildException"/> class with the given message and a 
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message describing the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or 
        /// <see langword="null"/> if no inner exception is specified. </param>
        public TypeBuildException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuildException"/> class with the given message and a 
        /// reference to the inner exception that is the cause of this exception and the node where the error was 
        /// detected.
        /// </summary>
        /// <param name="message">The message describing the error.</param>
        /// <param name="failingNode">The node where the type build failure was detected.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or 
        /// <see langword="null"/> if no inner exception is specified. </param>
        internal TypeBuildException(string message, TypeBuildNode failingNode, Exception innerException)
            : base(message, innerException)
        {
            this.failingNode = failingNode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuildException"/> class with serialized data.
        /// </summary>
        /// <param name="serializationInfo">The <see cref="StreamingContext"/> that contains contextual information 
        /// about the source or destination. </param>
        /// <param name="context">The <see cref="SerializationInfo"/> that holds the serialized object data about the 
        /// exception being thrown. </param>
        protected TypeBuildException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        [NonSerialized]
        private TypeBuildNode failingNode;

        /// <summary>
        /// The node where the error was detected.
        /// </summary>
        public TypeBuildNode FailingNode
        {
            get { return this.failingNode; }
        }
    }
}
