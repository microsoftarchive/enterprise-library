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
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{

    [TemplatePart(Name = "PART_Header", Type = typeof(ContentControl))]
    public class SectionModelContainer : Control
    {
        private SelectionHelper selectionHelper;
        private ContentControl Header;
        private SectionViewModel SectionModel;

        static SectionModelContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SectionModelContainer), new FrameworkPropertyMetadata(typeof(SectionModelContainer)));
        }

        public SectionModelContainer()
        {
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(SectionModelContainer_DataContextChanged);

        }

        void SectionModelContainer_GotFocus(object sender, RoutedEventArgs e)
        {
            if (VisualTreeWalker.TryFindParent<ElementModelContainer>(e.OriginalSource as DependencyObject) == null)
            {
                SectionModel.Select();
            }
        }

        void SectionModelContainer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SectionModel = (SectionViewModel)e.NewValue;
            if (selectionHelper != null)
            {
                selectionHelper.Attach(SectionModel);
            }
        }

        public override void OnApplyTemplate()
        {
            Focusable = false;
            base.OnApplyTemplate();
            Header = (ContentControl)Template.FindName("PART_Header", this);

            this.selectionHelper = new SelectionHelper(Header);
            selectionHelper.Attach(SectionModel);
        }
    }
}
