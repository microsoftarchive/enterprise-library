//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Runtime.InteropServices;

namespace ValidationQuickStart
{
    /// <summary>
    /// Contains InterOp method calls used by the application.
    /// </summary>
    public sealed class NativeMethods
    {
        /// private constructor, not intended for instantiation
        private NativeMethods()
        {
        }

        [DllImport("user32.dll")]
        internal static extern
            bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern
            bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        internal static extern
            bool IsIconic(IntPtr hWnd);

        internal static int SW_RESTORE = 9;
    }
}
