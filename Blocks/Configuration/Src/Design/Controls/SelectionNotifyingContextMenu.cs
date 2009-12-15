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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    public class SelectionNotifyingContextMenu : ContextMenu
    {
        internal static readonly RoutedEvent IsSelectedChangedEvent =
            EventManager.RegisterRoutedEvent("IsSelectedChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<bool>),
                typeof(SelectionNotifyingContextMenu));

        // Using a DependencyProperty as the backing store for CurrentSelection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSelectionProperty =
            DependencyProperty.Register("CurrentSelection", typeof(MenuItem), typeof(SelectionNotifyingContextMenu), new UIPropertyMetadata(null));

        static SelectionNotifyingContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectionNotifyingContextMenu), new FrameworkPropertyMetadata(typeof(SelectionNotifyingContextMenu)));

            EventManager.RegisterClassHandler(
                typeof(SelectionNotifyingContextMenu),
                IsSelectedChangedEvent,
                new RoutedPropertyChangedEventHandler<bool>(OnIsSelectedChanged));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SelectionNotifyingMenuItem();
        }

        private static void OnIsSelectedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            var contextMenu = (SelectionNotifyingContextMenu)sender;
            var originalSourceMenuItem = e.OriginalSource as MenuItem;

            if (e.NewValue)
            {
                contextMenu.CurrentSelection = originalSourceMenuItem;
            }
            else if (contextMenu.CurrentSelection == originalSourceMenuItem)
            {
                contextMenu.CurrentSelection = null;
            }

            e.Handled = true; 
        }

        public MenuItem CurrentSelection
        {
            get { return (MenuItem)GetValue(CurrentSelectionProperty); }
            internal set { SetValue(CurrentSelectionProperty, value); }
        }
    }
}
