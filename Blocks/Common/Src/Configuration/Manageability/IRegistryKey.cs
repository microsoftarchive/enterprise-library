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
	/// Provides access to a registry key sub keys and values.
	/// </summary>
	/// <remarks>
	/// This interface allows for unit testing without requiring access to the machine's registry.
	/// </remarks>
	public interface IRegistryKey : IDisposable
	{
		/// <summary>
		/// Closes the registry key.
		/// </summary>
		void Close();

		/// <summary>
		/// Gets the Boolean value represented by the value for requested name in the registry key.
		/// </summary>
		/// <remarks>
		/// An integer value of 1 is considered <langword>true</langword>, any other 
		/// value is considered <langword>false</langword>.
		/// </remarks>
		/// <param name="valueName">The name of the value to get.</param>
		/// <returns>The Boolean value for the requested name in the registry key.</returns>
		/// <exception cref="RegistryAccessException">when there is no value for the given name,
		/// or the value exists but it is not an integer representing a boolean.</exception>
		bool? GetBoolValue(String valueName);

		/// <summary>
		/// Gets the enum value for the given name.
		/// </summary>
		/// <param name="valueName">The name of the value to get.</param>
		/// <returns>The enum value of type  <typeparamref name="T"/> represented by the value
		/// for the requested name in the registry key.</returns>
		/// <exception cref="RegistryAccessException">when there is no value for the given name,
		/// or the value exists but it is not an string, or it is a string value but it is not a 
		/// valid value name for enum type <typeparamref name="T"/>.</exception>
		/// <typeparam name="T">The enum type.</typeparam>
		/// <devdoc>
		/// FxCop message CA1004 is supressed because the T parameter is used to drive the
		/// type of the method return value, so it is not possible to provide a method
		/// parameter that enables generic parameter inference.
		/// </devdoc>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004")]
		T? GetEnumValue<T>(String valueName) where T : struct;

		/// <summary>
		/// Gets the integer value for the given name.
		/// </summary>
		/// <param name="valueName">The name of the value to get.</param>
		/// <returns>The integer value for the requested name in the registry key.</returns>
		/// <exception cref="RegistryAccessException">when there is no value for the given name,
		/// or the value exists but it is not an integer.</exception>
		int? GetIntValue(String valueName);

		/// <summary>
		/// Gets the string value for the given name.
		/// </summary>
		/// <param name="valueName">The name of the value to get.</param>
		/// <returns>The string value for the requested name in the registry key.</returns>
		/// <exception cref="RegistryAccessException">when there is no value for the given name,
		/// or the value exists but it is not a string.</exception>
		String GetStringValue(String valueName);

		/// <summary>
		/// Gets the <see cref="Type"/> value for the given name.
		/// </summary>
		/// <param name="valueName">The name of the value to get.</param>
		/// <returns>The instance of <see cref="Type"/> represented by the value for
		/// the requested name in the registry key.</returns>
		/// <exception cref="RegistryAccessException">when there is no value for the given name,
		/// or the value exists but it is not an string, or it is a string value but it is not a 
		/// valid type name.</exception>
		Type GetTypeValue(String valueName);

		/// <summary>
		/// Gets the names for the values.
		/// </summary>
		/// <returns>The value names.</returns>
		string[] GetValueNames();

		/// <summary>
		/// Gets the sub key for the given key name.
		/// </summary>
		/// <param name="name">The name fo the key to get.</param>
		/// <returns>The sub key with the requested name if it exists; otherwise <see langword="null"/>.
		/// </returns>
		IRegistryKey OpenSubKey(string name);

		/// <summary>
		/// Gets the indication of whether the registry key represents a policy.
		/// </summary>
		Boolean IsPolicyKey { get; }

		/// <summary>
		/// Gets the full name of the registry key.
		/// </summary>
		String Name { get; }
	}
}
