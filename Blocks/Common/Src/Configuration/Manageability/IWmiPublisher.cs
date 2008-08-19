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
	/// <summary>
	/// Provides a way to publish and revoke WMI objects.
	/// </summary>
	/// <remarks>
	/// This interface allows for unit testing without actually publishing the WMI objects.
	/// </remarks>
	public interface IWmiPublisher
	{
		/// <summary>
		/// Makes an instance visible through management instrumentation.
		/// </summary>
		/// <param name="instance">The object that is to be visible through management instrumentation.</param>
		void Publish(ConfigurationSetting instance);

		/// <summary>
		/// Makes an instance that was previously published through the Publish method no longer visible 
		/// through management instrumentation. 
		/// </summary>
		/// <param name="instance">The object to remove from visibility for management instrumentation.</param>
		void Revoke(ConfigurationSetting instance);
	}
}
