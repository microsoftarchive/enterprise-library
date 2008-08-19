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
	/// Provides data for the <see cref="DataInstrumentationProvider.commandExecuted"/> event.
	/// </summary>
    public class CommandExecutedEventArgs : EventArgs
    {
        private DateTime startTime;

		/// <summary>
		/// Gets the time the command started its execution.
		/// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandExecutedEventArgs"/> class.
		/// </summary>
		/// <param name="startTime">The time the command started its execution.</param>
        public CommandExecutedEventArgs(DateTime startTime)
        {
            this.startTime = startTime;
        }
    }
}
