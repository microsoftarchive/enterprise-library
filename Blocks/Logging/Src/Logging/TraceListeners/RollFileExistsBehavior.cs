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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// Defines the behavior when the rool file is created.
	/// </summary>
	public enum RollFileExistsBehavior
	{
		/// <summary>
		/// Overwrites the file if it already exists.
		/// </summary>
		Overwrite,
		/// <summary>
		/// Use a secuence number at the end of the generated file if it already exists. If it fails again then increment the secuence until a non existent filename is found.
		/// </summary>
		Increment
	};
}
