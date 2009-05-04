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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	/// <summary>
	/// Represents an item in a drop down list.
	/// </summary>
	public struct AdmDropDownListItem
	{
		private String name;
		private String value;

		/// <summary>
		/// Initializes a new instance of the <see cref="AdmDropDownListItem"/> class.
		/// </summary>
		/// <param name="name">The item name.</param>
		/// <param name="value">The item value.</param>
		public AdmDropDownListItem(String name, String value)
		{
			this.name = name;
			this.value = value;
		}

		/// <summary>
		/// Gets the name of the item.
		/// </summary>
		public String Name
		{
			get { return name; }
		}

		/// <summary>
		/// Gets the value of the item.
		/// </summary>
		public String Value
		{
			get { return this.value; }
		}
	}
}
