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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="ElementReference"/> represents a reference to a <see cref="ElementViewModel"/> instance.<br/>
    /// The referred element will resolve once it becomes available in the <see cref="ElementLookup"/>. <br/>
    /// Once the referred element is resolved, a <see cref="ElementFound"/> event is triggered. <br/>
    /// An instance of <see cref="ElementReference"/> provides basic facilities to keep track of changes to the referred element.<br/>
    /// </summary>
    /// <remarks>
    /// Instances of <see cref="ElementReference"/> can be created by calling the <see cref="ElementLookup.CreateReference(string)"/> or <see cref="ElementLookup.CreateReference(string, Type, string)"/> method.<br/>
    /// </remarks>
    /// <seealso cref="ElementLookup.CreateReference(string)"/>
    /// <seealso cref="ElementLookup.CreateReference(string, Type, string)"/>
    public abstract class ElementReference : IDisposable
    {
        ElementViewModel element;
        PropertyChangedEventHandler elementPropertyChanged;
        EventHandler elementDeleted;

        /// <summary>
        /// Initializes a new instance of <see cref="ElementReference"/>.
        /// </summary>
        /// <param name="element">The elment to monitor.</param>
        protected ElementReference(ElementViewModel element)
        {
            this.element = element;
            this.elementPropertyChanged = new PropertyChangedEventHandler(element_PropertyChanged);
            this.elementDeleted = new EventHandler(element_Deleted);
            
            if (element != null)
            {
                InitializeChangeEvents(element);
            }
        }

        /// <summary>
        /// Gets the referred <see cref="ElementViewModel"/> instance.
        /// </summary>
        /// <value>
        /// If the referred <see cref="ElementViewModel"/> instance is available, returns the referred element.<br/>
        /// Otherwise returns <see langword="null"/>.
        /// </value>
        public ElementViewModel Element
        {
            get { return element; }
        }

        /// <summary>
        /// Occurs when the referenced element was resolved. <br/>
        /// </summary>
        public event EventHandler ElementFound;

        /// <summary>
        /// Raises the <see cref="ElementFound"/> event.
        /// </summary>
        /// <param name="element">The discovered <see cref="ElementViewModel"/></param>
        protected virtual void OnElementFound(ElementViewModel element)
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
            element.PropertyChanged += elementPropertyChanged;
            element.Deleted += elementDeleted;
        }

        void element_Deleted(object sender, EventArgs e)
        {
            OnElementDeleted();
        }

        void element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Path")
            {
                var handler = PathChanged;
                if (handler != null)
                {
                    handler(sender, new PropertyValueChangedEventArgs<string>(((ElementViewModel)sender).Path));
                }
            }

            if (e.PropertyName == "Name")
            {
                var handler = NameChanged;
                if (handler != null)
                {
                    handler(sender, new PropertyValueChangedEventArgs<string>(((ElementViewModel)sender).Name));
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="ElementDeleted"/> event.
        /// </summary>
        protected virtual void OnElementDeleted()
        {
            element.PropertyChanged -= elementPropertyChanged;
            element.Deleted -= elementDeleted;

            this.element = null;

            var handler = ElementDeleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the referenced element is deleted. <br/>
        /// </summary>
        public event EventHandler ElementDeleted;

        /// <summary>
        /// Occurs when the <see cref="ElementViewModel.Name"/> of the referenced element changed. <br/>
        /// </summary>
        public event PropertyValueChangedEventHandler<string> NameChanged;


        /// <summary>
        /// Occurs when the <see cref="ElementViewModel.Path"/> of the referenced element changed. <br/>
        /// </summary>
        public event PropertyValueChangedEventHandler<string> PathChanged;

        /// <summary>
        /// Indicates the object is being disposed.
        /// </summary>
        /// <param name="disposing">Indicates <see cref="Dispose(bool)"/> was invoked through an explicit call to <see cref="Dispose()"/> instead of a finalizer call.</param>
        /// <filterpriority>2</filterpriority>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (element != null)
                {
                    element.PropertyChanged -= elementPropertyChanged;
                    element.Deleted -= elementDeleted;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Defines a property changed delegate over a specific type.
    /// </summary>
    /// <typeparam name="T">The property type for the property changed handler.</typeparam>
    /// <param name="sender">The invoker of the property changed value.</param>
    /// <param name="e">The property value changed event arguments.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances")]
    public delegate void PropertyValueChangedEventHandler<T>(object sender, PropertyValueChangedEventArgs<T> e);

    /// <summary>
    /// The event arguments for <see cref="PropertyValueChangedEventHandler{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type for the property value.</typeparam>
    public class PropertyValueChangedEventArgs<T> : EventArgs
    {
        readonly T value;

        /// <summary>
        /// Initializes a new instance of <see cref="PropertyValueChangedEventArgs{T}"/>.
        /// </summary>
        /// <param name="value">The propety value.</param>
        public PropertyValueChangedEventArgs(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the value of the property changed.
        /// </summary>
        public T Value { get { return value; } }
    }
}
