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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Interaction logic for ElementCollectionEditor.xaml
    /// </summary>
    public partial class ElementCollectionEditor : UserControl
    {
        ElementCollectionViewModel viewModel;
        public ElementCollectionEditor()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            BindableProperty property = DataContext as BindableProperty;
            if (property == null) return;

            CustomEditorBinder.BindProperty(this, property);

            ElementProperty elementProperty = property.Property as ElementProperty;
            if (elementProperty != null)
            {
                viewModel = elementProperty.DeclaringElement.ChildElement(elementProperty.DeclaringProperty.Name) as ElementCollectionViewModel;
            }

            if (viewModel != null)
            {
                var properties = TypeDescriptor.GetProperties(viewModel.CollectionElementType).OfType<PropertyDescriptor>().Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any()).ToArray();

                foreach (var childProperty in properties.Where(x => x.IsBrowsable))
                {
                    Collection.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = new GridLength(75, GridUnitType.Star)
                    });
                }

                Collection.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(25)
                });

                Redraw();

                viewModel.ChildElements.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ChildElements_CollectionChanged);
            }
        }

        void ChildElements_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Collection.Children.Clear();
            Collection.RowDefinitions.Clear();

            Redraw();
        }

        private void Redraw()
        {
            var properties = TypeDescriptor.GetProperties(viewModel.CollectionElementType).OfType<PropertyDescriptor>().Where(x => x.Attributes.OfType<ConfigurationPropertyAttribute>().Any()).ToArray();

            for (int n = 0; n <= viewModel.ChildElements.Count + 1; n++)
            {
                Collection.RowDefinitions.Add(new RowDefinition());
            }

            int i = 0;
            foreach (var property in properties)
            {
                var label = new Label() { Content = property.DisplayName };
                Collection.Children.Add(label);
                label.SetValue(Grid.RowProperty, 0);
                label.SetValue(Grid.ColumnProperty, i);


                var gridSplitter = new GridSplitter() { Width = 2, HorizontalAlignment = HorizontalAlignment.Right };
                Collection.Children.Add(gridSplitter);
                gridSplitter.SetValue(Grid.RowProperty, 0);
                gridSplitter.SetValue(Grid.ColumnProperty, i);
                gridSplitter.SetValue(Grid.RowSpanProperty, viewModel.ChildElements.Count + 1);
                i++;
            }

            ContextMenuButton addButton = new ContextMenuButton();
            Collection.Children.Add(addButton);
            addButton.Command = viewModel.AddCommands.First();
            addButton.SetValue(Grid.RowProperty, 0);
            addButton.SetValue(Grid.ColumnProperty, i);
            addButton.Style = FindResource("ContextAdderButtonMenuStyle") as Style;
            addButton.VerticalAlignment = VerticalAlignment.Center;



            int j = 1;
            foreach (var element in viewModel.ChildElements)
            {
                i = 0;
                foreach (var propertyDescriptor in properties)
                {
                    var property = element.Property(propertyDescriptor.Name);

                    ContentControl contentControl = new ContentControl();
                    Collection.Children.Add(contentControl);
                    contentControl.SetValue(ContentControl.ContentProperty, property);
                    contentControl.SetValue(Grid.RowProperty, j);
                    contentControl.SetValue(Grid.ColumnProperty, i);

                    i++;
                }

                Button deleteButton = new Button();
                Collection.Children.Add(deleteButton);
                deleteButton.Command = element.Commands.Where(x => x.Placement == CommandPlacement.ContextDelete).First();
                deleteButton.SetValue(Grid.RowProperty, j);
                deleteButton.SetValue(Grid.ColumnProperty, i);
                deleteButton.Style = FindResource("DeleteButtonStyle") as Style;
                deleteButton.VerticalAlignment = VerticalAlignment.Center;

                j++;
            }
        }
    }
}
