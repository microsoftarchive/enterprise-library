//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;

[assembly : ConfigurationSectionManageabilityProvider(LoggingSettings.SectionName, typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(PriorityFilterDataManageabilityProvider), typeof(PriorityFilterData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CategoryFilterDataManageabilityProvider), typeof(CategoryFilterData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(LogEnabledFilterDataManageabilityProvider), typeof(LogEnabledFilterData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomLogFilterDataManageabilityProvider), typeof(CustomLogFilterData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(BinaryLogFormatterDataManageabilityProvider), typeof(BinaryLogFormatterData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(TextFormatterDataManageabilityProvider), typeof(TextFormatterData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomFormatterDataManageabilityProvider), typeof(CustomFormatterData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(EmailTraceListenerDataManageabilityProvider), typeof(EmailTraceListenerData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(FlatFileTraceListenerDataManageabilityProvider), typeof(FlatFileTraceListenerData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(FormattedEventLogTraceListenerDataManageabilityProvider), typeof(FormattedEventLogTraceListenerData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(MsmqTraceListenerDataManageabilityProvider), typeof(MsmqTraceListenerData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(WmiTraceListenerDataManageabilityProvider), typeof(WmiTraceListenerData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomTraceListenerDataManageabilityProvider), typeof(CustomTraceListenerData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(SystemDiagnosticsTraceListenerDataManageabilityProvider), typeof(SystemDiagnosticsTraceListenerData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(RollingFlatFileTraceListenerDataManageabilityProvider), typeof(RollingFlatFileTraceListenerData), typeof(LoggingSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(XmlTraceListenerDataManageabilityProvider), typeof(XmlTraceListenerData), typeof(LoggingSettingsManageabilityProvider))]
