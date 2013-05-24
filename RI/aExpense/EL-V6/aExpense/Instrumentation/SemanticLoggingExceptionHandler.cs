#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace AExpense.Instrumentation
{
    /// <summary>
    /// Custom exception handler for tracing exceptions using <see cref="AExpenseEvents"/>. 
    /// </summary>
    [ConfigurationElementType(typeof(CustomHandlerData))]
    public class SemanticLoggingExceptionHandler : IExceptionHandler
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "attributes")]
        public SemanticLoggingExceptionHandler(NameValueCollection attributes)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            AExpenseEvents.Log.ExceptionHandlerLoggedException(exception.ToString(), handlingInstanceId);
            return exception;
        }
    }
}