//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI
{
    /// <summary>
    /// A custom tree view that lets me popup the context menu in the appropriate place when the Application
    /// </summary>
	public class CustomTreeView : TreeView
	{
        private const int WM_CONTEXTMENU = 0x7B; 

		private Container components = null;

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomTreeView"/> class.
        /// </summary>
		public CustomTreeView()
		{
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        ///<summary>
        ///Overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)"></see>.
        ///</summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_CONTEXTMENU && ContextMenu != null && SelectedNode != null)
            {
                if (m.LParam.ToInt32() == -1)
                {
                    Debug.WriteLine("LPARAM = " + m.LParam.ToString());
                    Point p = new Point(SelectedNode.Bounds.Left + 5, SelectedNode.Bounds.Bottom);
                    ContextMenu.Show(this, p);
                    DefWndProc(ref m);
                    return;
                }
            }
            base.WndProc (ref m);
        }


		private void InitializeComponent()
		{
			components = new Container();
		}
	}
}
