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
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// A custom property added to a <see cref="ElementViewModel"/> to support properties that reference items in other <see cref="ElementViewModel"/>s.
    /// </summary>
    /// <remarks>
    /// The element reference property relies on the <see cref="ElementViewModel"/>s registered with <see cref="ElementLookup"/> to create
    /// and monitor <see cref="ElementReference"/>s.  This allows the property to provide a default value if the reference is not yet available
    /// and provide monitoring for name changes in the referenced item.
    /// 
    /// ElementReferenceProperty provides <see cref="SuggestedValues"/> and <see cref="HasSuggestedValues"/> to support
    /// drop-down lists for selecting values.
    /// 
    /// If the referenced item is available, the properties of that referenced item can be found through <see cref="ChildProperties"/>.
    /// 
    /// Reference properties are automatically created by the use of the <see cref="ReferenceAttribute"/> on the <see cref="ConfigurationElement"/>.
    /// 
    /// Note:  The declaring property must have a <see cref="string"/> property type
    /// </remarks>
    public class ElementReferenceProperty : ElementProperty
    {
        private bool insideElementScopeChanged = false;
        private readonly ChangeScopePropertyWatcher changeScopePropertyWatcher;
        private ReferenceAttribute referenceAttribute;
        private ElementReference reference;
        private ElementLookup lookup;
        private DeferredElementScope elementScope;
        private bool scopeInitialized;
        private bool referenceInitialized;

        ///<summary>
        /// Initializes an instance of the ElementReferenceProperty.  This is usually created by the <see cref="SectionViewModel"/>.
        ///</summary>
        ///<param name="serviceProvider">The service provide used within the Enterprise Library configuration system.</param>
        ///<param name="lookup">The element lookup registry.</param>
        ///<param name="parent">The ElementViewModel on which this property is attached.</param>
        ///<param name="declaringProperty">The descriptor declaring this property.</param>
        ///<exception cref="ArgumentException">Thrown if the declaring property <see cref="PropertyDescriptor.PropertyType"/> is not a <see cref="string"/> type.</exception>
        [InjectionConstructor]
        public ElementReferenceProperty(IServiceProvider serviceProvider, ElementLookup lookup, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : this(serviceProvider, lookup, parent, declaringProperty, Enumerable.Empty<Attribute>())
        {
        }


        ///<summary>
        /// Initializes an instance of the ElementReferenceProperty.  This is usually created by the <see cref="SectionViewModel"/>.
        ///</summary>
        ///<param name="serviceProvider">The service provide used within the Enterprise Library configuration system.</param>
        ///<param name="lookup">The element lookup registry.</param>
        ///<param name="parent">The <see cref="ElementViewModel"/> on which this property is attached.</param>
        ///<param name="declaringProperty">The descriptor declaring this property.</param>
        ///<param name="additionalAttributes">Additional attributes to apply to the reference proeprty.</param>
        ///<exception cref="ArgumentException">Thrown if the declaring property <see cref="PropertyDescriptor.PropertyType"/> is not a <see cref="string"/> type.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "As designed")]
        public ElementReferenceProperty(IServiceProvider serviceProvider, ElementLookup lookup, ElementViewModel parent, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, parent, declaringProperty, additionalAttributes.Union(new Attribute[] { new ValidationAttribute(typeof(ElementReferenceValidator)) }))
        {
            if (declaringProperty.PropertyType != typeof(string)) throw new ArgumentException(Resources.ReferencePropertyInvalidType);

            this.lookup = lookup;
            this.changeScopePropertyWatcher = new ChangeScopePropertyWatcher();
            this.changeScopePropertyWatcher.ChangeScopePropertyChanged += new EventHandler(ChangeScopePropertyWatcherChangeScopePropertyChanged);

            referenceAttribute = base.Attributes.OfType<ReferenceAttribute>().FirstOrDefault();
            Debug.Assert(referenceAttribute != null);

            ((INotifyCollectionChanged)ValidationResults).CollectionChanged += ValidationCollectionChanged;
        }

        private void ValidationCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("ChildProperties");
            OnPropertyChanged("HasChildProperties");
        }

        void ChangeScopePropertyWatcherChangeScopePropertyChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("SuggestedValues");
        }

        private void EnsureScopeInitialized()
        {
            if (scopeInitialized) return;

            elementScope = new DeferredElementScope(referenceAttribute, base.DeclaringElement, lookup);
            elementScope.CollectionChanged += ElementScopeCollectionChanged;

            changeScopePropertyWatcher.Refresh(elementScope);
            scopeInitialized = true;
        }

        private void EnsureReferenceInitialized(bool refresh)
        {
            EnsureScopeInitialized();

            if (!elementScope.IsScopeResolved) return;
            if (referenceInitialized && !refresh) return;

            if (reference != null) reference.Dispose();

            reference = lookup.CreateReference(elementScope.ScopePath, referenceAttribute.TargetType, (string)base.GetValue());
            reference.ElementFound += ReferenceElementFound;
            reference.ElementDeleted += ReferenceElementDeleted;
            reference.NameChanged += ReferenceNameChanged;

            referenceInitialized = true;
        }


        ///<summary>
        /// Provides an opportunity to initialize the property after creation and prior to visualization.
        ///</summary>
        ///<param name="context"></param>
        public override void Initialize(InitializeContext context)
        {
            base.Initialize(context);
            EnsureReferenceInitialized(true);
        }

        /// <summary>
        /// Converter that should be used to convert value to and from a string representation.
        /// </summary>
        public override TypeConverter Converter
        {
            get
            {
                if (!IsRequired) return new ReferencePropertyConverter();

                return base.Converter;
            }
        }

        void ReferenceNameChanged(object sender, PropertyValueChangedEventArgs<string> args)
        {
            string referenceName = args.Value;
            base.Value = referenceName;
        }

        void ElementScopeCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if ((e.Action == NotifyCollectionChangedAction.Add && e.NewItems.Count == 0) ||
                 (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems.Count == 0)) return;

            insideElementScopeChanged = true;
            try
            {
                base.OnPropertyChanged("SuggestedValues");
                changeScopePropertyWatcher.Refresh(elementScope);
                Validate();
            }
            finally
            {
                insideElementScopeChanged = false;
            }
        }

        void ReferenceElementDeleted(object sender, EventArgs e)
        {
            Value = GetEmptyValue();
        }

        void ReferenceElementFound(object sender, EventArgs e)
        {
            Validate();
        }

        /// <summary>
        /// Gets the value to use when the referred to object is no longer available.
        /// </summary>
        /// <returns>The string representation of an empty-value for this element.</returns>
        protected virtual string GetEmptyValue()
        {
            return null;
        }

        /// <summary>
        /// Returns the <see cref="BindableProperty.BindableValue"/> for an element that is displayed in a suggested value list.
        /// </summary>
        /// <param name="element">The element to collect a value for.</param>
        /// <returns>A string value for the element.</returns>
        protected virtual string GetValueFromElement(ElementViewModel element)
        {
            return element.NameProperty.BindableProperty.BindableValue;
        }


        /// <summary>
        /// Returns the suggested elements for this reference.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<ElementViewModel> GetSuggestedElements()
        {
            return elementScope;
        }

        ///<summary>
        /// The <see cref="ElementViewModel"/> referenced.  This will return null if the reference has not
        /// yet been realized, such as the case if the property has been loaded but the reference has not
        /// yet been loaded,
        ///</summary>
        public ElementViewModel ReferencedElement
        {
            get
            {
                EnsureReferenceInitialized(false);
                if (reference == null) return null;
                return reference.Element;
            }
        }

        /// <summary>
        /// Returns a value indicating that this property has suggested values.
        /// </summary>
        /// <value>Returns <see langword="true"/> if there are suggested values for this property. 
        /// Otherwise, returns <see langword="false"/>.
        /// </value>
        /// <seealso cref="Property.SuggestedValues"/>
        public override bool HasSuggestedValues
        {
            get { return true; }
        }

        /// <summary>
        /// Get a list of suggested values.
        /// </summary>
        public override IEnumerable<object> SuggestedValues
        {
            get
            {
                EnsureReferenceInitialized(false);

                IEnumerable<string> additionalSuggestions =
                    IsRequired ? Enumerable.Empty<string>() : new[] { string.Empty };

                return GetSuggestedElements()
                    .Select(x => GetValueFromElement(x))
                    .Union(additionalSuggestions)
                    .OrderBy(x => x)
                    .Cast<object>();
            }
        }

        /// <summary>
        /// The value of the property.
        /// </summary>
        public override object Value
        {
            get
            {
                EnsureReferenceInitialized(false);
                return base.Value;
            }
            set
            {
                if (changeScopePropertyWatcher.InsideChangeScopePropertyChanged) return;
                if (insideElementScopeChanged) return;

                base.Value = value;
                EnsureReferenceInitialized(true);
                Validate();

                OnPropertyChanged("ChildProperties");
                OnPropertyChanged("HasChildProperties");
                this.DeclaringElement.OnElementReferencesChanged();
            }
        }

        /// <summary>
        /// Gets a value indiciating that this <see cref="Property"/> has child properties.
        /// </summary>
        /// <seealso cref="Property.ChildProperties"/>
        public override bool HasChildProperties
        {
            get
            {
                ElementViewModel element = this.ReferencedElement;
                if (element == null) return false;
                if (this.ValidationResults.Count(e => e.IsError) > 0) return false;

                return (element.Properties.Count > 0);
            }
        }

        /// <summary>
        /// Gets the child properties for this <see cref="Property"/>.
        /// </summary>
        public override IEnumerable<Property> ChildProperties
        {
            get
            {
                ElementViewModel element = this.ReferencedElement;
                if (element == null) return Enumerable.Empty<Property>();
                if (this.ValidationResults.Count(e => e.IsError) > 0) return Enumerable.Empty<Property>();

                return element.Properties;
            }
        }

        /// <summary>
        /// Gets the element that provides the scope for searching for referenced elements.
        /// </summary>
        /// <remarks>
        /// Element references will be in the <see cref="ElementViewModel.DescendentElements()"/> collection
        /// of this element.
        /// </remarks>
        public virtual ElementViewModel ContainingScopeElement
        {
            get { return elementScope.ScopeElement; }
        }


        private class DeferredElementScope : IElementChangeScope
        {
            private readonly ReferenceAttribute referenceAttribute;
            private readonly ElementLookup lookup;
            private ElementViewModel scopeElement;
            private IElementChangeScope elementChangeScope;
            private IElementChangeScope scopeChangeScope;

            public DeferredElementScope(ReferenceAttribute referenceAttribute,
                                        ElementViewModel baseDeclaringElement,
                                        ElementLookup lookup)
            {
                this.referenceAttribute = referenceAttribute;
                this.lookup = lookup;

                if (referenceAttribute.ScopeIsDeclaringElement)
                {
                    CreateElementScope(baseDeclaringElement);
                }
                else
                {
                    if (!TryCreateScopeElement())
                    {
                        scopeChangeScope =
                            lookup.CreateChangeScope(x => x.ConfigurationType.IsAssignableFrom(referenceAttribute.ScopeType));
                        scopeChangeScope.CollectionChanged += ScopeChangedHandler;
                    }
                }
            }

            public bool IsScopeResolved
            {
                get { return scopeElement != null; }
            }

            public string ScopePath
            {
                get { return scopeElement.Path; }
            }

            public ElementViewModel ScopeElement
            {
                get { return scopeElement; }
            }

            private void ScopeChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (scopeElement != null) return;

                if (TryCreateScopeElement())
                {
                    scopeChangeScope.CollectionChanged -= ScopeChangedHandler;
                    InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }

            private bool TryCreateScopeElement()
            {
                var foundElement = lookup.FindInstancesOfConfigurationType(referenceAttribute.ScopeType).FirstOrDefault();
                if (foundElement != null)
                {
                    CreateElementScope(foundElement);
                    return true;
                }

                return false;
            }

            private void CreateElementScope(ElementViewModel element)
            {
                scopeElement = element;

                this.elementChangeScope = lookup.CreateChangeScope(
                            x => referenceAttribute.TargetType.IsAssignableFrom(x.ConfigurationType)
                                 && x.AncestorElements().Where(y => y.Path == scopeElement.Path).Any());

                elementChangeScope.CollectionChanged += ElementScopeChangedHandler;
            }

            private void ElementScopeChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
            {
                InvokeCollectionChanged(e);
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged;

            private void InvokeCollectionChanged(NotifyCollectionChangedEventArgs e)
            {
                NotifyCollectionChangedEventHandler changed = CollectionChanged;
                if (changed != null) changed(this, e);
            }

            public IEnumerator<ElementViewModel> GetEnumerator()
            {
                if (elementChangeScope == null) return Enumerable.Empty<ElementViewModel>().GetEnumerator();
                return elementChangeScope.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (elementChangeScope != null)
                {
                    elementChangeScope.CollectionChanged -= ElementScopeChangedHandler;
                    elementChangeScope.Dispose();
                    elementChangeScope = null;
                }

                if (scopeChangeScope != null)
                {
                    scopeChangeScope.CollectionChanged -= ScopeChangedHandler;
                    scopeChangeScope.Dispose();
                    scopeChangeScope = null;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Indicates the object is being disposed.
        /// </summary>
        /// <param name="disposing">Indicates <see cref="ViewModel.Dispose(bool)"/> was invoked through an explicit call to <see cref="ViewModel.Dispose()"/> instead of a finalizer call.</param>
        protected override void Dispose(bool disposing)
        {
            if (elementScope != null)
            {
                elementScope.CollectionChanged -= ElementScopeCollectionChanged;
                elementScope.Dispose();
                elementScope = null;
            }

            if (reference != null)
            {
                reference.ElementFound -= ReferenceElementFound;
                reference.ElementDeleted -= ReferenceElementDeleted;
                reference.NameChanged -= ReferenceNameChanged;

                reference.Dispose();
                reference = null;
            }

            if (changeScopePropertyWatcher != null)
            {
                changeScopePropertyWatcher.Dispose();
            }

            ((INotifyCollectionChanged)ValidationResults).CollectionChanged -= ValidationCollectionChanged;

            base.Dispose(disposing);
        }
    }

    internal class ChangeScopePropertyWatcher : IDisposable
    {
        List<ElementViewModel> monitoredElements = new List<ElementViewModel>();
        private bool insideChangeScopePropertyChanged = false;

        public ChangeScopePropertyWatcher()
        {

        }

        public void Refresh(IEnumerable<ElementViewModel> elements)
        {
            ClearMonitoredElements();

            foreach (var element in elements)
            {
                element.PropertyChanged += ElementPropertyChanged;
                monitoredElements.Add(element);
            }
        }

        private void ClearMonitoredElements()
        {
            foreach (var monitoredElement in monitoredElements)
            {
                monitoredElement.PropertyChanged -= ElementPropertyChanged;
            }
            monitoredElements.Clear();
        }

        void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                DoChangeScopeNamePropertyChanged();
            }
        }

        private void DoChangeScopeNamePropertyChanged()
        {
            try
            {
                insideChangeScopePropertyChanged = true;
                var handler = ChangeScopePropertyChanged;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            finally
            {
                insideChangeScopePropertyChanged = false;
            }
        }

        public event EventHandler ChangeScopePropertyChanged;

        public bool InsideChangeScopePropertyChanged
        {
            get { return insideChangeScopePropertyChanged; }
        }

        protected virtual void Dispose(bool disposing)
        {
            ClearMonitoredElements();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
