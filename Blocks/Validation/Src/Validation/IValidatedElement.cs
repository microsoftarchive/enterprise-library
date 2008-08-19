//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
	/// <summary>
	/// Represents the description of how validation must be performed on a language element.
	/// </summary>
	internal interface IValidatedElement
	{
		IEnumerable<IValidatorDescriptor> GetValidatorDescriptors();

		CompositionType CompositionType { get; }
		string CompositionMessageTemplate { get; }
		string CompositionTag { get; }
		bool IgnoreNulls { get; }
		string IgnoreNullsMessageTemplate { get; }
		string IgnoreNullsTag { get; }
		MemberInfo MemberInfo { get; }
		Type TargetType { get; }
	}
}