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
    /// <summary>
    /// The <see cref="SelectionNotifyingContextMenu"/> tracks the currently
    /// selected <see cref="SelectionNotifyingMenuItem"/> as the user navigates
    /// through the menu.
    /// <br/>
    /// This is used by the design-time infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public class SelectionNotifyingContextMenu : ContextMenu
    {
        internal static readonly RoutedEvent IsSelectedChangedEvent =
            EventManager.RegisterRoutedEvent("IsSelectedChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<bool>),
                typeof(SelectionNotifyingContextMenu));

        ///<summary>
        /// The currently selected <see cref="MenuItem"/>.
        ///</summary>
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

        /// <summary>
        /// Creates or identifies the element used to display the specified item.
        /// </summary>
        /// <returns>
        /// A <see cref="SelectionNotifyingMenuItem"/>.
        /// </returns>
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

        ///<summary>
        /// Gets the currently selected <see cref="MenuItem"/> from <see cref="CurrentSelectionProperty"/>.
        ///</summary>
        public MenuItem CurrentSelection
        {
            get { return (MenuItem)GetValue(CurrentSelectionProperty); }
            internal set { SetValue(CurrentSelectionProperty, value); }
        }
    }
}
