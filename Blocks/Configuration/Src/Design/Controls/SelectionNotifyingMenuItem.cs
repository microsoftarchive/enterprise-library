using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Console.Wpf.Controls
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