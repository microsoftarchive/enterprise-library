//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block QuickStart
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

namespace SecurityQuickStart
{
	sealed class WaitCursorHelper : IDisposable
	{
		private Control owner;
		private Cursor previousCursor;

		public WaitCursorHelper(Control owner)
		{
			this.owner = owner;
			this.previousCursor = this.owner.Cursor;
			this.owner.Cursor = Cursors.WaitCursor;
		}

		void IDisposable.Dispose()
		{
			this.owner.Cursor = this.previousCursor;
		}
	}
}
