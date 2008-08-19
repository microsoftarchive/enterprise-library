//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
	/// <summary>
	/// Abstract implememtation of the <see cref="ILogFormatter"/> interface.
	/// </summary>
	public abstract class LogFormatter : ILogFormatter
	{
		/// <summary>
		/// Formats a log entry and return a string to be outputted.
		/// </summary>
		/// <param name="log">Log entry to format.</param>
		/// <returns>A string representing the log entry.</returns>
		public abstract string Format(LogEntry log);
	}
}
