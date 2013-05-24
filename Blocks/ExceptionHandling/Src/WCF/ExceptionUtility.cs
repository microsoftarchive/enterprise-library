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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    static class ExceptionUtility
    {
        private static readonly Regex guidExpression =
            new Regex("[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", RegexOptions.Compiled);

        /// <summary>
        /// Logs the server exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "As designed. Exception is logged.")]
        public static Guid LogServerException(Exception exception)
        {
            // try to get the handoling instance from the exception message or get a new one.
            Guid handlingInstanceId = GetHandlingInstanceId(exception);

            // Log exception info to configured log object.
            bool logged = false;
            try
            {
                if (Logger.IsLoggingEnabled())
                {
                    IDictionary<string, object> properties = new Dictionary<string, object>();
                    properties.Add(Properties.Resources.HandlingInstanceID, handlingInstanceId);
                    Logger.Write(exception, properties);
                    logged = true;
                }
            }
            catch (Exception e)
            {
                // if we can't log, then trace the exception information
                Trace.TraceError(Properties.Resources.ServerUnhandledExceptionNotLogged, handlingInstanceId, e);
            }
            finally
            {
                if (!logged)
                {
                    // if we can't log, then trace the exception information
                    Trace.TraceError(Properties.Resources.ServerUnhandledException, handlingInstanceId, exception);
                }                
            }

            return handlingInstanceId;
        }

        /// <summary>
        /// Gets the handling instance id.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static Guid GetHandlingInstanceId(Exception exception)
        {
            return GetHandlingInstanceId(exception, Guid.NewGuid());
        }

        /// <summary>
        /// Gets the handling instance id.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="optionalHandlingInstanceId">The optional handling instance id.</param>
        /// <returns></returns>
        public static Guid GetHandlingInstanceId(Exception exception, Guid optionalHandlingInstanceId)
        {
            Guid result = optionalHandlingInstanceId;

            Match match = guidExpression.Match(exception.Message);
            if (match.Success)
            {
                result = new Guid(match.Value);
            }
            return result;
        }

        /// <summary>
        /// Formats the exception message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="handlingInstanceId">The handling instance id.</param>
        /// <returns></returns>
        public static string FormatExceptionMessage(string message, Guid handlingInstanceId)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = FormatExceptionMessage(Properties.Resources.ClientUnhandledExceptionMessage, handlingInstanceId);
            }

            return Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionUtility.FormatExceptionMessage(message, handlingInstanceId);
        }

        /// <summary>
        /// Gets the message from the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="optionalMessage">The optional message.</param>
        /// <param name="handlingInstanceId">The handling instance id.</param>
        /// <returns></returns>
        public static string GetMessage(Exception exception, string optionalMessage, Guid handlingInstanceId)
        {
            string result = exception.Message;

            if (!string.IsNullOrEmpty(optionalMessage))
            {
                result = FormatExceptionMessage(optionalMessage, handlingInstanceId);
            }

            return result;
        }
    }
}
