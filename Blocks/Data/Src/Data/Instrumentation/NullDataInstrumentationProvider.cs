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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// An implementation of <see cref="IDataInstrumentationProvider"/> that
    /// does nothing with the instrumentation events.
    /// </summary>
    public class NullDataInstrumentationProvider : IDataInstrumentationProvider
    {
        /// <summary>
        /// Fires the CommandExecuted event.
        /// </summary>
        /// <param name="startTime">The time the command started its execution.</param>
        public void FireCommandExecutedEvent(DateTime startTime)
        {
        }

        /// <summary>
        /// Fires the CommandFailed event.
        /// </summary>
        /// <param name="commandText">The text of the command that failed its execution.</param>
        /// <param name="connectionString">The connection string of the <see cref="Database"/> that executed the failed command, with credentials removed.</param>
        /// <param name="exception">The exception thrown when the command failed.</param>
        public void FireCommandFailedEvent(string commandText, string connectionString, Exception exception)
        {
        }

        /// <summary>
        /// Fires the ConnectionOpened event.
        /// </summary>
        public void FireConnectionOpenedEvent()
        {
        }

        /// <summary>
        /// Fires the ConnectionFailed event.
        /// </summary>
        /// <param name="connectionString">The connection string that caused the failed connection, with credentials removed.</param>
        /// <param name="exception">The exception thrown when the connection failed.</param>
        public void FireConnectionFailedEvent(string connectionString, Exception exception)
        {
        }
    }
}
