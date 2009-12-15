using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class ValidationModel : System.Windows.IWeakEventListener
    {
        private readonly ConfigurationSourceModel sourceModel;
        private readonly CompositeErrorsCollection errorsCollection = new CompositeErrorsCollection();
        private bool populating = false;

        public ValidationModel(ElementLookup lookup, ConfigurationSourceModel sourceModel)
        {
            this.sourceModel = sourceModel;
            this.sourceModel.Sections.CollectionChanged += SectionsChanged;
            ResetCollection();
            lookup.CollectionChanged += ElementsChanged;
        }

        private void SectionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ResetCollection();
        }

        private void ElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ResetCollection();
        }

        private void ResetCollection()
        {
            errorsCollection.NotifyEnabled = false;
            errorsCollection.Clear();
            PopulateCollection();
            errorsCollection.NotifyEnabled = true;
        }

        private void PopulateCollection()
        {
            populating = true;

            foreach (var section in sourceModel.Sections)
            {
                AddViewModelProperties(section);
            }

            populating = false;
        }

        private void AddViewModelProperties(ElementViewModel element)
        {
            foreach (var prop in element.Properties)
            {
                errorsCollection.Add(prop.ValidationErrors);
            }

            foreach (var child in element.ChildElements)
            {
                AddViewModelProperties(child);
            }

            CollectionChangedEventManager.AddListener(element.Properties, this);
        }

        public IEnumerable<ValidationError> ValidationErrors
        {
            get
            {
                return errorsCollection;
            }
        }

        #region IWeakEventListener
        bool System.Windows.IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(CollectionChangedEventManager))
            {
                if (populating) return true;
                var changeEvents = (NotifyCollectionChangedEventArgs)e;

                if (changeEvents.Action == NotifyCollectionChangedAction.Reset)
                {
                    ResetCollection();
                }
                else
                {
                    if (changeEvents.OldItems != null)
                    {
                        foreach (var oldItem in changeEvents.OldItems)
                        {
                            errorsCollection.Remove(((Property)oldItem).ValidationErrors);
                        }
                    }

                    if (changeEvents.NewItems != null)
                    {
                        foreach (var newItem in changeEvents.NewItems)
                        {
                            errorsCollection.Add(((Property)newItem).ValidationErrors);
                        }
                    }
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}