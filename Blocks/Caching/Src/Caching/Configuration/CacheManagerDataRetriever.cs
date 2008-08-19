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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Resolves default names for cache managers.
	/// </summary>
	public class CacheManagerDataRetriever : IConfigurationNameMapper
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Returns the default cache manager name from the configuration in the <paramref name="configSource"/>, if the
		/// value for <paramref name="name"/> is <see langword="null"/> (<b>Nothing</b> in Visual Basic).
		/// </summary>
		/// <param name="name">The current name.</param>
		/// <param name="configurationSource">The source for configuration information.</param>
		/// <returns>The default cache manager name if <paramref name="name"/> is <see langword="null"/> (<b>Nothing</b> in Visual Basic),
		/// otherwise the original value for <b>name</b>.</returns>
		public string MapName(string name, IConfigurationSource configurationSource)
		{
			if (name == null)
			{
				CachingConfigurationView view = new CachingConfigurationView(configurationSource);
				return view.DefaultCacheManager;
			}

			return name;
		}
	}
}
