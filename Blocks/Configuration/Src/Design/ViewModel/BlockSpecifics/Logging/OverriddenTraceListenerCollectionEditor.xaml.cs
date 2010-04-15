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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// Interaction logic for OverriddenTraceListenerCollectionEditor.xaml
    /// </summary>
    public partial class OverriddenTraceListenerCollectionEditor : UserControl, IEnvironmentalOverridesEditor
    {
        EnvironmentSourceViewModel environment;
        ElementCollectionViewModel collection;

        public OverriddenTraceListenerCollectionEditor()
        {
            InitializeComponent();

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(OverridenTraceListenerCollectionEditor_DataContextChanged);
        }

        void OverridenTraceListenerCollectionEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var property = (EnvironmentOverriddenProperty)((BindableProperty)e.NewValue).Property;
            var originalProperty = (ElementProperty)property.OriginalProperty;
            collection = (ElementCollectionViewModel)originalProperty.DeclaringElement.ChildElements
                                                          .Single(x => x.DeclaringProperty == property.DeclaringProperty);

            BindItems(collection);
            ((INotifyCollectionChanged)collection.ChildElements).CollectionChanged += CollectionChanged;
        }

        private void BindItems(ElementCollectionViewModel collection)
        {
            this.Items.ItemsSource = collection.ChildElements.OfType<TraceListenerReferenceViewModel>().Select(x => new EnvironmentalTraceListenerReferenceDataViewModel(x, environment));
        }

        void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BindItems(collection);
        }

        public void Initialize(EnvironmentSourceViewModel environment)
        {
            this.environment = environment;
        }

        private class EnvironmentalTraceListenerReferenceDataViewModel
        {
            readonly TraceListenerReferenceViewModel original;
            readonly EnvironmentSourceViewModel environment;

            public EnvironmentalTraceListenerReferenceDataViewModel(TraceListenerReferenceViewModel original, EnvironmentSourceViewModel environment)
            {
                this.original = original;
                this.environment = environment;
            }

            public Property NameProperty
            {
                get { return original.GetNameProperty(environment); }
            }
        }
    }
#pragma warning restore 1591
}
