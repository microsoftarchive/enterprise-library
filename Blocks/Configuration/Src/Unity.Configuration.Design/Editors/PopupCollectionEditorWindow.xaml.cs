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

namespace Microsoft.Practices.Unity.Configuration.Design.Editors
{
    /// <summary>
    /// Interaction logic for PopupCollectionEditorWindow.xaml
    /// </summary>
    public partial class PopupCollectionEditorWindow : Window
    {
        public PopupCollectionEditorWindow()
        {
            InitializeComponent();
        }

        public object ContainedContent
        {
            get { return Contained.Content; }
            set { Contained.Content = value; }
        }
    }
}
