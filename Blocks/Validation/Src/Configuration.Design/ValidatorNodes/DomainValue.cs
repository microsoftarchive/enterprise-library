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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
	/// <summary>
	/// Represents a domain value that is editable in the design time.
	/// </summary>
	public class DomainValue
	{
		private string value;
		
		/// <summary>
		/// Initialize a new instance of the <see cref="DomainValue"/> class.
		/// </summary>
		public DomainValue()
		{ }

		/// <summary>
		/// Initialize a new instance of the <see cref="DomainValue"/> class.
		/// </summary>
		public DomainValue(string value)
		{
			this.value = value;
		}

		/// <summary>
		/// Gets or sets the value for the domain entry.
		/// </summary>
		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}
	}
}
