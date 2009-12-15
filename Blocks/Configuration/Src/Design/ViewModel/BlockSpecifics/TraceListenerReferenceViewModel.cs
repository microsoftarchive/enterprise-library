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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class TraceListenerReferenceViewModel : CollectionElementViewModel
    {
        private readonly IUnityContainer builder;

        public TraceListenerReferenceViewModel(IUnityContainer builder, ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement)
        {
            this.builder = builder;
        }

        ///<summary>
        ///</summary>
        public Property NameProperty
        {
            get { return this.Property("Name"); }
        }

        public Property GetNameProperty(EnvironmentalOverridesViewModel environment)
        {
            var overridesProperty = this.Properties.OfType<EnvironmentalOverridesViewModel.OverridesProperty>().Where(x => x.Environment == environment).FirstOrDefault();
            return overridesProperty.ChildProperties.Where(x => x.PropertyName == "Name").FirstOrDefault();
        }
    }

    public class TraceListenerReferenceNameProperty : ElementReferenceProperty
    {
        private readonly ElementLookup lookup;
        private TraceListenerReferenceViewModel traceListenerReferenceViewModel;
        private IElementChangeScope changeScope;
        private ChangeScopePropertyWatcher propertyWatcher;

        public TraceListenerReferenceNameProperty(IServiceProvider serviceProvider, ElementLookup lookup, ElementViewModel parent, PropertyDescriptor declaringProperty) 
            : base(serviceProvider, lookup, parent, declaringProperty)
        {
            this.lookup = lookup;
            traceListenerReferenceViewModel = (TraceListenerReferenceViewModel)parent;

            propertyWatcher = new ChangeScopePropertyWatcher();
            propertyWatcher.ChangeScopePropertyChanged += ScopedElementNamePropertyChangedHandler;
            
            
            changeScope = lookup.CreateChangeScope(x =>x.ConfigurationType == typeof(TraceListenerReferenceData)
                                                       && x.AncesterElements().Any(y => y.Path == traceListenerReferenceViewModel.ParentElement.Path));
            changeScope.CollectionChanged += ContainingCollectionChangeScope;

            propertyWatcher.Refresh(changeScope);
           
        }

        private void ScopedElementNamePropertyChangedHandler(object sender, EventArgs e)
        {
            OnPropertyChanged("SuggestedValues");
        }

        private void ContainingCollectionChangeScope(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("SuggestedValues");
            propertyWatcher.Refresh(changeScope);
        }

        public override IEnumerable<object> SuggestedValues
        {
            get
            {
                var otherListenerReferences =
                    traceListenerReferenceViewModel.ParentElement.ChildElements
                        .Where(x => x.ConfigurationType == typeof (TraceListenerReferenceData))
                        .Where(x => x != traceListenerReferenceViewModel)
                        .Select(l => l.Name)
                        .Union(new[] { string.Empty })
                        .Cast<Object>();

                return base.SuggestedValues.Except(otherListenerReferences);
            }
        }
    }
}
