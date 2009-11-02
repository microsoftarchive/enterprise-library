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
using Microsoft.Practices.Unity;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class LoggingSectionViewModel : PositionedSectionViewModel
    {
        ElementLookup lookup;
        LoggingSettings loggingSettings;

        public LoggingSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section) 
        {
            if (section as LoggingSettings == null) throw new ArgumentException("section");
            
            loggingSettings = section as LoggingSettings;
            lookup = builder.Resolve<ElementLookup>();

            loggingSettings.SpecialTraceSources.AllEventsTraceSource.Name = "All Events";
            loggingSettings.SpecialTraceSources.ErrorsTraceSource.Name = "Logging Errors & Warnings";
            loggingSettings.SpecialTraceSources.NotProcessedTraceSource.Name = "Unprocessed Category";


            var sourcesPosition = Positioning.PositionCollection("Trace Sources", 
                            typeof(NamedElementCollection<TraceSourceData>), 
                            typeof(TraceSourceData), 
                            new PositioningInstructions { FixedColumn = 0, FixedRow = 0 });

            var specialSources = Positioning.PositionCollection("Special Sources",
                            typeof(SpecialTraceSourcesData),
                            typeof(TraceSourceData),
                            new PositioningInstructions { FixedColumn = 0, RowAfter = sourcesPosition });

            Positioning.PositionCollection("Filters", 
                            typeof(NameTypeConfigurationElementCollection<LogFilterData, CustomLogFilterData>),
                            typeof(LogFilterData),
                            new PositioningInstructions
                            {
                                RowAfter = specialSources,
                                FixedColumn = 0
                            });

            Positioning.PositionCollection("Trace Listeners", 
                            typeof(TraceListenerDataCollection), 
                            typeof(TraceListenerData), 
                            new PositioningInstructions { FixedColumn = 1, FixedRow = 0 });

            Positioning.PositionCollection("Formatters", 
                            typeof(NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>), 
                            typeof(FormatterData), 
                            new PositioningInstructions { FixedColumn = 2, FixedRow = 0 });
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
    }
}
