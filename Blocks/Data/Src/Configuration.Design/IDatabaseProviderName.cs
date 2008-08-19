//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	/// <summary>
	/// Represents a database provider name used for a connection.
	/// </summary>
	public interface IDatabaseProviderName
	{
		/// <summary>
		/// When implemented by a class, gets the name of the provider.
		/// </summary>
		/// <value>
		/// The name of the provider.
		/// </value>
		string DatabaseProviderName { get;}
	}
}
