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
using System.Text;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents a mechanism to attribute properties that need a <see cref="StorageCreationCommand"/>. This class is abstract
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public abstract class StorageCreationAttribute : Attribute
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="StorageCreationAttribute"/> class.
		/// </summary>
		protected StorageCreationAttribute()
		{
		}

		/// <summary>
		/// Create the <see cref="StorageCreationCommand"/> for the property.
		/// </summary>
		/// <param name="instance">The instance for the property.</param>
		/// <param name="propertyInfo">The property this attribute applies.</param>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="StorageCreationCommand"/> instance.</returns>
		public abstract StorageCreationCommand CreateCommand(object instance, PropertyInfo propertyInfo, IServiceProvider serviceProvider);
	}
}
