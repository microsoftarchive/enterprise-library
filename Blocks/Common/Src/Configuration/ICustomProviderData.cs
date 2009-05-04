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
using System.Configuration;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	/// <summary>
	/// Represents the configuration settings for a custom provider.
	/// </summary>
	public interface ICustomProviderData
	{
		/// <summary>
		/// Gets the name for the represented provider.
		/// </summary>
		string Name	{ get; }

		/// <summary>
		/// Gets the attributes for the represented provider.
		/// </summary>
		NameValueCollection Attributes { get; }
	}
}
