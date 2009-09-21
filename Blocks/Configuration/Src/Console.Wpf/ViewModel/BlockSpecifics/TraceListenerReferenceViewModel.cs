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

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class TraceListenerReferenceViewModel : CollectionElementViewModel, IElementExtendedPropertyProvider
    {
        private readonly IServiceProvider serviceProvider;

        public TraceListenerReferenceViewModel(IServiceProvider serviceProvider, ElementCollectionViewModel containingCollection, ConfigurationElement thisElement) 
            : base(serviceProvider, containingCollection, thisElement)
        {
            this.serviceProvider = serviceProvider;
        }

        public bool CanExtend(ElementViewModel subject)
        {
            return typeof(TraceSourceData).IsAssignableFrom(subject.ConfigurationType);
        }

        public IEnumerable<Property> GetExtendedProperties(ElementViewModel subject)
        {
            var elementLookup = serviceProvider.GetService(typeof(ElementLookup)) as ElementLookup;
            var listenerCollection = elementLookup.FindInstancesOfConfigurationType(typeof(LoggingSettings), typeof(TraceListenerDataCollection)).First(); 

            TraceSourceData traceSource = (TraceSourceData)subject.ConfigurationElement;
            if (subject.DescendentElements().Where(e => e == this).Any())
            {
                var reference = elementLookup.CreateReference(listenerCollection.Path, this.Name);
                yield return new SendToProperty(
                    serviceProvider,
                    subject,
                    this,
                    reference);
            }
        }

        private class SendToProperty : Property
        {
            private readonly IServiceProvider serviceProvider;
            private readonly ElementReference elementReference;
            private readonly TraceListenerReferenceViewModel traceListenerReference;

            public SendToProperty(IServiceProvider serviceProvider, object component, TraceListenerReferenceViewModel traceListenerReference, ElementReference elementReference)
                : base(serviceProvider, component, null)
            {
                this.serviceProvider = serviceProvider;
                this.elementReference = elementReference;
                

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

                    //OnPropertyChanged("Value");                    
                    // if string.IsEmptyOrNull(value)
                    //    traceSource.Listeners.ReferenceData
                    // else
                    //    is in listeners collection
                    //          set referencedata.name = (listener.name || value)
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
                    var lookup = this.serviceProvider.EnsuredGetService<ElementLookup>();

                    return new [] {string.Empty}.Concat(
                        lookup.FindInstancesOfConfigurationType(typeof (TraceListenerData)).Select(e => e.Name))
                        .ToArray();
                }
            }

            public override bool ReadOnly
            {
                get
                {
                    // if readonly && suggested values => then can pick
                    // if !readonly && suggested values ==> then can pick or enter

                    return false;
                }
            }
        }
    }


}
