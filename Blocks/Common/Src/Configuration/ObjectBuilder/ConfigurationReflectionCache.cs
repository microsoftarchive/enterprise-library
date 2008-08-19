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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Caches reflection information used by the building process.
	/// </summary>
	public sealed class ConfigurationReflectionCache
	{
		private Dictionary<PairKey<Type, Type>, Attribute> typeAttributes;
		private object typeAttributesLock = new object();
		private Dictionary<PairKey<Type, Type>, Attribute> typeInheritedAttributes;
		private object typeInheritedAttributesLock = new object();

		private Dictionary<Type, ICustomFactory> typeCustomFactories;
		private object typeCustomFactoriesLock = new object();

		private Dictionary<Type, IConfigurationNameMapper> typeNameMappers;
		private object typeNameMappersLock = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationReflectionCache"/> class.
		/// </summary>
		public ConfigurationReflectionCache()
		{
			typeAttributes = new Dictionary<PairKey<Type, Type>, Attribute>();
			typeInheritedAttributes = new Dictionary<PairKey<Type, Type>, Attribute>();
			typeCustomFactories = new Dictionary<Type, ICustomFactory>();
			typeNameMappers = new Dictionary<Type, IConfigurationNameMapper>();
		}

		/// <summary>
		/// Returns the attribute of type <typeparamref name="TAttribute"/> applied the given type.
		/// </summary>
		/// <remarks>
		/// The attribute will be retrieved from the cache, if it has been requested before.
		/// The query will not look for inherited attributes.
		/// </remarks>
		/// <typeparam name="TAttribute">The type of attribute required.</typeparam>
		/// <param name="type">The type to query for the attribute.</param>
		/// <returns>The attribute, or null if the attribute type is not applied to <paramref name="type"/>.</returns>
		public TAttribute GetCustomAttribute<TAttribute>(Type type)
			where TAttribute : Attribute
		{
			return GetCustomAttribute<TAttribute>(type, false);
		}

		/// <summary>
		/// Returns true if there is a cached entry for the attribute type for the given type
		/// </summary>
		/// <remarks>
		/// A null cached entry will return true.
		/// </remarks>
		/// <typeparam name="TAttribute">The type of attribute required.</typeparam>
		/// <param name="type">The type to query for the attribute.</param>
		/// <returns>true if the result for the attribute query is in the cache.</returns>
		public bool HasCachedCustomAttribute<TAttribute>(Type type)
		{
			PairKey<Type, Type> key = new PairKey<Type, Type>(type, typeof(TAttribute));
			return this.typeAttributes.ContainsKey(key);
		}

		/// <summary>
		/// Returns the attribute of type <typeparamref name="TAttribute"/> applied the given type.
		/// </summary>
		/// <remarks>
		/// The attribute will be retrieved from the cache, if it has been requested before.
		/// </remarks>
		/// <typeparam name="TAttribute">The type of attribute required.</typeparam>
		/// <param name="type">The type to query for the attribute.</param>
		/// <param name="inherit">Specifies whether to search the inheritance chain of the type to find the attributes.</param>
		/// <returns>The attribute, or null if the attribute type is not applied to <paramref name="type"/>.</returns>
		public TAttribute GetCustomAttribute<TAttribute>(Type type, bool inherit)
			where TAttribute : Attribute
		{
			TAttribute attribute
				= DoGetCustomAttribute<TAttribute>(
					type,
					inherit ? typeInheritedAttributes : typeAttributes,
					inherit ? typeInheritedAttributesLock : typeAttributesLock,
					inherit);			
			return attribute;
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Returns the custom factory associated to <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type to query for the custom factory.</param>
		/// <returns>The custom factory for <paramref name="type"/>, or null if the type does not have an 
		/// <see cref="AssemblerAttribute">Assembler</see> attribute.</returns>
		public ICustomFactory GetCustomFactory(Type type)
		{
			ICustomFactory storedObject;
			bool exists = false;
			lock (typeCustomFactoriesLock)
			{
				exists = typeCustomFactories.TryGetValue(type, out storedObject);
			}
			if (!exists)
			{
				storedObject = CreateCustomFactory(type);
				lock (typeCustomFactoriesLock)
				{
					typeCustomFactories[type] = storedObject;
				}
			}

			return storedObject;
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Returns the name mapper associated to <paramref name="type"/>.
		/// </summary>
		/// <param name="type">The type to query for the name mapper.</param>
		/// <returns>The name mapper for <paramref name="type"/>, or null if the type does not have a 
		/// <see cref="ConfigurationNameMapperAttribute">ConfigurationNameMapper</see> attribute.</returns>
		public IConfigurationNameMapper GetConfigurationNameMapper(Type type)
		{
			IConfigurationNameMapper storedObject;
			bool exists = false;
			lock (typeNameMappersLock)
			{
				exists = typeNameMappers.TryGetValue(type, out storedObject);
			}
			if (!exists)
			{
				storedObject = CreateConfigurationNameMapper(type);
				lock (typeNameMappersLock)
				{
					typeNameMappers[type] = storedObject;
				}
			}

			return storedObject;
		}

		private TAttribute DoGetCustomAttribute<TAttribute>(Type type, Dictionary<PairKey<Type, Type>, Attribute> cache, object lockObject, bool inherit)
			where TAttribute : Attribute
		{
			PairKey<Type, Type> key = new PairKey<Type, Type>(type, typeof(TAttribute));

			Attribute storedObject;
			bool exists = false;
			lock (lockObject)
			{
				exists = cache.TryGetValue(key, out storedObject);
			}
			if (!exists)
			{
				storedObject = RetrieveAttribute<TAttribute>(key, inherit);
				lock (lockObject)
				{
					cache[key] = storedObject;
				}
			}

			return storedObject as TAttribute;
		}

		private TAttribute RetrieveAttribute<TAttribute>(PairKey<Type, Type> key, bool inherit)
			where TAttribute : Attribute
		{
			TAttribute attribute = (TAttribute)Attribute.GetCustomAttribute(key.Left, typeof(TAttribute), inherit);
			return attribute;
		}

		private ICustomFactory CreateCustomFactory(Type type)
		{
			CustomFactoryAttribute attribute
			   = GetCustomAttribute<CustomFactoryAttribute>(type);
			if (attribute != null)
			{
				return (ICustomFactory)Activator.CreateInstance(attribute.FactoryType);
			}
			else
			{
				return null;
			}
		}

		private IConfigurationNameMapper CreateConfigurationNameMapper(Type type)
		{
			ConfigurationNameMapperAttribute attribute
			   = GetCustomAttribute<ConfigurationNameMapperAttribute>(type);
			if (attribute != null)
			{
				return (IConfigurationNameMapper)Activator.CreateInstance(attribute.NameMappingObjectType);
			}
			else
			{
				return null;
			}
		}
	}

	internal class PairKey<TLeft, TRight>
	{
		private TLeft left;
		private TRight right;

		internal PairKey(TLeft left, TRight right)
		{
			this.left = left;
			this.right = right;
		}

		public override bool Equals(object obj)
		{
			PairKey<TLeft, TRight> other = obj as PairKey<TLeft, TRight>;

			if (other == null)
				return false;

			return (Equals(left, other.left) && Equals(right, other.right));
		}

		public override int GetHashCode()
		{
			int hashForType = left == null ? 0 : left.GetHashCode();
			int hashForID = right == null ? 0 : right.GetHashCode();
			return hashForType ^ hashForID;
		}

		public TLeft Left 
		{ 
			get { return left; } 
		}

		public TRight Right
		{
			get { return right; }
		}
	}
}
