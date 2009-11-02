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
using Console.Wpf.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.Controls
{
    [TemplatePart(Name = "PART_ItemsControl", Type = typeof(ItemsControl))]
    [TemplatePart(Name = "PART_NewButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Header", Type = typeof(ContentControl))]
    public class CollectionElementEditor : Control
    {
        static CollectionElementEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CollectionElementEditor), new FrameworkPropertyMetadata(typeof(CollectionElementEditor)));
        }

        ItemsControl items;
        ElementCollectionViewModel collection;
        ContentControl header;

        Button newButton;
        string headerTemplateKey;
        string itemTemplateKey;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            items = Template.FindName("PART_ItemsControl", this) as ItemsControl;
            newButton = Template.FindName("PART_NewButton", this) as Button;
            header = Template.FindName("PART_Header", this) as ContentControl;

            newButton.Click += new RoutedEventHandler(newButton_Click);
            items.ItemsSource = collection.ChildElements;
            
            
            
            items.ItemTemplate = FindResource(itemTemplateKey) as DataTemplate;
            header.ContentTemplate = FindResource(headerTemplateKey) as DataTemplate;

        }

        void newButton_Click(object sender, RoutedEventArgs e)
        {
            collection.AddNewCollectionElement(collection.CollectionElementType);
        }

        public CollectionElementEditor()
        {
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(CustomAttributesEditor_DataContextChanged);
        }

        void CustomAttributesEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var property = (ElementProperty)e.NewValue;
            var containingSection = property.ContainingSection;
            collection = containingSection.CreateElementCollection(property.DeclaringElement, property.DeclaringProperty);

            CollectionEditorTemplateAttribute templateAttribute = property.Attributes.OfType<CollectionEditorTemplateAttribute>().FirstOrDefault();
            headerTemplateKey = templateAttribute.HeaderTemplate;
            itemTemplateKey = templateAttribute.ItemTemplate;
        }
    }
}
