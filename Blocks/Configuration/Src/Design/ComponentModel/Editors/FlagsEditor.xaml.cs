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
using System.Collections.ObjectModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.ComponentModel;
using System.Collections;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Interaction logic for FlagsEditor.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
    public partial class FlagsEditor : UserControl
    {
        ///<summary>
        /// Initializes a new instance of <see cref="FlagsEditor"/>.
        ///</summary>
        public FlagsEditor()
        {
            InitializeComponent();

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(FlagsEditor_DataContextChanged);
        }

        bool updatingSource;
        ListItemChangedWatcher itemChangedWatcher;
        List<FlagsEditorItem> items;
        Property property; 

        void FlagsEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BindableProperty bindableProperty = DataContext as BindableProperty;
            if (bindableProperty == null) return;
            CustomEditorBinder.BindProperty(this, bindableProperty);
            
            property = bindableProperty.Property;
            if (property != null)
            {
                if (!property.PropertyType.IsEnum) throw new InvalidOperationException(Properties.Resources.EnumEditorExceptionInvalidPropertyType);

                RefreshVisual(property);

                property.PropertyChanged += new PropertyChangedEventHandler(property_PropertyChanged);
            }
        }

        void property_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                RefreshVisual(property);
            }
        }

        private void RefreshVisual(Property property)
        {
            if (updatingSource) return;

            int value = Convert.ToInt32(property.Value, CultureInfo.CurrentCulture);

            items = Enum.GetValues(property.PropertyType)
                        .OfType<Enum>()
                        .Select(x => Convert.ToInt32(x, CultureInfo.CurrentCulture))
                        .Where(x => x != 0)
                        .Select(x => new FlagsEditorItem
                        {
                            Selected = 0 < (x & value),
                            Value = Enum.GetName(property.PropertyType, x)
                        }).ToList();

            itemChangedWatcher = new ListItemChangedWatcher(items);
            itemChangedWatcher.Changed += new EventHandler(itemChangedWatcher_Changed);

            this.Flags.ItemsSource = items;
        }

        void itemChangedWatcher_Changed(object sender, EventArgs e)
        {
            updatingSource = true;
            try
            {
                RefreshValue();
            }
            finally
            {
                updatingSource = false;
            }
        }

        private void RefreshValue()
        {
            int value = 0;
            foreach (var item in items.Where(x=>x.Selected))
            {
                var valuePart = Enum.Parse(property.PropertyType, item.Value);
                value += Convert.ToInt32(valuePart, CultureInfo.CurrentCulture);
            }

            property.Value = Enum.ToObject(property.PropertyType, value);
        }


        private class ListItemChangedWatcher
        {
            public ListItemChangedWatcher(IEnumerable list)
            {
                foreach (var item in list)
                {
                    INotifyPropertyChanged itemChanged = item as INotifyPropertyChanged;
                    if (itemChanged != null)
                    {
                        itemChanged.PropertyChanged += new PropertyChangedEventHandler(itemChanged_PropertyChanged);
                    }
                }
            }

            void itemChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                var handler = Changed;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }


            public event EventHandler Changed;
        }

        class FlagsEditorItem : INotifyPropertyChanged
        {
            private string value;
            private bool selected;

            public string Value
            {
                get{return value;}
                set{this.value = value;}
            }

            public bool Selected
            {
                get{return selected;}
                set
                {
                    selected =value;
                    OnPropertyChanged("Selected");
                }
            }

            protected virtual void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }

}
