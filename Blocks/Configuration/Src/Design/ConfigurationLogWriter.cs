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
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// A static log writer for the configuration tool that, currently relies underlying <see cref="Trace"/>
    /// mechanisms for publishing.
    /// </summary>
    /// <remarks>
    /// Logging is off by default and may be turned in the configuration file by configuration a trace switch 
    /// named configurationToolLogging.
    /// 
    /// <example>
    /// <![CDATA[
    /// <system.diagnostics>
    ///   <switches>
    ///     <add name="configurationToolLogging" value="Warning"/>
    ///   </switches>
    /// </system.diagnostics>
    ///]]>
    /// </example>
    /// </remarks>
    public static class ConfigurationLogWriter
    {
        /// <summary>
        /// The <see cref="TraceSwitch"/> for the configuration tool.
        /// </summary>
        public static readonly TraceSwitch LoggingSwitch = new TraceSwitch("configurationToolLogging", "Default logging settings for tracing in the configuration tool");

        /// <summary>
        /// Logs an exception using <see cref="Exception.ToString"/>
        /// with level of <see cref="TraceLevel.Error"/>
        /// </summary>
        /// <param name="ex"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static void LogException(Exception ex)
        {
            Guard.ArgumentNotNull(ex, "ex");

            LogError(ex.ToString());
        }

        /// <summary>
        /// Logs an exception by combining a message and exception
        /// with level of <see cref="TraceLevel.Error"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ConfigurationLogWriter.LogError(System.String)", Justification = "Template for two lines - not language specific.")]
        public static void LogException(string message, Exception ex)
        {
            LogError(string.Format(CultureInfo.CurrentCulture, "{0}/n{1}", message, ex));
        }

        /// <summary>
        /// Logs a message with level of <see cref="TraceLevel.Error"/>
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceError, message);
        }

        /// <summary>
        /// Logs a message with level of <see cref="TraceLevel.Warning"/>
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(string message)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceWarning, message);
        }

        /// <summary>
        /// Logs a message with level of <see cref="TraceLevel.Info"/>
        /// </summary>
        /// <param name="message"></param>
        public static void LogInfo(string message)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceInfo, message);
        }
    }
}
