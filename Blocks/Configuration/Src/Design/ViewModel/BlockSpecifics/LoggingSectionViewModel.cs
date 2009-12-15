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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class LoggingSectionViewModel : SectionViewModel
    {
        private LoggingSettings loggingSettings;
        private ElementLookup lookup;

        public LoggingSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
            if (section as LoggingSettings == null) throw new ArgumentException("section");

            loggingSettings = section as LoggingSettings;
            lookup = builder.Resolve<ElementLookup>();


            loggingSettings.SpecialTraceSources.AllEventsTraceSource.Name = "All Events";
            loggingSettings.SpecialTraceSources.ErrorsTraceSource.Name = "Logging Errors & Warnings";
            loggingSettings.SpecialTraceSources.NotProcessedTraceSource.Name = "Unprocessed Category";
        }


        public override IEnumerable<ElementViewModel> GetRelatedElements(ElementViewModel element)
        {
            var related = base.GetRelatedElements(element).ToList();

            if (typeof (TraceSourceData).IsAssignableFrom(element.ConfigurationType))
            {
                var listenerNames =
                    element.DescendentElements(x => x.ConfigurationType == typeof (TraceListenerReferenceData))
                        .Select(x => x.Name)
                        .ToArray();

                var actualListeners =
                    DescendentElements(
                        x =>
                        typeof (TraceListenerData).IsAssignableFrom(x.ConfigurationType) &&
                        listenerNames.Contains(x.Name));

                related.AddRange(actualListeners);
            }
            else if (typeof (TraceListenerData).IsAssignableFrom(element.ConfigurationType))
            {
                var allSources = DescendentElements(x => typeof (TraceSourceData).IsAssignableFrom(x.ConfigurationType));
                foreach (var source in allSources)
                {
                    var sourceTraceListenerReferences =
                        source.DescendentElements(x => typeof (TraceListenerReferenceData) == x.ConfigurationType);
                    var sourceTraceListenerReferencesThatMatchMe =
                        sourceTraceListenerReferences.Where(x => x.Name == element.Name);

                    if (sourceTraceListenerReferencesThatMatchMe.Any())
                    {
                        related.Add(source);
                    }
                }
            }

            return related;
        }

        protected override object CreateBindable()
        {
            var traceSources =
                DescendentElements().Where(x => x.ConfigurationType == typeof (NamedElementCollection<TraceSourceData>))
                    .First();
            var specialTraceSources =
                DescendentElements().Where(x => x.ConfigurationType == typeof (SpecialTraceSourcesData)).First();
            var traceListeners =
                DescendentElements().Where(x => x.ConfigurationType == typeof (TraceListenerDataCollection)).First();
            var formatters =
                DescendentElements().Where(
                    x =>
                    x.ConfigurationType ==
                    typeof (NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>)).First();
            var categoryFilters =
                DescendentElements().Where(
                    x =>
                    x.ConfigurationType ==
                    typeof (NameTypeConfigurationElementCollection<LogFilterData, CustomLogFilterData>)).First();

            return new HorizontalListViewModel(
                new TwoVerticalVisualsViewModel(
                    new HeaderedListViewModel(traceSources),
                    new TwoVerticalVisualsViewModel(
                        new HeaderedListViewModel(specialTraceSources, Enumerable.Empty<CommandModel>()),
                        new HeaderedListViewModel(categoryFilters))),
                new HeaderedListViewModel(traceListeners),
                new HeaderedListViewModel(formatters));
        }
    }
}