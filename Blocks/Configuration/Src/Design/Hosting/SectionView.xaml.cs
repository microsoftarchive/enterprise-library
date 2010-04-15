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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting
{
    /// <summary>
    /// Defines the main surface views for sections in the <see cref="ConfigurationSourceModel"/>.
    /// <br/>
    /// This is used by the design-time infrastructure and is not intended to be used directly from your code.
    /// </summary>
    /// 
    public partial class SectionView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SectionView"/>.
        /// </summary>
        public SectionView()
        {
            InitializeComponent();
        }
    }
}
