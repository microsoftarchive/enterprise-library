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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Reflection;
using System.Resources;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Helper class to load resources strings.
	/// </summary>
	public static class ResourceStringLoader
	{

        /// <summary>
        /// Load a resource string.
        /// </summary>
        /// <param name="baseName">The base name of the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The string from the resource.</returns>
        public static string LoadString(string baseName, string resourceName)
        {
            return LoadString(baseName, resourceName, Assembly.GetCallingAssembly());
        }

		/// <summary>
		/// Load a resource string.
		/// </summary>
		/// <param name="baseName">The base name of the resource.</param>
		/// <param name="resourceName">The resource name.</param>
		/// <param name="asm">The assembly to load the resource from.</param>
		/// <returns>The string from the resource.</returns>
		public static string LoadString(string baseName, string resourceName, Assembly asm)
		{
			if (string.IsNullOrEmpty(baseName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "baseName");
			if (string.IsNullOrEmpty(resourceName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "resourceName");


			string value = null;
            if (null != asm) value = LoadAssemblyString(asm, baseName, resourceName);
			if (null == value) value = LoadAssemblyString(Assembly.GetExecutingAssembly(), baseName, resourceName);
			if (null == value) return string.Empty;
			return value;
		}		

		private static string LoadAssemblyString(Assembly asm, string baseName, string resourceName)
		{
			try
			{
				ResourceManager rm = new ResourceManager(baseName, asm);
				return rm.GetString(resourceName);
			}
			catch (MissingManifestResourceException)
			{
			}
			return null;
		}
	}
}
