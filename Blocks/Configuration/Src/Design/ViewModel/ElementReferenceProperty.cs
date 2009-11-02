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
using System.Linq;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Console.Wpf.ViewModel.Services;
using System.Diagnostics;
using System.Collections.Generic;

namespace Console.Wpf.ViewModel
{
    public class ElementReferenceProperty : ElementProperty, IPropertyNeedsInitialization
    {
        private ReferenceAttribute referenceAttribute;
        private ElementReference reference;
        private ElementLookup lookup;
        private IElementChangeScope elementScope;
        private ElementViewModel scopeElement;
        private bool scopeInitialized;
        private bool referenceInitialized;

        public ElementReferenceProperty(IServiceProvider serviceProvider, ElementLookup lookup, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty)
        {
            if (declaringProperty.PropertyType != typeof(string)) throw new ArgumentException("TODO");

            this.lookup = lookup;

            referenceAttribute = base.Attributes.OfType<ReferenceAttribute>().FirstOrDefault();
            Debug.Assert(referenceAttribute != null);
        }

        private void EnsureScopeInitialized()
        {
            if (scopeInitialized) return;

            scopeElement = referenceAttribute.ScopeIsDeclaringElement ?
                base.DeclaringElement :
                lookup.FindInstancesOfConfigurationType(referenceAttribute.ScopeType).FirstOrDefault();

            elementScope = lookup.CreateChangeScope(x => referenceAttribute.TargetType.IsAssignableFrom(x.ConfigurationType) && x.AncesterElements().Where(y => y.Path == scopeElement.Path).Any());
            elementScope.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(elementScope_CollectionChanged);

            scopeInitialized = true;
        }

        private void EnsureReferenceInitialized(bool refresh)
        {
            EnsureScopeInitialized();

            if (scopeElement == null) return; 
            if (referenceInitialized && !refresh) return;

            if (reference != null) reference.Dispose();

            reference = lookup.CreateReference(scopeElement.Path, referenceAttribute.TargetType, (string)base.GetUnConvertedValueDirect());
            reference.ElementFound += new EventHandler(reference_ElementFound);
            reference.ElementDeleted += new EventHandler(reference_ElementDeleted);
            reference.NameChanged += new PropertyValueChangedEventHandler<string>(reference_NameChanged);

            referenceInitialized = true;
        }


        public void Initialize()
        {
            EnsureReferenceInitialized(true);
        }

        void reference_NameChanged(object sender, PropertyValueChangedEventArgs<string> args)
        {
            string referenceName = args.Value;
            SetConvertedValueDirect(referenceName);
        }

        void elementScope_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnPropertyChanged("SuggestedValues");   
        }

        void reference_ElementDeleted(object sender, EventArgs e)
        {
            
        }

        void reference_ElementFound(object sender, EventArgs e)
        {
            
        }

        public override bool SuggestedValuesEditable
        {
            get
            {
                return true;
            }
        }

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
            get{ return true; }
        }

        public override IEnumerable<object> SuggestedValues
        {
            get
            {
                EnsureReferenceInitialized(false);
                return elementScope.Select(x => x.Property(referenceAttribute.PropertyToMatch).Value);
            }
        }

        public override object Value
        {
            get
            {
                EnsureReferenceInitialized(false);
                return base.Value;
            }
            set
            {
                base.Value = value;
                EnsureReferenceInitialized(true);
            }
        }
    }
}
