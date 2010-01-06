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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class TwoColumnsViewModel : ViewModel
    {
        ViewModel left;
        ViewModel right;

        public TwoColumnsViewModel(ViewModel left, ViewModel right)
        {
            this.left = left;
            this.right = right;
        }

        public ViewModel Left
        {
            get { return left; }
        }

        public ViewModel Right
        {
            get { return right; }
        }

        public string ColumnName
        {
            get;
            set;
        }
    }
}
