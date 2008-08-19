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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Values that either allow or disallow multiple nodes in a parent.
	/// </summary>
	public enum NodeMultiplicity
	{
		/// <summary>
		/// Allows multiple nodes.
		/// </summary>
		Allow = 0,
		/// <summary>
		/// Disallows multiple nodes.
		/// </summary>
		Disallow = 1
	}
}
