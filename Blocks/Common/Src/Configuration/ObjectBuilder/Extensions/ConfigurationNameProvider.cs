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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Manages the creation of names for anonymous instances.
	/// </summary>
	public static class ConfigurationNameProvider
	{
		private const string nameSuffix = "___";

		/// <summary>
		/// Creates a new name.
		/// </summary>
		/// <returns>The created name.</returns>
		public static string MakeUpName()
		{
			return Guid.NewGuid().ToString() + nameSuffix;
		}

		/// <summary>
		/// Tests a name to determine if it has been created.
		/// </summary>
		/// <param name="name">The name to test.</param>
		/// <returns><b>true</b> if the name was made up.</returns>
		public static bool IsMadeUpName(string name)
		{
			if (name == null) return false;
			
			return name.EndsWith(nameSuffix);
		}	
	}
}
