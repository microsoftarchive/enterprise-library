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

using System.Windows.Input;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls.Toolkit.Windows.Controls
{
    internal static class KeyboardHelper
    {
        public static void GetMetaKeyState(out bool ctrl, out bool shift)
        {
            ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        }
    }
}
