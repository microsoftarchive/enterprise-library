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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Collections.Specialized;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using Microsoft.Practices.Unity;


namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// Service class used to access <see cref="ElementViewModel"/> instances loaded in the designer.
    /// </summary>
    /// <remarks>
    /// In order to get an instance of this class, declare it as a constructor argument on the consuming component or use the <see cref="IUnityContainer"/> to obtain an instance from code.
    /// </remarks>
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

        ObservableCollection<SectionViewModel> sections = new ObservableCollection<SectionViewModel>();
        ObservableCollection<ElementViewModel> customElementViewModels = new ObservableCollection<ElementViewModel>();
        
        /// <summary>
        /// This constructor supports the configuration design-time and is not intended to be used directly from your code.
        /// </summary>
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
                                                                    if (args.Action == NotifyCollectionChangedAction.Reset) return;
                                                                    OnCollectionChanged(args);
                                                                }
                                                            };
        }

        void sourceModelDependency_Refresh(object sender, EventArgs e)
        {
            refreshing = true;

            ClearElementTrackers();
            ClearPathTrackers();
            
            references.Clear();
            sections.Clear();
            customElementViewModels.Clear();
            elementCollectionContainers.Clear();

            innerCollection.Clear();
            innerCollection.Add(new CollectionContainer { Collection = sections });
            innerCollection.Add(new CollectionContainer { Collection = customElementViewModels });

            refreshing = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void ClearPathTrackers()
        {
            foreach (var tracker in elementPathTrackers.Values)
            {
                tracker.Dispose();
            }
            elementPathTrackers.Clear();
        }

        private void ClearElementTrackers()
        {
            foreach (var tracker in elements.Values)
            {
                tracker.Dispose();
            }

            elements.Clear();
        }

        /// <summary>
        /// Returns all <see cref="ElementViewModel"/> instances that where created for configuration elements of type <paramref name="configurationType"/>, contained inside a configuration element of type <paramref name="scope"/>.
        /// </summary>
        /// <param name="scope">The configuration type that contains the <see cref="ElementViewModel"/> returned.</param>
        /// <param name="configurationType">The configuration type of which <see cref="ElementViewModel"/> instances should be returned.</param>
        /// <returns>
        /// All <see cref="ElementViewModel"/> instances that where created for configuration elements of type <paramref name="configurationType"/>, contained inside a configuration element of type <paramref name="scope"/>.
        /// </returns>
        public IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(Type scope, Type configurationType)
        {
            var scopeElements = FindInstancesOfConfigurationType(scope);
            return FindInstancesOfConfigurationType(scopeElements.SelectMany(x => x.DescendentElements()), configurationType);
        }

        /// <summary>
        /// Returns all <see cref="ElementViewModel"/> instances that where created for configuration elements of type <paramref name="configurationType"/>.
        /// </summary>
        /// <param name="configurationType">The configuration type of which <see cref="ElementViewModel"/> instances should be returned.</param>
        /// <returns>
        /// All <see cref="ElementViewModel"/> instances that where created for configuration elements of type <paramref name="configurationType"/>.
        /// </returns>
        public IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(Type configurationType)
        {
            return FindInstancesOfConfigurationType(allElements, configurationType);
        }

        private static IEnumerable<ElementViewModel> FindInstancesOfConfigurationType(IEnumerable<ElementViewModel> elements, Type configurationType)
        {
            return elements.Where(x => configurationType.IsAssignableFrom(x.ConfigurationType)).ToList();
        }

        /// <summary>
        /// Adds a custom element to the <see cref="ElementLookup"/>.
        /// </summary>
        /// <remarks>
        /// Custom elements are elements that are created by user code.
        /// </remarks>
        /// <param name="element">
        /// The element that should be added to the <see cref="ElementLookup"/>.
        /// </param>
        public void AddCustomElement(ElementViewModel element)
        {
            customElementViewModels.Add(element);
        }

        /// <summary>
        /// Adds a <see cref="SectionViewModel"/> instance to the <see cref="ElementLookup"/>.
        /// </summary>
        /// <param name="sectionModel">The <see cref="SectionViewModel"/> that should be added.</param>
        public void AddSection(SectionViewModel sectionModel)
        {
            sections.Add(sectionModel);

            AddElement(sectionModel);
        }

        /// <summary>
        /// Removes a <see cref="SectionViewModel"/> instance from the <see cref="ElementLookup"/>.
        /// </summary>
        /// <param name="sectionViewModel">The <see cref="SectionViewModel"/> that should be removed.</param>
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
                elementCollectionContainers.Remove(element);
            }

            elements.Remove(element);

            TrackPathPropertyChangedAndUpdateReferences elementPathTracker;
            if (elementPathTrackers.TryGetValue(element, out elementPathTracker))
            {
                elementPathTrackers.Remove(element);
                elementPathTracker.Dispose();
            }

            RemoveChildElements(element);

            if (element.ChildElements.Any())
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (ObservableCollection<ElementViewModel>)element.ChildElements));
            }
        }

        private void AddElement(ElementViewModel element)
        {
            CollectionContainer collectionContainer = new CollectionContainer() { Collection = element.ChildElements };
            elementCollectionContainers.Add(element, collectionContainer);
            elements.Add(element, new TrackChildElementCreationAndRemoval(this, element));
            elementPathTrackers.Add(element, new TrackPathPropertyChangedAndUpdateReferences(this, element));

            innerCollection.Add(collectionContainer);

            if (element.ChildElements.Any())
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (ObservableCollection<ElementViewModel>)element.ChildElements));
            }
        }

        /// <summary>
        /// Returns the <see cref="ElementViewModel"/> that matched a given <see cref="ElementViewModel.ElementId"/>.
        /// </summary>
        /// <param name="elementId">The <see cref="ElementViewModel.ElementId"/> for which an <see cref="ElementViewModel"/> should be returned.</param>
        /// <returns>
        /// If an element with <see cref="ElementViewModel.ElementId"/> is found, returns the <see cref="ElementViewModel"/>; Otherwise <see langword="null"/>.
        /// </returns>
        public ElementViewModel GetElementById(Guid elementId)
        {
            return allElements.FirstOrDefault(x => x.ElementId == elementId);
        }

        /// <summary>
        /// Returns an <see cref="IElementChangeScope"/> that can be used to monitor <see cref="IElementExtendedPropertyProvider"/> instances.
        /// </summary>
        /// <returns>
        /// An <see cref="IElementChangeScope"/> that can be used to monitor <see cref="IElementExtendedPropertyProvider"/> instances.
        /// </returns>
        public IElementChangeScope FindExtendedPropertyProviders()
        {
            return new ElementChangeScope(this, x => x is IElementExtendedPropertyProvider);
        }

        /// <summary>
        /// Creates an <see cref="ElementReference"/> that can be used to monitor changes and events to an <see cref="ElementViewModel"/> instance.
        /// </summary>
        /// <param name="ancestorPath">
        /// The <see cref="ElementViewModel.Path"/> used to narrow the scope for this reference. <br/>
        /// The reference will only apply to elements that are contained inside this path.</param>
        /// <param name="elementType">
        /// The configuration type for which a reference should be created.
        /// </param>
        /// <param name="elementName">
        /// The name of the element for which this element should be created.
        /// </param>
        /// <returns>
        /// an <see cref="ElementReference"/> that can be used to monitor changes and events to an <see cref="ElementViewModel"/> instance.
        /// </returns>
        public ElementReference CreateReference(string ancestorPath, Type elementType, string elementName)
        {
            ElementViewModel element = null;
            ElementViewModel parentElement = elements.Keys.Where(x => x.Path == ancestorPath).FirstOrDefault();
            if (parentElement != null) element = parentElement.DescendentElements(x => MatchesNamePropertyValue(x, elementName) && elementType.IsAssignableFrom(x.ConfigurationType)).FirstOrDefault();

            var reference = new ElementReferenceOverAncestorPath(this, element, ancestorPath, elementName);

            references.Add(reference);
            return reference;
        }

        private static bool MatchesNamePropertyValue(ElementViewModel element, string name)
        {
            if (element.NameProperty != null)
            {
                string elementName = element.NameProperty.Value as string;
                return string.Compare(name, elementName, StringComparison.CurrentCulture) == 0;
            }
            return false;
        }

        /// <summary>
        /// Creates an <see cref="ElementReference"/> that can be used to monitor changes and events to an <see cref="ElementViewModel"/> instance with the specified <see cref="ElementViewModel.Path"/>.
        /// </summary>
        /// <param name="elementPath">The <see cref="ElementViewModel.Path"/> for which the <see cref="ElementReference"/> should be created.</param>
        /// <returns>
        /// An <see cref="ElementReference"/> that can be used to monitor changes and events to an <see cref="ElementViewModel"/> instance with the specified <see cref="ElementViewModel.Path"/>.
        /// </returns>
        public ElementReference CreateReference(string elementPath)
        {
            ElementViewModel element = elements.Keys.Where(x => x.Path == elementPath).FirstOrDefault();
            ElementReferenceOverAbsolutePath reference = new ElementReferenceOverAbsolutePath(this, element, elementPath);

            references.Add(reference);
            return reference;
        }

        /// <summary>
        /// Creates an instance of <see cref="IElementChangeScope"/> which can be used to monitor <see cref="ElementViewModel"/> instances that match <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A predicate that returns <see langword="true"/> for all elements that should be included in the <see cref="IElementChangeScope"/>.</param>
        /// <returns>
        /// An instance of <see cref="IElementChangeScope"/> which can be used to monitor <see cref="ElementViewModel"/> instances that match <paramref name="predicate"/>.
        /// </returns>
        public IElementChangeScope CreateChangeScope(Func<ElementViewModel, bool> predicate)
        {
            return new ElementChangeScope(this, predicate);
        }

        #region INotifyCollectionChanged implementation

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="NotifyCollectionChangedEventArgs"/> that contains additional information about the <see cref="CollectionChanged"/> event.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        private abstract class ElementReferenceImplementationBase : ElementReference
        {
            readonly ElementLookup lookup;
            readonly NotifyCollectionChangedEventHandler lookupChangedHandler;

            public ElementReferenceImplementationBase(ElementLookup lookup, ElementViewModel element)
                : base(element)
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

            protected override void Dispose(bool disposing)
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
                    this.elementName = GetElementNamePropertyValue(Element);
                };
            }

            protected override bool Matches(ElementViewModel element)
            {
                if (element.ParentElement == null) return false;
                return element.AncestorElements().Any(x => x.Path == this.ancestorPath) && GetElementNamePropertyValue(element) == elementName;
            }

            private static string GetElementNamePropertyValue(ElementViewModel element)
            {
                if (element.NameProperty != null)
                {
                    return element.NameProperty.Value as string;
                }
                return null;
            }
        }

        private class TrackPathPropertyChangedAndUpdateReferences : IDisposable
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
                    foreach (var reference in lookup.references.ToArray())
                    {
                        reference.TryMatch(element);
                    }
                }
            }

            public void Dispose()
            {
                element.PropertyChanged -= elementPropertyChangedHandler;
                GC.SuppressFinalize(this);
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
                element.ChildElementsCollectionChange += childElementsChangedHandler;
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
                    element.ChildElementsCollectionChange -= childElementsChangedHandler;
                }
                GC.SuppressFinalize(this);
            }
        }

        private class ElementChangeScope : IElementChangeScope
        {
            readonly ElementLookup lookup;
            readonly Func<ElementViewModel, bool> predicate;
            public ElementChangeScope(ElementLookup lookup, Func<ElementViewModel, bool> predicate)
            {
                this.lookup = lookup;
                this.predicate = predicate;
                this.lookup.CollectionChanged += lookup_CollectionChanged;
            }

            void lookup_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                NotifyCollectionChangedEventArgs args;

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        var newItems = e.NewItems.OfType<ElementViewModel>().Where(predicate).ToList();
                        if (newItems.Count == 0) return;

                        args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems);

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        var oldItems = e.OldItems.OfType<ElementViewModel>().Where(predicate).ToList();
                        if (oldItems.Count == 0) return;

                        args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems);
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
                lookup.CollectionChanged -= lookup_CollectionChanged;
                GC.SuppressFinalize(this);
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
    }

    /// <summary>
    /// An implementation of <see cref="INotifyCollectionChanged"/> and <see cref="IEnumerable{ElementViewModel}"/> that can be used to iterate over and receive <see cref="INotifyCollectionChanged.CollectionChanged"/> events for <see cref="ElementViewModel"/> instances.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface IElementChangeScope : INotifyCollectionChanged, IEnumerable<ElementViewModel>, IDisposable
    {
    }
}
