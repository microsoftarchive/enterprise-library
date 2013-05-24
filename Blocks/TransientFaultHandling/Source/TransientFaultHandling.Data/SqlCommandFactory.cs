#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Data;
using System.Globalization;

using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling
{
    /// <summary>
    /// Provides factory methods for instantiating SQL commands.
    /// </summary>
    public static class SqlCommandFactory
    {
        #region Public members
        /// <summary>
        /// Returns the default time-out that will be applied to all SQL commands constructed by this factory class.
        /// </summary>
        public const int DefaultCommandTimeoutSeconds = 60;
        #endregion

        #region Generic SQL commands
        /// <summary>
        /// Creates a generic command of type Stored Procedure and assigns the default command time-out.
        /// </summary>
        /// <param name="connection">The database connection object to be associated with the new command.</param>
        /// <returns>A new SQL command that is initialized with the Stored Procedure command type and initial settings.</returns>
        public static IDbCommand CreateCommand(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            IDbCommand command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = DefaultCommandTimeoutSeconds;

            return command;
        }

        /// <summary>
        /// Creates a generic command of type Stored Procedure and assigns the specified command text and default command time-out to it.
        /// </summary>
        /// <param name="connection">The database connection object to be associated with the new command.</param>
        /// <param name="commandText">The text of the command to run against the data source.</param>
        /// <returns>A new SQL command that is initialized with the Stored Procedure command type, specified text, and initial settings.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "As designed. User must review")]
        public static IDbCommand CreateCommand(IDbConnection connection, string commandText)
        {
            if (commandText == null) throw new ArgumentNullException("commandText");
            if (string.IsNullOrWhiteSpace(commandText))  throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StringCannotBeEmpty, "commandText"), "commandText");

            IDbCommand command = CreateCommand(connection);
            try
            {
                command.CommandText = commandText;
                return command;
            }
            catch
            {
                command.Dispose();
                throw;
            }
        }
        #endregion

        #region Other system commands
        /// <summary>
        /// Creates a SQL command that is intended to return the connection's context ID, which is useful for tracing purposes.
        /// </summary>
        /// <param name="connection">The database connection object to be associated with the new command.</param>
        /// <returns>A new SQL command that is initialized with the specified connection.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "As designed. User must review")]
        public static IDbCommand CreateGetContextInfoCommand(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            IDbCommand command = CreateCommand(connection);
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT CONVERT(UNIQUEIDENTIFIER, CONVERT(NVARCHAR(36), CONTEXT_INFO()))";

                return command;
            }
            catch
            {
                command.Dispose();
                throw;
            }
        }
        #endregion
    }
}
