//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Provides common functions for the Exception Handling Application Block classes. Cannot inherit from this class.
    /// </summary>
    public static class ExceptionUtility
    {
        private const string HandlingInstanceToken = "{handlingInstanceID}";        

        /// <summary>
        /// Formats a message by replacing the token "{handlingInstanceID}" with the handlingInstanceID.
        /// </summary>
        /// <param name="message">The original message.</param>
        /// <param name="handlingInstanceId">The handlingInststanceID passed into the exceptionHandlerData.</param>
        /// <returns>The formatted message.</returns>
        public static string FormatExceptionMessage(string message, Guid handlingInstanceId)
        {
            return message.Replace(HandlingInstanceToken, handlingInstanceId.ToString());
        }

        /// <summary>
		/// Formats an exception message so that it can be sent to the event log later, by someone else.
        /// </summary>
        /// <param name="policyName">The policy that is running.</param>
        /// <param name="offendingException">The exception that occured in the chain.</param>
        /// <param name="chainException">The exception when the chain failed.</param>
        /// <param name="originalException">The original exception.</param>		
		public static string FormatExceptionHandlingExceptionMessage(string policyName, Exception offendingException, Exception chainException, Exception originalException)
        {
            StringBuilder message = new StringBuilder();
            StringWriter writer = null;
			string result = null;
            try
            {
                writer = new StringWriter(message, CultureInfo.CurrentCulture);

                if (policyName.Length > 0)
                {
                    writer.WriteLine(string.Format(Resources.Culture, Resources.PolicyName, policyName));
                }

                FormatHandlingException(writer, Resources.OffendingException, offendingException);
                FormatHandlingException(writer, Resources.OriginalException, originalException);
                FormatHandlingException(writer, Resources.ChainException, chainException);
            }
            finally
            {
                if (writer != null)
                {
					result = writer.ToString();
                    writer.Close();
                }
            }

			return result;
        }

        private static void FormatHandlingException(StringWriter writer, string header, Exception ex)
        {
            if (ex != null)
            {
                writer.WriteLine();
                writer.WriteLine(header);
                writer.Write(writer.NewLine);
                TextExceptionFormatter formatter = new TextExceptionFormatter(writer, ex);
                formatter.Format();
            }
        }
    }
}