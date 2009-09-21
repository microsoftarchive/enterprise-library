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
using Console.Wpf.ViewModel.Services;
using System.ComponentModel;
using Console.Wpf.ViewModel;

namespace Console.Wpf
{
    public abstract class ElementReference : IDisposable
    {
        ElementViewModel element;

        protected ElementReference(ElementViewModel element)
        {
            this.element = element;
            if (element != null)
            {
                InitializeChangeEvents(element);
            }
        }

        public ElementViewModel Element
        {
            get { return element; }
        }

        protected virtual void DoElementFound(ElementViewModel element)
        {
            this.element = element;

            InitializeChangeEvents(element);

            var handler = ElementFound;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void InitializeChangeEvents(ElementViewModel element)
        {
            element.PropertyChanged += new PropertyChangedEventHandler(element_PropertyChanged);
            element.Deleted += new EventHandler(element_Deleted);
        }

        void element_Deleted(object sender, EventArgs e)
        {
            DoElementDeleted();
        }

        void element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Path")
            {
                var handler = PathChanged;
                if (handler != null)
                {
                    handler(sender, e);
                }
            }

            if (e.PropertyName == "Name")
            {
                var handler = NameChanged;
                if (handler != null)
                {
                    handler(sender, e);
                }
            }
        }

        public event EventHandler ElementFound;

        protected virtual void DoElementDeleted()
        {
            this.element = null;

            var handler = ElementDeleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler ElementDeleted;

        //TODO: create custom EventHandler for name change
        public event PropertyChangedEventHandler NameChanged;

        //TODO: create custom EventHandler for path change
        public event PropertyChangedEventHandler PathChanged;

        public virtual void Dispose(bool disposing)
        {

        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
