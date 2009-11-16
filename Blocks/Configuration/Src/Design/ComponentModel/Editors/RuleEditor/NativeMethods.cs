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
using System.Runtime.InteropServices;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    internal sealed class NativeMethods
    {
        private NativeMethods()
        {
        }

        [DllImport("user32", CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, ref CHARFORMAT2 lParam);

        /// <summary> 
        /// Contains information about character formatting in a rich edit control. 
        /// </summary> 
        /// <remarks><see cref="CHARFORMAT2"/> requires Rich Edit 2.0.</remarks> 
        [StructLayout(LayoutKind.Sequential)]
            internal struct CHARFORMAT2
        {
            public int cbSize;
            public uint dwMask;
            public uint dwEffects;
            public int yHeight;
            public int yOffset;
            public int crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] public char[] szFaceName;
            public short wWeight;
            public short sSpacing;
            public int crBackColor;
            public int LCID;
            public uint dwReserved;
            public short sStyle;
            public short wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
        }

        /// <summary> 
        /// Contains information about paragraph formatting in a rich edit control. 
        /// </summary> 
        /// <remarks><see cref="PARAFORMAT2"/> requires Rich Edit 2.0.</remarks> 
        [StructLayout(LayoutKind.Sequential)]
            internal struct PARAFORMAT2
        {
            public int cbSize;
            public uint dwMask;
            public short wNumbering;
            public short wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public short wAlignment;
            public short cTabCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public int[] rgxTabs;
            public int dySpaceBefore;
            public int dySpaceAfter;
            public int dyLineSpacing;
            public short sStyle;
            public byte bLineSpacingRule;
            public byte bOutlineLevel;
            public short wShadingWeight;
            public short wShadingStyle;
            public short wNumberingStart;
            public short wNumberingStyle;
            public short wNumberingTab;
            public short wBorderSpace;
            public short wBorderWidth;
            public short wBorders;
        }
	}
}
