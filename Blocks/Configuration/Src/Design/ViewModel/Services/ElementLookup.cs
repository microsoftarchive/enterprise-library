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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Collections.Specialized;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    public class ElementLookup : INotifyCollectionChanged
    {
        ConfigurationSourceDependency sourceModelDependency;

        CompositeCollection innerCollection;
        INotifyCollectionChanged innerCollectionChanged;
        IEnumerable<ElementViewModel> allElements;

        Dictionary<ElementViewModel, TrackChildElementCreationAndRemoval> elements = new Dictionary<ElementViewModel, TrackChildElementCreationAndRemoval>();
        Dictionary<ElementViewModel, TrackPathPropertyChangedAndUpdateReferences> elementPathTrackers = new Dictionary<ElementViewModel, TrackPathPropertyChangedAndUpdateReferences>();
        List<ElementReferenceImplementationBase> references = new List<ElementReferenceImplementationBase>();
        Dictionary<ElementViewModel, CollectionContainer> elementCollectionContainers = new Dictionary<ElementViewModel, CollectionContainer>();
        private bool refreshing;

        //TODO : Should depend on configurationSourceModel.
        ObservableCollection<SectionViewModel> sections = new ObservableCollection<SectionViewModel>();
        ObservableCollection<ElementViewModel> customElementViewModels = new ObservableCollection<ElementViewModel>();

        public ElementLookup(ConfigurationSourceDependency sourceModelDependency)
        {
            this.sourceModelDependency = sourceModelDependency;
            this.sourceModelDependency.Cleared += new EventHandler(sourceModelDependency_Refresh);

            innerCollection = new CompositeCollection();
            innerCollection.Add(new CollectionContainer { Collection = sections });
            innerCollection.Add(new CollectionContainer { Collection = customElementViewModels });

            var view = ((ICollectionViewFactory)innerCollection).CreateView();
            innerCollectionChanged = view;

            allElements = view.OfType<ElementViewModel>();

            innerCollectionChanged.CollectionChanged += (sender, args) =>
                                                            {
                                                                if (!refreshing)
                                                                {
                                                                    OnCollectionChanged(args);
                                                                }
                                                            };
        }

        void sourceModelDependency_Refresh(object sender, EventArgs e)
        {
            refreshing = true;
            elements.Clear();
            elementPathTrackers.Clear();
            references.Clear();
            sections.Clear();
            customElementViewModels.Clear();

            innerCollection.Clear();
            innerCollection.Add(new CollectionContainer { Collection = sections });
            innerCollection.Add(new CollectionContainer { Collection = customElementViewModels });
            
            refreshing = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(Type scope, Type configurationType)
        {
            var scopeElements = FindInstancesOfConfigurationType(scope);
            return FindInstancesOfConfigurationType(scopeElements.SelectMany(x=>x.DescendentElements()), configurationType);
        }

        public IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(Type configurationType)
        {
            return FindInstancesOfConfigurationType(allElements, configurationType);
        }

        private IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(IEnumerable<ElementViewModel> elements, Type configurationType)
        {
            var a = elements.Where(x => configurationType.IsAssignableFrom(x.ConfigurationType)).ToList();
            return a;
        }

        public void AddCustomElement(ElementViewModel element)
        {
            customElementViewModels.Add(element);
        }

        public void AddSection(SectionViewModel sectionModel)
        {
            sections.Add(sectionModel);
            
            AddElement(sectionModel);
        }


        public void RemoveSection(SectionViewModel sectionViewModel)
        {
            sections.Remove(sectionViewModel);

            RemoveElement(sectionViewModel);
            
        }

        private void RemoveChildElements(ElementViewModel element)
        {
            foreach (ElementViewModel childElement in element.ChildElements)
            {
                RemoveElement(childElement);

                RemoveChildElements(childElement);
            }
        }

        private void RemoveElement(ElementViewModel element)
        {
            CollectionContainer collectionContainer;
            if (elementCollectionContainers.TryGetValue(element, out collectionContainer))
            {
                innerCollection.Remove(collectionContainer);
            }
            elements.Remove(element);
            elementPathTrackers.Remove(element);

            RemoveChildElements(element);
        }

        private void AddElement(ElementViewModel element)
        {
            CollectionContainer collectionContainer = new CollectionContainer() { Collection = element.ChildElements };
            elementCollectionContainers.Add(element, collectionContainer);
            elements.Add(element, new TrackChildElementCreationAndRemoval(this, element));
            elementPathTrackers.Add(element, new TrackPathPropertyChangedAndUpdateReferences(this, element));
            innerCollection.Add(collectionContainer);
        }

        public IElementChangeScope FindExtendedPropertyProviders()
        {
            return new ElementChangeScope(this, x=>x is IElementExtendedPropertyProvider);
        }

        public ElementReference CreateReference(string ancestorPath, Type elementType, string elementName)
        {
            ElementViewModel element = null;
            ElementViewModel parentElement = elements.Keys.Where(x => x.Path == ancestorPath).FirstOrDefault();
            if (parentElement != null) element = parentElement.DescendentElements(x => x.Name == elementName && elementType.IsAssignableFrom(x.ConfigurationType)).FirstOrDefault();

            var reference = new ElementReferenceOverAncestorPath(this, element, ancestorPath, elementName);

            references.Add(reference);
            return reference;

        }

        public ElementReference CreateReference(string elementPath)
        {
            ElementViewModel element = elements.Keys.Where(x => x.Path == elementPath).FirstOrDefault();
            ElementReferenceOverAbsolutePath reference = new ElementReferenceOverAbsolutePath(this, element, elementPath);

            references.Add(reference);
            return reference;
        }

        public IElementChangeScope CreateChangeScope(Func<ElementViewModel, bool> predicate)
        {
            return new ElementChangeScope(this, predicate);
        }

        #region INotifyCollectionChanged implementation

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion


        private abstract class ElementReferenceImplementationBase : ElementReference
        {
            readonly ElementLookup lookup;
            readonly NotifyCollectionChangedEventHandler lookupChangedHandler;

            public ElementReferenceImplementationBase(ElementLookup lookup, ElementViewModel element)
                :base(element)
            {
                this.lookup = lookup;
                this.lookupChangedHandler = new NotifyCollectionChangedEventHandler(lookup_CollectionChanged);
                this.lookup.CollectionChanged += lookupChangedHandler;
            }

            void lookup_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:

                        if (base.Element != null) return;

                        FindMatchingeElement(e.NewItems.OfType<ElementViewModel>());

                        break;

                    case NotifyCollectionChangedAction.Reset:

                        if (base.Element != null) return;

                        FindMatchingeElement(lookup.allElements);
                        
                        break;

                }
            }

            private void FindMatchingeElement(IEnumerable<ElementViewModel> elements)
            {
                var match = elements.Where(Matches).FirstOrDefault();
                if (match != null)
                {
                    base.OnElementFound(match);
                }
            }

            protected abstract bool Matches(ElementViewModel element);

            public void TryMatch(ElementViewModel element)
            {
                if (this.Element != null) return;
                if (Matches(element)) OnElementFound(element);
            }

            public override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    if (lookup != null)
                    {
                        lookup.CollectionChanged -= lookupChangedHandler;
                        lookup.references.Remove(this);
                    }
                }
            }
        }

        private class ElementReferenceOverAbsolutePath : ElementReferenceImplementationBase
        {
            string path;

            public ElementReferenceOverAbsolutePath(ElementLookup lookup, ElementViewModel element, string path)
                : base(lookup, element)
            {
                this.path = path;
                
                base.PathChanged += (sender, args) =>
                    {
                        this.path = Element.Path;
                    };
            }

            protected override bool Matches(ElementViewModel element)
            {
                return this.path == element.Path;   
            }

        }
        
        private class ElementReferenceOverAncestorPath : ElementReferenceImplementationBase
        {
            string ancestorPath;
            string elementName;

            public ElementReferenceOverAncestorPath(ElementLookup lookup, ElementViewModel element, string ancestorPath, string elementName)
                : base(lookup, element)
            {
                this.ancestorPath = ancestorPath;
                this.elementName = elementName;

                base.PathChanged += (sender, args) =>
                {
                    this.ancestorPath = Element.ParentElement.Path;
                    this.elementName = Element.Name;
                };
            }

            protected override bool Matches(ElementViewModel element)
            {
                if (element.ParentElement == null) return false;
                return element.AncesterElements().Any(x => x.Path == this.ancestorPath) && element.Name == elementName;
            }
        }

        private class TrackPathPropertyChangedAndUpdateReferences  : IDisposable
        {
            readonly ElementLookup lookup;
            readonly ElementViewModel element;
            readonly PropertyChangedEventHandler elementPropertyChangedHandler;

            public TrackPathPropertyChangedAndUpdateReferences(ElementLookup lookup, ElementViewModel element)
	        {
                this.lookup = lookup;
                this.element = element;

                elementPropertyChangedHandler = new PropertyChangedEventHandler(element_PropertyChanged);
                this.element.PropertyChanged += elementPropertyChangedHandler;
	        }

            void element_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Path")
                {
                    string newPath = element.Path;
                    foreach (var reference in lookup.references)
                    {
                        reference.TryMatch(element);
                    }
                }
            }

            public void  Dispose()
            {
                element.PropertyChanged -= elementPropertyChangedHandler;
            }
        }

        private class TrackChildElementCreationAndRemoval : IDisposable
        {
            readonly NotifyCollectionChangedEventHandler childElementsChangedHandler;
            readonly ElementViewModel element;
            readonly ElementLookup lookup;

            public TrackChildElementCreationAndRemoval(ElementLookup lookup, ElementViewModel element)
            {
                this.lookup = lookup;
                this.element = element;

                AddElements(element.ChildElements);
                
                childElementsChangedHandler = new NotifyCollectionChangedEventHandler(ChildElements_CollectionChanged);
                element.ChildElements.CollectionChanged += childElementsChangedHandler;

            }

            void ChildElements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        AddElements(e.NewItems.OfType<ElementViewModel>());
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        RemoveElements(e.OldItems.OfType<ElementViewModel>());
                        break;

                    case NotifyCollectionChangedAction.Replace:

                        break;

                    case NotifyCollectionChangedAction.Reset:
                        
                        break;

                }
            }

            private void RemoveElements(IEnumerable<ElementViewModel> elementsToRemove)
            {
                foreach (ElementViewModel element in elementsToRemove)
                {
                    lookup.RemoveElement(element);
                }
            }

            private void AddElements(IEnumerable<ElementViewModel> children)
            {
                foreach (var child in children)
                {
                    lookup.AddElement(child);
                }
            }

            public void Dispose()
            {
                if (element != null && element.ChildElements != null && childElementsChangedHandler != null)
                {
                    element.ChildElements.CollectionChanged -= childElementsChangedHandler;
                }
            }
        }

        private class ElementChangeScope : IElementChangeScope
        {
            readonly ElementLookup lookup;
            readonly Func<ElementViewModel, bool> predicate;
            readonly NotifyCollectionChangedEventHandler lookupChangedHandler;
            public ElementChangeScope(ElementLookup lookup, Func<ElementViewModel, bool> predicate)
            {
                this.lookup = lookup;
                this.predicate = predicate;
                this.lookupChangedHandler = new NotifyCollectionChangedEventHandler(lookup_CollectionChanged);
                
                this.lookup.CollectionChanged += lookupChangedHandler;
            }

            void lookup_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                NotifyCollectionChangedEventArgs args;
                
                switch(e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.NewItems.OfType<ElementViewModel>().Where(predicate).ToList());
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.OldItems.OfType<ElementViewModel>().Where(predicate).ToList());
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                        break;

                    default: return;
                }


                var handler = CollectionChanged;
                if (handler != null)
                {
                    handler(this, args);
                }
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged;

            public void Dispose()
            {
                lookup.CollectionChanged -= lookupChangedHandler;
            }

            public IEnumerator<ElementViewModel> GetEnumerator()
            {
                return lookup.allElements.Where(predicate).GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(ElementViewModel scope, Type configurationType)
        {
            return scope.DescendentElements(x => x.ConfigurationType == configurationType);
        }
    }

    public interface IElementChangeScope : INotifyCollectionChanged, IEnumerable<ElementViewModel>, IDisposable
    {
    }
}
