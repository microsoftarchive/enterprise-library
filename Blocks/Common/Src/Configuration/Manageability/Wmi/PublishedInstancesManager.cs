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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	///<summary>
	/// Manages publication of configuration settings.
	///</summary>
	///<typeparam name="TSetting">The settings base class that defines a key.</typeparam>
	///<typeparam name="TKey">The publication key.</typeparam>
	public class PublishedInstancesManager<TSetting, TKey>
		where TSetting : ConfigurationSetting
	{
		private IDictionary<Type, IDictionary<TKey, TSetting>> publishedInstances
			= new Dictionary<Type, IDictionary<TKey, TSetting>>();

		private readonly object publishedInstancesLock = new object();

		/// <summary>
		/// Makes the setting available for WMI clients.
		/// </summary>
		///<param name="setting">The setting to publish.</param>
		///<param name="key">The key to use when publish the setting.</param>
		public void Publish(TSetting setting, TKey key)
		{
			Type thisType = setting.GetType();

			lock (publishedInstancesLock)
			{
				IDictionary<TKey, TSetting> publishedInstancesForType;
				if (!publishedInstances.TryGetValue(thisType, out publishedInstancesForType))
				{
					publishedInstancesForType = new Dictionary<TKey, TSetting>();
					publishedInstances[thisType] = publishedInstancesForType;
				}

				try
				{
					publishedInstancesForType.Add(key, setting);
				}
				catch (ArgumentException)
				{
					if (setting != publishedInstancesForType[key])
					{
						throw;
					}
				}
			}
		}

		/// <summary>
		/// Makes the setting unavailable for WMI clients.
		/// </summary>
		///<param name="setting">The setting to publish.</param>
		///<param name="key">The key used to publish the setting.</param>
		public void Revoke(TSetting setting, TKey key)
		{
			Type thisType = setting.GetType();

			lock (publishedInstancesLock)
			{
				IDictionary<TKey, TSetting> publishedInstancesForType;
				if (publishedInstances.TryGetValue(thisType, out publishedInstancesForType))
				{
					// the dictionary will not throw if the key doesn't exist
					publishedInstancesForType.Remove(key);
				}
			}
		}

		/// <summary>
		/// Clear collection of published instances.
		/// </summary>
		public void ClearPublishedInstances()
		{
			lock (publishedInstancesLock)
			{
				publishedInstances = new Dictionary<Type, IDictionary<TKey, TSetting>>();
			}
		}

		/// <summary>
		/// Returns an enumeration of the published <see cref="ConfigurationSectionSetting"/> instances.
		/// </summary>
		/// <typeparam name="T">A valid setting class.</typeparam>
		/// <returns>All the published instances for <typeparamref name="T"/>.</returns>
		public IEnumerable<T> GetInstances<T>()
			where T : TSetting
		{
			lock (publishedInstancesLock)
			{
				IDictionary<TKey, TSetting> publishedInstancesForType;
				if (publishedInstances.TryGetValue(typeof(T), out publishedInstancesForType))
				{
					T[] values = new T[publishedInstancesForType.Count];
					publishedInstancesForType.Values.CopyTo(values, 0);
					return values;
				}
				else
				{
					return new T[0];
				}
			}
		}

		/// <summary>
		/// Returns the setting instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="key">The key to use for lookup.</param>
		/// <returns>The published setting instance specified by the values for the key properties,
		/// or <see langword="null"/> if no such an instance is currently published.</returns>
		public T BindInstance<T>(TKey key)
			where T : TSetting
		{
			lock (publishedInstancesLock)
			{
				IDictionary<TKey, TSetting> publishedInstancesForType;
				if (publishedInstances.TryGetValue(typeof(T), out publishedInstancesForType))
				{
					TSetting instance;
					publishedInstancesForType.TryGetValue(key, out instance);

					return instance as T;
				}
				else
				{
					return null;
				}
			}
		}
	}
}
