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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    internal class CompositeValidationResultsCollection : IEnumerable, IEnumerable<ValidationResult>, INotifyCollectionChanged
    {
        private readonly CompositeCollection validationErrorCompositeCollection = new CompositeCollection();
        private readonly Dictionary<IEnumerable<ValidationResult>, CollectionContainer> validationErrorsCollectionContainers = 
            new Dictionary<IEnumerable<ValidationResult>, CollectionContainer>();

        private readonly ICollectionView view;
        private readonly ViewWatchingEnumerable<ValidationResult> viewWatchingEnumerable;
            
        public CompositeValidationResultsCollection()
        {
            this.view = CreateView(); 
            viewWatchingEnumerable = new ViewWatchingEnumerable<ValidationResult>(view);
        }

        public bool NotifyEnabled
        {
            get
            {
                return viewWatchingEnumerable.NotifyEnabled;
            }
            set
            {
                viewWatchingEnumerable.NotifyEnabled = value;
            }
        }

        public void Clear()
        {
            validationErrorCompositeCollection.Clear();
            validationErrorsCollectionContainers.Clear();
        }

        public void Add(IEnumerable<ValidationResult> collection)
        {
            if (validationErrorsCollectionContainers.ContainsKey(collection)) return;

            var container = new CollectionContainer() { Collection = collection };
            validationErrorsCollectionContainers.Add(collection, container);
            validationErrorCompositeCollection.Add(container);
        }

        public void Remove(IEnumerable<ValidationResult> collection)
        {
            if (!validationErrorsCollectionContainers.ContainsKey(collection)) return;

            var container = validationErrorsCollectionContainers[collection];
            validationErrorsCollectionContainers.Remove(collection);
            validationErrorCompositeCollection.Remove(container);
        }

        private ICollectionView CreateView()
        {
            return ((ICollectionViewFactory)validationErrorCompositeCollection).CreateView();
        }

        public IEnumerator<ValidationResult> GetEnumerator()
        {
            return viewWatchingEnumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { ((INotifyCollectionChanged) viewWatchingEnumerable).CollectionChanged += value; }
            remove { ((INotifyCollectionChanged)viewWatchingEnumerable).CollectionChanged -= value; }
        }

        
        private class ViewWatchingEnumerable<T> : IEnumerable<T>, INotifyCollectionChanged
        {
            private readonly IEnumerable enumerator;
            private readonly INotifyCollectionChanged collectionNotify; 
            private bool notifyEnabled = true;
            private int lastCollectionCount;

            public ViewWatchingEnumerable(IEnumerable enumerator)
            {
                this.enumerator = enumerator;
                collectionNotify = enumerator as INotifyCollectionChanged;
                if (collectionNotify != null)
                {
                    collectionNotify.CollectionChanged += InnerCollectionChanged;
                }
            }

            public bool NotifyEnabled
            {
                get { return notifyEnabled; }
                set
                {
                    var oldValue = notifyEnabled;
                    notifyEnabled = value;

                    if (notifyEnabled == true && oldValue != notifyEnabled)
                    {
                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    }
                }
            }

            private void InnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                OnCollectionChanged(e);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new CastingEnumerator<T>(enumerator.GetEnumerator());
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class CastingEnumerator<TItem> : IEnumerator<TItem>
            {
                [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
                private readonly IEnumerator _enumerator;

                public CastingEnumerator(IEnumerator enumerator)
                {
                    _enumerator = enumerator;
                }

                protected virtual void Dispose(bool disposing)
                {
                    var disposable = _enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }    
                }

                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }

                public bool MoveNext()
                {
                    return _enumerator.MoveNext();
                }

                public void Reset()
                {
                    _enumerator.Reset();
                }

                public TItem Current
                {
                    get { return (TItem)_enumerator.Current; }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged;

            protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            {
                if (ShouldNotify())
                {
                    NotifyCollectionChangedEventHandler changed = CollectionChanged;
                    if (changed != null) changed(this, e);
                }
            }

            private bool ShouldNotify()
            {
                if (!notifyEnabled) return false;

                var count = enumerator.OfType<T>().Count();
                var shouldNotify = (count != lastCollectionCount || lastCollectionCount != 0);
                lastCollectionCount = count;

                return shouldNotify;
            }
        }
    }
}
