using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class AddLoggingBlockCommand : AddApplicationBlockCommand
    {
        public AddLoggingBlockCommand(ConfigurationSourceModel configurationSourceModel, AddApplicationBlockCommandAttribute attribute)
            : base(configurationSourceModel, attribute)
        {

        }

        protected override System.Configuration.ConfigurationSection CreateConfigurationSection()
        {

            return new LoggingSettings() //DesignResources.LoggingSettingsDisplayName)
            {
                DefaultCategory = "General",
                SpecialTraceSources = new SpecialTraceSourcesData(
                        new TraceSourceData("All Events", SourceLevels.All),
                        new TraceSourceData("Unprocessed Category", SourceLevels.All),
                        new TraceSourceData("Logging Errors & Warnings", SourceLevels.All)
                        {
                            TraceListeners = 
                            {{
                                 new TraceListenerReferenceData("Event Log Listener")
                            }}
                        }),

                TraceListeners = 
                {{
                    new FormattedEventLogTraceListenerData("Event Log Listener", "Application", "Text Formatter")
                }},

                Formatters = 
                {{
                    new TextFormatterData("Text Formatter", TextFormatterData.DefaultTemplate)
                }},

                TraceSources = 
                {{
                     new TraceSourceData("General", SourceLevels.All)
                        {
                            TraceListeners = 
                            {{
                                 new TraceListenerReferenceData("Event Log Listener")
                            }}
                        }
                }}
            };
        }
    }
}
