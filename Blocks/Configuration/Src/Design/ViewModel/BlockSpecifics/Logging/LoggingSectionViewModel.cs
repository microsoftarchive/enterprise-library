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
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class LoggingSectionViewModel : SectionViewModel
    {
        private LoggingSettings loggingSettings;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public LoggingSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
            if (section as LoggingSettings == null) throw new ArgumentException("section");

            loggingSettings = section as LoggingSettings;

            loggingSettings.SpecialTraceSources.AllEventsTraceSource.Name = "All Events";
            loggingSettings.SpecialTraceSources.ErrorsTraceSource.Name = "Logging Errors & Warnings";
            loggingSettings.SpecialTraceSources.NotProcessedTraceSource.Name = "Unprocessed Category";
        }


        public override IEnumerable<ElementViewModel> GetRelatedElements(ElementViewModel element)
        {
            var related = base.GetRelatedElements(element).ToList();

            if (typeof (TraceSourceData).IsAssignableFrom(element.ConfigurationType))
            {
                var listenerReferenceNames =
                    element.DescendentElements(x => x.ConfigurationType == typeof (TraceListenerReferenceData))
                        .Select(x => x.NamePropertyInternalValue)
                        .ToArray();

                var actualListeners =
                    DescendentElements(
                        x =>
                        typeof (TraceListenerData).IsAssignableFrom(x.ConfigurationType) &&
                        listenerReferenceNames.Contains(x.NamePropertyInternalValue));

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
                        sourceTraceListenerReferences.Select(x => x.Property("Name")).
                                                      Cast<ElementReferenceProperty>().
                                                      Where(x => x.ReferencedElement != null && x.ReferencedElement.ElementId == element.ElementId);

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

            return new HorizontalListLayout(
                new TwoVerticalsLayout(
                    new HeaderedListLayout(traceSources),
                    new TwoVerticalsLayout(
                        new HeaderedListLayout(specialTraceSources, Enumerable.Empty<CommandModel>()),
                        new HeaderedListLayout(categoryFilters))),
                new HeaderedListLayout(traceListeners),
                new HeaderedListLayout(formatters));
        }
    }
#pragma warning restore 1591
}
