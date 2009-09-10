//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// Interface that defines the events that are raised for instrumentation
    /// of the Data Access Application Block.
    /// </summary>
    public interface IDataInstrumentationProvider
    {
        /// <summary>
        /// Fires the CommandExecuted event.
        /// </summary>
        /// <param name="startTime">The time the command started its execution.</param>
        void FireCommandExecutedEvent(DateTime startTime);

        /// <summary>
        /// Fires the CommandFailed event.
        /// </summary>
        /// <param name="commandText">The text of the command that failed its execution.</param>
        /// <param name="connectionString">The connection string of the <see cref="Database"/> that executed the failed command, with credentials removed.</param>
        /// <param name="exception">The exception thrown when the command failed.</param>
        void FireCommandFailedEvent(string commandText, string connectionString, Exception exception);

        /// <summary>
        /// Fires the ConnectionOpened event.
        /// </summary>
        void FireConnectionOpenedEvent();

        /// <summary>
        /// Fires the ConnectionFailed event.
        /// </summary>
        /// <param name="connectionString">The connection string that caused the failed connection, with credentials removed.</param>
        /// <param name="exception">The exception thrown when the connection failed.</param>
        void FireConnectionFailedEvent(string connectionString, Exception exception);
    }
}
