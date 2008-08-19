//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	/// <summary>
	/// Base class for configuration data defined for all types of StorageEncryptionProviders
	/// </summary>
	public class StorageEncryptionProviderData : NameTypeConfigurationElement
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="StorageEncryptionProviderData"/> class.
		/// </summary>
		public StorageEncryptionProviderData()
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="StorageEncryptionProviderData"/> class with a name and the type of the <see cref="IStorageEncryptionProvider"/>.
		/// </summary>
		/// <param name="name">
		/// The name of the <see cref="IStorageEncryptionProvider"/>.
		/// </param>
		/// <param name="type">
		/// The type of <see cref="IStorageEncryptionProvider"/>.
		/// </param>
		public StorageEncryptionProviderData(string name, Type type)
			: base(name, type)
		{
		}
	}
}