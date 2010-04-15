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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class AddLoggingBlockCommand : AddApplicationBlockCommand
    {
        public AddLoggingBlockCommand(ConfigurationSourceModel configurationSourceModel, AddApplicationBlockCommandAttribute attribute, IUIServiceWpf uiService)
            : base(configurationSourceModel, attribute, uiService)
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
                    new FormattedEventLogTraceListenerData("Event Log Listener", "Enterprise Library Logging", "Text Formatter")
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
#pragma warning restore 1591
}
