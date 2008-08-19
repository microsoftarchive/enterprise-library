//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Represents the common configuration data for all hash providers.</para>
    /// </summary>
	public class HashProviderData : NameTypeConfigurationElement
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="HashProviderData"/> class.</para>
        /// </summary>
        public HashProviderData()
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="HashProviderData"/> clas with a name and a <see cref="IHashProvider"/> type.
		/// </summary>
		/// <param name="name">The name of the configured <see cref="IHashProvider"/>.</param>
		/// <param name="type">The type of the <see cref="IHashProvider"/>.</param>
		public HashProviderData(string name, Type type)
			: base(name, type)
		{
		}
    }
}