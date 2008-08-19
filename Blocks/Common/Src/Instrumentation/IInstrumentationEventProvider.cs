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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	/// <summary>
	/// Defines the contract implemented by objects that provide instrumentation events for block objects.
	/// An implementation of this interface is used when the block object itself cannot provide the
	/// instrumentation events itself for some reason and desires to delegate the responsibility for
	/// containing the instrumentations to another object. One important usage of instances of this class are
	/// when a block object wishes to pass the instrumentation events to a contained block object.
	/// </summary>
	public interface IInstrumentationEventProvider
	{
		/// <summary>
		/// Returns the object to which the instrumentation events have been delegated.
		/// </summary>
		/// <returns>Object to which the instrumentation events have been delegated.</returns>
		object GetInstrumentationEventProvider();
	}
}
