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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    /// <summary>
    /// Interaction logic for OverridenTraceListenerCollectionEditor.xaml
    /// </summary>
    public partial class OverridenTraceListenerCollectionEditor : UserControl, IEnvironmentalOverridesEditor
    {
        EnvironmentalOverridesViewModel environment;

        public OverridenTraceListenerCollectionEditor()
        {
            InitializeComponent();

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(OverridenTraceListenerCollectionEditor_DataContextChanged);
        }

        void OverridenTraceListenerCollectionEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var property = (EnvironmentalOverridesViewModel.EnvironmentOverriddenProperty)((BindableProperty)e.NewValue).Property;
            
            var collection = (ElementCollectionViewModel)property.OriginalPropertyViewModel.ChildElements
                                                          .Single(x => x.DeclaringProperty == property.DeclaringProperty);

            this.Items.ItemsSource = collection.ChildElements.OfType<TraceListenerReferenceViewModel>().Select(x => new EnvironmentalTraceListenerReferenceDataViewModel(x, environment));
        }

        public void Initialize(EnvironmentalOverridesViewModel environment)
        {
            this.environment = environment;
        }

        private class EnvironmentalTraceListenerReferenceDataViewModel
        {
            TraceListenerReferenceViewModel original;
            EnvironmentalOverridesViewModel environment;
            Property traceSourceOverridesProperty;
            Property overridesProperty;

            public EnvironmentalTraceListenerReferenceDataViewModel(TraceListenerReferenceViewModel original, EnvironmentalOverridesViewModel environment)
            {
                this.original = original;
                this.environment = environment;
                
                ElementViewModel traceSource = this.original.AncesterElements().Where(x => typeof(TraceSourceData).IsAssignableFrom(x.ConfigurationType)).First();
                traceSourceOverridesProperty = traceSource.Properties.OfType<EnvironmentalOverridesViewModel.OverridesProperty>().Where(x => x.Environment == environment).FirstOrDefault();
                traceSourceOverridesProperty.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(overridesProperty_PropertyChanged);

                overridesProperty = this.original.Properties.OfType<EnvironmentalOverridesViewModel.OverridesProperty>().Where(x => x.Environment == environment).FirstOrDefault();
            }

            void overridesProperty_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Value")
                {
                    overridesProperty.Value = traceSourceOverridesProperty.Value;
                }
            }

            public Property NameProperty
            {
                get { return original.GetNameProperty(environment); }
            }
        }
    }
}
