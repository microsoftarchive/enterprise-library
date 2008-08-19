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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation
{
	/// <summary>
	/// Represents the WMI event fired when a validation operation is sucessful.
	/// </summary>
	public class ValidationSucceededEvent : ValidationCalledEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationSucceededEvent"/> class.
		/// </summary>
		/// <param name="typeBeingValidated">The name of the type being validated.</param>
		public ValidationSucceededEvent(string typeBeingValidated)
			: base(typeBeingValidated)
		{
		}
	}
}
