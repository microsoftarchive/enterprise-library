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
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Replaces the exception in the chain of handlers with a cleansed exception.
    /// </summary>
    [ConfigurationElementType(typeof(ReplaceHandlerData))]
    public class ReplaceHandler : IExceptionHandler
    {
        private readonly IStringResolver exceptionMessageResolver;
        private readonly Type replaceExceptionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceHandler"/> class with an exception message and the type of <see cref="Exception"/> to use.
        /// </summary>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <param name="replaceExceptionType">The type of <see cref="Exception"/> to use to replace.</param>
        public ReplaceHandler(string exceptionMessage, Type replaceExceptionType)
            : this(new ConstantStringResolver(exceptionMessage), replaceExceptionType)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceHandler"/> class with an exception message
        /// resolver and the type of <see cref="Exception"/> to use.
        /// </summary>
        /// <param name="exceptionMessageResolver">The exception message resolver.</param>
        /// <param name="replaceExceptionType">The type of <see cref="Exception"/> to use to replace.</param>
        public ReplaceHandler(IStringResolver exceptionMessageResolver, Type replaceExceptionType)
        {
            if (replaceExceptionType == null) throw new ArgumentNullException("replaceExceptionType");
            if (!typeof(Exception).IsAssignableFrom(replaceExceptionType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionTypeNotException, replaceExceptionType.Name), "replaceExceptionType");
            }

            this.exceptionMessageResolver = exceptionMessageResolver;
            this.replaceExceptionType = replaceExceptionType;
        }

        /// <summary>
        /// The type of exception to replace.
        /// </summary>
        public Type ReplaceExceptionType
        {
            get { return replaceExceptionType; }
        }

        /// <summary>
        /// Gets the message for the new exception.
        /// </summary>
        public string ExceptionMessage
        {
            get { return exceptionMessageResolver.GetString(); }
        }

        /// <summary>
        /// Replaces the exception with the configured type for the specified policy.
        /// </summary>
        /// <param name="exception">The original exception.</param>        
        /// <param name="handlingInstanceId">The unique identifier attached to the handling chain for this handling instance.</param>
        /// <returns>Modified exception to pass to the next exceptionHandlerData in the chain.</returns>
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            return ReplaceException(
                ExceptionUtility.FormatExceptionMessage(ExceptionMessage, handlingInstanceId));
        }

        /// <summary>
        /// Replaces an exception with a new exception of a specified type.
        /// </summary>                
        /// <param name="replaceExceptionMessage">The message for the new exception.</param>
        /// <returns>The replaced or "cleansed" exception.  Returns null if unable to replace the exception.</returns>
        private Exception ReplaceException(string replaceExceptionMessage)
        {

            object[] extraParameters = new object[] { replaceExceptionMessage };
            return (Exception)Activator.CreateInstance(replaceExceptionType, extraParameters);
        }
    }
}
