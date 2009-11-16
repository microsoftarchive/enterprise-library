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
using System.Configuration;
using System.Linq;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// A custom property added to a <see cref="ElementViewModel"/> to support properties that reference items in other ElementViewModels.
    /// </summary>
    /// <remarks>
    /// The element reference property relies on the <see cref="ElementViewModel"/>s registered with <see cref="ElementLookup"/> to create
    /// and monitor <see cref="ElementReference"/>s.  This allows the property to provide a default value if the reference is not yet available
    /// and provide monitoring for name changes in the referened item.
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
        internal static string NoReferenceDisplayName = "<none>";

        private readonly ChangeScopePropertyWatcher changeScopePropertyWatcher;
        private ReferenceAttribute referenceAttribute;
        private ElementReference reference;
        private ElementLookup lookup;
        private IElementChangeScope elementScope;
        private DeferredPathResolver pathResolver;
        private bool scopeInitialized;
        private bool referenceInitialized;

        ///<summary>
        /// Initializes an instance of the ElementReferenceProperty.  This is usually created by the <see cref="SectionViewModel"/>.
        ///</summary>
        ///<param name="serviceProvider">The service provide used within the Enterprise Library configuraiton system.</param>
        ///<param name="lookup">The element lookup registry.</param>
        ///<param name="parent">The ElementViewModel on which this property is attached.</param>
        ///<param name="declaringProperty">The descriptor declaring this property.</param>
        ///<exception cref="ArgumentException">Thrown if the declaring property <see cref="PropertyDescriptor.PropertyType"/> is not a <see cref="string"/> type.</exception>
        public ElementReferenceProperty(IServiceProvider serviceProvider, ElementLookup lookup, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty, new Attribute[] { new TypeConverterAttribute(typeof(ReferencePropertyConverter)) })
        {
            if (declaringProperty.PropertyType != typeof(string)) throw new ArgumentException(Resources.ReferencePropertyInvalidType);  

            this.lookup = lookup;
            this.changeScopePropertyWatcher = new ChangeScopePropertyWatcher();
            this.changeScopePropertyWatcher.ChangeScopePropertyChanged += new EventHandler(ChangeScopePropertyWatcherChangeScopePropertyChanged);

            referenceAttribute = base.Attributes.OfType<ReferenceAttribute>().FirstOrDefault();
            Debug.Assert(referenceAttribute != null);
        }

        void ChangeScopePropertyWatcherChangeScopePropertyChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("SuggestedValues");
        }

        private void EnsureScopeInitialized()
        {
            if (scopeInitialized) return;

            pathResolver = new DeferredPathResolver(referenceAttribute, base.DeclaringElement, lookup);

            elementScope = lookup.CreateChangeScope(x => referenceAttribute.TargetType.IsAssignableFrom(x.ConfigurationType) && x.AncesterElements().Where(y => y.Path == pathResolver.Path).Any());
            elementScope.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ElementScopeCollectionChanged);

            changeScopePropertyWatcher.Refresh(elementScope);
            scopeInitialized = true;
        }

        private void EnsureReferenceInitialized(bool refresh)
        {
            EnsureScopeInitialized();

            if (!pathResolver.IsResolved) return;
            if (referenceInitialized && !refresh) return;

            if (reference != null) reference.Dispose();

            reference = lookup.CreateReference(pathResolver.Path, referenceAttribute.TargetType, (string)base.GetValue());
            reference.ElementFound += new EventHandler(reference_ElementFound);
            reference.ElementDeleted += new EventHandler(reference_ElementDeleted);
            reference.NameChanged += new PropertyValueChangedEventHandler<string>(ReferenceNameChanged);

            referenceInitialized = true;
        }

        public override void Initialize(InitializeContext context)
        {
            base.Initialize(context);
            EnsureReferenceInitialized(true);
        }

        void ReferenceNameChanged(object sender, PropertyValueChangedEventArgs<string> args)
        {
            string referenceName = args.Value;
            SetValue(referenceName);
        }

        void ElementScopeCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnPropertyChanged("SuggestedValues");
            changeScopePropertyWatcher.Refresh(elementScope);
        }

        static void reference_ElementDeleted(object sender, EventArgs e)
        {

        }

        static void reference_ElementFound(object sender, EventArgs e)
        {

        }

        public override bool SuggestedValuesEditable
        {
            get
            {
                return true;
            }
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

        public override bool HasSuggestedValues
        {
            get { return true; }
        }

        public override IEnumerable<object> SuggestedValues
        {
            get
            {
                EnsureReferenceInitialized(false);

                return elementScope
                        .Select(x => x.Property("Name").Value)
                        .Union( new []{string.Empty})
                        .OrderBy(x => x);
            }
        }

        public override string BindableValue
        {
            get
            {
                return base.BindableValue;
            }
            set
            {
                base.BindableValue = value;
            }
        }

        public override object Value
        {
            get
            {
                EnsureReferenceInitialized(false);
                return base.Value as string;
            }
            set
            {
                if (changeScopePropertyWatcher.InsideChangeScopePropertyChanged) return;

                base.Value = value;
                EnsureReferenceInitialized(true);
                OnPropertyChanged("ChildProperties");
                OnPropertyChanged("HasChildProperties");
            }
        }


        public override bool HasChildProperties
        {
            get
            {
                ElementViewModel element = this.ReferencedElement;
                if (element == null) return false;

                return (element.Properties.Count > 0);
            }
        }

        public override IEnumerable<Property> ChildProperties
        {
            get
            {
                ElementViewModel element = this.ReferencedElement;
                if (element == null) return Enumerable.Empty<Property>();

                return element.Properties;
            }
        }
       
        
        private class DeferredPathResolver
        {
            private readonly ReferenceAttribute referenceAttribute;
            private readonly ElementViewModel baseDeclaringElement;
            private readonly ElementLookup lookup;
            private ElementViewModel scopeElement;

            public DeferredPathResolver(ReferenceAttribute referenceAttribute,
                                        ElementViewModel baseDeclaringElement,
                                        ElementLookup lookup)
            {
                this.referenceAttribute = referenceAttribute;
                this.baseDeclaringElement = baseDeclaringElement;
                this.lookup = lookup;

                AttemptScopeResolution();
            }

            public string Path
            {
                get
                {
                    AttemptScopeResolution();
                    return scopeElement == null ? string.Empty : scopeElement.Path;
                }
            }

            public bool IsResolved
            {
                get
                {
                    return scopeElement != null;
                }
            }

            private void AttemptScopeResolution()
            {
                if (scopeElement == null)
                {
                    scopeElement = referenceAttribute.ScopeIsDeclaringElement ?
                        baseDeclaringElement :
                        lookup.FindInstancesOfConfigurationType(referenceAttribute.ScopeType).FirstOrDefault();
                }
            }
        }
    }

    internal class ChangeScopePropertyWatcher : IDisposable
    {
        PropertyChangedEventHandler propertyChangedHandler;
        List<ElementViewModel> monitoredElements = new List<ElementViewModel>();
        private bool insideChangeScopePropertyChanged = false;

        public ChangeScopePropertyWatcher()
        {
            propertyChangedHandler = new PropertyChangedEventHandler(element_PropertyChanged);
        }

        public void Refresh(IEnumerable<ElementViewModel> elements)
        {
            ClearMonitoredElements();

            foreach (var element in elements)
            {
                element.PropertyChanged += element_PropertyChanged;
                monitoredElements.Add(element);
            }
        }

        private void ClearMonitoredElements()
        {
            foreach (var monitoredElement in monitoredElements)
            {
                monitoredElement.PropertyChanged -= propertyChangedHandler;
            }
            monitoredElements.Clear();
        }

        void element_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        public void Dispose()
        {
            ClearMonitoredElements();
        }
    }
}
