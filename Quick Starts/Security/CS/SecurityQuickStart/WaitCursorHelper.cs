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
