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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;


namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    /// <summary>
    /// The <see cref="SelectionNotifyingMenuItem"/> tracks child <see cref="SelectionNotifyingMenuItem"/>s
    /// and provides notification to its parent if it becomes selected.
    /// <br/>
    /// This is used by the design-time infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public class SelectionNotifyingMenuItem : MenuItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal static DependencyProperty IsSelectedProperty =
            Selector.IsSelectedProperty.AddOwner(
                typeof(SelectionNotifyingMenuItem),
                new FrameworkPropertyMetadata(false,
                                              FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                              new PropertyChangedCallback(SelectionNotifyingMenuItem.OnIsSelectedChanged)));

        ///<summary>
        /// The currently selected <see cref="MenuItem"/>.
        ///</summary>
        public static readonly DependencyProperty CurrentSelectionProperty =
            DependencyProperty.Register("CurrentSelection", typeof(MenuItem), typeof(SelectionNotifyingMenuItem), new UIPropertyMetadata(null));

        static SelectionNotifyingMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectionNotifyingMenuItem), new FrameworkPropertyMetadata(typeof(SelectionNotifyingMenuItem)));

            EventManager.RegisterClassHandler(
             typeof(SelectionNotifyingMenuItem),
             SelectionNotifyingContextMenu.IsSelectedChangedEvent,
             new RoutedPropertyChangedEventHandler<bool>(OnIsSelectedChanged));
        }

        ///<summary>
        /// Initializes a new instance of <see cref="SelectionNotifyingMenuItem"/>.
        ///</summary>
        public SelectionNotifyingMenuItem()
        {
            Binding inputGestureBinding = new Binding("KeyGesture");
            base.SetBinding(MenuItem.InputGestureTextProperty, inputGestureBinding);
        }

        private static void OnIsSelectedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (sender == e.OriginalSource) return;

            var parentMenu = (SelectionNotifyingMenuItem)sender;
            var originalSourceMenuItem = e.OriginalSource as SelectionNotifyingMenuItem;

            if (originalSourceMenuItem == null) return;

            if (e.NewValue)
            {
                if (parentMenu.CurrentSelection != originalSourceMenuItem &&
                    originalSourceMenuItem.LogicalParent == parentMenu)
                {
                    parentMenu.CurrentSelection = originalSourceMenuItem;
                }
            }
            else if (parentMenu.CurrentSelection == originalSourceMenuItem)
            {
                parentMenu.CurrentSelection = null;
            }

            e.Handled = true;
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = (SelectionNotifyingMenuItem)d;
            item.RaiseEvent(new RoutedPropertyChangedEventArgs<bool>((bool)e.OldValue, (bool)e.NewValue,
                                                                     SelectionNotifyingContextMenu.IsSelectedChangedEvent));
        }

        /// <summary>
        /// Creates or identifies the element used to display a specified item.
        /// </summary>
        /// <returns>
        /// A <see cref="SelectionNotifyingMenuItem"/>.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new SelectionNotifyingMenuItem();
        }

        ///<summary>
        /// Gets the currently selected child <see cref="MenuItem"/>.
        ///</summary>
        public MenuItem CurrentSelection
        {
            get { return (MenuItem)GetValue(CurrentSelectionProperty); }
            internal set { SetValue(CurrentSelectionProperty, value); }
        }

        private object LogicalParent
        {
            get
            {
                if (Parent != null)
                {
                    return Parent;
                }
                return ItemsControlFromItemContainer(this);
            }
        }
    }
}
