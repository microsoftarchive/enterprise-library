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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	/// <summary>
	/// Indicates the configuration object type that is used for the attributed object.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ConfigurationElementTypeAttribute : Attribute
	{
		private Type configurationType;

		/// <summary>
		/// Initialize a new instance of the <see cref="EnterpriseLibrary.Common.Configuration.ConfigurationElementTypeAttribute"/> class.
		/// </summary>
		public ConfigurationElementTypeAttribute()
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationElementTypeAttribute"/> class with the configuration object type.
		/// </summary>
		/// <param name="configurationType">The <see cref="Type"/> of the configuration object.</param>
		public ConfigurationElementTypeAttribute(Type configurationType)
		{
			this.configurationType = configurationType;
		}

		/// <summary>
		/// Gets the <see cref="Type"/> of the configuration object.
		/// </summary>
		/// <value>
		/// The <see cref="Type"/> of the configuration object.
		/// </value>
		public Type ConfigurationType
		{
			get { return configurationType; }			
		}
	}
}
