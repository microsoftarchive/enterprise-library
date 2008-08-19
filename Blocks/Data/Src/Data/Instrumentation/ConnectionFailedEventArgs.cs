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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
	/// <summary>
	/// Provides data for the <see cref="DataInstrumentationProvider.connectionFailed"/> event.
	/// </summary>
    public class ConnectionFailedEventArgs : EventArgs
    {
        string connectionString;
		Exception exception;
        
		/// <summary>
		/// Gets the connection string that caused the failed connection, with credentials removed.
		/// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
        }

		/// <summary>
		/// Gets the exception thrown when the connection failed.
		/// </summary>
		public Exception Exception
		{
			get { return exception; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectionFailedEventArgs"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string that caused the failed connection, with credentials removed.</param>
		/// <param name="exception">The exception thrown when the connection failed.</param>
		public ConnectionFailedEventArgs(string connectionString, Exception exception)
        {
            this.connectionString = connectionString;
			this.exception = exception;
        }
    }
}
