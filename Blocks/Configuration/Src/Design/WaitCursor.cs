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
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Provides a simple way to set a wait cursor (or other cursor)
	/// around a block of code.
	/// </summary>
	/// <example>
	/// using( WaitCursor wait = new WaitCursor()
	/// {
	///		// Do something
	/// }
	/// </example>
	public sealed class WaitCursor : IDisposable
	{
		private Cursor oldCursor;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="WaitCursor"/> class.</para>
        /// </summary>
		public WaitCursor()  : this(Cursors.WaitCursor)
		{
		}

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="WaitCursor"/> class.</para>
        /// </summary>
        /// <param name="cursor">
        /// <para>The <see cref="Cursor"/> to use as the wait <see cref="Cursor"/>.</para>
        /// </param>
		public WaitCursor(Cursor cursor)
		{
            oldCursor = Cursor.Current;
            Cursor.Current = cursor;
		}

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="WaitCursor"/> and optionally releases the managed resources.</para>
        /// </summary>
		public void Dispose()
		{
            Cursor.Current = oldCursor;
		}
	}
}
