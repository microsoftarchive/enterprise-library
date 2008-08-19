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

// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// Indicates that the field or property should not exposed when the raised to an EventSource.
	/// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public sealed class InternalAttribute : IgnoreMemberAttribute 
    {
    }

	/// <summary>
	/// Indicates that this type can be raised to an EventSource.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public sealed class InstrumentationTypeAttribute : System.Management.Instrumentation.InstrumentationClassAttribute
	{
		/// <summary>
		/// Default constructor that marks the class as a type that can be raised.
		/// </summary>
		public InstrumentationTypeAttribute()
			: base(InstrumentationType.Event)
		{
		}
	}
}
