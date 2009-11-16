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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    public class SelectionNotifyingMenuItem : MenuItem
    {
        internal static DependencyProperty IsSelectedProperty =
            Selector.IsSelectedProperty.AddOwner(
                typeof(SelectionNotifyingMenuItem),
                new FrameworkPropertyMetadata(false,
                                              FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                              new PropertyChangedCallback(SelectionNotifyingMenuItem.OnIsSelectedChanged)));

        public SelectionNotifyingMenuItem()
        {
            
        }
        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = (SelectionNotifyingMenuItem)d;
            item.RaiseEvent(new RoutedPropertyChangedEventArgs<bool>((bool)e.OldValue, (bool)e.NewValue,
                                                                     SelectionNotifyingContextMenu.IsSelectedChangedEvent));
        }
    }
}
