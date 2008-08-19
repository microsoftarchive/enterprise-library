//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
	/// <summary>
	/// Represents an authorization rule.
	/// </summary>
	public interface IAuthorizationRule
	{
		/// <summary>
		/// Gets the name of the rule.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the expression of the rule.
		/// </summary>
		string Expression
		{
			get;
		}
	}
}
