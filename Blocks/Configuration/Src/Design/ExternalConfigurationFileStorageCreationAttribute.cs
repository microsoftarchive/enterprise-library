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
	/// Creates a storage creation command for configuration files.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class ExternalConfigurationFileStorageCreationAttribute : StorageCreationAttribute
	{
		/// <summary>
		/// Create the <see cref="ExternalConfigurationFileStorageCreationCommand"/> for the file.
		/// </summary>
		/// <param name="instance">The instance for the property. This should be a file name.</param>
		/// <param name="propertyInfo">The property this attribute applies.</param>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="StorageCreationCommand"/> instance.</returns>
		/// <returns>A <see cref="ExternalConfigurationFileStorageCreationCommand"/> instance based on the file.</returns>
		public override StorageCreationCommand CreateCommand(object instance, PropertyInfo propertyInfo, IServiceProvider serviceProvider)
		{
			if (null == propertyInfo) throw new ArgumentNullException("propertyInfo");

			string fileName = (string)propertyInfo.GetValue(instance, null);
			return new ExternalConfigurationFileStorageCreationCommand(fileName, serviceProvider);
		}
	}
}
