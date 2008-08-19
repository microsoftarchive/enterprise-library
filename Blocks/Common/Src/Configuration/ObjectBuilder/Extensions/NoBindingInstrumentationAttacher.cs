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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Instrumentation binder that does nothing. Implementation of Null Object Pattern.
	/// </summary>
	public class NoBindingInstrumentationAttacher : IInstrumentationAttacher
	{
		/// <summary>
		/// Null implementation of interface contract method. Does no binding.
		/// On purpose. Really... Look up NullObject pattern for justification.
		/// </summary>
		public void BindInstrumentation()
		{
		}
	}
}
