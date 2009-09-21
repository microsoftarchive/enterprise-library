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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Console.Wpf.ViewModel.Services;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class LoggingSectionViewModel : SectionViewModel
    {
        ElementLookup lookup;
        LoggingSettings loggingSettings;

        public LoggingSectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section)
            :base(serviceProvider, section) 
        {
            Initialize(serviceProvider, section);
        }

        public LoggingSectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, section, additionalAttributes)
        {
            Initialize(serviceProvider, section);
        }

        private void Initialize(IServiceProvider serviceProvider, ConfigurationSection section)
        {
            if (section as LoggingSettings == null) throw new ArgumentException("section");
            
            loggingSettings = section as LoggingSettings;
            lookup = serviceProvider.EnsuredGetService<ElementLookup>();
        }

        public override IEnumerable<ElementViewModel> GetRelatedElements(ElementViewModel element)
        {
            var related = base.GetRelatedElements(element).ToList();

            if (typeof(TraceSourceData).IsAssignableFrom(element.ConfigurationType))
            {
                var listenerNames = element.DescendentElements(x => x.ConfigurationType == typeof(TraceListenerReferenceData))
                                                .Select(x => x.Name)
                                                .ToArray();

                var actualListeners =  this.DescendentElements(x=> typeof(TraceListenerData).IsAssignableFrom( x.ConfigurationType) && listenerNames.Contains(x.Name));

                related.AddRange(actualListeners);
            } 
            else if (typeof(TraceListenerData).IsAssignableFrom(element.ConfigurationType))
            {
                var allSources = this.DescendentElements(x => typeof(TraceSourceData).IsAssignableFrom(x.ConfigurationType));
                foreach(var source in allSources)
                {
                    var sourceTraceListenerReferences = source.DescendentElements(x =>  typeof(TraceListenerReferenceData) == x.ConfigurationType);
                    var sourceTraceListenerReferencesThatMatchMe = sourceTraceListenerReferences.Where(x=>x.Name == element.Name);

                    if (sourceTraceListenerReferencesThatMatchMe.Any())
                    {
                        related.Add(source);
                    }
                }
            }

            return related;
        }

        public override IEnumerable<ViewModel> GetAdditionalGridVisuals()
        {
            var traceListenerCollection = ChildElements.Where(x => x.ConfigurationType == typeof(TraceListenerDataCollection)).First();
            var formattersCollection = ChildElements.Where(x => x.ConfigurationType == typeof(NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>)).First();
            var traceSourcesCollection = ChildElements.Where(x => x.ConfigurationType == typeof(NamedElementCollection<TraceSourceData>)).First();

            yield return new ElementViewModelWrappingHeaderViewModel(traceSourcesCollection, true) { Column = 0, Row = 0 };
            yield return new ElementViewModelWrappingHeaderViewModel(traceListenerCollection, true) { Column = 1, Row = 0 };
            yield return new ElementViewModelWrappingHeaderViewModel(formattersCollection, true) { Column = 2, Row = 0 };

            
        }

        public override void UpdateLayout()
        {
            int traceSourceRow = 1;
            foreach (var traceSource in DescendentElements(x => typeof(TraceSourceData).IsAssignableFrom(x.ConfigurationType)))
            {
                traceSource.Column = 0;
                traceSource.Row = traceSourceRow++;
            }

            int traceListenerRow = 1;
            foreach (var traceListener in DescendentElements(x=>typeof(TraceListenerData).IsAssignableFrom( x.ConfigurationType )))
            {
                traceListener.Column = 1;
                traceListener.Row = traceListenerRow++;
            }

            int formatterRow = 1;
            foreach (var formatter in DescendentElements(x => typeof(FormatterData).IsAssignableFrom(x.ConfigurationType)))
            {
                formatter.Column = 2;
                formatter.Row = formatterRow++;
            }
        }
    }
}
