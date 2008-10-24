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
using System.Windows.Forms;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Tests.TestClasses
{
	public class MockControl : Control
	{
		private int intControlProperty;
		public int IntControlProperty
		{
			get { return intControlProperty; }
			set { intControlProperty = value; }
		}

		private string stringControlProperty;
		public string StringControlProperty
		{
			get { return stringControlProperty; }
			set { stringControlProperty = value; }
		}

		public bool FireValidating()
		{
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			this.OnValidating(cancelEventArgs);

			return cancelEventArgs.Cancel;
		}
	}
}
