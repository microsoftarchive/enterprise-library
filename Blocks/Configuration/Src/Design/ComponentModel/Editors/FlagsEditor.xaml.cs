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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Interaction logic for FlagsEditor.xaml
    /// </summary>
    public partial class FlagsEditor : UserControl
    {
        public FlagsEditor()
        {
            InitializeComponent();

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(FlagsEditor_DataContextChanged);
        }


        ListItemChangedWatcher itemChangedWatcher;
        List<FlagsEditorItem> items;
        Property property; 

        void FlagsEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            property = e.NewValue as Property;
            if (property != null)
            {
                if (!property.Type.IsEnum) throw new InvalidOperationException("Enum editor can only be used with enum properties");

                int value = Convert.ToInt32( property.Value );

                items = Enum.GetValues(property.Type)
                            .OfType<Enum>()
                            .Select(x=> Convert.ToInt32(x))
                            .Where(x => x != 0)
                            .Select(x => new FlagsEditorItem
                                {
                                    Selected = 0 < (x & value),
                                    Value = Enum.GetName(property.Type, x)
                                }).ToList();

                itemChangedWatcher = new ListItemChangedWatcher(items);
                itemChangedWatcher.Changed += new EventHandler(itemChangedWatcher_Changed);

                this.Flags.ItemsSource = items;
            }
        }

        void itemChangedWatcher_Changed(object sender, EventArgs e)
        {
            RefreshValue();

        }

        private void RefreshValue()
        {
            int value = 0;
            foreach (var item in items.Where(x=>x.Selected))
            {
                var valuePart = Enum.Parse(property.Type, item.Value);
                value += Convert.ToInt32(valuePart);
            }

            property.Value = value;
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

        public class FlagsEditorItem : INotifyPropertyChanged
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
