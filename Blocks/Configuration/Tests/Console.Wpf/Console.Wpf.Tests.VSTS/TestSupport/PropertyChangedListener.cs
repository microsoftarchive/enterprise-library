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
using System.ComponentModel;

namespace Console.Wpf.Tests.VSTS.TestSupport
{
    public  class PropertyChangedListener : IDisposable
    {
        INotifyPropertyChanged subject;
        public List<string> ChangedProperties = new List<string>();
        PropertyChangedEventHandler handler;

        public PropertyChangedListener(INotifyPropertyChanged subject)
        {
            this.subject = subject;
            subject.PropertyChanged += handler = new PropertyChangedEventHandler(subject_PropertyChanged);
        }

        void subject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ChangedProperties.Add(e.PropertyName);
        }

        void IDisposable.Dispose()
        {
            subject.PropertyChanged -= handler;
        }
    }
}
