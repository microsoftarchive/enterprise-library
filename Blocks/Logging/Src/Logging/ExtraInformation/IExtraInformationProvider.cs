//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation
{
	/// <summary>
	/// Defines a method to populate an <see cref="IDictionary{K,T}"/> with helpful diagnostic information.
	/// </summary>
	public interface IExtraInformationProvider
	{
		/// <summary>
		/// Populates an <see cref="IDictionary{K,T}"/> with helpful diagnostic information.
		/// </summary>
		/// <param name="dict">Dictionary containing extra information used to initialize the <see cref="IExtraInformationProvider"></see> instance</param>
		void PopulateDictionary(IDictionary<string, object> dict);
	}
}
