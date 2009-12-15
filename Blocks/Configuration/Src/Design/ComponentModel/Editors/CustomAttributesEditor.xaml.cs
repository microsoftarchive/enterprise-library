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
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Interaction logic for CustomAttributesEditor.xaml
    /// </summary>
    public partial class CustomAttributesEditor : UserControl
    {
        List<ChangeMonitor> changeMonitors = new List<ChangeMonitor>();
        ObservableCollection<KeyValueItem> list = new ObservableCollection<KeyValueItem>();
        BindableProperty property;

        public CustomAttributesEditor()
        {
            InitializeComponent();

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(CustomAttributesEditor_DataContextChanged);
        }

        void CustomAttributesEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            property = (BindableProperty)e.NewValue;
            CustomEditorBinder.BindProperty(this, property);
            
            NameValueCollection value = (NameValueCollection)property.Value;
            list = new ObservableCollection<KeyValueItem>();
            foreach (string key in value)
            {
                var item = new KeyValueItem { Key = key, Value = value[key] };
                item.DeleteCommand = new KeyValueItemDeleteCommand(this, item);
                ChangeMonitor monitor = new ChangeMonitor(this, item);
                changeMonitors.Add(monitor);
                list.Add(item);
            }

            AddNewItem();

            this.Items.ItemsSource = list;
        }

        internal void UpdateValue()
        {
            var items = list.Where(x => !x.IsEmpty);
            NameValueCollection value = (NameValueCollection)property.Value;

            foreach (string k in value.AllKeys.ToArray())
            {
                if (!items.Any(x => x.Key == k))
                {
                    value.Remove(k);
                }
            }

            foreach(var item in list.Where(x=>!x.IsEmpty))
            {
                value[item.Key] = item.Value;
            }
        }

        internal void AddNewItem()
        {
            var item = new KeyValueItem { IsNew = true };
            item.DeleteCommand = new KeyValueItemDeleteCommand(this, item);
            ChangeMonitor monitor = new ChangeMonitor(this, item);
            changeMonitors.Add(monitor);
            list.Add(item);
        }

        private class ChangeMonitor
        {
            CustomAttributesEditor editor;
            KeyValueItem item;
            
            public ChangeMonitor(CustomAttributesEditor editor, KeyValueItem item)
            {
                this.editor = editor;
                this.item = item;

                item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            }

            void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (item.IsNew && !item.IsEmpty)
                {
                    item.IsNew = false;
                    editor.AddNewItem();
                }
                editor.UpdateValue();
            }
        }

        private class KeyValueItemDeleteCommand : ICommand
        {
            CustomAttributesEditor editor;
            KeyValueItem item;
            
            public KeyValueItemDeleteCommand(CustomAttributesEditor editor, KeyValueItem item)
            {
                this.editor = editor;
                this.item = item;
            }
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                if (editor.list.Count < 2)
                {
                    item.Key = string.Empty;
                    item.Value = string.Empty;
                }
                else
                {
                    editor.list.Remove(item);
                    editor.UpdateValue();
                }
            }
        }

        private class KeyValueItem : INotifyPropertyChanged
        {
            string key;
            string value;

            public string Key
            {
                get{return key;}
                set
                {
                    key =value;
                    OnPropertyChanged("Key");
                }
            }

            public string Value
            {
                get{return value;}
                set
                {
                    this.value = value;
                    OnPropertyChanged("Value");
                }
            }

            public bool IsNew
            {
                get;
                set;
            }

            public bool IsEmpty
            {
                get { return string.IsNullOrEmpty(Key) && string.IsNullOrEmpty(Value); }
            }

            public ICommand DeleteCommand
            {
                get;
                set;
            }

            public virtual void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            public event PropertyChangedEventHandler  PropertyChanged;
        }

    }
}
