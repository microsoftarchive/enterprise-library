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
	/// Determines if a child node should notify it's parent when an event occurs.
	/// </summary>
	public enum ParentNodeNotification
	{
		/// <summary>
		/// Notify the parent.
		/// </summary>
		Notify = 0,
		/// <summary>
		/// Do not notify the parent.
		/// </summary>
		DoNotNotify = 1
	}
}
