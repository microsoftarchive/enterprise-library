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
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class TraceListenerReferenceViewModel : CollectionElementViewModel, IElementExtendedPropertyProvider
    {
        private readonly IUnityContainer builder;

        public TraceListenerReferenceViewModel(IUnityContainer builder, ElementCollectionViewModel containingCollection, ConfigurationElement thisElement) 
            : base(containingCollection, thisElement)
        {
            this.builder = builder;
        }

        public bool CanExtend(ElementViewModel subject)
        {
            return typeof(TraceSourceData).IsAssignableFrom(subject.ConfigurationType);
        }

        public IEnumerable<Property> GetExtendedProperties(ElementViewModel subject)
        {
            var elementLookup = builder.Resolve<ElementLookup>();
            const string traceListenerCollectionLookupPath = "/configuration/loggingConfiguration/listeners";

            TraceSourceData traceSource = (TraceSourceData)subject.ConfigurationElement;
            if (subject.DescendentElements().Where(e => e == this).Any())
            {
                var reference = elementLookup.CreateReference(traceListenerCollectionLookupPath, typeof(TraceListenerData), this.Name);
                yield return ContainingSection.CreateProperty<SendToProperty>(
                    new ParameterOverride("component", subject),
                    new ParameterOverride("traceListenerReference", this),
                    new ParameterOverride("elementReference", reference));
            }
        }

        private class SendToProperty : Property
        {
            private readonly IServiceProvider serviceProvider;
            private readonly ElementReference elementReference;
            private readonly TraceListenerReferenceViewModel traceListenerReference;
            private readonly ElementLookup elementLookup;

            public SendToProperty(IServiceProvider serviceProvider, ElementLookup elementLookup, object component, TraceListenerReferenceViewModel traceListenerReference, ElementReference elementReference)
                : base(serviceProvider, component, null)
            {
                this.serviceProvider = serviceProvider;
                this.elementReference = elementReference;
                this.elementLookup = elementLookup;

                this.elementReference.NameChanged += (s, a) =>
                    {
                        traceListenerReference.Property("Name").Value = ((ElementViewModel)s).Name;
                        OnPropertyChanged("Value");
                    };

                this.traceListenerReference = traceListenerReference;
            }

            public override string Category
            {
                get
                {
                    return "Trace Listeners";   //todo: get from resource
                }
            }

            public override string DisplayName
            {
                get
                {
                    return "Send To Listener";
                }
            }

            public override string PropertyName
            {
                get
                {
                    return "SendTo";
                }
            }

            public override object Value
            {
                get
                {

                    return traceListenerReference.Name;

                }
                set
                {
                    if (value == null || string.Empty.Equals(value))
                    {
                        traceListenerReference.Delete();
                        return;
                    }

                    traceListenerReference.Property("Name").Value = value;
                }
            }

            public override bool HasSuggestedValues
            {
                get
                {
                    return true;
                }
            }

            public override IEnumerable<object> SuggestedValues
            {
                get
                {
                    return new [] {string.Empty}.Concat(
                        elementLookup.FindInstancesOfConfigurationType(typeof (TraceListenerData)).Select(e => e.Name))
                        .ToArray();
                }
            }

            public override bool ReadOnly
            {
                get
                {
                    return false;
                }
            }
        }
    }
}
