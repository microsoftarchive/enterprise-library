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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
	public class MockValueAccess : ValueAccess
	{
		public object valueToAccess;
		private string key;
		private bool fail;

		public MockValueAccess(object value)
			: this(value, null)
		{ }

		public MockValueAccess(object value, string key)
			: this(value, key, false)
		{ }

		public MockValueAccess(object value, string key, bool fail)
		{
			this.valueToAccess = value;
			this.key = key;
			this.fail = fail;
		}

		public override bool GetValue(object source, out object value, out string valueAccessFailureMessage)
		{
			valueAccessFailureMessage = null;
			value = null;

			if (!fail)
				value = this.valueToAccess;
			else
				valueAccessFailureMessage = "failure";

			return !fail;
		}

		public override string Key
		{
			get { return this.key; }
		}

		public object Value
		{
			get { return this.valueToAccess; }
			set { this.valueToAccess = value; }
		}
	}
}
