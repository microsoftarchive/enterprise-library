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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using LocalizationResources = Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties.Resources;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
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
        NewCollectionElementCommandImplementation newCollectionElementCommand;

        Button newButton;
        string headerTemplateKey;
        string itemTemplateKey;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            items = Template.FindName("PART_ItemsControl", this) as ItemsControl;
            newButton = Template.FindName("PART_NewButton", this) as Button;
            header = Template.FindName("PART_Header", this) as ContentControl;            
            newButton.Command = NewCollectionElementCommand;

            items.ItemsSource = collection.ChildElements;
            
            items.ItemTemplate = FindResource(itemTemplateKey) as DataTemplate;
            header.ContentTemplate = FindResource(headerTemplateKey) as DataTemplate;
        }

        public ICommand NewCollectionElementCommand
        {
            get { return newCollectionElementCommand; }
        }

        public CollectionElementEditor()
        {
            this.DataContextChanged += CustomAttributesEditor_DataContextChanged;
        }

        void CustomAttributesEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var property = (ElementProperty)e.NewValue;

            collection = (ElementCollectionViewModel) property.DeclaringElement.ChildElements
                                                          .Single(x => x.DeclaringProperty == property.DeclaringProperty);

            CollectionEditorTemplateAttribute templateAttribute = property.Attributes.OfType<CollectionEditorTemplateAttribute>().FirstOrDefault();
            if (templateAttribute == null) throw new InvalidOperationException(
                string.Format(LocalizationResources.Culture, 
                              LocalizationResources.ExceptionCollectionElementEditorNeedsTemplateAttribute,
                              property.DisplayName));

            headerTemplateKey = templateAttribute.HeaderTemplate;
            itemTemplateKey = templateAttribute.ItemTemplate;

            newCollectionElementCommand = new NewCollectionElementCommandImplementation(collection);
        }

        private class NewCollectionElementCommandImplementation : ICommand
        {
            ElementCollectionViewModel collection;
            public NewCollectionElementCommandImplementation(ElementCollectionViewModel collection)
            {
                this.collection = collection;
            }
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                collection.AddNewCollectionElement(collection.CollectionElementType);
            }
        }
    }
}
