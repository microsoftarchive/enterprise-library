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
	/// Represents the description of how validation must be performed on a type.
	/// </summary>
	internal interface IValidatedType : IValidatedElement
	{
		IEnumerable<IValidatedElement> GetValidatedProperties();
		IEnumerable<IValidatedElement> GetValidatedFields();
		IEnumerable<IValidatedElement> GetValidatedMethods();
		IEnumerable<MethodInfo> GetSelfValidationMethods();
	}
}
