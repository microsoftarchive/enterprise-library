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
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class TraceListenerReferenceViewModel : CollectionElementViewModel
    {
        private readonly ElementCollectionViewModel containingCollection;

        public TraceListenerReferenceViewModel(ElementCollectionViewModel containingCollection, ConfigurationElement thisElement)
            : base(containingCollection, thisElement, new Attribute[]
            {
                new EnvironmentalOverridesAttribute(true) { CustomOverridesPropertyType = typeof(TraceListenerReferenceEnvironmentOverriddenElementProperty) }
            })
        {
            this.containingCollection = containingCollection;
        }

        ///<summary>
        ///</summary>
        public override Property NameProperty
        {
            get { return this.Property("Name"); }
        }

        public Property GetNameProperty(EnvironmentSourceViewModel environment)
        {
            var overridesProperty = this.Properties.OfType<EnvironmentOverriddenElementProperty>().Where(x => x.Environment == environment).FirstOrDefault();
            return overridesProperty.ChildProperties.Where(x => x.PropertyName == "Name").FirstOrDefault();
        }

        protected override string GetLocalPathPart()
        {
            var collectionElements = containingCollection.ChildElements.OfType<TraceListenerReferenceViewModel>().Select(x=>x.ElementId).ToArray();
            int index = Array.IndexOf(collectionElements, this.ElementId);
            if (index == -1) index = collectionElements.Count();
            return string.Format(CultureInfo.InvariantCulture, "add[{0}]", index + 1);
        }
    }

    public class TraceListenerReferenceEnvironmentOverriddenElementProperty : EnvironmentOverriddenElementProperty
    {
        EnvironmentOverriddenElementProperty categoryOverridesProperty;

        public TraceListenerReferenceEnvironmentOverriddenElementProperty (IServiceProvider serviceProvider, IUIServiceWpf uiService, EnvironmentSourceViewModel environmentModel, ElementViewModel subject, EnvironmentOverriddenElementPayload overrides)
            : base(serviceProvider, uiService, environmentModel, subject, overrides)
	    {

	    }

        public override void Initialize(InitializeContext context)
        {
            var categoryElement = Subject.AncestorElements().Where(x => typeof(TraceSourceData).IsAssignableFrom(x.ConfigurationType)).First();
            categoryOverridesProperty = categoryElement.Properties.OfType<EnvironmentOverriddenElementProperty>().Where(x => x.Environment == this.Environment).First();

            categoryOverridesProperty.PropertyChanged += CategoryOverridesPropertyChanged;
            SetValue(categoryOverridesProperty.Value);
        }

        void CategoryOverridesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                Value = categoryOverridesProperty.Value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (categoryOverridesProperty != null)
                categoryOverridesProperty.PropertyChanged -= CategoryOverridesPropertyChanged;
            }
            base.Dispose(disposing);
        }
    }

#pragma warning restore 1591
}
