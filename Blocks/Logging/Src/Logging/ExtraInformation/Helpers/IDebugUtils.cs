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

using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers
{
	/// <summary>
	/// Contract for accessing debug information.
	/// </summary>
	public interface IDebugUtils
	{
		/// <summary>
		/// Returns a text representation of the stack trace with source information if available.
		/// </summary>
		/// <param name="stackTrace">The source to represent textually.</param>
		/// <returns>The textual representation of the stack.</returns>
		string GetStackTraceWithSourceInfo( StackTrace stackTrace );
	}
}
