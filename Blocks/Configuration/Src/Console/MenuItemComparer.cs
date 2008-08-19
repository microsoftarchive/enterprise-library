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
using System.Collections;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    class MenuItemComparer : IComparer
    {
		public int Compare(object lhs, object rhs)
		{
			return Compare((MenuItem)lhs, (MenuItem)rhs);
		}

        public int Compare(MenuItem lhs, MenuItem rhs)
        {
            if (null == lhs && null != rhs)
            {
                return -1;
            }
            if (null == rhs && null != lhs)
            {
                return 1;
            }
            if (null == lhs && null == rhs)
            {
                return 0;
            }
            return lhs.Text.CompareTo(rhs.Text);
        }
    }
}
