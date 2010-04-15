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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// Provides access to a registry key sub keys and values.
	/// </summary>
	public abstract class RegistryKeyBase
	{
        /// <summary>
        /// The name of the policy value.
        /// </summary>
		public const String PolicyValueName = "Available";

		/// <summary>
		/// Closes the registry key.
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// Gets an actual value from the registry.
		/// </summary>
		/// <param name="valueName">The name of the value to get.</param>
		/// <returns>The value from the registry, or <see langword="null"/> if
		/// there is no such value.</returns>
		protected abstract object DoGetValue(String valueName);

		/// <summary>
		/// Gets the sub key for the given key name.
		/// </summary>
		/// <param name="name">The name fo the key to get.</param>
		/// <returns>The sub key with the requested name if it exists; otherwise <see langword="null"/>.
		/// </returns>
		public abstract IRegistryKey DoOpenSubKey(String name);

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
		/// or the value exists but it is not an integer representing a Boolean.</exception>
		public bool? GetBoolValue(String valueName)
		{
			int? value = GetIntValue(valueName);

			return value.Value == 1;
		}

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
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004", Justification="The generic parameter is used to drive the type of the return value.")]
		public T? GetEnumValue<T>(String valueName)
			where T : struct
		{
			String value = GetStringValue(valueName);

			try
			{
				return (T)Enum.Parse(typeof(T), value);
			}
			catch (ArgumentException)
			{
				throw new RegistryAccessException(
                    String.Format(CultureInfo.CurrentCulture,
									Resources.ExceptionRegistryValueNotEnumValue,
									this.Name,
									valueName,
									typeof(T).Name,
									value));
			}
		}

		/// <summary>
		/// Gets the integer value for the given name.
		/// </summary>
		/// <param name="valueName">The name of the value to get.</param>
		/// <returns>The integer value for the requested name in the registry key.</returns>
		/// <exception cref="RegistryAccessException">when there is no value for the given name,
		/// or the value exists but it is not an integer.</exception>
		public int? GetIntValue(String valueName)
		{
			object value = GetValue(valueName);

			try
			{
				return (int)value;
			}
			catch (InvalidCastException)
			{
				throw new RegistryAccessException(
                    String.Format(CultureInfo.CurrentCulture,
									Resources.ExceptionRegistryValueOfWrongType,
									this.Name,
									valueName,
									typeof(int).Name,
									value.GetType().Name));
			}
		}

		/// <summary>
		/// Gets the string value for the given name.
		/// </summary>
		/// <param name="valueName">The name of the value to get.</param>
		/// <returns>The string value for the requested name in the registry key.</returns>
		/// <exception cref="RegistryAccessException">when there is no value for the given name,
		/// or the value exists but it is not a string.</exception>
		public String GetStringValue(String valueName)
		{
			object value = GetValue(valueName);

			try
			{
				return (String)value;
			}
			catch (InvalidCastException)
			{
				throw new RegistryAccessException(
                    String.Format(CultureInfo.CurrentCulture,
									Resources.ExceptionRegistryValueOfWrongType,
									this.Name,
									valueName,
									typeof(String).Name,
									value.GetType().Name));
			}
		}

		/// <summary>
		/// Gets the <see cref="Type"/> value for the given name.
		/// </summary>
		/// <param name="valueName">The name of the value to get.</param>
		/// <returns>The instance of <see cref="Type"/> represented by the value for
		/// the requested name in the registry key.</returns>
		/// <exception cref="RegistryAccessException">when there is no value for the given name,
		/// or the value exists but it is not an string, or it is a string value but it is not a 
		/// valid type name.</exception>
		public Type GetTypeValue(String valueName)
		{
			String value = GetStringValue(valueName);

			Type type = Type.GetType(value, false);

			if (type == null)
			{
				throw new RegistryAccessException(
                    String.Format(CultureInfo.CurrentCulture,
									Resources.ExceptionRegistryValueNotTypeName,
									this.Name,
									valueName,
									value));
			}

			return type;
		}

		/// <summary>
		/// Gets the names for the values.
		/// </summary>
		/// <returns>The value names.</returns>
		public abstract string[] GetValueNames();

		/// <summary>
		/// Gets the sub key for the given key name.
		/// </summary>
		/// <param name="name">The name fo the key to get.</param>
		/// <returns>The sub key with the requested name if it exists; otherwise <see langword="null"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">when <paramref name="name"/>is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">when <paramref name="name"/>is not a valid name.</exception>
		public IRegistryKey OpenSubKey(String name)
		{
			CheckValidName(name, "name");

			return DoOpenSubKey(name);
		}

		/// <summary>
		/// Gets the indication of whether the registry key represents a policy.
		/// </summary>
		public Boolean IsPolicyKey
		{
			get
			{
				return DoGetValue(PolicyValueName) != null;
			}
		}

		/// <summary>
		/// Gets the full name of the registry key.
		/// </summary>
		public abstract String Name { get; }

		private object GetValue(String valueName)
		{
			CheckValidName(valueName, "valueName");

			object value = DoGetValue(valueName);
			if (value == null)
			{
				throw new RegistryAccessException(
                    String.Format(CultureInfo.CurrentCulture,
						Resources.ExceptionMissingRegistryValue,
						this.Name,
						valueName));
			}

			return value;
		}

		private static void CheckValidName(String name, String argumentName)
		{
			if (name == null)
			{
				throw new ArgumentNullException(argumentName);
			}
			if (name.Length == 0)
			{
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
						Resources.ExceptionArgumentEmpty, 
						argumentName));
			}
		}
	}
}
