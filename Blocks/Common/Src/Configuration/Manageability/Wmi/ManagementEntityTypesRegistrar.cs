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
using System.Collections.Generic;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// This class is used to work around some limitations in the InstrumentationManager handling
	/// of management types.
	/// </summary>
	public static class ManagementEntityTypesRegistrar
	{
		private static IDictionary<Type, Type> registeredTypes = new Dictionary<Type, Type>();
		private static object registeredTypesLock = new object();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="types"></param>
		public static void SafelyRegisterTypes(params Type[] types)
		{
			lock (registeredTypesLock)
			{
				foreach (Type type in types)
				{
					if (!registeredTypes.ContainsKey(type))
					{
						DoRegisterType(type);
						registeredTypes.Add(type, type);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="types"></param>
		public static void SafelyUnregisterTypes(params Type[] types)
		{
			lock (registeredTypesLock)
			{
				foreach (Type type in types)
				{
					if (registeredTypes.ContainsKey(type))
					{
						DoUnregisterType(type);
						registeredTypes.Remove(type);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void UnregisterAll()
		{
			lock (registeredTypesLock)
			{
				foreach (Type type in registeredTypes.Keys)
				{
					DoUnregisterType(type);
				}
				registeredTypes.Clear();
			}
		}

		private static void DoRegisterType(Type type)
		{
			try
			{
				InstrumentationManager.RegisterType(type);
			}
			catch (NullReferenceException)
			{
				// work around for WMI.NET 2.0 issue
			}
		}

		private static void DoUnregisterType(Type type)
		{
			try
			{
				InstrumentationManager.UnregisterType(type);
			}
			catch (NullReferenceException)
			{
				// work around for WMI.NET 2.0 issue
			}
		}
	}
}
