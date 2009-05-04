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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// Provides access to the registry.
	/// </summary>
	/// <remarks>
	/// This interface allows for unit testing without requiring access to the machine's registry.
	/// </remarks>
	public interface IRegistryAccessor
	{
		/// <summary>
		/// Gets registry key HKCU.
		/// </summary>
		IRegistryKey CurrentUser { get; }

		/// <summary>
		/// Gets registry key HKLM.
		/// </summary>
		IRegistryKey LocalMachine { get; }
	}
}
